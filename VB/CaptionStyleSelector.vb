Imports DevExpress.Xpf.Docking
Imports System
Imports System.Linq
Imports System.Windows
Imports System.Windows.Controls

Namespace WpfApplication1
	Public Class CaptionStyleSelector
		Inherits StyleSelector

		Public Property AddNewTabStyle() As Style
		Public Overrides Function SelectStyle(ByVal item As Object, ByVal container As DependencyObject) As Style
			If TypeOf item Is ContentItem AndAlso TypeOf DirectCast(item, ContentItem).Content Is AddNewTabViewModel Then
				Return AddNewTabStyle
			End If
			Return MyBase.SelectStyle(item, container)
		End Function
	End Class
End Namespace
