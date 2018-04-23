using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using DevExpress.Xpf.Docking;

namespace WpfApplication1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged {
        protected virtual void OnPropertyChanged(string property) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
    public class MainWindowViewModel : NotifyPropertyChanged {
        int count;
        public MainWindowViewModel() {
            Documents.Add(new DocumentViewModel() { DisplayName = "Document" + count, Content = "Content" + count++ });
            AddNewTabViewModel addNewTabViewModel = new AddNewTabViewModel();
            addNewTabViewModel.RequestNewTab += this.OnDocumentRequestAddNewTab;
            Documents.Add(addNewTabViewModel);
        }        
        ObservableCollection<DocumentViewModel> _Documents;
        public ObservableCollection<DocumentViewModel> Documents {
            get {
                if(_Documents == null) {
                    _Documents = new ObservableCollection<DocumentViewModel>();
                    _Documents.CollectionChanged += this.OnItemsChanged;
                }
                return _Documents;
            }
        }
        void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if(e.NewItems != null && e.NewItems.Count != 0)
                foreach(DocumentViewModel document in e.NewItems) {
                    document.RequestClose += this.OnDocumentRequestClose;
                }
            if(e.OldItems != null && e.OldItems.Count != 0)
                foreach(DocumentViewModel document in e.OldItems)
                    document.RequestClose -= this.OnDocumentRequestClose;
        }
        void OnDocumentRequestClose(object sender, EventArgs e) {
            DocumentViewModel document = sender as DocumentViewModel;
            if(Documents.Count == 2) AddNewTab();
            if(document != null) {
                Documents.Remove(document);
            }
        }
        void OnDocumentRequestAddNewTab(object sender, EventArgs e) {
            AddNewTab();
        }
        void AddNewTab() {
            Documents.Insert(Documents.Count - 1, new DocumentViewModel() { DisplayName = "Document" + count, Content = "Content" + count++ });
        }
    }
    public class AddNewTabViewModel : DocumentViewModel {
        public AddNewTabViewModel() {
            AllowActivate = false;
        }
        ICommand _NewTabCommand;
        public ICommand NewTabCommand {
            get {
                if(_NewTabCommand == null)
                    _NewTabCommand = new RelayCommand(new Action<object>(OnNewTab));
                return _NewTabCommand;
            }
        }
        public event EventHandler RequestNewTab;
        void OnNewTab(object param) {
            EventHandler handler = this.RequestNewTab;
            if(handler != null)
                handler(this, EventArgs.Empty);
        }
    }
    public class DocumentViewModel : NotifyPropertyChanged {
        public DocumentViewModel() {
            AllowActivate = true;
        }
        private string _DisplayName;
        public string DisplayName {
            get { return _DisplayName; }
            set {
                if(_DisplayName == value) return;
                _DisplayName = value;
                OnPropertyChanged("DisplayName");
            }
        }
        private object _Content;
        public object Content {
            get { return _Content; }
            set {
                if(_Content == value) return;
                _Content = value;
                OnPropertyChanged("Content");
            }
        }
        private bool _AllowActivate;
        public bool AllowActivate {
            get { return _AllowActivate; }
            set {
                if(_AllowActivate == value)
                    return;
                _AllowActivate = value;
                OnPropertyChanged("AllowActivate");
            }
        }
        ICommand _CloseCommand;
        public ICommand CloseCommand {
            get {
                if(_CloseCommand == null)
                    _CloseCommand = new RelayCommand(new Action<object>(OnRequestClose));
                return _CloseCommand;
            }
        }
        public event EventHandler RequestClose;
        void OnRequestClose(object param) {
            EventHandler handler = this.RequestClose;
            if(handler != null)
                handler(this, EventArgs.Empty);
        }
    }
    public class RelayCommand : ICommand {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        public RelayCommand(Action<object> execute)
            : this(execute, null) {
        }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute) {
            if(execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #region ICommand Members
        public bool CanExecute(object parameter) {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter) {
            _execute(parameter);
        }
        #endregion // ICommand Members
    }
    public class CaptionStyleSelector : StyleSelector {
        public Style AddNewTabStyle { get; set; }
        public override Style SelectStyle(object item, DependencyObject container) {
            if(item is ContentItem && ((ContentItem)item).Content is AddNewTabViewModel)
                return AddNewTabStyle;
            return base.SelectStyle(item, container);
        }
    }
}
