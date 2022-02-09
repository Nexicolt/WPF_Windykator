using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Windykator_PRO.Database;
using Windykator_PRO.Database.Models;
using Windykator_PRO.Helpers;
using Windykator_PRO.Validation;

namespace Windykator_PRO.ViewModel
{
    internal class AddNewIssueViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        #region Constructor

        private readonly VindicationDatabase db = null;

        public AddNewIssueViewModel()
        {
            base.DisplayName = "Nowa sprawa";
            Messenger.Default.Register<CustomerGridDto>(this, GetClientDto);
            Messenger.Default.Register<DebtorGridDto>(this, GetDebtorDto);

            DocumentsOnIssueGridList = new ObservableCollection<DocumentGridView>();

            _GridWithSearchDocumentVisibility = Visibility.Visible;

            db = new VindicationDatabase();
        }

        #endregion Constructor

        public ICommand SelectDebtorModalCommand
        {
            get
            {
                return MainWindowViewModel.MainWindowHandler.DebtorsToSelectModalCommand;
            }
        }

        public ICommand SelectClientModalCommand
        {
            get
            {
                return MainWindowViewModel.MainWindowHandler.ClientsToSelectModalCommand;
            }
        }

        public ICommand IssueDetailsCommand
        {
            get
            {
                return MainWindowViewModel.MainWindowHandler.IssueDetailsCommand;
            }
        }

        private bool _ShowDocumentsOnIssue;
        public bool ShowDocumentsOnIssue
        {
            get => _ShowDocumentsOnIssue;

            set
            {
                _ShowDocumentsOnIssue = value;
                if (value == true)
                    GridWithSearchDocumentVisibility = Visibility.Hidden;
                else
                    GridWithSearchDocumentVisibility = Visibility.Visible;

                OnPropertyChanged(() => GridWithSearchDocumentVisibility);
                OnPropertyChanged(() => GirdWithAssociatedDocumentsVisibility);
            }
        }
        public string Customer_Name { get; set; }
        public string Customer_Location { get; set; }
        public string Currency { get; set; }
        public string Debtor_Name { get; set; }
        public string Debtor_Location { get; set; }
        public string Debtor_Due { get; set; }
        public string InternalNo { get; set; }

        private List<string> _CurrencyForComboBox;
        public List<string> CurrencyForComboBox
        {
            get
            {
                if (_CurrencyForComboBox is null)
                    _CurrencyForComboBox = db.Currency.Where(row => row.IsEnable).Select(row => row.Name).ToList();
                return _CurrencyForComboBox;
            }
            set { _CurrencyForComboBox = value; }
        }

        private List<string> _DocumentTypeFoeComboBox;
        public List<string> DocumentTypeFoeComboBox
        {
            get
            {
                if (_DocumentTypeFoeComboBox is null)
                    _DocumentTypeFoeComboBox = db.DocumentType.Where(row => row.IsEnable).Select(row => row.Name).ToList();
                return _DocumentTypeFoeComboBox;
            }
            set { _CurrencyForComboBox = value; }
        }

        public ICommand SearchDocsCommand
        {
            get
            {
                return new BaseCommand(() => SearchDocs());
            }
        }

        public ICommand AddDocumentToIssueCommand
        {
            get
            {
                return new BaseCommand(() => AddDocToIssue());
            }
        }

        public ICommand SaveIssueCommand
        {
            get
            {
                return new BaseCommand(() => SaveIssue());
            }
        }


        private Visibility _GridWithSearchDocumentVisibility;
        public Visibility GridWithSearchDocumentVisibility { get => _GridWithSearchDocumentVisibility; set { _GridWithSearchDocumentVisibility = value; } }
        public Visibility GirdWithAssociatedDocumentsVisibility { get => GridWithSearchDocumentVisibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible; }




        private void AddDocToIssue()
        {
            bool documentALreadyAdded = DocumentsOnIssueGridList.Where(x => x.DocNo == SelectedDocumentOnGrid.DocNo).Count() > 0;
            if (documentALreadyAdded)
            {
                ShowErrorMessageBox("Dokument został już dodany do sprawy");
                return;
            }
            DocumentsOnIssueGridList.Add(SelectedDocumentOnGrid);
            OnPropertyChanged(() => DocumentsOnIssueGridList);

        }

