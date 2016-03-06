Imports System.Text.RegularExpressions

''' <summary>
''' Ovladač server MariaDB
''' </summary>
Public Class MariaService
	Inherits ApacheService
	Implements IVersionMiner

	Public Sub New(c As ServiceControl)
		MyBase.New(c)

		' inicializace proměnných
		Me.serviceName = "MySQL"
		Me.servicePicture = "mariadb_small.png"
		Me.processName = "mysqld"
		Me.processPath = "mysql\bin\mysqld.exe"
		Me.serviceInstallParam = "--install"
		Me.serviceUninstallParam = "--remove"
		Me.allowedExitCodes = New Integer() {0, -1}
	End Sub

	''' <summary>
	''' Získá verzi MariaDB serveru
	''' </summary>
	''' <returns></returns>
	Public Overrides Function GetVersion() As String Implements IVersionMiner.GetVersion
		' najde si cestu k programu
		Dim prog As String = IO.Path.Combine(Xampp.Config.path, Me.processPath)
		' připraí informace spouštění
		Dim pi As New ProcessStartInfo(prog, "--version")
		' spustí skrytě
		pi.UseShellExecute = False
		pi.CreateNoWindow = True
		' přesměruje výstup programu
		pi.RedirectStandardOutput = True
		' spustí
		Dim p As Process = Process.Start(pi)
		' počká na výstup
		p.WaitForExit()
		' načte výstup
		Using p.StandardOutput
			' přečte výstup
			Dim output = p.StandardOutput.ReadToEnd()
			' naparsuje verzi
			Return Regex.Match(output, "Ver (.*)-").Value.Replace("Ver ", "").Replace("-", "")
		End Using
	End Function

	''' <summary>
	''' Obslouží pád procesu
	''' </summary>
	''' <param name="p"></param>
	Protected Overrides Sub HandleProcessFail(p As Process)
		' MariaDB vypisuje chyby do systémového logu
		Dim el = New EventLog("Application")

		Dim startingEvents = (From e As EventLogEntry In el.Entries ' projde záznamy
							  Where
								  e.Source = "MySQL" AndAlso ' performance
								  e.EntryType = EventLogEntryType.Information AndAlso ' informace o startu serveru je jako informace
								  Regex.IsMatch(
									e.Message.Split(New String() {vbCr, vbLf}, StringSplitOptions.RemoveEmptyEntries)(0),
									"mysqld(\.exe)? \(.*\) starting as process (" & p.Id & ") \.{3}",
									System.Text.RegularExpressions.RegexOptions.Singleline) ' najde zprávu
							  Order By e.TimeGenerated Descending ' seřadí
								  )

		' pokud startovací zázńam neexistuje nemá cenu se tím zabývat
		If startingEvents.Any() Then
			Return
		End If

		' vezme první položku poslední start serveru pod PID havarovaného serveru
		Dim startEvent = startingEvents.First()

		' najde chyby ke kterým došlo od startu serveru
		Dim processErrors = From e As EventLogEntry In el.Entries ' projde záznamy
							Where
								e.EntryType = EventLogEntryType.Error AndAlso ' musí být chyby
								e.Source = startEvent.Source AndAlso ' chyby jiných programů nás nezajímají
								e.TimeGenerated.Ticks >= startEvent.TimeGenerated.Ticks ' musí být vygenerovány po tom co byla vygenerována startovací událost
							Select e.Message.Split(
								New String() {vbCr, vbLf},
								StringSplitOptions.RemoveEmptyEntries
							).First() ' vybere zprávu, zahodí bordel okolo

		' pokud nejsou žádné zprávy nic neřešíme
		If processErrors.Any() Then
			' pokud alespoň k jedné chybě došlo, připravíme dialogové okno
			Dim dlg As New ErrorDialog(processName, p, processErrors.ToList(), Me.control.Thumb)
			' přidáme možnost quick-restartu v dialogovém okně chyby
			dlg.RestartProcess = New Action(AddressOf StartService)
			' zobrazíme dialogové okno
			dlg.ShowDialog()
		End If

	End Sub

End Class
