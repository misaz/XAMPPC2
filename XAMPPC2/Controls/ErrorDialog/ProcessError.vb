''' <summary>
''' Reperezentuje chybu vzniklou v některém z procesů serverů
''' </summary>
Public Class ProcessError
	''' <summary>
	''' Kód chyby
	''' </summary>
	Public Property Code As String

	''' <summary>
	''' Popis chyby
	''' </summary>
	Public Property Description As String

	''' <summary>
	''' Výstup z procesu
	''' </summary>
	Public Property RawProcessOutput As String

	''' <summary>
	''' Vytvoří instanci chyby v procesu. Předaný výstup z púroceusu je zároveň popiskem chyby, kód chyby se zanedbává.
	''' </summary>
	''' <param name="message"></param>
	Public Sub New(message As String)
		Me.Description = message
		Me.RawProcessOutput = message
	End Sub
End Class
