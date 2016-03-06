
Imports System.Threading
''' <summary>
''' Dialogové okno reprezuntující běh programu, sledovaný proces si okno spustí samo
''' </summary>
Public Class ProgressDialog

	Private _image As BitmapImage
	''' <summary>
	''' Ikona programu
	''' </summary>
	''' <returns></returns>
	Public Property Image
		Get
			Return _image
		End Get
		Set(value)
			_image = value
		End Set
	End Property

	Private _label As String
	''' <summary>
	''' Popisek probíhající akce
	''' </summary>
	''' <returns></returns>
	Public Property Label As String
		Get
			Return _label
		End Get
		Set(value As String)
			_label = value
		End Set
	End Property

	''' <summary>
	''' Sledovaný proces
	''' </summary>
	Private proc As Process

	''' <summary>
	''' Inicializuje okno
	''' </summary>
	''' <param name="label">Popsiek probíhající akce</param>
	''' <param name="image">Ikona běžícího programu</param>
	''' <param name="process">Sledovaný proces</param>
	Public Sub New(label As String, image As BitmapImage, process As Process)
		' inicializace
		Me.proc = process
		Me.Label = label
		Me.Image = image

		' data-binding
		Me.DataContext = Me

		' incializace formuláře
		InitializeComponent()
	End Sub

	''' <summary>
	''' spustí sledovaný proces, čeká na jeho ukončení, zajistí aby okno graficky jenom neprobliklo
	''' </summary>
	Private Sub handleProcess()
		' nastaví příznaky spuštění procesu
		proc.StartInfo.UseShellExecute = False
		proc.StartInfo.CreateNoWindow = True
		proc.StartInfo.Verb = "runas"
		Try
			' zkusí spustit proces
			proc.Start()
		Catch ex As Exception
			MessageBox.Show("Proces nelze spustit", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error)
		End Try
		' počká na jeho ukončení
		proc.WaitForExit()
		' zajistí aby okno neprobliklo
		' todo čekat pouze pokud process skončil brzo
		Thread.Sleep(1000)
		' ve vlákně kde běží obsluhy GUI zavře okno
		Me.Dispatcher.Invoke(
			New Action(
				Sub()
					' zavře okno
					Me.Close()
				End Sub)
			)
	End Sub

	''' <summary>
	''' Obsluha načtení okna, nastaví oknu WinAPI flagy, spustí sledovaný proces
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
		' Skryje systémová tlačítka
		WpfWindowTools.WindowHeaderTools.HideCloseButton(Me)
		' ošetří volání procesu
		Dim t As New Task(New Action(AddressOf handleProcess))
		' spustí, tak aby neblokovalo vlákno kde běží obsluha GUI
		t.Start()
	End Sub
End Class
