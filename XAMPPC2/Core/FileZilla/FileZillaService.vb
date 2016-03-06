''' <summary>
''' Ovldač serveru filezilla
''' </summary>
Public Class FileZillaService
	Inherits ApacheService
	Implements IVersionMiner


	Sub New(c As ServiceControl)
		MyBase.New(c)

		' incializace proměnných
		Me.serviceName = "FileZillaServer"
		Me.servicePicture = "filezilla.png"
		Me.processName = "FileZillaServer"
		Me.processPath = "FileZillaFTP\FileZillaServer.exe"
		Me.serviceInstallParam = "/install"
		Me.serviceUninstallParam = "/uninstall"
		Me.startParams = "-compat -start"
	End Sub

	''' <summary>
	''' (ne)Obsluha chyb spadnutí
	''' </summary>
	''' <param name="p"></param>
	Protected Overrides Sub HandleProcessFail(p As Process)
		' filezilla nevrací žádným způsobem informace o chybách
		' naštěstí k nim moc nedochází v případě tohoto serveru
	End Sub

	''' <summary>
	''' Získá verzi nainstalovaného serveru
	''' </summary>
	Public Overrides Function GetVersion() As String
		' získá informace o jeho verzi
		Dim v = FileVersionInfo.GetVersionInfo(IO.Path.Combine(Xampp.Config.path, Me.processPath))
		' vydoluje z nich verzi
		Return String.Format("{0}.{1}.{2}", v.ProductMajorPart, v.ProductMinorPart, v.ProductBuildPart)
	End Function

End Class
