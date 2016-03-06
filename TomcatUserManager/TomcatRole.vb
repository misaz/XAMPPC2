Imports System.Xml
Imports TomcatUserManager

''' <summary>
''' Role serveru tomcat
''' </summary>
Public Class TomcatRole
	Implements ITomcatAccessItem, ICloneable

	Private _element As XmlElement
	''' <summary>
	''' XML element reprezentující roli
	''' </summary>
	''' <returns></returns>
	Public Property Element() As XmlElement
		Get
			Return _element
		End Get
		Private Set(ByVal value As XmlElement)
			_element = value
		End Set
	End Property

	''' <summary>
	''' inicializace Role z elementu
	''' </summary>
	''' <param name="element">XML element</param>
	Public Sub New(element As XmlElement)
		Me.Element = element
	End Sub

	''' <summary>
	''' vytvoření nové role podle jména
	''' </summary>
	''' <param name="name"></param>
	Public Sub New(name As String)
		' vytvoří element
		Dim doc = New XmlDocument()
		' připraví element
		Me.Element = doc.CreateElement("role")
		' inicializuje jméno
		Me.Name = name
	End Sub

	''' <summary>
	''' Jméno role
	''' </summary>
	Public Property Name As String
		Get
			Return Me.Element.GetAttribute("rolename")
		End Get
		Set(value As String)
			Me.Element.SetAttribute("rolename", value)
		End Set
	End Property

	''' <summary>
	''' piktogram role
	''' </summary>
	Public ReadOnly Property Thumbnail As BitmapSource
		Get
			Dim bmps As BitmapSource = New BitmapImage(New Uri("pack://application:,,,/Assets/group.png"))
			Return bmps
		End Get
	End Property

	''' <summary>
	''' naklonuje roli
	''' </summary>
	Public Function Clone() As Object Implements ICloneable.Clone
		Return New TomcatRole(Me.Name)
	End Function

	''' <summary>
	''' porovná role
	''' </summary>
	''' <param name="x1">Role 1</param>
	''' <param name="x2">Role 2</param>
	''' <returns></returns>
	Shared Operator =(x1 As TomcatRole, x2 As TomcatRole)
		Return x1.Name = x2.Name
	End Operator

	Shared Operator <>(x1 As TomcatRole, x2 As TomcatRole)
		Return Not (x1 = x2)
	End Operator

	''' <summary>
	''' Vrátí XMl element reprezentující roli
	''' </summary>
	''' <returns></returns>
	Public Function GetElement() As XmlNode Implements ITomcatAccessItem.GetElement
		Return Element
	End Function
End Class
