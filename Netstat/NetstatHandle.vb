Imports System.Net.NetworkInformation

''' <summary>
''' Výstup netstat
''' </summary>
Public Class NetstatHandle
	''' <summary>
	''' Lokální adresa
	''' </summary>
	Public Property Address As String

	''' <summary>
	''' Lokální port
	''' </summary>
	Public Property Port As String

	''' <summary>
	''' ID procesu, který obsluhuje spojení
	''' </summary>
	Public Property Pid As Integer

	''' <summary>
	''' Jméno procesu, který obsluhuje spojení
	''' </summary>
	Public Property ProcessName As String

	''' <summary>
	''' Časové razítko o tom, kdy byl záznam netstatu poprvé spozorován
	''' </summary>
	Public Property Started As DateTime

	''' <summary>
	''' Časové razítko o tom, kdy byl záznam netstatu naposledy spozorován
	''' </summary>
	Public Property Closed As DateTime

	''' <summary>
	''' Informace o tom, zdali je záznam nově vznikly
	''' </summary>
	Public Property IsNew As Boolean

	''' <summary>
	''' Informace o tom, zdali je záznam zaniklý
	''' </summary>
	Public Property IsOld As Boolean

	Public Overloads Shared Operator =(ByVal n1 As NetstatHandle, n2 As NetstatHandle)
		Return n1.Address = n2.Address And
			n1.Port = n2.Port And
			n1.Pid = n2.Pid And
			n1.ProcessName = n2.ProcessName
	End Operator

	Public Overloads Shared Operator <>(n1 As NetstatHandle, n2 As NetstatHandle)
		Return Not n1 = n2
	End Operator
End Class
