Imports System.Security.Principal
Imports System.Windows.Media.Imaging
Imports XAMPPC2

Module Module1

	Declare Function GetConsoleWindow Lib "kernel32.dll" () As IntPtr
	Declare Function ShowWindow Lib "user32.dll" (hWnd As IntPtr, nCmdShow As Integer) As Boolean

	''' <summary>
	''' Správce služeb Windows
	''' </summary>
	Sub Main()
		' získá odkaz na konzoli
		Dim windowPointer = GetConsoleWindow()
		' skryje okno
		ShowWindow(windowPointer, 0)
		' získá argumenty příkazového řádku
		Dim args = Environment.GetCommandLineArgs().ToList()
		' ověří že uživatel pod kterým je aplikace spuštěná je opravdu
		RequireAdmin()
		' odstraní první argument (cestu k spustitelnému souboru)
		args.RemoveAt(0)
		' akce je pvní argument
		'	pokud je Stop/start tak volíme variantu pro 3 argumenty
		'	pokud je heklp, vypisujeme nápovědu
		'	poduj něco jiného spouštíme program
		Dim action As String = args(0)
		Try
			If action = "start" Or action = "stop" Then
				StartOrStopService(args)
			ElseIf action = "help" Then
				help()
			Else
				ProcessOther(args)
			End If
		Catch ex As Exception

		End Try

	End Sub

	''' <summary>
	''' Vypíše nápovědu
	''' </summary>
	Private Sub help()
		Console.WriteLine("Service Manager - help")
		Console.WriteLine("author: Michal Žůrek (misaz)")
		Console.WriteLine("version: 1.0")
		Console.WriteLine("2015")
		Console.WriteLine("========================")
		Console.WriteLine("Allow to start or stop Windows Service. Working on background and shows dialog window visualising current action.")
		Console.WriteLine()
		Console.WriteLine("Usage:")
		Console.WriteLine(vbTab & "start|stop service_name image_path")
		Console.WriteLine("Or:")
		Console.WriteLine(vbTab & "label image_path program_path program_arguments")
		Console.WriteLine()
		Console.WriteLine(vbTab & "service_name: name of managed Windows Service")
		Console.WriteLine(vbTab & "image_path: relative path to image what will be shown in dialog window. Path is relative to working directory.")
		Console.WriteLine(vbTab & "program_path: Program what will be started.")
		Console.WriteLine(vbTab & "program_arguments: Arguments what will be given to started program")
		Console.ReadKey()
	End Sub

	''' <summary>
	''' Spustí proces a počká na jeho dokončení
	''' </summary>
	''' <param name="args">argumenty spouštění</param>
	Private Sub ProcessOther(args As List(Of String))
		' načte popsiek akce
		Dim label As String = args(0)
		' načte cestu k ikoně programu
		Dim img As String = args(1)
		' načte cestu k programu
		Dim prog As String = args(2)
		' odstraníme použité argumenty
		args.RemoveAt(0)
		args.RemoveAt(0)
		args.RemoveAt(0)
		' načteme argumenty spouštění
		Dim startArgs = String.Join(" ", args)

		' načte obrázek
		Dim bmp = New BitmapImage(New Uri(IO.Path.Combine(IO.Directory.GetCurrentDirectory(), "Assets", img), UriKind.Absolute))
		' připraví proces
		Dim p As New Process()
		' připraví informace o spouštění
		p.StartInfo = New ProcessStartInfo(prog, startArgs)
		' bude se spouštět na pozadí (bez okna)
		p.StartInfo.CreateNoWindow = True
		p.StartInfo.UseShellExecute = False
		' vytvoří dialog
		Dim dlg As New ProgressDialog(label, bmp, p)
		' otevře dialog, ten se o vše ostatní postará
		dlg.ShowDialog()
	End Sub

	''' <summary>
	''' Spustí nebo zastaví službu WIndows
	''' </summary>
	''' <param name="args"></param>
	Private Sub StartOrStopService(args As List(Of String))
		' akce (start/stop)
		Dim action As String = args(0)
		' jméno služby
		Dim service As String = args(1)
		' cesta k obrázku
		Dim picture As String = args(2)
		' název akce v uživatelsky příjeměnším formátu
		Dim actionUserFriendly As String
		' inicializace akce v uživatelsky příjemějším formátu
		If action = "start" Then
			actionUserFriendly = "Spouštění"
		Else
			actionUserFriendly = "Zastavování"
		End If
		' inicializace obrázku
		Dim bmp = New BitmapImage(New Uri(IO.Path.Combine(IO.Directory.GetCurrentDirectory(), "Assets", picture), UriKind.Absolute))
		' příprava procesu
		Dim p = New Process()
		p.StartInfo = New ProcessStartInfo("net", action & " " & service)
		' vyžádá zvýšená oprávnění
		p.StartInfo.Verb = "runas"
		' inicializuje se dialogové okno, které se postará o vše ostatní
		Dim dlg As New ProgressDialog(String.Format("{0} služby {1}", actionUserFriendly, service), bmp, p)
		dlg.ShowDialog()
	End Sub

	''' <summary>
	''' Zkontroluje a případně požádá o zvýšení uživatelských oprávnění
	''' </summary>
	Private Sub RequireAdmin()
		' pokud není administrátor aplikaci restartuje s požadavkem o zbýšení práv a se stejnými parametry
		If Not IsAdmin() Then
			Dim p = New Process()
			p.StartInfo = New ProcessStartInfo(Environment.GetCommandLineArgs()(0), Process.GetCurrentProcess().StartInfo.Arguments)
			p.StartInfo.Verb = "runas"
			Try
				p.Start()
			Catch ex As Exception
				' když uživatel v UAC odklikne NE
				MsgBox("Nedostatečná oprávnění", MsgBoxStyle.Exclamation, "Chyba")
			End Try
			End
		End If
	End Sub

	''' <summary>
	''' Vrátí zdali uživatel pod kterým je aplikace spuštěná naléží do interní role Administrator
	''' </summary>
	Private Function IsAdmin() As Boolean
		' získá Windows Principal
		Dim wp = New WindowsPrincipal(WindowsIdentity.GetCurrent())
		' zkontroluje roli
		Return wp.IsInRole(WindowsBuiltInRole.Administrator)
	End Function

End Module
