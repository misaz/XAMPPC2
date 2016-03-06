Imports System.Xml
Imports TomcatUserManager

''' <summary>
''' Komentář v souboru konfigurace
''' </summary>
Public Class Comment
	Implements ITomcatAccessItem

	Private element As XmlComment

	Public Sub New(element As XmlNode)
		Me.element = element
	End Sub

	Public Function GetElement() As XmlNode Implements ITomcatAccessItem.GetElement
		Return element
	End Function
End Class
