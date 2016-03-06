Imports System.Reflection
Imports System.Text.RegularExpressions

Namespace VirtualHost

	Public Class PropertyBlock
		Implements IVirtualHostConfigurationFileItem, ICloneable

		Public Property BlockName As String
		Public Property Params As String = ""
		Public Property Properties As New List(Of IVirtualHostConfigurationFileItem)

		Public Sub New()
			Me.BlockName = Me.GetType().Name
		End Sub

		Public Shared Function ParseBlockName(line As String) As String
			Dim definitionRegex = "<(?<blockname>[a-zA-Z]*) ?(?<params>.*)>"
			Dim r As New Regex(definitionRegex)
			Dim m = r.Match(line)
			If Not m.Success Then
				Throw New ArgumentException("No block starting just here.")
			End If

			If m.Groups("blockname").Value Is Nothing Then
				Throw New ArgumentException("No block staring just here")
			End If
			Return m.Groups("blockname").Value
		End Function

		Public Shared Function Parse(vhostContent As String) As PropertyBlock
			Dim output As New PropertyBlock()

			Dim lines As String() = vhostContent.Trim().Split(New String() {vbCrLf.ToString()}, StringSplitOptions.None)
			Dim firstLine = lines(0)

			Dim definitionRegex = "<(?<blockname>[a-zA-Z]*) ?(?<params>.*)>"
			Dim r As New Regex(definitionRegex)
			Dim m = r.Match(firstLine)
			If Not m.Success Then
				Throw New ArgumentException("No block starting just here.")
			End If

			Dim bn = m.Groups("blockname").Value

			If PropertyBlock.IsBlcokRegistered(bn) Then
				output = Activator.CreateInstance(PropertyBlock.GetRegisteredBlock(bn))
			End If

			output.BlockName = bn
			If m.Groups("params") IsNot Nothing Then
				output.Params = m.Groups("params").Value
			End If

			Dim line As String

			For i = 1 To lines.Length - 1

				line = lines(i).Trim()

				If line = "" Then
					output.Properties.Add(New EmptyLine())
				ElseIf line.StartsWith("#") Then
					output.Properties.Add(Comment.Parse(line))
				ElseIf line.StartsWith("</" & bn) Then
					Return output
				ElseIf line.StartsWith("<") Then
					i += 1
					Dim blockname As String = PropertyBlock.ParseBlockName(line)
					Dim content = line & vbCrLf
					While Not lines(i).Trim().StartsWith("</" & blockname)
						content &= lines(i).Trim() & vbCrLf
						i += 1
					End While
					content &= lines(i).Trim() & vbCrLf
					output.Properties.Add(PropertyBlock.Parse(content))
				Else
					output.Properties.Add(VirtualHostFileProperty.Parse(line))
				End If
			Next

			Return output
		End Function

		Private Shared Function IsBlcokRegistered(blockName As String) As Boolean
			Dim q = From t In RegisteredBlocks()
					Where t.Name = blockName

			Return q.Any()
		End Function

		Private Shared Function RegisteredBlocks() As List(Of Type)
			Dim q = From t In Assembly.GetExecutingAssembly().GetTypes()
					Where t.BaseType = GetType(PropertyBlock)
			Return q.ToList()
		End Function

		Private Shared Function GetRegisteredBlock(bn As String) As Type
			Dim q = From b In RegisteredBlocks()
					Where b.Name = bn

			If q.Any() Then
				Return q.First()
			Else
				Return Nothing
			End If
		End Function

		Public Overrides Function ToString() As String
			Return Me.ToString(0)
		End Function

		Public Overloads Function ToString(tabs As Integer) As String Implements IVirtualHostConfigurationFileItem.ToString
			If Me.Properties.Count = 0 Then
				Return ""
			End If
			Dim t As String
			If tabs > -1 Then
				t = New String(vbTab, tabs)
			Else
				t = ""
			End If
			Dim props = (From p In Properties
						 Select (t & p.ToString(tabs + 1))).ToArray()

			Dim params As String = Me.GetParams()
			If params.Trim() <> "" Then
				params = " " & params
			End If
			Return String.Format("{3}<{0}{4}>{1}{2}{1}{3}</{0}>", BlockName, vbCrLf, String.Join(vbCrLf, props), t, params)

		End Function

		Private Function GetParams() As String
			Return Me.Params
		End Function

		Public Function GetProperty(propname As String) As String
			Dim q = (From i In Properties
					 Where i.GetType() = GetType(VirtualHostFileProperty) AndAlso CType(i, VirtualHostFileProperty).PropertyName = propname)

			If q.Any() Then
				Return CType(q.First(), VirtualHostFileProperty).Value
			Else
				Return Nothing
			End If
		End Function

		Public Sub SetProperty(propname As String, value As String)
			Dim q = (From i In Properties
					 Where i.GetType() = GetType(VirtualHostFileProperty) AndAlso CType(i, VirtualHostFileProperty).PropertyName = propname)


			If q.Any() Then
				Dim x As VirtualHostFileProperty = q.First()
				x.Value = value
			Else
				Dim newP As New VirtualHostFileProperty()
				newP.PropertyName = propname
				newP.Value = value
				Me.Properties.Add(newP)
			End If
		End Sub

		Public Function Clone() As Object Implements ICloneable.Clone, IVirtualHostConfigurationFileItem.Clone
			Dim type As Type = Me.GetType()
			Dim output As PropertyBlock = Activator.CreateInstance(type)
			output.Params = Me.Params.Clone()
			output.BlockName = Me.BlockName.Clone()

			For Each i In Me.Properties
				output.Properties.Add(i.Clone())
			Next
			Return output
		End Function

		Public Shared Operator =(c1 As PropertyBlock, c2 As PropertyBlock)
			If c1.Params <> c2.Params Then
				Return False
			End If
			For i = 0 To c1.Properties.Count - 1
				If Not (c1.Properties(i) Is c2.Properties(i)) Then
					Return False
				End If
			Next
			Return True
		End Operator

		Public Shared Operator <>(c1 As PropertyBlock, c2 As PropertyBlock)
			Return Not c1 = c2
		End Operator

	End Class
End Namespace
