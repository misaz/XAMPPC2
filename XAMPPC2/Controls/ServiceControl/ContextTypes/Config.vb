Imports System.Xml
Imports System.Xml.Serialization

''' <summary>
''' Konfigurační soubor
''' </summary>
Public Class Config
	''' <summary>
	''' Titulek konfiguračního souboru
	''' </summary>
	<XmlAttribute>
	Public Property Title As String

	''' <summary>
	''' Cesta ke konfiguračnímu souboru
	''' </summary>
	<XmlAttribute>
	Public Property Source As String

	''' <summary>
	''' Načte konfigurační soubor z XML elementu Config
	''' </summary>
	''' <param name="el">element</param>
	''' <returns>naparsovaný konfigurační soubor</returns>
	Public Shared Function DeserializeObject(el As XmlElement) As Config
		Dim des As New XmlSerializer(GetType(Config))
		Dim obj As Config

		Dim stream As New IO.StringReader(el.OuterXml)
		obj = des.Deserialize(stream)
		Return obj
	End Function

End Class
