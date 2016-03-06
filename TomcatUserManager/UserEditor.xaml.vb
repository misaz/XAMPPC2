Imports System.ComponentModel

''' <summary>
''' Editor uživatele tomcat
''' </summary>
Public Class UserEditor

	''' <summary>
	''' upravovaný uživatel
	''' </summary>
	Public Property User As TomcatUser

	''' <summary>
	''' Naposledy uložený uživatel
	''' </summary>
	''' <returns></returns>
	Private Property SavedUser As TomcatUser

	''' <summary>
	''' Vytvoří editor uživatele
	''' </summary>
	''' <param name="u">upravovaný uživatel</param>
	Public Sub New(u As TomcatUser)
		' inicializuje uživatele
		Me.User = u
		' incicializuje naposledy uloženého uživatele
		Save_Click(Nothing, Nothing)
		' inicializuje data-binding
		Me.DataContext = Me
		' inicializuje formulář
		InitializeComponent()
	End Sub

	''' <summary>
	''' Uloží uživatele
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub Save_Click(sender As Object, e As RoutedEventArgs)
		' naklonuje uživatele do kopie
		Me.SavedUser = User.Clone()
	End Sub

	''' <summary>
	''' Obsluha události zavření okna
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	Private Sub Window_Closing(sender As Object, e As CancelEventArgs)
		' pokud je uživatel neuložený => vovalá dialogové okno, zdali si přeje uložit změny
		' pokud klikne na jo, před zavřením uloží změny
		If User <> SavedUser AndAlso
			MessageBox.Show(
				"Chcete uložit provedené změny?",
				"Provedené změny",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning) = MessageBoxResult.Yes Then

			Save_Click(Nothing, Nothing)
		Else
			' zahodí změny (=přepíše úpravy z naposled uložené kopie)
			User.Name = SavedUser.Name
			User.Password = SavedUser.Password
			User.Role = SavedUser.Role
		End If
	End Sub
End Class
