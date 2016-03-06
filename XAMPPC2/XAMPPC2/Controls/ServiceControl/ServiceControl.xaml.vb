Class ServiceControl
	Private service As IService

	Public Property Thumbnail As String

	Public Property Thumb
		Get
			Dim b = New BitmapImage(New Uri("/XAMPPC2;component/" & Thumbnail, UriKind.Relative))
			Return b
		End Get
		Set(value)

		End Set
	End Property


	Public Sub New()

		' This call is required by the designer.
		InitializeComponent()
		Me.DataContext = Me
		' Add any initialization after the InitializeComponent() call.

	End Sub

	Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
		tgl.Switch()
	End Sub

	Private Sub tgl_Toogle(sender As Object, e As EventArgs)

	End Sub
End Class
