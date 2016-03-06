Imports System.IO

Namespace VirtualHost

	Public Class VirtualHostManager

		Private block As PropertyBlock
		Private fileName As String

		Public Property Items As List(Of IVirtualHostConfigurationFileItem)
			Get
				Return block.Properties
			End Get
			Set(value As List(Of IVirtualHostConfigurationFileItem))
				block.Properties = value
			End Set
		End Property

		Public Function GetVirtualHosts() As List(Of VirtualHost)
			Dim q = From i In Items
					Where i.GetType() = GetType(VirtualHost)
					Select CType(i, VirtualHost)

			Return q.ToList()
		End Function


		Public Sub New(fileName As String)
			Me.ParseFile(fileName)
		End Sub

		Public Sub ParseFile(fileName As String)
			Me.fileName = fileName
			Dim content = "<ABC>" & vbCrLf
			Using sr As New StreamReader(fileName)
				content &= sr.ReadToEnd()
			End Using
			content &= "</ABC>"

			block = PropertyBlock.Parse(content)
		End Sub

		Public Overrides Function ToString() As String
			Dim s = block.ToString(-1)
			Return s.Substring("<ABC>".Length, s.Length - "<ABC></ABC>".Length).Trim()
		End Function

		Public Sub Save()
			For Each vhost In GetVirtualHosts()
				vhost.UpdatehostEntry()
			Next
			Using sw As New StreamWriter(fileName)
				sw.Write(Me.ToString())
			End Using
			Hosts.LocalInstance.HostsManager.Save()
		End Sub
	End Class

End Namespace