Module Module1

	Sub Main()
		' spustí systémovou ovládací konzoli s otevřeným správcem služeb
		Process.Start("C:\Windows\system32\mmc.exe", "C:\Windows\System32\services.msc")
	End Sub

End Module
