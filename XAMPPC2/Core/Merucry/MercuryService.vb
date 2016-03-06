Public Class MercuryService
	Inherits FileZillaService
	Implements IVersionMiner

	Public Sub New(c As ServiceControl)
		MyBase.New(c)

		' inicializace proměnných
		Me.servicePicture = "mercury.png"
		Me.processName = "mercury"
		Me.processPath = "MercuryMail\mercury.exe"
	End Sub

	''' <summary>
	''' Vrátí informaci o tom, zdali lze server nainstalovat jako službu Windows
	''' </summary>
	''' <returns></returns>
	Public Overrides Function CanStartAsWindowsService() As Boolean
		Return False
	End Function

	''' <summary>
	''' Spustí server
	''' </summary>
	Public Overrides Sub StartService()
		' připravíme spouštění
		Dim pinfo = New ProcessStartInfo(IO.Path.Combine(Xampp.Config.path, Me.processPath), startParams)
		' schováme okna
		pinfo.UseShellExecute = False
		pinfo.CreateNoWindow = True
		' přesměrujeme výstup
		pinfo.RedirectStandardOutput = True
		pinfo.RedirectStandardError = True
		Try
			' začneme sledovat
			Me.watchingProceses.Add(Process.Start(pinfo))
		Catch ex As Exception
			MessageBox.Show("Proces nebyl nalezen", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error)
		End Try
	End Sub
End Class
