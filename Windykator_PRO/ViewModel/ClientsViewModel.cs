using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windykator_PRO.Database;
using Windykator_PRO.Helpers;

namespace Windykator_PRO.ViewModel
{
    /// <summary>
    /// Reprezentant wiersza, w gridzie
    /// </summary>
    internal class CustomerGridDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public string Street { get; set; }

        public int IssuesQuantity { get; set; }

        public decimal Due { get; set; }
        public string Currency { get; set; }
    }

    internal class ClientsViewModel : AllViewModel<CustomerGridDto>
    {
        #region Constructor

        private VindicationDatabase db = null;

        public ClientsViewModel()
        {
            base.DisplayName = "Klienci";
            db = new VindicationDatabase();
            SearchCriteria_IsIndyvidual = true;
        }

        #endregion Constructor

        #region Properties

        public string SearchCriteria_Name { get; set; }
        public string SearchCriteria_City { get; set; }
        public string SearchCriteria_Street { get; set; }
        public string SearchCriteria_SelectedCurrency { get; set; }
        public bool SearchCriteria_IsIndyvidual { get; set; }

        public bool SearchCriteria_IsCompany { get => !SearchCriteria_IsIndyvidual; }
        public bool SearchCriteria_DuringCourtProcess { get; set; }
        public bool SearchCriteria_NotDuringCourtProcess { get => !SearchCriteria_DuringCourtProcess; }

        public decimal? SearchCriteria_DueFrom { get; set; }
        public decimal? SearchCriteria_DueTo { get; set; }

        public CustomerGridDto SelectedItemOnGrid { get; set; }

        /// <summary>
        /// Lista walut dla comboboxa
        /// </summary>
        public List<string> _currencyForComboBox { get; set; }

        public List<string> CurrencyForComboBox
        {
            get
            {
                if (_currencyForComboBox == null)
                    _currencyForComboBox = db.Currency.Select(row => row.Name).ToList();
                return _currencyForComboBox;
            }
        }

        #endregion Properties

        #region Commands

        public ICommand CreateClientCommand
        {
            get
            {
                return MainWindowViewModel.MainWindowHandler.CreateClientCommand;
            }
        }

        public ICommand EditClientCommand
        {
            get
            {
                return new BaseCommand(() => EditClient());
            }
        }

        private void EditClient()
        {
            if (SelectedItemOnGrid is null)
            {
                ShowErrorMessageBox("Należy wskazać klienta z listy");
                return;
            }

            MainWindowViewModel.MainWindowHandler.EditClientCommand(SelectedItemOnGrid.Id);
        }

        public ICommand Import
        {
            get
            {
                return MainWindowViewModel.MainWindowHandler.Import;
            }
        }

        public ICommand DeleteCustomer
        {
            get => new BaseCommand(() => DeleteCustomerFunc());
        }

        #endregion Commands

        /// <summary>
        /// Lista dla grida
        /// </summary>

        protected override void LoadGridData()
        {
            Search();
        }

        private void DeleteCustomerFunc()
        {
            if (SelectedItemOnGrid == null)
            {
                ShowErrorMessageBox("Należy wskazać klienta na liście");
                return;
            }

            //SPrawdź,czy mogę usunąć
            bool haveAsignedIssue = db.Issue.Where(row => row.Customer.Id == SelectedItemOnGrid.Id).Count() > 0;
            if (haveAsignedIssue)
            {
                ShowErrorMessageBox("Klient posiada przypisaną, conajmniej jedną sprawę.\nUsunięcie niemożliwe");
                return;
            }

            var customer = db.Customer.Where(row => row.Id == SelectedItemOnGrid.Id).FirstOrDefault();
            var adress = db.Adress.Where(row => row.Id == customer.Adress.Id).FirstOrDefault();

            if (adress != null) customer.Delete();
            if (customer != null) customer.Delete();

            db.SaveChanges();
        }

        protected override void Search()
        {
            var baseQueryable = db.Customer_GridView.AsQueryable();
            if (!string.IsNullOrEmpty(SearchCriteria_Name))
            {
                baseQueryable = baseQueryable.Where(row => row.Name.ToLower().Contains(SearchCriteria_Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(SearchCriteria_City))
            {
                baseQueryable = baseQueryable.Where(row => row.City.ToLower().Contains(SearchCriteria_City.ToLower()));
            }
            if (!string.IsNullOrEmpty(SearchCriteria_Street))
            {
                baseQueryable = baseQueryable.Where(row => row.Street.ToLower().Contains(SearchCriteria_Street.ToLower()));
            }
            if (SearchCriteria_IsIndyvidual)
            {
                baseQueryable = baseQueryable.Where(row => row.IsIndyvidual);
            }
            else
            {
                baseQueryable = baseQueryable.Where(row => !row.IsIndyvidual);
            }

            if (SearchCriteria_DuringCourtProcess)
            {
                baseQueryable = baseQueryable.Where(row => row.DuringDuringCourtProcess);
            }
            else
            {
                baseQueryable = baseQueryable.Where(row => !row.DuringDuringCourtProcess);
            }

            if (SearchCriteria_DueFrom.HasValue)
            {
                baseQueryable = baseQueryable.Where(row => row.Due >= SearchCriteria_DueFrom.Value);
            }
            if (SearchCriteria_DueTo.HasValue)
            {
                baseQueryable = baseQueryable.Where(row => row.Due <= SearchCriteria_DueTo.Value);
            }
            if (!string.IsNullOrEmpty(SearchCriteria_SelectedCurrency))
            {
                baseQueryable = baseQueryable.Where(row => row.Currency == SearchCriteria_SelectedCurrency);
            }

            GridList = new ObservableCollection<CustomerGridDto>(baseQueryable.Select(row => new CustomerGridDto
            {
                Id = row.Id,
                Name = row.Name,
                City = row.City,
                Street = row.Street,
                IssuesQuantity = row.IssuesCount,
                Currency = row.Currency,
                Due = row.Due
            }).ToList());
        }
    }
}