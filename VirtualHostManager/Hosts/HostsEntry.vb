Namespace Hosts

	Public Class HostsEntry
		Implements IHostsFileItem

		Public Property IP As String
		Public Property Domain As String

		Public Shared Function Parse(line As String) As HostsEntry
			Dim output = New HostsEntry()
			Dim parts = line.Split(" ")
			output.IP = parts(0)
			output.Domain = parts(1)
			Return output
		End Function

		Public Overrides Function ToString() As String
			Return Me.IP & " " & Me.Domain
		End Function
	End Class

End Namespace
