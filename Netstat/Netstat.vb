Imports System.Net
Imports System.Net.NetworkInformation

''' <summary>
''' Zjišťuje informace o síti
''' </summary>
Public Class Netstat

	''' <summary>
	''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa365928(v=vs.85).aspx"/>
	''' </summary>
	Declare Function GetExtendedTcpTable Lib "iphlpapi.dll" (pTcpTable As Byte(),
															 ByRef dwOutBufLen As Integer,
															 sort As Boolean,
															 ipVersion As Integer,
															 tblClass As TCP_TABLE_CLASS,
															 reserved As Integer) As Integer

	''' <summary>
	''' Položky navrácené předchozím voláním
	''' </summary>
	Private previousItems As New List(Of NetstatHandle)

	''' <summary>
	''' cache TCP tabulky
	''' </summary>
	Private tcpTableCache As Byte() = Nothing

	''' <summary>
	''' Časová známky kde došlo k vytvoření cache.
	''' </summary>
	Private cached As DateTime

	''' <summary>
	''' Vrátí buffer TCP tabulky
	''' </summary>
	Public Function GetTcpTable() As Byte()
		' hledá v cache
		If tcpTableCache Is Nothing Or (DateTime.Now - cached).TotalMilliseconds > 3000 Then ' pokud nenajde v cache nebo cache expirovala
			' zísá data
			tcpTableCache = ReadTcpTable()
			' aktualizuje cache
			cached = DateTime.Now
			Return tcpTableCache
		Else ' použije cache
			Return tcpTableCache
		End If
	End Function

	''' <summary>
	''' Přečte TCP tabulku, nepoužívá cache
	''' </summary>
	Private Function ReadTcpTable() As Byte()
		' velikost bufferu
		Dim size As Integer = 20000
		' deklarace bufferu
		Dim buffer(size) As Byte
		' získá tabulku dat do bufferu a návratový kód operace
		Dim code = GetExtendedTcpTable(buffer, size, True, 2, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0)
		If code <> 0 Then ' v operaci došlo k chybě
			Throw New NetstatException()
		End If
		Return buffer
	End Function

	''' <summary>
	''' Možnosti požadavku o tabulku
	''' </summary>
	Public Enum TCP_TABLE_CLASS
		TCP_TABLE_BASIC_LISTENER
		TCP_TABLE_BASIC_CONNECTIONS
		TCP_TABLE_BASIC_ALL
		TCP_TABLE_OWNER_PID_LISTENER
		TCP_TABLE_OWNER_PID_CONNECTIONS
		TCP_TABLE_OWNER_PID_ALL
		TCP_TABLE_OWNER_MODULE_LISTENER
		TCP_TABLE_OWNER_MODULE_CONNECTIONS
		TCP_TABLE_OWNER_MODULE_ALL
	End Enum

	''' <summary>
	''' Tabulka TCP spojení
	''' </summary>
	Structure MIB_TCPTABLE_OWNER_PID
		''' <summary>
		''' Počet položek v tabulce
		''' </summary>
		Public dwNumEntries As Integer

		''' <summary>
		''' Tabulka řádů dat
		''' </summary>
		Public table As MIB_TCPROW_OWNER_PID()
	End Structure

	''' <summary>
	''' Interpretace TCP spojení v tabulce
	''' </summary>
	Structure MIB_TCPROW_OWNER_PID
		''' <summary>
		''' Stav spojení
		''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa366913(v=vs.85).aspx"/>
		''' </summary>
		Public dwState As Integer

		''' <summary>
		''' Klientská IP adresa na které klient poslouchá a vysílá
		''' </summary>
		Public Local As IPEndPoint

		''' <summary>
		''' Vzdálená IP adresa na kteroé vzdálený server poslouchá a vysílá z ni
		''' </summary>
		Public Remote As IPEndPoint

		''' <summary>
		''' ID procesu obsluhující spojení
		''' </summary>
		Public dwOwningPid As Integer
	End Structure

	''' <summary>
	''' Vrátí TCP posluchače
	''' </summary>
	''' <returns></returns>
	Private Function GetListeningConnections() As MIB_TCPROW_OWNER_PID()
		' připraví buffer a uloží do něj TCP tabulku
		Dim buffer() As Byte = GetTcpTable()
		' připraví výstup
		Dim items = New List(Of MIB_TCPROW_OWNER_PID)()
		' načte počet položek v tabulce, což je první (int) položka v struktuře
		Dim parts = BitConverter.ToInt32(buffer, 0)
		' offset posune na 4 (zde začíná první položka)
		Dim offset = 4
		' načte všechny položky
		For i = 0 To parts - 1
			' vytřvoří položku
			Dim item As New MIB_TCPROW_OWNER_PID()
			' načte stav
			item.dwState = BitConverter.ToInt32(buffer, offset + 0)

			' načte části lokální IP adresy
			Dim p1 As Long = CUInt(buffer(offset + 4))
			Dim p2 As Long = CUInt(buffer(offset + 5)) << 8
			Dim p3 As Long = CUInt(buffer(offset + 6)) << 16
			Dim p4 As Long = CUInt(buffer(offset + 7)) << 24
			' skompletuje IP adresy
			Dim lip As Int64 = p1 Or p2 Or p3 Or p4

			' naparsuje port na klientovi
			' port má bity tak jak má mít (Windows-like) a byty naopak (Unix-like)
			Dim lprt = BitConverter.ToUInt16(New Byte() {buffer(offset + 9), buffer(offset + 8)}, 0)

			' vzdálená adresa, parsuje se stejně jako lokální
			Dim rip As Int64 = (buffer(offset + 12)) Or (buffer(offset + 13) << 8) Or (buffer(offset + 14) << 16) Or (buffer(offset + 15) << 24)
			' vzdálený port
			Dim rprt As Int32
			If rip <> 0 Then
				rprt = CInt(buffer(offset + 16) << 8) + CInt(buffer(offset + 17)) + CInt(buffer(offset + 18) << 24) + CInt(buffer(offset + 19) << 16)
			Else
				rprt = 0
			End If

			' inicializace položky
			item.Local = New IPEndPoint(lip, lprt)
			item.Remote = New IPEndPoint(rip, rprt)

			' načtení a inicializace ID obsluhujícího procesu
			item.dwOwningPid = BitConverter.ToInt32(buffer, offset + 20)
			' posunutí offset
			offset += 6 * 4
			' zařazení položky do výstupu
			items.Add(item)
		Next
		' navrácení výstupu jako pole
		Return items.ToArray()
	End Function

	''' <summary>
	''' Vrátí seznam portů na kterých poslouchá nějaký půroces
	''' </summary>
	Public Function GetListeningProceses() As List(Of NetstatHandle)
		' získá seznam nasloucháačů
		Dim items = GetListeningConnections()
		' připraví výstup
		Dim output As New List(Of NetstatHandle)()
		' projde naslouchače
		For i = 0 To items.Count - 1
			Dim x = items(i)
			Try
				' inicializuje netstat handle
				Dim y = New NetstatHandle() With
				{
					.Address = x.Local.Address.ToString(),
					.Pid = x.dwOwningPid,
					.ProcessName = Process.GetProcessById(x.dwOwningPid).ProcessName,
					.Port = x.Local.Port
				}
				' zkotroluje zdali je nový
				If Not containsPrevious(y) Then
					y.Started = DateTime.Now
					y.IsNew = True
					y.IsOld = False
				Else
					' zkotroluje zdali již sice není striktně nový, ale byl nedávno přidán (max 3s)
					Dim dad = FindDad(y)
					If dad IsNot Nothing Then
						y.Started = dad.Started
						If (DateTime.Now - y.Started).Seconds < 3 Then
							y.IsNew = True
						End If
					End If
				End If
				' zařadí do výstupu
				output.Add(y)
			Catch ex As Exception

			End Try
		Next

		' najde ty které skončili s poslechem
		' projde předchozí
		For Each p In previousItems
			' zjistí, zdali již nejsou obsaženy v aktuálním výstupu
			If Not contains(output, p) Then
				' označí
				p.IsNew = False
				p.Started = Nothing
				p.IsOld = True
				' pokud je označován poprvé, označí časovým razítkem
				If p.Closed = DateTime.MinValue Then
					p.Closed = DateTime.Now
				End If
				' pokud je označem před méně než 3s je zařazen do výstupu
				If (DateTime.Now - p.Closed).Seconds < 3 Then
					output.Add(p)
				Else
					' sbohem na věčné časy
				End If
			End If
		Next

		previousItems = output
		Return output
	End Function

	''' <summary>
	''' Získá seznam portů na kterých poslouchá předaný proces
	''' </summary>
	''' <param name="pid">ID procesu u kterého se bude získávat na kterých portech poslouchá</param>
	''' <returns>seznám portů</returns>
	Public Function GetPortsListenedbyPID(pid As Integer) As List(Of Integer)
		' data budou pro vyšší výkon získávána nativně

		' incializace buffere a získání nativní rtabulky		
		Dim buffer() As Byte = GetTcpTable()
		' incializace výstupu
		Dim items = New List(Of Integer)()
		' počet položek v tabulce
		Dim parts = BitConverter.ToInt32(buffer, 0)
		' index prvního bytu první položky
		Dim offset = 4
		' projde položky
		For i = 0 To parts - 1
			' získá PID podle offsetu jak má být
			Dim _itemPid = BitConverter.ToInt32(buffer, offset + 20)
			' ověří zdali pid procesu = hleadnánemu PIDu
			If _itemPid = pid Then
				' vloží do výstupu port na kterém poslouchá
				' port má bity tak jak má mít (Windows-like) a byty naopak (Unix-like)
				items.Add(BitConverter.ToUInt16(New Byte() {buffer(offset + 9), buffer(offset + 8)}, 0))
			End If
			' posune offset pro další položku
			offset += 6 * 4
		Next
		Return items
	End Function

	''' <summary>
	''' Zjistí zdali list výsledků netstat obsahuje položku
	''' </summary>
	''' <param name="list">List výstupu netstatu</param>
	''' <param name="item">hledaná položka</param>
	Private Function contains(list As List(Of NetstatHandle), item As NetstatHandle)
		' projde list
		For Each p In list
			' pokud se procházená = hledaná => je v listu > return ano
			If p = item Then
				Return True
			End If
		Next
		' pokud nikde nebylo vráceno ano => return ne
		Return False
	End Function

	''' <summary>
	''' Zjistí zdali předchozí vrácený výsledek obsahoval položku
	''' </summary>
	''' <param name="item">hledaná položka</param>
	Private Function containsPrevious(item As NetstatHandle) As Boolean
		Return contains(previousItems, item)
	End Function

	''' <summary>
	''' Najde položku v předchozím výstupu
	''' </summary>
	''' <param name="item">hleadaná položka</param>
	''' <returns></returns>
	Public Function FindDad(item As NetstatHandle) As NetstatHandle
		' prodje předchozív výstup
		For Each p In previousItems
			' pokud polžoky sedí (přetypovaný operátor)
			If p = item Then
				' vrátí položku
				Return p
			End If
		Next
		' pokud žádná položka neodpovídala, vrátí Nothing
		Return Nothing
	End Function
End Class
