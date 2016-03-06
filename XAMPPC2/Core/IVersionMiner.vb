''' <summary>
''' Umí vydolovat verzi k nějaké službě
''' </summary>
Public Interface IVersionMiner

	''' <summary>
	''' Vrátí verzi služby
	''' </summary>
	Function GetVersion() As String

End Interface
