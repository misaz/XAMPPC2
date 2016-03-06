Imports System.IO
Imports System.Xml

''' <summary>
''' Správce uživatelů tomcat
''' </summary>
Public Class TomcatUserManager

	''' <summary>
	''' jméno souboru
	''' </summary>
	Private fileName As String

	''' <summary>
	''' položky 
	''' </summary>
	Public items As New List(Of ITomcatAccessItem)

	''' <summary>
	''' načte správce podle souboru
	''' </summary>
	''' <param name="fileName"></param>
	Public Sub New(fileName As String)
		' inicializuje jméno
		Me.fileName = fileName
		' naparsuje soubor
		Me.Parse()
	End Sub

	''' <summary>
	''' naparsuje spravované prvky
	''' </summary>
	Private Sub Parse()
		' vyčistí předchozí prvky
		items.Clear()
		Try
			' načte XML dokument
			Dim x As XmlDocument = New XmlDocument()
			x.Load(Me.fileName)

			' projde prvky
			For Each n As XmlNode In x.DocumentElement.ChildNodes
				If n.NodeType = XmlNodeType.Comment Then ' uloží komentář
					items.Add(New Comment(n))
				ElseIf n.NodeType = XmlNodeType.Element Then
					If n.Name = "role" Then ' načte roli
						items.Add(New TomcatRole(n))
					ElseIf n.Name = "user" Then ' načte uživatele
						items.Add(New TomcatUser(n))
					End If
				End If
			Next
		Catch ex As Exception
			Throw New FileNotFoundException("tomcat-users.xml file was not found")
		End Try
	End Sub

	''' <summary>
	''' Uloží změny do souboru
	''' </summary>
	Public Sub Save()
		' vytvoří dokument
		Dim doc As New XmlDocument()
		' vytvoří kořenový element
		Dim root = doc.CreateElement("tomcat-users")
		' vloží kořenový element do dokumentu
		doc.AppendChild(root)
		' projde položky
		For Each item In items
			' importuje jejich XML elementy do nově vytvořeného dokumentu včetně jejich podvlastností
			root.AppendChild(doc.ImportNode(item.GetElement(), True))
		Next
		' pokud nějaký soubor již existuje, odstraní jej
		If File.Exists(fileName) Then
			File.Delete(fileName)
		End If
		' uloží
		doc.Save(fileName)
	End Sub
End Class
