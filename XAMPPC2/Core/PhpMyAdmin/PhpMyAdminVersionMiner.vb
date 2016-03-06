Imports System.Text.RegularExpressions
Imports XAMPPC2

''' <summary>
''' Umí získat verzi phpMyAdmin
''' </summary>
Public Class PhpMyAdminVersionMiner
	Implements IVersionMiner

	''' <summary>
	''' Cesta k souboru, kde se verze nachází
	''' </summary>
	Private filePath As String = "phpMyAdmin\libraries\Config.class.php"

	''' <summary>
	''' Vrátí verzi phpMyAdmin
	''' </summary>
	Public Function GetVersion() As String Implements IVersionMiner.GetVersion
		' načtení souboru
		Using sr As New IO.StreamReader(IO.Path.Combine(Xampp.Config.path, Me.filePath))
			' přečtení obsahu souboru
			Dim content = sr.ReadToEnd()
			' vydolování verze
			Return Regex.Match(content, "\$this->set\('PMA_VERSION', '(.*)'\);").Value.Replace("$this->set('PMA_VERSION', '", "").Replace("');", "")
		End Using
	End Function
End Class
