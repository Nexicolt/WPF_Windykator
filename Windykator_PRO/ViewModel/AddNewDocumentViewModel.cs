using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Windykator_PRO.Database;
using Windykator_PRO.Helpers;
using Windykator_PRO.Validation;
using Windykator_PRO.View.NewDocument;

namespace Windykator_PRO.ViewModel
{

    public class AssortmentGridItem
    {
        public string Name { get; set; }

        public string Type { get; set; }
    }

    public class AssortmentOnDocumentGridItem
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public decimal Quantity { get; set; }
    }
    public class AddNewDocumentViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        private readonly VindicationDatabase db = null;

        private string selectedAssortmntName;
        private string selectedAssortmentType;

        private decimal _UserInputQuantity;
        public decimal UserInputQuantity
        {
            get => UserInputQuantity;
            set
            {
                if (_UserInputQuantity != value)
                    _UserInputQuantity = value;

                AddAssortmentToDocument(value);
            }
        }

        public ICommand ChangeGridCommand { get => new BaseCommand(() => ChangeGridView()); }

        public ICommand AddPositionCommand { get => new BaseCommand(() => AddPosition()); }

        public ICommand RemoveAddedPositionCommand { get => new BaseCommand(() => RemoveAddedPosition()); }

        public ICommand SearchCommand { get => new BaseCommand(() => Search()); }


        public decimal AssortmentQuantity { get; set; }
        public AddNewDocumentViewModel()
        {
            this.DisplayName = "Nowy dokument";
            db = new VindicationDatabase();

            InitAssortmentsGrid();

            AssortmentSearchGridVisibility = Visibility.Visible;
            AssortmentOnDocumentGridVisibility = Visibility.Hidden;
            AddButtonVisibility = Visibility.Visible;
            RemoveButtonVisibility = Visibility.Hidden;

            AssortmentsOnDocumentGridList = new ObservableCollection<AssortmentOnDocumentGridItem>();
        }


        private void Search()
        {

            List<AssortmentGridItem> list = null;
            if (!string.IsNullOrEmpty(SearchCeriteria_AssortmentType))
            {

                if (!string.IsNullOrEmpty(SearchCriteria_AssortmentName))
                {
                    if (SearchCeriteria_AssortmentType == "Asortyment")
                    {
                        list = db.Assortment.Where(row => row.IsEnable && row.Name.ToLower().Contains(SearchCriteria_AssortmentName))
                            .Select(x => new AssortmentGridItem { Name = x.Name, Type = "Asortyment" }).ToList();
                    }
                    else
                    {
                        list = db.Service.Where(row => row.IsEnable && row.Name.ToLower().Contains(SearchCriteria_AssortmentName))
                            .Select(x => new AssortmentGridItem { Name = x.Name, Type = "Usługa" }).ToList();
                    }
                }
                else
                {
                    if (SearchCeriteria_AssortmentType == "Asortyment")
                    {
                        list = db.Assortment.Where(row => row.IsEnable)
                            .Select(x => new AssortmentGridItem { Name = x.Name, Type = "Asortyment" }).ToList();
                    }
                    else
                    {
                        list = db.Service.Where(row => row.IsEnable)
                            .Select(x => new AssortmentGridItem { Name = x.Name, Type = "Usługa" }).ToList();
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(SearchCriteria_AssortmentName))
                {
                    list = db.Assortment.Where(row => row.IsEnable && row.Name.ToLower().Contains(SearchCriteria_AssortmentName))
                        .Select(x => new AssortmentGridItem { Name = x.Name, Type = "Asortyment" })
                    .Concat(db.Service.Where(row => row.IsEnable && row.Name.ToLower().Contains(SearchCriteria_AssortmentName))
                    .Select(x => new AssortmentGridItem { Name = x.Name, Type = "Asortyment" })).ToList();
                }
                else
                {
                    list = db.Assortment.Where(row => row.IsEnable).Select(x => new AssortmentGridItem { Name = x.Name, Type = "Asortyment" })
                   .Concat(db.Service.Where(row => row.IsEnable).Select(x => new AssortmentGridItem { Name = x.Name, Type = "Asortyment" })).ToList();
                }
            }

            AssortmentsGridList = new ObservableCollection<AssortmentGridItem>(list);
            OnPropertyChanged(() => AssortmentsGridList);
        }

        private void RemoveAddedPosition()
        {
            if (SelectedItemOnAssortmentOnDocumentsGrid == null)
            {
                ShowErrorMessageBox("Wskaż pozycje do usunięcia");
                return;
            }

            AssortmentsOnDocumentGridList.Remove(SelectedItemOnAssortmentOnDocumentsGrid);
            OnPropertyChanged(() => AssortmentsOnDocumentGridList);

        }
        private void AddPosition()
        {
            if (SelectedItemOnAssortmentsGrid is null)
            {
                ShowErrorMessageBox("Wskaż asortyment/usługę z listy");
                return;
            }

            selectedAssortmentType = SelectedItemOnAssortmentsGrid.Type;
            selectedAssortmntName = SelectedItemOnAssortmentsGrid.Name;

            var quantityPrompt = new AskForProductQuantityWIndow(this);
            quantityPrompt.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            quantityPrompt.Show();
        }
        private void ChangeGridView()
        {
            AssortmentSearchGridVisibility = (AssortmentSearchGridVisibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
            AssortmentOnDocumentGridVisibility = (AssortmentOnDocumentGridVisibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;

            AddButtonVisibility = (AddButtonVisibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
            RemoveButtonVisibility = (RemoveButtonVisibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;

            OnPropertyChanged(() => AssortmentOnDocumentGridVisibility);
            OnPropertyChanged(() => AssortmentSearchGridVisibility);
            OnPropertyChanged(() => AddButtonVisibility);
            OnPropertyChanged(() => RemoveButtonVisibility);
        }

        private void InitAssortmentsGrid()
        {
            AssortmentsGridList = new ObservableCollection<AssortmentGridItem>(db.Service.Where(row => row.IsEnable)
                    .Select(x => new AssortmentGridItem { Name = x.Name, Type = "Usługa" })
                    .ToList()
                    .Concat(db.Assortment.Where(row => row.IsEnable)
                    .Select(x => new AssortmentGridItem { Name = x.Name, Type = "Asortyment" })
                    .ToList())
                    );
        }

        private void AddAssortmentToDocument(decimal value)
        {
            AssortmentsOnDocumentGridList.Add(new AssortmentOnDocumentGridItem { Name = selectedAssortmntName, Type = selectedAssortmentType, Quantity = value });
            OnPropertyChanged(() => AssortmentsOnDocumentGridList);
        }

        public string DocNo { get; set; }
        public string DocType { get; set; }
        public string Author { get; set; }

        public string SearchCriteria_AssortmentName { get; set; }
        public string SearchCeriteria_AssortmentType { get; set; }


        public Visibility AssortmentSearchGridVisibility { get; set; }

        public Visibility AssortmentOnDocumentGridVisibility { get; set; }

        public Visibility AddButtonVisibility { get; set; }

        public Visibility RemoveButtonVisibility { get; set; }

        private List<string> _DocumentTypesForCombobox;
        public List<string> DocumentTypesForCombobox
        {
            get
            {
                if (_DocumentTypesForCombobox == null)
                    _DocumentTypesForCombobox = db.DocumentType.Where(x => x.IsEnable).Select(x => x.Name).ToList();
                return _DocumentTypesForCombobox;
            }
        }

        public List<string> AssortmentTypesForComboBox { get => new List<string>() { "Asortyment", "Usługa" }; }

        public AssortmentGridItem SelectedItemOnAssortmentsGrid { get; set; }

        public AssortmentOnDocumentGridItem SelectedItemOnAssortmentOnDocumentsGrid { get; set; }
        public ObservableCollection<AssortmentGridItem> AssortmentsGridList { get; set; }
        public ObservableCollection<AssortmentOnDocumentGridItem> AssortmentsOnDocumentGridList { get; set; }

        public string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case "DocNo":
                        result = DocumentValidator.IsCorrectDocNo(DocNo);
                        break;

                    case "DocType":
                        result = StringValidator.IsEmpty(DocType);
                        break;
                }

                return result;
            }
        }

        public string Error => null;
    }
}
