Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports DevExpress.Xpf.Docking

Namespace WpfApplication1
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
			DataContext = New MainWindowViewModel()
		End Sub
	End Class
	Public MustInherit Class NotifyPropertyChanged
		Implements INotifyPropertyChanged
		Protected Overridable Sub OnPropertyChanged(ByVal [property] As String)
			Dim handler As PropertyChangedEventHandler = PropertyChangedEvent
			If handler IsNot Nothing Then
				handler(Me, New PropertyChangedEventArgs([property]))
			End If
		End Sub
		#Region "INotifyPropertyChanged Members"
		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
		#End Region
	End Class
	Public Class MainWindowViewModel
		Inherits NotifyPropertyChanged
		Private count As Integer
		Public Sub New()
'INSTANT VB TODO TASK: Assignments within expressions are not supported in VB.NET
'ORIGINAL LINE: Documents.Add(New DocumentViewModel() { DisplayName = "Document" + count, Content = "Content" + count++ });
            Documents.Add(New DocumentViewModel() With {.DisplayName = "Document" & count, .Content = "Content"})
			Dim addNewTabViewModel As New AddNewTabViewModel()
			AddHandler addNewTabViewModel.RequestNewTab, AddressOf OnDocumentRequestAddNewTab
			Documents.Add(addNewTabViewModel)
		End Sub
		Private _Documents As ObservableCollection(Of DocumentViewModel)
		Public ReadOnly Property Documents() As ObservableCollection(Of DocumentViewModel)
			Get
				If _Documents Is Nothing Then
					_Documents = New ObservableCollection(Of DocumentViewModel)()
					AddHandler _Documents.CollectionChanged, AddressOf OnItemsChanged
				End If
				Return _Documents
			End Get
		End Property
		Private Sub OnItemsChanged(ByVal sender As Object, ByVal e As NotifyCollectionChangedEventArgs)
			If e.NewItems IsNot Nothing AndAlso e.NewItems.Count <> 0 Then
				For Each document As DocumentViewModel In e.NewItems
					AddHandler document.RequestClose, AddressOf OnDocumentRequestClose
				Next document
			End If
			If e.OldItems IsNot Nothing AndAlso e.OldItems.Count <> 0 Then
				For Each document As DocumentViewModel In e.OldItems
					RemoveHandler document.RequestClose, AddressOf OnDocumentRequestClose
				Next document
			End If
		End Sub
		Private Sub OnDocumentRequestClose(ByVal sender As Object, ByVal e As EventArgs)
			Dim document As DocumentViewModel = TryCast(sender, DocumentViewModel)
			If Documents.Count = 2 Then
				AddNewTab()
			End If
			If document IsNot Nothing Then
				Documents.Remove(document)
			End If
		End Sub
		Private Sub OnDocumentRequestAddNewTab(ByVal sender As Object, ByVal e As EventArgs)
			AddNewTab()
		End Sub
		Private Sub AddNewTab()
