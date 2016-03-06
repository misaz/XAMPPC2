Imports System.ComponentModel

Public Class MainWindow
	Implements INotifyPropertyChanged

	Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

	Private manager As VirtualHost.VirtualHostManager

	Public Property VirtualHostItems
		Get
			Return manager.GetVirtualHosts()
		End Get
		Set(value)

		End Set
	End Property

	Public Sub New()
		manager = New VirtualHost.VirtualHostManager(GetConfigFilePath())
		Me.DataContext = Me
		InitializeComponent()
	End Sub

	Private Function GetConfigFilePath() As String
		Dim path As String
		Dim args = Environment.GetCommandLineArgs()
		If args.Length > 1 Then
			path = args(1)
		ElseIf IO.File.Exists("apache\conf\extra\httpd-vhosts.conf") Then
			path = "apache\conf\extra\httpd-vhosts.conf"
		ElseIf IO.File.Exists("C:\xampp\apache\conf\extra\httpd-vhosts.conf") Then
			path = "C:\xampp\apache\conf\extra\httpd-vhosts.conf"
		Else
			path = InputBox("Cesta k httpd-vhosts.conf")
		End If
		Return path
	End Function

	Private Sub EditVirtualHost(sender As ListViewItem, e As EventArgs)
		EditVirtualHost(sender.Content)
	End Sub

	Private Sub EditVirtualHost(vhost As VirtualHost.VirtualHost)
		Dim wnd As New VirtualHostEditor(vhost)
		wnd.Show()
		Dim timer As New Threading.DispatcherTimer()
		timer.Interval = New TimeSpan(50)
		AddHandler timer.Tick, Sub()
								   timer.Stop()
								   wnd.Activate()
							   End Sub
		timer.Start()
	End Sub

	Private Sub AddNew_Click(sender As Object, e As RoutedEventArgs)
		Dim vhost As VirtualHost.VirtualHost = VirtualHost.VirtualHost.DefaultVhost
		manager.Items.Add(vhost)
		RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VirtualHostItems"))
		EditVirtualHost(vhost)
	End Sub

	Private Sub Delete_Click(sender As Object, e As RoutedEventArgs)
		Dim item As VirtualHost.VirtualHost = lstItems.SelectedItem
		If item IsNot Nothing Then
			item.DeleteHostEntry()
			Me.manager.Items.Remove(item)
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VirtualHostItems"))
		End If
	End Sub

	Private Sub Save_Click(sender As Object, e As RoutedEventArgs)
		manager.Save()
		restartWarning.Visibility = Visibility.Visible
	End Sub
End Class
