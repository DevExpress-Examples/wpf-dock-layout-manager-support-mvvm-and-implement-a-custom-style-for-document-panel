using DevExpress.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace WpfApplication1
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel() {
            Documents = new ObservableCollection<DocumentViewModel>();
            Documents.Add(new DocumentViewModel() { DisplayName = "Document" + documentCount, Content = "Content" + documentCount++ });
            Documents.Add(new AddNewTabViewModel());
            CloseCommand = new DelegateCommand<DocumentViewModel>(Close);
            AddNewCommand = new DelegateCommand(AddNew);
        }
        public ObservableCollection<DocumentViewModel> Documents {
            get { return GetProperty(() => Documents); }
            private set { SetProperty(() => Documents, value); }
        }
        public ICommand CloseCommand { get; private set; }
        public ICommand AddNewCommand { get; private set; }
        public void Close(DocumentViewModel viewModel) {
            Documents.Remove(viewModel);
        }
        public void AddNew() {
            Documents.Insert(Documents.Count - 1, new DocumentViewModel() { DisplayName = "Document " + documentCount, Content = "Content" + documentCount++ });
        }
        int documentCount;
    }

    public class DocumentViewModel : ViewModelBase
    {
        public DocumentViewModel() {
            AllowActivate = true;
        }
        public string DisplayName {
            get { return GetProperty(() => DisplayName); }
            set { SetProperty(() => DisplayName, value); }
        }
        public object Content {
            get { return GetProperty(() => Content); }
            set { SetProperty(() => Content, value); }
        }
        public bool AllowActivate {
            get { return GetProperty(() => AllowActivate); }
            set { SetProperty(() => AllowActivate, value); }
        }
    }

    public class AddNewTabViewModel : DocumentViewModel
    {
        public AddNewTabViewModel() {
            AllowActivate = false;
        }
    }
}
