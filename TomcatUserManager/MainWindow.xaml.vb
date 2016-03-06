Imports System.ComponentModel
Imports System.IO

Class MainWindow
	Implements INotifyPropertyChanged

	Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

	''' <summary>
	''' Správce uživatelů tomcat
	''' </summary>
	Private manager As TomcatUserManager

	''' <summary>
	''' informace o tom, zdali byly data změněna a je je pře zavřením formuláře třeba uložit
	''' </summary>
	Private changed As Boolean = False

	''' <summary>
	''' Získá sprqavované položky
	''' </summary>
	Public ReadOnly Property TomcatItems
		Get
			' vyřadí komentáře
			Return (From x In manager.items Where x.GetType() <> GetType(Comment)).ToArray()
		End Get
	End Property

	''' <summary>
	''' Určuje zdali se zaktivuje odstraňvoací tlačítko
	''' nastavení libovolné hodnoty do proměnné provede aktualizaci stavu tlačítka
	''' </summary>
	Public Property EnableDelete As Boolean
		Get
			' pokud není vybrána žádná položka, tlačítko zůstane deaktivováno
			Return viewItems.SelectedItem IsNot Nothing
		End Get
		Set(value As Boolean)
			' data-binding
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EnableDelete"))
		End Set
	End Property

	''' <summary>
	''' Připraví okno
	''' </summary>
	Public Sub New()
		Dim args = Environment.GetCommandLineArgs()
		Try
			If args.Length >= 2 Then ' pokud je cesta v parametru spouštění
				manager = New TomcatUserManager(args(1))
			ElseIf File.Exists("tomcat\conf\tomcat-users.xml") Then ' pokud je soubor relativně od spouštění
				manager = New TomcatUserManager("tomcat\conf\tomcat-users.xml")
			ElseIf File.Exists("C:\xampp\tomcat\conf\tomcat-users.xml") Then ' pokud je soubor v defaultní cestě
				manager = New TomcatUserManager("C:\xampp\tomcat\conf\tomcat-users.xml")
			Else ' už mi došly nápaday, nechá si zadat cestu od uživatele
				manager = New TomcatUserManager(InputBox("Zadejte cestu k souboru tomcat-users.xml"))
			End If
		Catch ex As Exception ' cesta sice předána, ale nefunguje
			MessageBox.Show("Soubor tomcat-users.xml nebyl nalezen.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error)
		End Try
		' incializace data-binding
		Me.DataContext = Me
		' inicializace formuláře
		InitializeComponent()
	End Sub

	''' <summary>
	''' Otevře editor položky
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub ListView_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
		' označí, že došlo ke změně dat
		Me.changed = True
		' najde označenou položku
		Dim item As ITomcatAccessItem = viewItems.SelectedItem
		' deklarace okna
		Dim wnd As Window
		' podle typu vytvoří okno
		If item.GetType() = GetType(TomcatUser) Then
			wnd = New UserEditor(item)
		ElseIf item.GetType() = GetType(TomcatRole) Then
			wnd = New RoleEditor(item)
		End If
		' připraví obsluhu události pro zavření okna
		AddHandler wnd.Closed, AddressOf UpdateItems
		' otevře okno
		wnd.Show()
	End Sub

	''' <summary>
	''' Aktualizuje seznam položek
	''' </summary>
	Private Sub UpdateItems()
		' aktualizace data-binding
		RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TomcatItems"))
	End Sub

	''' <summary>
	''' Přidá nového uživatele a otevře okno editoru
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub AddUser_Click(sender As Object, e As RoutedEventArgs)
		' označí, že došlo ke změně
		Me.changed = True
		' vytvoří nového uživatele
		Dim u As New TomcatUser("", "", "")
		' předá uživatele správci
		manager.items.Add(u)
		' aktualizuje seznam položek
		UpdateItems()
		' otevře okno, připraví okno, zobrazí okno
		Dim wnd As New UserEditor(u)
		AddHandler wnd.Closed, AddressOf UpdateItems
		wnd.Show()
	End Sub

	''' <summary>
	''' Přidá novou roli a otevbře okno editoru
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub AddRole_Click(sender As Object, e As RoutedEventArgs)
		' označí že došlo ke změně
		Me.changed = True
		' vytvoří roli
		Dim r As New TomcatRole("")
		' předá novou roli správci
		manager.items.Add(r)
		' aktualizuje seznam položek
		UpdateItems()
		' otevře okno, připraví okno, zobrazí okno
		Dim wnd As New RoleEditor(r)
		AddHandler wnd.Closed, AddressOf UpdateItems
		wnd.Show()
	End Sub

	''' <summary>
	''' uživatel vybral jinou polžku
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub viewItems_Selected(sender As Object, e As RoutedEventArgs)
		' zaktualizuje stav talčítka odstranit položku
		EnableDelete = True
	End Sub

	''' <summary>
	''' Odstraní vybranou položku
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub Delete_Click(sender As Object, e As RoutedEventArgs)
		' ozančí že došlo ke změně
		Me.changed = True
		' odebere položku
		Me.manager.items.Remove(viewItems.SelectedItem)
		' zaktualizuje seznam
		UpdateItems()
		'zaktualizuje stav talčítka odstranit
		EnableDelete = True
	End Sub

	''' <summary>
	''' Uloží změny
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub Save_Click(sender As Object, e As RoutedEventArgs)
		' uloží změny
		manager.Save()
		' označí že nejsou žádné změny k uložení
		Me.changed = False
	End Sub

	''' <summary>
	''' Obslouží zavření okna
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub Window_Closing(sender As Object, e As CancelEventArgs)
		' pokud nejsou nějaké ulžoené změny =>
		' zorabzí dotaz o uložení změn před zavřením okna, pokud v něm uživatel klikne, že chce uložt změny =>
		' uloží před zavřením okna změny
		If Me.changed AndAlso
			MessageBox.Show(
				"Přejete si uložit provedené změny?",
				"Uložit změny",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning) = MessageBoxResult.Yes Then

			manager.Save()
		End If
	End Sub
End Class
