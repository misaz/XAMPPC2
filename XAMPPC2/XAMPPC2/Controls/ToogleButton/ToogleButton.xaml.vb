Public Class ToogleButton

	Public Event Toogle(ByRef sender As Object, ByRef e As EventArgs)

	Public Sub New()

		' This call is required by the designer.
		InitializeComponent()
		' Add any initialization after the InitializeComponent() call.

	End Sub

	Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
		Me.Switch()
	End Sub

	Public Sub Switch()
		IsToogled = Not IsToogled
	End Sub

	Public Property IsToogled As Boolean
		Get
			Return tooglerViewbox.HorizontalAlignment = HorizontalAlignment.Right
		End Get
		Set(value As Boolean)
			If value Then
				tooglerViewbox.HorizontalAlignment = HorizontalAlignment.Right
			Else
				tooglerViewbox.HorizontalAlignment = HorizontalAlignment.Left
			End If
			Me.RefreshColors()
			RaiseEvent Toogle(Me, New EventArgs())
		End Set
	End Property

	Private Sub RefreshColors()
		If Me.IsToogled Then
			tooglerViewbox.HorizontalAlignment = HorizontalAlignment.Right
			tooglerBackground.Background = New SolidColorBrush(Color.FromArgb(255, 131, 255, 131))
			tooglerBorder.BorderBrush = New SolidColorBrush(Color.FromArgb(255, 0, 130, 0))
		Else
			tooglerViewbox.HorizontalAlignment = HorizontalAlignment.Left
			tooglerBackground.Background = New SolidColorBrush(Color.FromArgb(255, 255, 198, 198))
			tooglerBorder.BorderBrush = New SolidColorBrush(Color.FromArgb(255, 255, 0, 0))
		End If
	End Sub


End Class
