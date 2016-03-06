Namespace VirtualHost

	Public Class Comment
		Implements IVirtualHostConfigurationFileItem

		Public Property Content As String

		Public Overrides Function ToString() As String
			Return String.Format("# {0}", Me.Content)
		End Function

		Public Shared Function Parse(line As String) As Comment
			Return New Comment() With {.Content = line.Trim().Substring(1).Trim()}
		End Function

		Public Overloads Function ToString(tabs As Integer) As String Implements IVirtualHostConfigurationFileItem.ToString
			Return New String(vbTab, tabs) & String.Format("# {0}", Content)
		End Function

		Public Function Clone() As Object Implements IVirtualHostConfigurationFileItem.Clone
			Dim n As New Comment()
			n.Content = Me.Content.Clone()
			Return n
		End Function

		Public Shared Operator =(c1 As Comment, c2 As Comment)
			Return c1.Content = c2.Content
		End Operator

		Public Shared Operator <>(c1 As Comment, c2 As Comment)
			Return Not c1 = c2
		End Operator
	End Class

End Namespace
