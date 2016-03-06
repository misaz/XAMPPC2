Imports System.ComponentModel
Imports System.Windows.Threading

Class MainWindow
	Implements INotifyPropertyChanged
	''' <summary>
	''' Výstup z knihovny Netstat
	''' </summary>
	Property NetstatReponse As NetstatHandle()

	Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

	''' <summary>
	''' Časovač pro aktualizaci seznamu
	''' </summary>
	Private WithEvents timer As New DispatcherTimer()

	''' <summary>
	''' Netstat
	''' </summary>
	Private netstat As New Netstat()

	Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
		' inicializace DataBinding
		Me.DataContext = Me
		' nastavení intervalu
		timer.Interval = New TimeSpan(0, 0, 0, 0, 750)
		' spuštění intervalu
		timer.Start()
		' okamžitě procede aktualizaci dat (aby se nečekalo na první tick, timeru)
		RefreshNetstat()
	End Sub

	''' <summary>
	''' Aktualizuje netstat informace
	''' </summary>
	Private Sub RefreshNetstat()
		' získá informace netstat a seřadí je pdole portu
		NetstatReponse = (From x In netstat.GetListeningProceses() Order By Integer.Parse(x.Port)).ToArray()
		' aktualizace data-binding
		RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NetstatReponse"))
		' spustí časovač
		timer.Start()
	End Sub

	''' <summary>
	''' Spustí aktualizaci informací netstatu v novém vlákně
	''' </summary>
	Private Sub RefreshNetstatInOtherThread() Handles timer.Tick
		' zastaví časovač, aby 
		timer.Stop()
		' vytvoří nové vlákno ve kterém spustí úlohu
		Dim t As New Threading.Thread(AddressOf RefreshNetstat)
		' spustí vlákno
		t.Start()
	End Sub

	''' <summary>
	''' Obsluha události načtení okna
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub Window_SourceInitialized(sender As Object, e As EventArgs)
		' skryje ikonu oka
		WpfWindowTools.WindowHeaderTools.HideWindowIcon(Me)
	End Sub
End Class
