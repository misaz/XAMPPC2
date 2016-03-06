Namespace VirtualHost

	Public Class Directory
		Inherits PropertyBlock

		Public Property Path As String
			Get
				Dim param = Me.Params
				If param.StartsWith("""") Then
					param = param.Substring(1)
				End If
				If param.EndsWith("""") Then
					param = param.Substring(0, param.Length - 1)
				End If
				Return param
			End Get

			Set(value As String)
				Dim val = value
				If Not val.StartsWith("""") Then
					val = """" & val
				End If
				If Not val.EndsWith("""") Then
					val = val & """"
				End If
				Me.Params = val
			End Set
		End Property

	End Class

End Namespace
