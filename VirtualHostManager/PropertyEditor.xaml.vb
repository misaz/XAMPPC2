Public Class PropertyEditor

	Private owner As PropertyBlockEditor

	Public Event Update()

	Public Sub New(item As VirtualHost.VirtualHostFileProperty, owner As PropertyBlockEditor)
		Me.DataContext = item
		Me.owner = owner
		InitializeComponent()
	End Sub

	Private Sub Delete_Click(sender As Object, e As RoutedEventArgs)
		owner.Remove(Me.DataContext, Me)
	End Sub

	Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
		RaiseEvent Update()
	End Sub
End Class
