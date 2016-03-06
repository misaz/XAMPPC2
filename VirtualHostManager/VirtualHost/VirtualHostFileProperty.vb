Namespace VirtualHost

	Public Class VirtualHostFileProperty
		Implements IVirtualHostConfigurationFileItem

		Public Property PropertyName As String = ""
		Public Property Value As String = ""

		Public Sub New()

		End Sub

		Public Sub New(propname As String, value As String)
			Me.PropertyName = propname
			Me.Value = value
		End Sub

		Public Shared Function Parse(line As String) As VirtualHostFileProperty
			Dim output As New VirtualHostFileProperty()

			Dim parts = line.Trim().Split(" ").ToList()
			output.PropertyName = parts(0)
			parts.RemoveAt(0)
			output.Value = String.Join(" ", parts)


			Return output
		End Function

		Public Function Clone() As Object Implements IVirtualHostConfigurationFileItem.Clone
			Dim n = New VirtualHostFileProperty()
			n.PropertyName = Me.PropertyName.Clone()
			n.Value = Me.Value.Clone()
			Return n
		End Function

		Public Overrides Function ToString() As String
			Return PropertyName & " " & Value
		End Function

		Public Overloads Function ToString(tabs As Integer) As String Implements IVirtualHostConfigurationFileItem.ToString
			Return New String(vbTab, tabs) & Me.ToString()
		End Function
	End Class

End Namespace
