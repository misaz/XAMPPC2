Imports System.Threading

Class MainWindow

	Private Sub Form_Loaded(sender As Object, e As RoutedEventArgs) Handles wnd.Loaded
		Dim apaches = New ApacheService(apache)
		Dim marias = New MariaService(mysql)
		Dim filezillas = New FileZillaService(filezilla)
		Dim tomcats = New TomcatService(tomcat)
		Dim mercurys = New MercuryService(mercury)

		apache.Service = apaches
		mysql.Service = marias
		filezilla.Service = filezillas
		tomcat.Service = tomcats
		mercury.Service = mercurys

		SetVersion(ApacheVersion, apaches)
		SetVersion(MariaDbVersion, marias)
		SetVersion(XamppCPVersion, New Xampp.XamppControlpanelVersionMiner())
		SetVersion(PhpVersion, New PhpVersionMiner())
		SetVersion(PhpMyAdminVersion, New PhpMyAdminVersionMiner())
		SetVersion(FileZillaVersion, filezillas)
		SetVersion(TomcatVersion, tomcats)
		SetVersion(MercuryVersion, mercurys)
	End Sub

	Private Sub SetVersion(field As TextBlock, miner As IVersionMiner)
		Dim t = New Thread(
			Sub()
				Try
					Dim version = miner.GetVersion()
					field.Dispatcher.Invoke(New Action(
											Sub()
												field.Text = version
											End Sub))
				Catch ex As Exception

				End Try
			End Sub)


		t.Start()
	End Sub

End Class