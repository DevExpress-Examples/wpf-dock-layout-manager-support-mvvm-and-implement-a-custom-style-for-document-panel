Imports DevExpress.Mvvm
Imports System
Imports System.Collections.ObjectModel
Imports System.Linq

Namespace WpfApplication1
	Public Class MainWindowViewModel
		Inherits ViewModelBase

		Public Sub New()
			Documents = New ObservableCollection(Of DocumentViewModel)()
            Documents.Add(New DocumentViewModel() With {
                .DisplayName = "Document" & documentCount,
                .Content = "Content" & documentCount
            })
            documentCount += 1
            Documents.Add(New AddNewTabViewModel())
			CloseCommand = New DelegateCommand(Of DocumentViewModel)(AddressOf Close)
			AddNewCommand = New DelegateCommand(AddressOf AddNew)
		End Sub
        Public Property Documents() As ObservableCollection(Of DocumentViewModel)
            Get
                Return GetProperty(Function() Documents)
            End Get
            Private Set(ByVal value As ObservableCollection(Of DocumentViewModel))
                SetProperty(Function() Documents, value)
            End Set
        End Property
        Private _closeCommand As ICommand
        Public Property CloseCommand() As ICommand
            Get
                Return _closeCommand
            End Get
            Private Set(ByVal value As ICommand)
                _closeCommand = value
            End Set
        End Property
        Private _addNewCommand As ICommand
        Public Property AddNewCommand() As ICommand
            Get
                Return _addNewCommand
            End Get
            Private Set(ByVal value As ICommand)
                _addNewCommand = value
            End Set
        End Property
        Public Sub Close(ByVal viewModel As DocumentViewModel)
            Documents.Remove(viewModel)
        End Sub
        Public Sub AddNew()
            Documents.Insert(Documents.Count - 1, New DocumentViewModel() With {
                .DisplayName = "Document " & documentCount,
                .Content = "Content" & documentCount
            })
            documentCount += 1
        End Sub
		Private documentCount As Integer
	End Class

	Public Class DocumentViewModel
		Inherits ViewModelBase

		Public Sub New()
			AllowActivate = True
		End Sub
		Public Property DisplayName() As String
			Get
				Return GetProperty(Function() DisplayName)
			End Get
			Set(ByVal value As String)
				SetProperty(Function() DisplayName, value)
			End Set
		End Property
		Public Property Content() As Object
			Get
				Return GetProperty(Function() Content)
			End Get
			Set(ByVal value As Object)
				SetProperty(Function() Content, value)
			End Set
		End Property
		Public Property AllowActivate() As Boolean
			Get
				Return GetProperty(Function() AllowActivate)
			End Get
			Set(ByVal value As Boolean)
				SetProperty(Function() AllowActivate, value)
			End Set
		End Property
	End Class

	Public Class AddNewTabViewModel
		Inherits DocumentViewModel

		Public Sub New()
			AllowActivate = False
		End Sub
	End Class
End Namespace
