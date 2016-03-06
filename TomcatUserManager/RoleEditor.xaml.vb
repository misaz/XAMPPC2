Imports System.ComponentModel

''' <summary>
''' Editor role tomcatu
''' </summary>
Public Class RoleEditor
	''' <summary>
	''' Upravována role
	''' </summary>
	Public Property Role As TomcatRole

	''' <summary>
	''' Naposledy uložená role, podle te se rozhoduje zdali došlo ke změnám
	''' </summary>
	Private Property SavedRole As TomcatRole

	Public Sub New(r As TomcatRole)
		' incializuje roli
		Role = r
		' incializuje naposledy uloženou roli
		Save_Click(Nothing, Nothing)
		' inicializuje data-binding
		Me.DataContext = Me
		' incializuje formulář
		InitializeComponent()
	End Sub

	''' <summary>
	''' Uloží roli
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub Save_Click(sender As Object, e As RoutedEventArgs)
		' naklonuje upravovanou roli do uložené
		Me.SavedRole = Role.Clone()
	End Sub

	''' <summary>
	''' Zkontroluje změny
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub Window_Closing(sender As Object, e As CancelEventArgs)
		' pokud došlo k změnám (využívá přetížený operátor), zeptá se na uložení
		If Role <> SavedRole AndAlso
			MessageBox.Show(
				"Chcete uložit provedené změny?",
				"Provedené změny",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning) = MessageBoxResult.Yes Then

			Save_Click(Nothing, Nothing)
		Else ' pokud ne, aplikuje změny do role
			Role.Name = SavedRole.Name
		End If
	End Sub
End Class
