Imports System.ComponentModel

Public Class VirtualHostEditor
	Implements INotifyPropertyChanged
	Private vhost As VirtualHost.VirtualHost
	Private lastSaved As VirtualHost.VirtualHost

	Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

	Public Property OutputThumbnail As String
		Get
			Return vhost.ToString()
		End Get
		Set(value As String)

		End Set
	End Property


	Public Sub New(vhost As VirtualHost.VirtualHost)
		Me.vhost = vhost
		Me.lastSaved = vhost.Clone()
		Me.DataContext = vhost
		InitializeComponent()
		directoryConfig.Collection = vhost.Directory.Properties
		AddHandler directoryConfig.Update, AddressOf UpdateData
		outThumb.DataContext = Me
		AddHandler vhost.Updated, AddressOf UpdateData
	End Sub

	Private Sub UpdateData()
		RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("OutputThumbnail"))
	End Sub

	Private Sub AddDirectoryProperty_Click(sender As Object, e As RoutedEventArgs)
		vhost.Directory.Properties.Add(New VirtualHost.VirtualHostFileProperty())
	End Sub

	Private Sub Window_Closing(sender As Object, e As CancelEventArgs)
	End Sub

End Class
