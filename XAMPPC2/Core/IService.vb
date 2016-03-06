''' <summary>
''' Reoprezentuje správce serveru
''' </summary>
Public Interface IService
	''' <summary>
	''' Změnil se stav serveru
	''' </summary>
	''' <param name="sender">Zdroj události</param>
	''' <param name="e">parametry události</param>
	Event StateChanged(sender As Object, e As EventArgs)

	''' <summary>
	''' Požadavek o přepnutí serveru do jiného stavu
	''' </summary>
	Sub StateSwitchRequest()

	''' <summary>
	''' Vrátí aktuální stav serveru
	''' </summary>
	Function GetState() As ServiceState

	''' <summary>
	''' Vráti zdali server může být nainstalován jako služba Windows
	''' </summary>
	Function CanStartAsWindowsService() As Boolean

	''' <summary>
	''' Nainstaluje službu Windows
	''' </summary>
	Sub InstallService()

	''' <summary>
	''' odinstaluje službu Windows
	''' </summary>
	Sub UninstallService()

	''' <summary>
	''' Vrátí informaci o tom, zdali je server naisntalován jako služba Windows
	''' </summary>
	''' <returns></returns>
	Function IsServiceInstalled() As Boolean

	''' <summary>
	''' Vrátí seznam portů na kterých server poslouchá
	''' </summary>
	''' <returns></returns>
	Function GetUsedPorts() As List(Of Integer)

	''' <summary>
	''' Vrátí ID procesů serveru
	''' </summary>
	Function GetUsedPids() As List(Of Integer)

	''' <summary>
	''' Vrátí zdali je server nainstalován
	''' </summary>
	Function IsInstalled() As Boolean

End Interface
