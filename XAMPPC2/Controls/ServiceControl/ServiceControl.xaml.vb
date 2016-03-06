Imports System.ComponentModel
Imports System.IO
Imports System.Resources
Imports System.Threading
Imports System.Xml

''' <summary>
''' Ovládací tlačítko serveru
''' </summary>
Class ServiceControl
	Implements INotifyPropertyChanged
	Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

	''' <summary>
	''' Cesta k ikoně serveru
	''' </summary>
	Public Property Thumbnail As String

	Private _source As String
	''' <summary>
	''' Zdroj dat kontextové nabídky
	''' </summary>
	Public Property Source As String
		Get
			Return _source
		End Get
		Set(value As String)
			_source = value
			' přípraví kontextovou nabídku
			LoadServerContextMenu()
		End Set
	End Property

	''' <summary>
	''' Ikona serveru, nastavuje se přes vlastnost Thumbnail
	''' </summary>
	Public ReadOnly Property Thumb
		Get
			Return New BitmapImage(New Uri("/XAMPPC2;component/" & Thumbnail, UriKind.Relative))
		End Get
	End Property

	''' <summary>
	''' Vrací pravdivostní hodnotu podle toho zda jde server instalovat jako službu Windows
	''' </summary>
	Public ReadOnly Property VisibleServiceTool As Boolean
		Get
			Return Not IsNothing(Me.Service) AndAlso Me.Service.CanStartAsWindowsService()
		End Get
	End Property

	''' <summary>
	''' Vrací pravdivostní hodnotu zdali je server v složce XAMPP fyzicky nainstalovaný
	''' </summary>
	Public ReadOnly Property IsServiceRegisteredBool As Boolean
		Get
			Return Not IsNothing(Service) AndAlso Service.IsInstalled()
		End Get
	End Property

	''' <summary>
	''' Vrací Visiblity.Visible nebo Hidden podle toho zdali je server v složce XAMPP fyzicky nainstalovaný
	''' </summary>
	Public ReadOnly Property IsServiceRegistered As Visibility
		Get
			If IsServiceRegisteredBool Then
				Return Visibility.Visible
			Else
				Return Visibility.Hidden
			End If
		End Get
	End Property

	''' <summary>
	''' ID procesů serveru v textové podobě
	''' </summary>
	Public Property Pids As String = ""

	''' <summary>
	''' Porty na kterých server poslouchá v textové podbě
	''' </summary>
	Public Property Ports As String = ""

	Private _showPortsAndPidsTooltip As Visibility = Visibility.Hidden
	''' <summary>
	''' Vrací Visibility.Visible nebo Hidden podle toho, zdali server běží
	''' </summary>
	''' <returns></returns>
	Public Property ShowPortsAndPidsTooltip As Visibility
		Get
			Return _showPortsAndPidsTooltip
		End Get
		Set(value As Visibility)
			' nastaví hodnotu
			_showPortsAndPidsTooltip = value

			' pokud mají být zobrazeny a nejosu dostupné, vyžádá jejich okamžité načtení (obvykle probíhá v jiném vlákně)
			If value = Visibility.Visible And (Ports = "" Or Pids = "") Then
				CheckPortAndPid()
			End If

			' aktualizace data-binding
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ShowPortsAndPidsTooltip"))
		End Set
	End Property

	Private _service As IService
	''' <summary>
	''' Obsluhovaná služba
	''' </summary>
	Public Property Service As IService
		Get
			Return _service
		End Get
		Set(value As IService)
			' uloží hodnotu
			_service = value

			' registuje obsluhu události k změně stavu serveru
			AddHandler _service.StateChanged, AddressOf ProcessStateChange

			' aktualizace data-binding
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VisibleServiceToold"))
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsServiceInstalled"))
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsServiceRegistered"))
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsServiceRegisteredBool"))

			' vyžádá okamžité zpracování změn (obvykle probíhá v jiném vlákně)
			ProcessStateChange()
		End Set
	End Property

	''' <summary>
	''' Vrací pravdivostní hodnotu podle toho zdali je server nainstalován jako služba Windows
	''' </summary>
	Public Property IsServiceInstalled As Boolean
		Get
			If Not IsNothing(Me.Service) Then
				Return Me.Service.IsServiceInstalled() ' zpropaguje to co si o tom "myslí" samotná služba služba
			Else
				Return False ' pokud služba není načtená => False
			End If
		End Get
		Set(value As Boolean)

		End Set
	End Property

	Private _administrationLink As String
	''' <summary>
	''' Cesta/URL k administraci serveru
	''' </summary>
	''' <returns></returns>
	Public Property AdministrationLink As String
		Get
			Return _administrationLink
		End Get
		Set(ByVal value As String)
			' uloží hodnotu
			_administrationLink = value

			' aktualizace data-binding
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("AdministrationLink"))
		End Set
	End Property

	Private _DisplayAdministrationLink As Boolean
	''' <summary>
	''' Vrací pravdivostní hodnotu, zdali se má zobrazovat tlačítko adminsitrace serveru
	''' </summary>
	''' <returns></returns>
	Public Property DisplayAdministrationLink As Boolean
		Get
			Return _DisplayAdministrationLink
		End Get
		Set(ByVal value As Boolean)
			' naastaví hodnotu
			_DisplayAdministrationLink = value

			' aktualizace data-binding
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DisplayAdministrationLink"))
		End Set
	End Property

	''' <summary>
	''' Vytvoří novou instanci prvku
	''' </summary>
	Public Sub New()
		' inicializuje komponenty
		InitializeComponent()

		'data binding
		Me.DataContext = Me
	End Sub

	''' <summary>
	''' Načte kontextovou nabídku možností serveru
	''' </summary>
	''' <remarks>kontextové menu je součástí Resources formuláře</remarks>
	Private Sub LoadServerContextMenu()
		Dim ctxm As ContextMenu = Me.Resources("itemMenu")
		' Načte XML
		Dim xmld As XmlDocument = New XmlDocument()
		' Zdroj je v Resources
		xmld.LoadXml(My.Resources.ResourceManager.GetObject(Source))

		' připraví seznamy XML elementů
		Dim rawConfigs = xmld.DocumentElement.GetElementsByTagName("Config")
		Dim rawLogs = xmld.DocumentElement.GetElementsByTagName("Log")
		Dim rawWebAdmins = xmld.DocumentElement.GetElementsByTagName("WebApp")

		'načte konfigurace
		Dim configs As New List(Of Config)
		For Each c In rawConfigs
			configs.Add(Config.DeserializeObject(c))
		Next

		' načte logy
		Dim logs As New List(Of Log)
		For Each l In rawLogs
			logs.Add(Log.DeserializeObject(l))
		Next

		' načte administrace
		Dim webAdmins As New List(Of WebApp)
		For Each wa In rawWebAdmins
			webAdmins.Add(WebApp.DeserializeObject(wa))
		Next

		' zpracuje konfigurace
		For Each c In configs
			' tvorba položky menu
			Dim menuitem As New MenuItem()
			menuitem.Header = c.Title
			menuitem.Tag = c.Source

			' nastavení obsluh událostí
			AddHandler menuitem.Click, AddressOf MenuItem_Click
			AddHandler menuitem.MouseDoubleClick, AddressOf MenuItem_Click

			' vloží položku do získaného kontextového menu
			ctxm.Items(0).Items.add(menuitem)
		Next

		' zpracuje logy
		For Each l In logs
			' tvorba položky menu
			Dim menuitem As New MenuItem()
			menuitem.Header = l.Title
			menuitem.Tag = l.Source

			' nastavení obsluh událostí
			AddHandler menuitem.Click, AddressOf MenuItem_Click
			AddHandler menuitem.MouseDoubleClick, AddressOf MenuItem_Click

			' vloží  položku do získaného kontextového menu
			ctxm.Items(1).Items.add(menuitem)
		Next

		' připraví odkaz do webové adminsitrace
		If webAdmins.Count > 0 Then
			Me.AdministrationLink = webAdmins(0).Address
			Me.DisplayAdministrationLink = True
		Else
			Me.DisplayAdministrationLink = False
		End If

	End Sub

	''' <summary>
	''' Vyžádá přepnutí stavu serveru
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub tgl_Toogle(sender As Object, e As EventArgs) Handles tgl.Toogle, btnTogler.Click
		' Požádá službu o donucení serveru změnit svůj stav
		_service.StateSwitchRequest()

		' zpracuje změny stavu
		ProcessStateChange()
	End Sub

	''' <summary>
	''' Otevře kontextové menu na tlačítku
	''' </summary>
	''' <param name="sender">Reserved</param>
	''' <param name="e">Reserved</param>
	Private Sub btnConfig_Click(sender As Object, e As RoutedEventArgs) Handles btnConfig.Click
		' kde se otevře ...
		btnConfig.ContextMenu.PlacementTarget = sender

		' otevře se ...
		btnConfig.ContextMenu.IsOpen = True
	End Sub

	''' <summary>
	''' Vlákno obsluhující ověřování ID procesů serveru a portů na kterých server poslouchá
	''' </summary>
	Private portAndPidChecker As Thread = New Thread(AddressOf CheckPortAndPid)

	''' <summary>
	''' Zpracuje změny stavu serveru
	''' </summary>
	Public Sub ProcessStateChange()
		' rozhodne se pdole současného stavu
		' nastaví barvu pozadí tlačítka, jestli se zobrazí tooltip s informacema o stavu a stav toogle buttonu
		Select Case _service.GetState()
			Case ServiceState.Running
				btnTogler.Background = New SolidColorBrush(Color.FromRgb(200, 255, 175))
				tgl.SetToogle(True)
				ShowPortsAndPidsTooltip = Visibility.Visible
			Case ServiceState.Starting
				btnTogler.Background = New SolidColorBrush(Color.FromRgb(250, 255, 175))
				tgl.SetToogle(True)
				ShowPortsAndPidsTooltip = Visibility.Hidden
			Case ServiceState.Stoped
				btnTogler.Background = New SolidColorBrush(Color.FromRgb(255, 175, 175))
				tgl.SetToogle(False)
				ShowPortsAndPidsTooltip = Visibility.Hidden
		End Select


		' zajistí aktualizaci stavu v jiném vlákně pokud předchozí aktualizace skončila nebo 
		' ani nezačala (nezačala nastane po spuštění aplikace při první aktualizaci)
		If portAndPidChecker.ThreadState = ThreadState.Stopped Or
			portAndPidChecker.ThreadState = ThreadState.Unstarted Then

			' vytvoří nové vlákno
			portAndPidChecker = New Thread(AddressOf CheckPortAndPid)

			' spustí jej
			portAndPidChecker.Start()
		End If
	End Sub

	''' <summary>
	''' Zjistí změny ID procesů serveru a porty na kterých server poslouchá, 
	''' vygeneruje je v uživatelské přívětivé textové podobě
	''' </summary>
	Private Sub CheckPortAndPid()
		' přípraví textové podoby portů na ktercýh server naslouchá
		Me.Ports = String.Join(", ", Service.GetUsedPorts())
		' přípraví textové podoby ID procesů serveru
		Me.Pids = String.Join(", ", Service.GetUsedPids())

		' aktualizace data-biding
		RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ports"))
		RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Pids"))
	End Sub

	''' <summary>
	''' Obsluha kliknutí tlačítka kontextového menu
	''' </summary>
	''' <param name="sender">zdroj události (tlačítko menu)</param>
	''' <param name="e">Reserved</param>
	Private Sub MenuItem_Click(sender As Object, e As EventArgs)
		' Získá cestu k kongiguračnímu souboru/logu/administraci, která se má otevřít
		Dim path As String = sender.Tag
		' vloží do ní složku ovládaného XAMPPu
		path = path.Replace("%xamppdir%", Xampp.Config.path)
		Try
			' skusí spustit
			Process.Start(path)
		Catch ex As Exception

		End Try
	End Sub

	''' <summary>
	''' nainstaluje/odinstaluje server jako službu WIndows
	''' </summary>
	''' <param name="sender">zdroj události (tlačítko menu)</param>
	''' <param name="e">Reserved</param>
	Private Sub RunAsWindowsService(sender As MenuItem, e As RoutedEventArgs)
		' isntalace/odinstalace služby
		If Not sender.IsChecked And Not Me.Service.IsServiceInstalled() Then
			Service.InstallService()
		Else
			Service.UninstallService()
		End If
		' aktualizace zatržení tlačítka
		sender.IsChecked = Service.IsServiceInstalled()
	End Sub

End Class