'INSTANT VB TODO TASK: Assignments within expressions are not supported in VB.NET
'ORIGINAL LINE: Documents.Insert(Documents.Count - 1, New DocumentViewModel() { DisplayName = "Document" + count, Content = "Content" + count++ });
            Documents.Insert(Documents.Count - 1, New DocumentViewModel() With {.DisplayName = "Document" & count, .Content = "Content"})
		End Sub
	End Class
	Public Class AddNewTabViewModel
		Inherits DocumentViewModel
		Public Sub New()
			AllowActivate = False
		End Sub
		Private _NewTabCommand As ICommand
		Public ReadOnly Property NewTabCommand() As ICommand
			Get
				If _NewTabCommand Is Nothing Then
					_NewTabCommand = New RelayCommand(New Action(Of Object)(AddressOf OnNewTab))
				End If
				Return _NewTabCommand
			End Get
		End Property
		Public Event RequestNewTab As EventHandler
		Private Sub OnNewTab(ByVal param As Object)
			Dim handler As EventHandler = Me.RequestNewTabEvent
			If handler IsNot Nothing Then
				handler(Me, EventArgs.Empty)
			End If
		End Sub
	End Class
	Public Class DocumentViewModel
		Inherits NotifyPropertyChanged
		Public Sub New()
			AllowActivate = True
		End Sub
		Private _DisplayName As String
		Public Property DisplayName() As String
			Get
				Return _DisplayName
			End Get
			Set(ByVal value As String)
				If _DisplayName = value Then
					Return
				End If
				_DisplayName = value
				OnPropertyChanged("DisplayName")
			End Set
		End Property
		Private _Content As Object
		Public Property Content() As Object
			Get
				Return _Content
			End Get
			Set(ByVal value As Object)
				If _Content Is value Then
					Return
				End If
				_Content = value
				OnPropertyChanged("Content")
			End Set
		End Property
		Private _AllowActivate As Boolean
		Public Property AllowActivate() As Boolean
			Get
				Return _AllowActivate
			End Get
			Set(ByVal value As Boolean)
				If _AllowActivate = value Then
					Return
				End If
				_AllowActivate = value
				OnPropertyChanged("AllowActivate")
			End Set
		End Property
		Private _CloseCommand As ICommand
		Public ReadOnly Property CloseCommand() As ICommand
			Get
				If _CloseCommand Is Nothing Then
					_CloseCommand = New RelayCommand(New Action(Of Object)(AddressOf OnRequestClose))
				End If
				Return _CloseCommand
			End Get
		End Property
		Public Event RequestClose As EventHandler
		Private Sub OnRequestClose(ByVal param As Object)
			Dim handler As EventHandler = Me.RequestCloseEvent
			If handler IsNot Nothing Then
				handler(Me, EventArgs.Empty)
			End If
		End Sub
	End Class
	Public Class RelayCommand
		Implements ICommand
		Private ReadOnly _execute As Action(Of Object)
		Private ReadOnly _canExecute As Predicate(Of Object)
		Public Sub New(ByVal execute As Action(Of Object))
			Me.New(execute, Nothing)
		End Sub
		Public Sub New(ByVal execute As Action(Of Object), ByVal canExecute As Predicate(Of Object))
			If execute Is Nothing Then
				Throw New ArgumentNullException("execute")
			End If
			_execute = execute
			_canExecute = canExecute
		End Sub
		#Region "ICommand Members"
		Public Function CanExecute(ByVal parameter As Object) As Boolean Implements ICommand.CanExecute
			Return If(_canExecute Is Nothing, True, _canExecute(parameter))
		End Function
		Public Custom Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
			AddHandler(ByVal value As EventHandler)
				AddHandler CommandManager.RequerySuggested, value
			End AddHandler
			RemoveHandler(ByVal value As EventHandler)
				RemoveHandler CommandManager.RequerySuggested, value
			End RemoveHandler
			RaiseEvent(ByVal sender As System.Object, ByVal e As System.EventArgs)
			End RaiseEvent
		End Event
		Public Sub Execute(ByVal parameter As Object) Implements ICommand.Execute
			_execute(parameter)
		End Sub
		#End Region ' ICommand Members
	End Class
	Public Class CaptionStyleSelector
		Inherits StyleSelector
		Private privateAddNewTabStyle As Style
		Public Property AddNewTabStyle() As Style
			Get
				Return privateAddNewTabStyle
			End Get
			Set(ByVal value As Style)
				privateAddNewTabStyle = value
			End Set
		End Property
		Public Overrides Function SelectStyle(ByVal item As Object, ByVal container As DependencyObject) As Style
			If TypeOf item Is ContentItem AndAlso TypeOf (CType(item, ContentItem)).Content Is AddNewTabViewModel Then
				Return AddNewTabStyle
			End If
			Return MyBase.SelectStyle(item, container)
		End Function
	End Class
End Namespace
