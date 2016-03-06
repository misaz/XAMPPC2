Imports System.IO
Namespace Hosts

	Public Class HostsManager

		Private Property Items As New List(Of IHostsFileItem)
		Private Const HostsFilePath As String = "C:\Windows\System32\drivers\etc\hosts"

		Public Sub New()
			Me.Load()
		End Sub

		Public Sub Load()
			Items.Clear()
			Using sr As New StreamReader(HostsFilePath)
				While Not sr.EndOfStream
					Dim line = sr.ReadLine().Trim()
					If line = "" Then
						Items.Add(New EmptyLine())
					ElseIf line.StartsWith("#") Then
						Items.Add(Comment.Parse(line))
					Else
						Items.Add(HostsEntry.Parse(line))
					End If
				End While
			End Using
		End Sub

		Public Sub Save()
			Using sw As New StreamWriter(HostsFilePath)
				For Each i In Items
					sw.WriteLine(i.ToString())
				Next
			End Using
		End Sub

		Public Function IsDomainSetToLocalhost(domain As String)
			Dim q = From x In GetHostsEntries()
					Where x.Domain = domain

			If q.Any() Then
				Return CType(q.First(), HostsEntry).IP = "127.0.0.1"
			Else
				Return False
			End If
		End Function

		Private Function GetHostsEntries() As List(Of HostsEntry)
			Dim q = From x In Items
					Where x.GetType() = GetType(HostsEntry)
					Select CType(x, HostsEntry)

			Return q.ToList()
		End Function

		Public Function GetOrCreateEntry(domain As String) As HostsEntry
			Dim q = From x In GetHostsEntries()
					Where x.Domain = domain

			If q.Any() Then
				Return q.First()
			Else
				Dim e As New HostsEntry()
				e.Domain = domain
				e.IP = "0.0.0.0"
				Items.Add(e)
				Return e
			End If
		End Function

		Friend Sub AddEntry(entry As HostsEntry)
			Me.Items.Add(entry)
		End Sub

		Friend Sub RemoveEntry(entry As HostsEntry)
			Me.Items.Remove(entry)
		End Sub
	End Class
End Namespace
