Imports System.Drawing

Public Class ToolHyperlink

	Public Property Label As String
	Public Property Program As String


	Public Sub New()
		InitializeComponent()
		Me.DataContext = Me
	End Sub

	Public ReadOnly Property Thumbnail As ImageSource
		Get
			Dim source As ImageSource
			Dim icnH
			Dim ic = Icon.ExtractAssociatedIcon(Program)
			Try
				icnH = ic.Handle
			Catch ex As Exception
				icnH = SystemIcons.Application.Handle
			End Try
			source = Interop.Imaging.CreateBitmapSourceFromHIcon(icnH, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
			ic.Dispose()
			Return source
		End Get
	End Property

	Private Sub Hyperlink_RequestNavigate(sender As Object, e As RequestNavigateEventArgs)
		Try
			Process.Start(Me.Program)
		Catch ex As Exception

		End Try
		e.Handled = True
	End Sub
End Class
