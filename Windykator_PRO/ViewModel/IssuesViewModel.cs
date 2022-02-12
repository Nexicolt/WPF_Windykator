using GalaSoft.MvvmLight.Messaging;
using System;
using System.Linq;
using System.Windows.Input;
using Windykator_PRO.Database;

namespace Windykator_PRO.ViewModel
{

    public class IssueGridView
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string InternalNo { get; set; }
        public string Debtor { get; set; }
        public string Client { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }
    }
    public class IssuesViewModel : AllViewModel<IssueGridView>
    {
        private VindicationDatabase db = null;
        public IssuesViewModel()
        {
            this.DisplayName = "Sprawy";
            Messenger.Default.Register<CustomerGridDto>(this, GetClientDto);
            Messenger.Default.Register<DebtorGridDto>(this, GetDebtorDto);
            db = new VindicationDatabase();

            SearchCriteria_IsCompany = true;

            LoadGridData();
        }

        public ICommand CreateClientCommand
        {
            get
            {
                return MainWindowViewModel.MainWindowHandler.CreateIssueCommand;
            }
        }

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


        public ICommand CreateNewIssueCommand
        {
            get => MainWindowViewModel.MainWindowHandler.CreateIssueCommand;
        }

        public string SearchCriteria_Debtor { get; set; }
        public DateTime? SearchCriteria_DateFrom { get; set; }
        public DateTime? SearchCriteria_DateTo { get; set; }
        public string SearchCriteria_InternalNo { get; set; }
        public bool SearchCriteria_IsCompany { get; set; }
        public bool SearchCriteria_IsFinished { get; set; }
        public bool SearchCriteria_IsDuring { get; set; }
        public bool SearchCriteria_IsIndyvidualCustomer { get => !SearchCriteria_IsCompany; }
        public string SearchCriteria_Client { get; set; }

        private CustomerGridDto _selectedCustomer;
        private DebtorGridDto _selectedDebtor;

        private IssueGridView _SelectedDocumentOnGrid;
        public IssueGridView SelectedDocumentOnGrid
        {
            get => _SelectedDocumentOnGrid;

            set
            {
                _SelectedDocumentOnGrid = value;
                if (value != null)
                    MainWindowViewModel.MainWindowHandler.IssueDetailsToShowId = value.Id;

                
            }
        }

        public ICommand IssueDetailsCommand { get => MainWindowViewModel.MainWindowHandler.IssueDetailsCommand; }
        private void GetClientDto(CustomerGridDto customerDto)
        {
            SearchCriteria_Client = $"{customerDto.Name}, {customerDto.City} ul. {customerDto.Street} ";
            _selectedCustomer = customerDto;
            OnPropertyChanged(() => SearchCriteria_Client);
        }

        private void GetDebtorDto(DebtorGridDto debtorDto)
        {
            SearchCriteria_Debtor = $"{debtorDto.Name}, {debtorDto.City} ul. {debtorDto.Street} ";
            _selectedDebtor = debtorDto;
            OnPropertyChanged(() => SearchCriteria_Debtor);
        }

        protected override void LoadGridData()
        {
            GridList = new System.Collections.ObjectModel.ObservableCollection<IssueGridView>
                (
                db.Issue.Where(row => row.IsEnable).Select(row => new IssueGridView
                {
                    Id = row.Id,
                    Client = row.Customer.Name,
                    Cost = row.Cost,
                    Date = row.CreateDate.Value,
                    Debtor = row.Debtor.Name,
                    InternalNo = row.InternalNo,
                    Status = (row.IsFinished) ? "FIN" : "PEN"
                }).ToList()
                );
            OnPropertyChanged(() => GridList);
        }

        protected override void Search()
        {

            var baseQuery = db.Issue.Where(row => row.IsEnable);
            if (SearchCriteria_DateFrom != null)
            {
                DateTime startDate = new DateTime(SearchCriteria_DateFrom.Value.Year, SearchCriteria_DateFrom.Value.Month, SearchCriteria_DateFrom.Value.Day, 0, 0, 0);
                baseQuery = baseQuery.Where(row => row.CreateDate >= startDate);
            }
            if (SearchCriteria_DateTo != null)
            {
                DateTime finishDate = new DateTime(SearchCriteria_DateTo.Value.Year, SearchCriteria_DateTo.Value.Month, SearchCriteria_DateTo.Value.Day, 23, 59, 59);

                baseQuery = baseQuery.Where(row => row.CreateDate >= finishDate);
            }
            if (!string.IsNullOrEmpty(SearchCriteria_InternalNo))
            {
                baseQuery = baseQuery.Where(row => row.InternalNo.ToLower().Contains(SearchCriteria_InternalNo.ToLower()));
            }
            if (SearchCriteria_IsIndyvidualCustomer)
            {
                baseQuery = baseQuery.Where(row => row.Customer.IsIndyvidual);//Trzeci
            }
            else
            {
                baseQuery = baseQuery.Where(row => row.Customer.IsCompany);
            }

            if (SearchCriteria_IsDuring)
            {
                baseQuery = baseQuery.Where(row => !row.IsFinished);
            }
            if (SearchCriteria_IsFinished)
            {
                baseQuery = baseQuery.Where(row => row.IsFinished);
            }
            if (_selectedCustomer != null)
            {
                baseQuery = baseQuery.Where(row => row.Customer.Id == _selectedCustomer.Id);
            }
            if (_selectedDebtor != null)
            {
                baseQuery = baseQuery.Where(row => row.Debtor.Id == _selectedDebtor.Id);
            }

            GridList = new System.Collections.ObjectModel.ObservableCollection<IssueGridView>
                (
                baseQuery.Select(row => new IssueGridView
                {
                    Id = row.Id,
                    Client = row.Customer.Name,
                    Cost = row.Cost,
                    Date = row.CreateDate.Value,
                    Debtor = row.Debtor.Name,
                    InternalNo = row.InternalNo,
                    Status = (row.IsFinished) ? "FIN" : "PEN"
                }).ToList()
                );
            OnPropertyChanged(() => GridList);
        }

        protected override void ClearSearchCriteria()
        {
            SearchCriteria_IsCompany = true;
            SearchCriteria_IsDuring = false;
            SearchCriteria_IsFinished = false;

            _selectedCustomer = null;
            SearchCriteria_Debtor = "";
            SearchCriteria_Client = "";
            SearchCriteria_DateFrom = null;
            SearchCriteria_DateTo = null;
            _selectedDebtor = null;

            ClearAllProperties(this);
            LoadGridData();
        }
    }
}