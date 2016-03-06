''' <summary>
''' Stavy serveru
''' </summary>
Public Enum ServiceState
	''' <summary>
	''' Server běží a poslouchá na alespoň jednom portu
	''' </summary>
	Running

	''' <summary>
	''' Server běží, ale neposlouchá na žádném portu
	''' </summary>
	Starting

	''' <summary>
	''' Server neběží
	''' </summary>
	Stoped
End Enum
