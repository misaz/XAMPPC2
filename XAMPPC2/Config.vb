Imports System.IO

Namespace Xampp
	''' <summary>
	''' Konfigurace ovládacího panelu
	''' </summary>
	Module Config
		''' <summary>
		''' Cesta k balíku XAMPP
		''' </summary>
		Public path As String = Directory.GetCurrentDirectory()

		''' <summary>
		''' Cesta k startovací službě
		''' </summary>
		Public serviceStarterPath As String = "ServiceStarter.exe"
	End Module

	''' <summary>
	''' Stará se o verzi nového ovládacího panelu XAMPP
	''' </summary>
	Class XamppControlpanelVersionMiner
		Implements IVersionMiner

		''' <summary>
		''' Vrátí verzi ovládacího panelu
		''' </summary>
		Public Function GetVersion() As String Implements IVersionMiner.GetVersion
			Return My.Application.Info.Version.ToString()
		End Function
	End Class
End Namespace