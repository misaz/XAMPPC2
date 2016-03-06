Namespace Hosts

	Public Class Comment
		Implements IHostsFileItem

		Public Property Content As String

		Public Shared Function Parse(line As String) As Comment
			Dim output = New Comment()

			output.Content = line.Trim().Substring(1).Trim()

			Return output
		End Function

		Public Overrides Function ToString() As String
			Return "# " & Content
		End Function
	End Class

End Namespace
