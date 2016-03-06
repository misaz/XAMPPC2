Imports System.IO
Imports System.ServiceProcess
Imports System.Text.RegularExpressions
Imports XAMPPC2

''' <summary>
''' Ovladač serveru apache
''' </summary>
Public Class ApacheService
	Implements IService, IVersionMiner

	Public Event StateChanged(sender As Object, e As EventArgs) Implements IService.StateChanged

	''' <summary>
	''' Předchozí stav serveru, slouží k porovnávání, zda došlo ke změně
	''' </summary>
	Private previousState As ServiceState = Nothing

	''' <summary>
	''' Časovač, který kontroluje změnu stavu
	''' </summary>
	Private WithEvents timerChecker As New Threading.DispatcherTimer()

	''' <summary>
	''' Tačítko ovládající server
	''' </summary>
	Protected control As ServiceControl

	''' <summary>
	''' Jméno serveru
	''' </summary>
	Protected serviceName As String

	''' <summary>
	''' Ikona serveru
	''' </summary>
	Protected servicePicture As String

	''' <summary>
	''' Jméno procesu serveru
	''' </summary>
	Protected processName As String

	''' <summary>
	''' Cesta k procesu serveru
	''' </summary>
	Protected processPath As String

	''' <summary>
	''' Argument příkazového řádku předáváný při registraci serveru jako službu Windows
	''' </summary>
	Protected serviceInstallParam As String

	''' <summary>
	''' Argument příkazového řádku předávaný při odinstalace služby Windows
	''' </summary>
	Protected serviceUninstallParam As String

	''' <summary>
	''' Procesy, které služba zpracovává
	''' </summary>
	Protected watchingProceses As New List(Of Process)

	''' <summary>
	''' Parametr předáváný procesu při startu serveru
	''' </summary>
	Protected startParams As String

	''' <summary>
	''' Návratové kódy, které nejsou považovaný za chyby
	''' </summary>
	Protected allowedExitCodes As Integer() = New Integer() {0}

	''' <summary>
	''' Vytvoří instanci správce serveru Apache
	''' </summary>
	''' <param name="c">ovládací tlačítko</param>
	Public Sub New(c As ServiceControl)
		' inicializace hodnot
		Me.control = c
		Me.serviceName = "Apache2.4"
		Me.servicePicture = "apache.png"
		Me.processName = "httpd"
		Me.processPath = "apache\bin\httpd.exe"
		Me.serviceInstallParam = "-k install"
		Me.serviceUninstallParam = "-k uninstall"
		Me.startParams = ""

		' spuštění časovače kontrolující změnu stavu
		timerChecker.Interval = New TimeSpan(0, 0, 0, 1, 0)
		timerChecker.Start()
	End Sub

	''' <summary>
	''' Vrátí verzi neinstalovaného serveru Apache
	''' </summary>
	Public Overridable Function GetVersion() As String Implements IVersionMiner.GetVersion
		' cesta k spouštěnému procesu
		Dim prog As String = Path.Combine(Xampp.Config.path, Me.processPath)

		' informace spouštění proceus
		Dim pi As New ProcessStartInfo(prog, "-v")

		' konfigurace startovače
		pi.UseShellExecute = False
		pi.CreateNoWindow = True
		pi.RedirectStandardOutput = True

		' spuštění procesu
		Dim p As Process
		p = Process.Start(pi)

		' Vyčká na ukončení procesu
		p.WaitForExit()

		' Přečte výstup z programu
		Using p.StandardOutput
			Dim output = p.StandardOutput.ReadToEnd()

			' parsování verze ....
			Dim m = Regex.Match(output, "\/(.*) ").Value.Substring(1).Trim()
			Return m
		End Using
	End Function

	''' <summary>
	''' Přepne stav serveru
	''' </summary>
	Public Sub StateSwitchRequest() Implements IService.StateSwitchRequest
		' rozhodne se o současném stavu
		Dim currentState = Me.GetState()
		Select Case currentState
			Case ServiceState.Running
				StopService()
			Case ServiceState.Starting
				StopService()
			Case ServiceState.Stoped
				StartService()
		End Select
	End Sub

	''' <summary>
	''' Ověří stav serveru
	''' </summary>
	Private Sub CheckState() Handles timerChecker.Tick
		' vyvolá událost
		RaiseEvent StateChanged(Me, Nothing)

		' ověří zdali nějaký server nehavaroval
		CheckProcessesState()
	End Sub

	''' <summary>
	''' Zkontroluje a ověří zdali nehavaroval některý ze sledovaných procesů
	''' </summary>
	Private Sub CheckProcessesState()
		' procesy, které již nebude třeba sledovat
		Dim toDelete As New List(Of Process)()
		For i = 0 To watchingProceses.Count - 1
			Dim p = watchingProceses(i)

			' pokud skončil
			If p.HasExited Then
				' pokud stavový kód není OK
				If Not Me.allowedExitCodes.Contains(p.ExitCode) Then
					HandleProcessFail(p)
				End If
				' přestaneme jej sledovat
				toDelete.Add(p)
			End If
		Next

		' odstraníme ze sledování ukončené procesy
		For Each p In toDelete
			Me.watchingProceses.Remove(p)
		Next
	End Sub

	''' <summary>
	''' Ověří stav ukončeného procesu, zdali nedošlo k chybě v procesu
	''' </summary>
	''' <param name="p">ověřovaný proces</param>
	Protected Overridable Sub HandleProcessFail(p As Process)
		Dim output = p.StandardError.ReadToEnd()
		If Not String.IsNullOrEmpty(output.Trim()) Then
			Dim dlg As New ErrorDialog(processName, p, output, Me.control.Thumb)
			dlg.RestartProcess = New Action(AddressOf StartService)
			dlg.ShowDialog()
		End If
	End Sub

	''' <summary>
	''' Zastaví server
	''' </summary>
	Public Overridable Sub StopService()
		' server běžící jako proces se zastavuje jinka než server spuštěný jako Windows služba
		If Me.IsServiceInstalled() Then
			Dim p = New Process()
			' spustí śerviceStarter
			p.StartInfo = New ProcessStartInfo(Xampp.Config.serviceStarterPath, String.Format("stop {0} {1}", Me.serviceName, Me.servicePicture))
			p.StartInfo.Verb = "runas"
			' spustí
			Try
				p.Start()
			Catch ex As Exception
				' ServiceStarter asi nenalezen
				MessageBox.Show("Ovladač služeb nebyl nalazen", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error)
			End Try
		Else
			' získá všechny procesy a zabije je
			Dim httpds = Me.GetProceses()
			For Each h In httpds
				h.Kill()
			Next
		End If
	End Sub

	''' <summary>
	''' Spustí server
	''' </summary>
	Public Overridable Sub StartService()
		' server, který má běžet jako proces se spouští jinak než server běžící jako služba Windows
		If Me.IsServiceInstalled() Then
			Dim p = New Process()
			' spustí servicestarter
			p.StartInfo = New ProcessStartInfo(Xampp.Config.serviceStarterPath, String.Format("start {0} {1}", Me.serviceName, Me.servicePicture))
			p.StartInfo.Verb = "runas"
			Try
				p.Start()
			Catch ex As Exception
				MessageBox.Show("Ovladač služeb nebyl nalazen", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error)
			End Try
		Else
			' spustí proces
			Dim pinfo = New ProcessStartInfo(Path.Combine(Xampp.Config.path, Me.processPath), startParams)
			' zajistí aby nešlo okno vidět
			pinfo.UseShellExecute = False
			pinfo.CreateNoWindow = True
			' přeměruje výstup serveru (aby bylo možno sledovat páddy serveru)
			pinfo.RedirectStandardOutput = True
			pinfo.RedirectStandardError = True
			Try
				' začneme nově spuštěný proces sledovat
				Me.watchingProceses.Add(Process.Start(pinfo))
			Catch ex As Exception
				MessageBox.Show("Proces nebyl nalezen", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error)
			End Try
		End If
	End Sub

	''' <summary>
	''' Vrátí procesy serveru
	''' </summary>
	Protected Overridable Function GetProceses() As Process()
		Return Process.GetProcessesByName(Me.processName)
	End Function

	''' <summary>
	''' Vrátí aktuální stav serveru
	''' </summary>
	Public Function GetState() As ServiceState Implements IService.GetState
		' Získá procesy
		Dim httpds = Me.GetProceses()
        ' Must listen on any port
        For Each httpd In httpds
			Dim n As New Netstat.Netstat()
			' najde seznam portů na kterých servernaslouchá
			Dim usedPorts = n.GetPortsListenedbyPID(httpd.Id)
			If usedPorts.Any() Then
				Return ServiceState.Running
			End If
		Next
		' stav nebyl doposud vrácen => server neposlouchá na žádném portu, přesto může běžet
		If httpds.Any() Then
			Return ServiceState.Starting
		Else
			Return ServiceState.Stoped
		End If
	End Function

	''' <summary>
	''' Vrátí pravdivostní hodnotu, zdali server může být spuštěn jako služba Windows
	''' </summary>
	Public Overridable Function CanStartAsWindowsService() As Boolean Implements IService.CanStartAsWindowsService
		Return True
	End Function

	''' <summary>
	''' Nainstaluje server jako službu Windows
	''' </summary>
	Public Overridable Sub InstallService() Implements IService.InstallService
		' zastaví server, pokud tak tomu doposud není
		If GetState() <> ServiceState.Stoped Then
			StopService()
		End If
		' spustí ServiceStarter, který službu nainstaluje
		Dim pinfo = New ProcessStartInfo(
			Xampp.Config.serviceStarterPath,
			String.Format("""Instalce služby {0}"" ""{1}"" {4}\{2} {3}",
						  Me.serviceName,
						  Me.servicePicture,
						  Me.processPath,
						  Me.serviceInstallParam,
						  Xampp.Config.path))
		' požádá o zbýšení oprávnění
		pinfo.UseShellExecute = False
		pinfo.Verb = "runas"
		Try
			Process.Start(pinfo)
		Catch ex As Exception
			MessageBox.Show("Ovladač služeb nebyl nalazen", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error)
		End Try
	End Sub

	''' <summary>
	''' Odinstaluje službu Windows příslušného serveru
	''' </summary>
	Public Overridable Sub UninstallService() Implements IService.UninstallService
		' zastaví server, pokud tomu tak není
		If GetState() <> ServiceState.Stoped Then
			StopService()
		End If
		' spustí ServiceStarter, který službu odinstaluje
		Dim pinfo = New ProcessStartInfo(
			Xampp.Config.serviceStarterPath,
			String.Format("""Odinstalace služby {0}"" ""{1}"" {4}\{2} {3}",
						  Me.serviceName,
						  Me.servicePicture,
						  Me.processPath,
						  Me.serviceUninstallParam,
						  Xampp.Config.path))

		' požádá o zvýšení oprávnění
		pinfo.Verb = "runas"
		Try
			Process.Start(pinfo)
		Catch ex As Exception
			MessageBox.Show("Ovladač služeb nebyl nalazen", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error)
		End Try
	End Sub

	''' <summary>
	''' Zjistí jestli je server nainstalovaný jako služba Windows
	''' </summary>
	Public Function IsServiceInstalled() As Boolean Implements IService.IsServiceInstalled
		Dim services = ServiceController.GetServices()
		Dim q = From s In services Where s.DisplayName = Me.serviceName
		Return q.Any()
	End Function

	''' <summary>
	''' Získá seznam portů na kterých server poslouchá
	''' </summary>
	Public Function GetUsedPorts() As List(Of Integer) Implements IService.GetUsedPorts
		Dim output As New List(Of Integer)
		Dim n As New Netstat.Netstat()
		' projde procesy
		For Each p In Me.GetProceses()
			' přidá do výstupu porty na kterých poslouchá procházený proces
			output.AddRange(n.GetPortsListenedbyPID(p.Id))
		Next
		Return output.Distinct().ToList()
	End Function

	''' <summary>
	''' Získá ID procesů serveru
	''' </summary>
	Public Function GetUsedPids() As List(Of Integer) Implements IService.GetUsedPids
		Return (From p In Me.GetProceses() Select p.Id).ToList()
	End Function

	''' <summary>
	''' Zjistí jestli je server v balíku XAMPP vůbec správně nainstalovaný
	''' </summary>
	Public Overridable Function IsInstalled() As Boolean Implements IService.IsInstalled
		Return File.Exists(Path.Combine(Xampp.Config.path, Me.processPath))
	End Function
End Class