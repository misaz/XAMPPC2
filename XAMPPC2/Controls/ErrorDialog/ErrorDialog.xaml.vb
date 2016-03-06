Imports System.ComponentModel
Imports System.Drawing

''' <summary>
''' Okno upozorňující na vzniklou chybu
''' </summary>
Public Class ErrorDialog
	Implements INotifyPropertyChanged

	Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

	''' <summary>
	''' Jméno chybujícího procesu
	''' </summary>
	Public Property ProcessName As String

	''' <summary>
	''' Návratový kód procesu
	''' </summary>
	Public Property ResponseCode As Integer

	''' <summary>
	''' Vzniklé chyby
	''' </summary>
	Public Property Errors As New List(Of ProcessError)

	''' <summary>
	''' Obrázek Procesu
	''' </summary>
	Public Property Image As ImageSource

	Private _restartprocess As Action
	''' <summary>
	''' Akce, která se zavolá, když uživatel vyžádá restart chybujícího serveru
	''' </summary>
	''' <returns></returns>
	Public Property RestartProcess As Action
		Get
			Return _restartprocess
		End Get
		Set(value As Action)
			' Nastaví hodnotu
			_restartprocess = value

			' Aktualizuje data-binding
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DisplayRestartProcess"))
		End Set
	End Property

	''' <summary>
	''' Nastavuje zdali se zobrazí tlačítko pro zobrazení procesu
	''' </summary>
	Public ReadOnly Property DisplayRestartProcess As Visibility
		Get
			If RestartProcess IsNot Nothing Then
				Return Visibility.Visible
			Else
				Return Visibility.Hidden
			End If
		End Get
	End Property

	''' <summary>
	''' Incializuje nové okno
	''' </summary>
	''' <param name="processName">jméno havarovaného procesu</param>
	''' <param name="process">havarovaný proces (může být Nothing)</param>
	''' <param name="errors">seznam navrácených chyb</param>
	''' <param name="image">ikona serveru</param>
	Public Sub New(processName As String, process As Process, errors As List(Of String), image As ImageSource)
		' příprava data-binding
		Me.DataContext = Me

		' inicializace
		Me.Image = image
		Me.ProcessName = processName

		' Zpracování návratového kódu
		If process IsNot Nothing Then
			ResponseCode = process.ExitCode
		Else
			ResponseCode = 0
		End If

		' zpracování grafické reprezentace chyb
		For Each e As String In errors
			Me.Errors.Add(New ProcessError(e))
		Next

		' incializace GUI
		InitializeComponent()

		' Načtení ikony okna (chybový červený křížek; systémová ikona)
		Dim ico = Interop.Imaging.CreateBitmapSourceFromHBitmap(
			SystemIcons.Error.ToBitmap().GetHbitmap(),
			IntPtr.Zero,
			Int32Rect.Empty,
			BitmapSizeOptions.FromEmptyOptions())

		' vložení systémové ikony do GUI
		ErrorImage.Source = ico

		' titulek okna
		Me.Title = "Chyba " & processName
	End Sub

	''' <summary>
	''' Inicializuje nové okno, chyby rozparsuje ze System.String
	''' </summary>
	''' <param name="processName">jméno havarovaného programu</param>
	''' <param name="process">hovarovaný proces</param>
	''' <param name="output">výstup havarovaného procesu</param>
	''' <param name="image">ikona serveru</param>
	Public Sub New(processName As String, process As Process, output As String, image As ImageSource)
		Me.New(
			processName,
			process,
			output.Split(New String() {vbCr, vbLf}, StringSplitOptions.RemoveEmptyEntries).ToList(),
			image)
	End Sub

	''' <summary>
	''' Obsluha události načtení formuláře, pokud neobsahuje žádné chyby okamžitě jej zavře.
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub ErrorDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
		If Me.Errors.Count = 0 Then
			Me.Close()
		End If
	End Sub

	''' <summary>
	''' Oblusha události kliknutí na tlačítko Google, otevře webový prohlížeč s hledáním chyby na http://google.com
	''' </summary>
	''' <param name="sender">zdroj události</param>
	''' <param name="e">Reserved</param>
	Private Sub Google_Click(sender As Object, e As RoutedEventArgs)
		' příprava URL dotazu
		Dim query As String = sender.Tag
		query = query.Replace(Xampp.Config.path, "")
		query = query.Replace(Xampp.Config.path.Replace("\", "/"), "")
		Dim url = String.Format("http://google.com/#q={0}", query)

		' zavolání otevření odkazu (systém vybere výchozí prohlížeč)
		Process.Start(url)
	End Sub

	''' <summary>
	''' Obsluha kliknutí potvrzovacího tlačítka
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub OK_Click(sender As Object, e As RoutedEventArgs)
		Me.Close()
	End Sub

	''' <summary>
	''' Vyvolá restart procesu
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub RestartProcess_Click(sender As Object, e As RoutedEventArgs)
		' vyvolá restart
		Me.RestartProcess.Invoke()

		' zavře okno
		Me.Close()
	End Sub
End Class
