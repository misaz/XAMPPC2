Imports System.Xml

''' <summary>
''' Funkce položky, která by se mohla oběvit ve výstupu
''' </summary>
Public Interface ITomcatAccessItem
	''' <summary>
	''' Vrátí svoji reprezentaci XML elementem, použitým do vstupu/výstupu
	''' </summary>
	''' <returns></returns>
	Function GetElement() As XmlNode

End Interface
