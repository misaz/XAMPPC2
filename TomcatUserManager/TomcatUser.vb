Imports System.Xml
Imports TomcatUserManager

''' <summary>
''' Uživatel serveru tomcat
''' </summary>
Public Class TomcatUser
	Implements ITomcatAccessItem, ICloneable

	Private _element As XmlElement
	''' <summary>
	''' XML element reprezentující uživatele
	''' </summary>
	Public Property Element() As XmlElement
		Get
			Return _element
		End Get
		Private Set(ByVal value As XmlElement)
			_element = value
		End Set
	End Property

	''' <summary>
	''' inicializuje uživatele z XML elementu
	''' </summary>
	''' <param name="element"></param>
	Public Sub New(element As XmlElement)
		Me.Element = element
	End Sub

	''' <summary>
	''' vytvoří uživatele podle jména, hesla a jeho rolí
	''' </summary>
	Public Sub New(name As String, password As String, role As String)
		' vytvoří element
		Dim doc = New XmlDocument()
		' inicializuje element uživatele
		Me.Element = doc.CreateElement("user")
		' nastaví vlastnosti
		Me.Name = name
		Me.Password = password
		Me.Role = role
	End Sub

	''' <summary>
	''' Jméno uživatele
	''' </summary>
	Public Property Name
		Get
			Return Me.Element.GetAttribute("username")
		End Get
		Set(value)
			Me.Element.SetAttribute("username", value)
		End Set
	End Property

	''' <summary>
	''' Heslo uživatele
	''' </summary>
	''' <returns></returns>
	Public Property Password
		Get
			Return Me.Element.GetAttribute("password")
		End Get
		Set(value)
			Me.Element.SetAttribute("password", value)

		End Set
	End Property

	''' <summary>
	''' Role do kterých uživatel patří
	''' </summary>
	Public Property Role
		Get
			Return Me.Element.GetAttribute("roles")
		End Get
		Set(value)
			Me.Element.SetAttribute("roles", value)

		End Set
	End Property

	''' <summary>
	''' Vrátí piktogram uživatele
	''' </summary>
	Public ReadOnly Property Thumbnail As BitmapSource
		Get
			Dim bmps As BitmapSource = New BitmapImage(New Uri("pack://application:,,,/Assets/user.png"))
			Return bmps
		End Get
	End Property

	''' <summary>
	''' naklonuje uživatele
	''' </summary>
	Public Function Clone() As Object Implements ICloneable.Clone
		Return New TomcatUser(Name, Password, Role)
	End Function

	Shared Operator =(x1 As TomcatUser, x2 As TomcatUser)
		Return x1.Name = x2.Name And x1.Password = x2.Password And x1.Role = x2.Role
	End Operator

	Shared Operator <>(x1 As TomcatUser, x2 As TomcatUser)
		Return Not (x1 = x2)
	End Operator

	''' <summary>
	''' Vrátí XML element reprezentující uživatele
	''' </summary>
	Public Function GetElement() As XmlNode Implements ITomcatAccessItem.GetElement
		Return Element
	End Function
End Class
