Imports System.Xml
Imports System.Xml.Serialization

' Podobné jako Config

Public Class WebApp
	<XmlAttribute>
	Public Property Title As String

	<XmlAttribute>
	Public Property Address As String

	Public Shared Function DeserializeObject(el As XmlElement) As WebApp
		Dim des As New XmlSerializer(GetType(WebApp))
		Dim obj As WebApp

		Dim stream As New IO.StringReader(el.OuterXml)
		obj = des.Deserialize(stream)
		Return obj
	End Function

End Class
