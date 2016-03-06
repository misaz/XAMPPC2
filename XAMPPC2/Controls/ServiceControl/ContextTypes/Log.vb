Imports System.Xml
Imports System.Xml.Serialization

' podobné jako Config

Public Class Log
	<XmlAttribute>
	Public Property Title As String

	<XmlAttribute>
	Public Property Source As String

	Public Shared Function DeserializeObject(el As XmlElement) As Log
		Dim des As New XmlSerializer(GetType(Log))
		Dim obj As Log

		Dim stream As New IO.StringReader(el.OuterXml)
		obj = des.Deserialize(stream)
		Return obj
	End Function

End Class
