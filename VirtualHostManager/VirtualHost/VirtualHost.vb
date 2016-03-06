Imports System.ComponentModel
Imports System.Text.RegularExpressions
Namespace VirtualHost
	Public Class VirtualHost
		Inherits PropertyBlock
		Implements IVirtualHostConfigurationFileItem, IEditableObject

		Public Event Updated()

		Public Property IP As String
			Get
				Return Me.Params.Split(":")(0).Trim()
			End Get
			Set(value As String)
				RaiseEvent Updated()
				Params = value & ":" & Port
			End Set
		End Property

		Public Property Port As String
			Get
				Return Me.Params.Split(":")(1).Trim()
			End Get
			Set(value As String)
				RaiseEvent Updated()
				Params = IP & ":" & value
			End Set
		End Property

		Public Property Domain As String
			Get
				Return Me.GetProperty("ServerName")
			End Get
			Set(value As String)
				Dim old = Hosts.LocalInstance.HostsManager.GetOrCreateEntry(Domain)
				If old IsNot Nothing Then
					old.Domain = value
				Else
					Dim entry = New Hosts.HostsEntry()
					entry.Domain = value
					entry.IP = "127.0.0.1"
					Hosts.LocalInstance.HostsManager.AddEntry(entry)
				End If
				Me.SetProperty("ServerName", value)
				RaiseEvent Updated()
			End Set
		End Property

		Public Property DirectoryPath As String
			Get
				Dim dir = Me.GetProperty("DocumentRoot")

				If dir.StartsWith("""") Then
					dir = dir.Substring(1)
				End If
				If dir.EndsWith("""") Then
					dir = dir.Substring(0, dir.Length - 1)
				End If

				Return dir
			End Get
			Set(value As String)
				Dim val = value
				If Not val.StartsWith("""") Then
					val = """" & val
				End If
				If Not val.EndsWith("""") Then
					val = val & """"
				End If
				Me.SetProperty("DocumentRoot", val)
				Me.Directory.Path = val
				RaiseEvent Updated()
			End Set
		End Property

		Public Sub UpdatehostEntry()
			Dim e = Hosts.LocalInstance.HostsManager.GetOrCreateEntry(Domain)
			e.IP = "127.0.0.1"
		End Sub

		Public Sub DeleteHostEntry()
			Dim m = Hosts.LocalInstance.HostsManager
			Dim entry = m.GetOrCreateEntry(Domain)
			m.RemoveEntry(entry)
		End Sub

		Public Property HostsEntry As Hosts.HostsEntry
			Get
				Return Hosts.LocalInstance.HostsManager.GetOrCreateEntry(Domain)
			End Get
			Set(value As Hosts.HostsEntry)
				Dim entry = Hosts.LocalInstance.HostsManager.GetOrCreateEntry(Domain)
				If entry Is Nothing Then
					entry = New Hosts.HostsEntry()
					Hosts.LocalInstance.HostsManager.AddEntry(entry)
				End If
				entry.Domain = Domain
				entry.IP = "127.0.0.1"
			End Set
		End Property

		Public Property Directory As Directory
			Get
				Dim q = (From p In Me.Properties
						 Where p.GetType() = GetType(Directory)).ToList()
				If q.Count() > 0 Then
					Return q.First
				Else
					Dim d = New Directory()
					Properties.Add(d)
					Return d
				End If
			End Get
			Set(value As Directory)
				Dim dirs = (From p In Me.Properties
							Where p.GetType() = GetType(Directory)).ToList()
				For i = 0 To dirs.Count - 1
					Properties.Remove(dirs(i))
				Next
				Properties.Add(value)
				RaiseEvent Updated()
			End Set
		End Property

		Private copy As VirtualHost

		Public Sub New()
			MyBase.New()
			Me.Params = "*:80"
		End Sub

		Public Sub BeginEdit() Implements IEditableObject.BeginEdit
			copy = Me.Clone()
		End Sub

		Public Sub EndEdit() Implements IEditableObject.EndEdit
			copy = Nothing
		End Sub

		Public Sub CancelEdit() Implements IEditableObject.CancelEdit
			Me.Params = copy.Params
			Me.Properties = copy.Properties
			copy = Nothing
		End Sub

		Public Shared ReadOnly Property DefaultVhost As VirtualHost
			Get
				Dim vhost As New VirtualHost()
				vhost.Properties.Add(New VirtualHostFileProperty("DocumentRoot", """C:\"""))
				vhost.Properties.Add(New VirtualHostFileProperty("ServerName", "mydomain.local"))
				Dim dir As New Directory()
				dir.Path = "C:\"
				dir.Properties.Add(New VirtualHostFileProperty("AllowOverride", "All"))
				dir.Properties.Add(New VirtualHostFileProperty("Order", "allow,deny"))
				dir.Properties.Add(New VirtualHostFileProperty("Allow", "from all"))
				vhost.Properties.Add(dir)
				Return vhost
			End Get
		End Property

	End Class

End Namespace
