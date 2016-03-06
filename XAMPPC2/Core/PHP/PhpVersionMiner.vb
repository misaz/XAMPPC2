Imports System.Text.RegularExpressions
Imports XAMPPC2

''' <summary>
''' Umí zjistit verzi PHP
''' </summary>
Public Class PhpVersionMiner
	Implements IVersionMiner

	''' <summary>
	''' cesta k procesu PHP
	''' </summary>
	Public processPath As String = "php/php.exe"

	''' <summary>
	''' Zjistí verzi PHP
	''' </summary>
	Public Function GetVersion() As String Implements IVersionMiner.GetVersion
		' najde proces PHP
		Dim prog As String = IO.Path.Combine(Xampp.Config.path, Me.processPath)
		' připraví informace o spouštění
		Dim pi As New ProcessStartInfo(prog, "-v")
		' spouštět se bude bez okna
		pi.UseShellExecute = False
		pi.CreateNoWindow = True
		' přesměrujeme výstup
		pi.RedirectStandardOutput = True
		' spustíme
		Dim p As Process = Process.Start(pi)
		' počkáme na výstup
		p.WaitForExit()

		' načteme výstup
		Using p.StandardOutput
			' přečteme výstup
			Dim output = p.StandardOutput.ReadToEnd()
			' naparsujeme verzi
			Return Regex.Match(output, "PHP ([0-9\.]*)").Value.Replace("PHP ", "")
		End Using
	End Function
End Class
