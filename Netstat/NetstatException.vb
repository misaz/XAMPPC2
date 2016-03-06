Imports System.Runtime.Serialization

''' <summary>
''' Výjmka ke které může dojít při chybování služby netstatu
''' </summary>
<Serializable>
Friend Class NetstatException
	Inherits Exception

	Public Sub New()
	End Sub

	Public Sub New(message As String)
		MyBase.New(message)
	End Sub

	Public Sub New(message As String, innerException As Exception)
		MyBase.New(message, innerException)
	End Sub

	Protected Sub New(info As SerializationInfo, context As StreamingContext)
		MyBase.New(info, context)
	End Sub
End Class
