Imports System.Globalization

''' <summary>
''' Převede typ položky na piktogram položky
''' </summary>
Public Class ItemKindBindingConvertor
	Implements IValueConverter

	Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
		Dim val As ItemKind = value
		Dim uris
		If val = ItemKind.User Then
			uris = "user.png"
		Else
			uris = "group.png"
		End If
		Return New Uri("pack:///application:,,,/Assets/" & uris, UriKind.Relative)
	End Function

	Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
		Return Nothing
	End Function
End Class