        private void SearchDocs()
        {
            var baseQuery = db.DocumentHeader.Where(row => row.IsEnable);

            if (!string.IsNullOrEmpty(SearchCriteria_DocType))
            {
                baseQuery = baseQuery.Where(row => row.DocumentType.Name == SearchCriteria_DocType);
            }

            if (!string.IsNullOrEmpty(SearchCriteria_DocNo))
            {
                baseQuery = baseQuery.Where(row => row.InternalNo.ToLower().Contains(SearchCriteria_DocNo.ToLower()));
            }

            if (SearchCriteria_DocDateFrom != null)
            {
                DateTime correctDateFrom = new DateTime(SearchCriteria_DocDateFrom.Value.Year, SearchCriteria_DocDateFrom.Value.Month, SearchCriteria_DocDateFrom.Value.Day, 0, 0, 0);
                baseQuery = baseQuery.Where(row => row.CreateDate <= correctDateFrom);
            }

            if (SearchCriteria_DocDateTo != null)
            {
                DateTime correctDateTo = new DateTime(SearchCriteria_DocDateTo.Value.Year, SearchCriteria_DocDateTo.Value.Month, SearchCriteria_DocDateTo.Value.Day, 0, 0, 0);
                baseQuery = baseQuery.Where(row => row.CreateDate <= correctDateTo);
            }

            DocumentsGridList = new ObservableCollection<DocumentGridView>(baseQuery.Select(row => new DocumentGridView
            {
                DocNo = row.InternalNo,
                DocType = row.DocumentType.Name,
                Author = row.Author.Login,
                CreateDate = (DateTime)row.CreateDate
            }).ToList());

            OnPropertyChanged(() => DocumentsGridList);
        }

        public DocumentGridView SelectedDocumentOnGrid { get; set; }

        public ObservableCollection<DocumentGridView> DocumentsGridList { get; set; }

        public ObservableCollection<DocumentGridView> DocumentsOnIssueGridList { get; set; }

        public string SearchCriteria_DocType { get; set; }
        public string SearchCriteria_DocNo { get; set; }
        public DateTime? SearchCriteria_DocDateFrom { get; set; }
        public DateTime? SearchCriteria_DocDateTo { get; set; }
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case "Debtor_Name":
                        result = StringValidator.IsEmpty(Debtor_Name);
                        break;
                    case "Customer_Name":
                        result = StringValidator.IsEmpty(Customer_Name);
                        break;
                    case "Currency":
                        result = StringValidator.IsEmpty(Currency);
                        break;
                    case "Debtor_Due":
                        result = StringValidator.IsNumber(Debtor_Due);
                        break;
                    case "InternalNo":
                        result = StringValidator.IsEmpty(InternalNo);
                        break;
                }

                return result;
            }
        }

        protected override bool IsValid()
        {
            return (string.IsNullOrEmpty(this["Debtor_Name"])
                && string.IsNullOrEmpty(this["Customer_Name"])
                && string.IsNullOrEmpty(this["Currency"])
                && string.IsNullOrEmpty(this["Debtor_Due"])
                && string.IsNullOrEmpty(this["InternalNo"])
                );
        }

        private void SaveIssue()
        {
            if (!IsValid())
            {
                ShowErrorMessageBox("Nie wszystkie pola zostały wypełnione poprawnie");
                return;
            }

            var currency = db.Currency.Where(row => row.IsEnable && row.Name == Currency).FirstOrDefault();
            var customer = db.Customer.Find(_selectedCustomer.Id);
            var debtor = db.Debtor.Find(_selectedDebtor.Id);

            var newIssue = new Issue
            {
                Currency = currency,
                Customer = customer,
                Debtor = debtor,
                InternalNo = InternalNo,
                IsFinished = false,
                Cost = decimal.Parse(Debtor_Due)
            };

            foreach (var doc in DocumentsOnIssueGridList)
            {
                var docDto = db.DocumentHeader.Where(x => x.InternalNo == InternalNo).FirstOrDefault();
                var colaboration = new DocumentToIssue { DocumentHeader = docDto, Issue = newIssue };
                db.DocumentToIssue.Add(colaboration);
            }

            db.Issue.Add(newIssue);

            db.SaveChanges();
            OnRequestClose();

        }

        private CustomerGridDto _selectedCustomer;
        private DebtorGridDto _selectedDebtor;

        private void GetClientDto(CustomerGridDto customerDto)
        {
            Customer_Name = customerDto.Name;
            Customer_Location = customerDto.City + " ul. " + customerDto.Street;
            _selectedCustomer = customerDto;
            OnPropertyChanged(() => Customer_Location);
            OnPropertyChanged(() => Customer_Name);
        }

        private void GetDebtorDto(DebtorGridDto debtorDto)
        {
            Debtor_Name = debtorDto.Name;
            Debtor_Location = debtorDto.City + " ul. " + debtorDto.Street;
            _selectedDebtor = debtorDto;
            OnPropertyChanged(() => Debtor_Name);
            OnPropertyChanged(() => Debtor_Location);
        }
    }
}