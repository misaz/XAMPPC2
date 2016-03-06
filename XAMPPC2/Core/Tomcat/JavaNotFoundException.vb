''' <summary>
''' Vyjimka v případě absence či porušené instalaci Javy
''' </summary>
Public Class JavaNotFoundException
	Inherits Exception

	Public Sub New(message As String)
		MyBase.New(message)
	End Sub

End Class
