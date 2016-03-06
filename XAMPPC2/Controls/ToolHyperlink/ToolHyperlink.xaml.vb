Imports System.Drawing
Imports System.IO

''' <summary>
''' Hypertextový odkaz
''' </summary>
Public Class ToolHyperlink

	''' <summary>
	''' Popisek odkazu
	''' </summary>
	Public Property Label As String

	''' <summary>
	''' Cestak k programu na který odkaz odkazuje
	''' </summary>
	''' <returns></returns>
	Public Property Program As String

	Public Sub New()
		' inicializace GUI
		InitializeComponent()

		' data-binding
		Me.DataContext = Me
	End Sub

	''' <summary>
	''' Ikona programu, ikona se načítá dynamicky z exe apikace na kterou odkaz odkazuje
	''' </summary>
	Public ReadOnly Property Thumbnail As ImageSource
		Get
			' výstup
			Dim source As ImageSource

			' WinAPI odkaz na ikonu
			Dim icnH As IntPtr

			' GDI ikona
			Dim ic As Icon = Nothing
			Try
				' zkusí zís´kaz ikonu z exe
				ic = Icon.ExtractAssociatedIcon(Program)

				' nastaví WinAPI odkaz
				icnH = ic.Handle
			Catch ex As Exception
				' pokud dojde k chybě (třeba nesmyslná cesta) nastaví ikonu výchozí aplikace
				icnH = SystemIcons.Application.Handle
			Finally
				' získá WPF strávitelnější bitmnapu
				source = Interop.Imaging.CreateBitmapSourceFromHIcon(icnH, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())

				' vyčistí paměť
				If Not ic Is Nothing Then
					ic.Dispose()
				End If
			End Try

			Return source
		End Get
	End Property

	''' <summary>
	''' Obsluha události požadavku o spuštění odkazovaného programu
	''' </summary>
	''' <param name="sender">Zdroj události</param>
	''' <param name="e">Objekt události</param>
	Private Sub Hyperlink_RequestNavigate(sender As Object, e As RequestNavigateEventArgs)
		Try
			Process.Start(Path.Combine(Directory.GetCurrentDirectory(), Me.Program))
		Catch ex As Exception
			MessageBox.Show(
				"Program nebyl nalezen. Opravte instalaci ovládacího panelu XAMPP.",
				"Chyba",
				MessageBoxButton.OK,
				MessageBoxImage.Error)
		End Try
		e.Handled = True
	End Sub
End Class
