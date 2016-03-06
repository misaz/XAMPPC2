Namespace VirtualHost

	Public Class EmptyLine
		Implements IVirtualHostConfigurationFileItem

		Public Function Clone() As Object Implements IVirtualHostConfigurationFileItem.Clone
			Return New EmptyLine()
		End Function

		Public Overrides Function ToString() As String
			Return ""
		End Function

		Public Overloads Function ToString(tabs As Integer) As String Implements IVirtualHostConfigurationFileItem.ToString
			Return Me.ToString()
		End Function

		Public Shared Operator =(c1 As EmptyLine, c2 As EmptyLine)
			Return True
		End Operator

		Public Shared Operator <>(c1 As EmptyLine, c2 As EmptyLine)
			Return False
		End Operator
	End Class

End Namespace
