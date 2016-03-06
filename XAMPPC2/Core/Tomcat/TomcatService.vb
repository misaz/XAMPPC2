Imports System.Management

''' <summary>
''' Ovladač serveru Tomcat
''' </summary>
Public Class TomcatService
	Inherits ApacheService
	Implements IVersionMiner

	Sub New(c As ServiceControl)
		MyBase.New(c)

		' inicializace proměnných
		Me.serviceName = "tomcat7"
		Me.servicePicture = "tomcat.png"
		Me.processName = "Tomcat7"
		Me.processPath = "tomcat\bin\tomcat7.exe"
		Me.serviceInstallParam = "//IS//"
		Me.serviceUninstallParam = "//DS//"
		Me.startParams = ""
	End Sub

	''' <summary>
	''' Obsloužení pádu serveru
	''' </summary>
	''' <param name="p"></param>
	Protected Overrides Sub HandleProcessFail(p As Process)

	End Sub

	''' <summary>
	''' Vrátí verzi serveru Tomcat
	''' </summary>
	Public Overrides Function GetVersion() As String
		' připraví cestu
		Dim path = IO.Path.Combine(Xampp.path, "tomcat\bin\version.bat")
		' připraví parametry spouštění
		Dim pi = New ProcessStartInfo(path)
		' schová okno
		pi.UseShellExecute = False
		pi.CreateNoWindow = True
		' přesměruje výstup
		pi.RedirectStandardError = True
		pi.RedirectStandardOutput = True
		' spustí proces
		Dim process As Process = Process.Start(pi)
		' počká na výstup
		process.WaitForExit()

		' načte výstup
		Using process.StandardOutput
			' přečte výstup
			Dim response = process.StandardOutput.ReadToEnd()
			Dim version = (From l In response.Split(New Char() {vbCr, vbLf}, StringSplitOptions.RemoveEmptyEntries) ' rozdělíme po řádcích
						   Where l.Contains("Server version") ' najdeme řádek s verzí
						   Select l.Split(":").Last().Split("/").Last()).First() ' rozparsujeme
			Return version
		End Using
		Throw New VersionUnknownException("Tomcat version unknown :(")
	End Function

	''' <summary>
	''' Vrátí procesy serveru
	''' </summary>
	Protected Overrides Function GetProceses() As Process()
		' připraví catalina args - argumenty, které obsahuje server tomcat
		Dim catalinaArg = String.Format("-Dcatalina.home=""{0}tomcat""", Xampp.Config.path).ToLower()

		' výchozí třída Process z .NET Frameworku neumí načítat informace o procesech 
		' spuštěných externě(argumenty cmd), proto se používá WMI, které to umí
		Dim x = New ManagementObjectSearcher("SELECT Handle,CommandLine FROM Win32_Process WHERE Name='java.exe'").Get()

		' dotaz do výsledku volání WMI
		Dim q = From p As ManagementObject In x
				Where
					p IsNot Nothing AndAlso ' pokud vůbec je proces
					p("CommandLine") IsNot Nothing AndAlso ' pokud má nějaký parametr příkazového řádku
					p("CommandLine").ToString().ToLower().Contains(catalinaArg) ' obsahuje catalina arg
				Select Process.GetProcessById(CInt(p("Handle"))) ' Vrátí procesy c .NET Framewoku friendly formátu, které podmínky splňují

		' převede výsledek do listu
		Dim output = q.ToArray()
		Return output
	End Function

	''' <summary>
	''' Spustí server
	''' </summary>
	Public Overrides Sub StartService()
		If IsServiceInstalled() Then
			' Služba Windows se spouští standartně, jako u Apache
			MyBase.StartService()
		Else
			Try
				' spustí Javu s parametry tomcatu
				Dim si = New ProcessStartInfo(GetJavaExePath(), GetTomcatParams())
				' bez okna
				si.UseShellExecute = False
				si.CreateNoWindow = True
				' přesměruje výstup
				si.RedirectStandardOutput = True
				si.RedirectStandardError = True
				'sleduje proces
				Me.watchingProceses.Add(Process.Start(si))
			Catch ex As Exception
				' Něco se podělalo
				Dim erw As New ErrorDialog("tomcat", Nothing, "", New BitmapImage(New Uri("pack://application:,,,/Assets/tomcat.png")))
				erw.Errors.Add(New ProcessError(ex.Message))
				erw.ShowDialog()
			End Try
		End If
	End Sub

	''' <summary>
	''' Vrátí cestu k java.exe
	''' </summary>
	Private Function GetJavaExePath() As String
		If ExistsJavaHome() Then
			Return IO.Path.Combine(Environment.GetEnvironmentVariable("JAVA_HOME"), "bin\java.exe")
		Else
			Throw New JavaNotFoundException("JAVA_HOME not found")
		End If
	End Function

	''' <summary>
	''' Vrátí parametry pro spuštění tomcatu
	''' </summary>
	Private Function GetTomcatParams() As String
		Return String.Format("-Djava.util.logging.config.file=""{0}\tomcat\conf\logging.properties"" -Djava.util.logging.manager=org.apache.juli.ClassLoaderLogManager -Djava.endorsed.dirs=""{0}\tomcat\endorsed"" -classpath ""{0}\tomcat\bin\bootstrap.jar;{0}\tomcat\bin\tomcat-juli.jar"" -Dcatalina.base=""{0}\tomcat"" -Dcatalina.home=""{0}\tomcat"" -Djava.io.tmpdir=""{0}\tomcat\temp"" org.apache.catalina.startup.Bootstrap start", Xampp.Config.path)
	End Function

	''' <summary>
	''' Vrátí jestli je v systému zaregistrována cesta k Javě
	''' </summary>
	Private Function ExistsJavaHome() As Boolean
		Return Environment.GetEnvironmentVariable("JAVA_HOME") IsNot Nothing
	End Function

	''' <summary>
	''' Vrátí jestli je v systému zaregistrována cesta k JRE
	''' </summary>
	Private Function ExistsJreHome() As Boolean
		Return Environment.GetEnvironmentVariable("JRE_HOME") IsNot Nothing
	End Function

	''' <summary>
	''' Vrátí jestli je v systému zaregistrována cesta k Catalina
	''' </summary>
	Private Function ExistsCatalinaHome() As Boolean
		Return Environment.GetEnvironmentVariable("CATALINA_HOME") IsNot Nothing
	End Function

	''' <summary>
	''' Vrátí zdali jsou server a všechny ptořebné závislosti nainstalovány
	''' </summary>
	Public Overrides Function IsInstalled() As Boolean
		Return MyBase.IsInstalled() And ExistsJavaHome() And ExistsJreHome() And ExistsCatalinaHome()
	End Function

End Class
