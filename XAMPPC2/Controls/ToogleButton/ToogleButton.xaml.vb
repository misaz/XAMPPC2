''' <summary>
''' Přepínací (kolébkové tlačítko)
''' </summary>
Public Class ToogleButton

	''' <summary>
	''' Událost volána při změně stavu stlačítka
	''' </summary>
	''' <param name="sender">Zdroj události</param>
	''' <param name="e">Parametry události</param>
	Public Event Toogle(sender As Object, e As EventArgs)

	''' <summary>
	''' Kliknutí na tlačítko, Přepne stav
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
		Me.Switch()
	End Sub

	''' <summary>
	''' Přepne stav tlačítka
	''' </summary>
	Public Sub Switch()
		' zneguzje stav tlačítka
		IsToogled = Not IsToogled
	End Sub

	''' <summary>
	''' Informace zdali je tlačítko ve stavu On
	''' </summary>
	''' <returns></returns>
	Public Property IsToogled As Boolean
		Get
			Return tooglerViewbox.HorizontalAlignment = HorizontalAlignment.Right
		End Get
		Set(value As Boolean)
			SetToogle(value)
			RaiseEvent Toogle(Me, New EventArgs())
		End Set
	End Property

	''' <summary>
	''' Nastavuje hodnotu tlačítka v GUI
	''' </summary>
	''' <param name="value">Hodnota tlačítka</param>
	Public Sub SetToogle(value As Boolean)
		' nastaví pozici On/Off
		If value Then
			tooglerViewbox.HorizontalAlignment = HorizontalAlignment.Right
		Else
			tooglerViewbox.HorizontalAlignment = HorizontalAlignment.Left
		End If
		' nastaví barvu
		Me.RefreshColors()
	End Sub

	''' <summary>
	''' Zaktualizuje barvu pozadí tlačítka
	''' </summary>
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
