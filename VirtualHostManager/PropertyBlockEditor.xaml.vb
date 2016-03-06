Imports VirtualHostManager

Public Class PropertyBlockEditor

	Private _collection As List(Of IVirtualHostConfigurationFileItem)
	Public Property Collection As List(Of IVirtualHostConfigurationFileItem)
		Get
			Return _collection
		End Get
		Set(value As List(Of IVirtualHostConfigurationFileItem))
			_collection = value
			lstProps.Items.Clear()
			For Each item In _collection
				Dim control = New PropertyEditor(item, Me)
				AddHandler control.Update, AddressOf RaiseUpdate
				lstProps.Items.Add(control)
			Next
		End Set
	End Property

	Private Sub RaiseUpdate()
		RaiseEvent Update()
	End Sub

	Public Event Update()

	Public Sub New()
		InitializeComponent()
	End Sub

	Public Sub Remove(prop As VirtualHost.VirtualHostFileProperty, editor As PropertyEditor)
		_collection.Remove(prop)
		lstProps.Items.Remove(editor)
		RaiseUpdate()
	End Sub

	Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
		Dim prop As New VirtualHost.VirtualHostFileProperty()
		Dim editor As New PropertyEditor(prop, Me)
		AddHandler editor.Update, AddressOf RaiseUpdate
		_collection.Add(prop)
		lstProps.Items.Add(editor)
		RaiseUpdate()
	End Sub
End Class
