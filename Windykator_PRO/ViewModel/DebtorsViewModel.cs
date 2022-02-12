using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Windykator_PRO.Database;
using Windykator_PRO.Helpers;

namespace Windykator_PRO.ViewModel
{
    internal class DebtorGridDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int IssuesQuantity { get; set; }

        private decimal _debt;
        public decimal? Debt
        {
            get => _debt; 
            set
            {
                if(value == null)
                    value = 0;
                _debt = (decimal)value;
            }
        }
        public string Currency { get; set; }
    }

    internal class DebtorsViewModel : AllViewModel<DebtorGridDto>
    {
        private VindicationDatabase db = null;

        public DebtorsViewModel(bool selectButton = false)
        {
            this.DisplayName = "Dłużnicy";
            db = new VindicationDatabase();
            SearchCriteria_IsIndyvidual = false;
            SelectDebtorButtonVisbility = (!selectButton) ? Visibility.Hidden : Visibility.Visible;

        }

        #region Properties

        public Visibility SelectDebtorButtonVisbility { get; set; }
        public string SearchCriteria_Name { get; set; }
        public string SearchCriteria_City { get; set; }
        public string SearchCriteria_Street { get; set; }
        public string SearchCriteria_SelectedCurrency { get; set; }
        public bool SearchCriteria_IsIndyvidual { get; set; }

        public bool SearchCriteria_IsCompany { get => !SearchCriteria_IsIndyvidual; }
        public bool SearchCriteria_DuringCourtProcess { get; set; }
        public bool SearchCriteria_NotDuringCourtProcess { get => !SearchCriteria_DuringCourtProcess; }

        public decimal? SearchCriteria_DebtFrom { get; set; }
        public decimal? SearchCriteria_DebtTo { get; set; }

        public DebtorGridDto SelectedItemOnGrid { get; set; }

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

        public ICommand CreateDebtorCommand
        {
            get
            {
                return MainWindowViewModel.MainWindowHandler.CreateDebtorCommand;
            }
        }

        public ICommand EditDebtorCommand
        {
            get
            {
                return new BaseCommand(() => EditDebtor());
            }
        }

        private void EditDebtor()
        {
            if (SelectedItemOnGrid is null)
            {
                ShowErrorMessageBox("Należy wskazać dłużnika z listy");
                return;
            }

            MainWindowViewModel.MainWindowHandler.EditDebtorCommand(SelectedItemOnGrid.Id);
        }

        public ICommand Import
        {
            get
            {
                return MainWindowViewModel.MainWindowHandler.Import;
            }
        }

        public ICommand DeleteDebtorCommand { get => new BaseCommand(() => DeleteDebtor()); }

        public ICommand SelectDebtorCommand
        {

            get => new BaseCommand(() => SelectDebtor());
        }

        private void SelectDebtor()
        {
            if (SelectedItemOnGrid is null)
            {
                ShowErrorMessageBox("Należy wskazać klienta z listy");
                return;
            }
            Messenger.Default.Send(SelectedItemOnGrid);
            OnRequestClose();

        }
        #endregion Commands

        /// <summary>
        /// Lista dla grida
        /// </summary>

        protected override void LoadGridData()
        {
            Search();
        }

        private void DeleteDebtor()
        {
            if (SelectedItemOnGrid == null)
            {
                ShowErrorMessageBox("Należy wskazać dłużnika na liście");
                return;
            }

            //SPrawdź,czy mogę usunąć
            bool haveAsignedIssue = db.Issue.Where(row => row.Debtor.Id == SelectedItemOnGrid.Id).Count() > 0;
            if (haveAsignedIssue)
            {
                ShowErrorMessageBox("Dłużnik posiada przypisaną, conajmniej jedną sprawę.\nUsunięcie niemożliwe");
                return;
            }

            var customer = db.Debtor.Where(row => row.Id == SelectedItemOnGrid.Id).FirstOrDefault();
            var adress = db.Adress.Where(row => row.Id == customer.Adress.Id).FirstOrDefault();

            if (adress != null) customer.Delete();
            if (customer != null) customer.Delete();

            GridList.Remove(SelectedItemOnGrid);

            db.SaveChanges();
        }

        protected override void Search()
        {
            var baseQueryable = db.Debtor_GridView.AsQueryable();
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

            if (SearchCriteria_DebtFrom.HasValue)
            {
                baseQueryable = baseQueryable.Where(row => row.Debt >= SearchCriteria_DebtFrom.Value);
            }
            if (SearchCriteria_DebtTo.HasValue)
            {
                baseQueryable = baseQueryable.Where(row => row.Debt <= SearchCriteria_DebtTo.Value);
            }
            if (!string.IsNullOrEmpty(SearchCriteria_SelectedCurrency))
            {
                baseQueryable = baseQueryable.Where(row => row.Currency == SearchCriteria_SelectedCurrency);
            }

            GridList = new ObservableCollection<DebtorGridDto>(baseQueryable.Select(row => new DebtorGridDto
            {
                Id = row.Id,
                Name = row.Name,
                City = row.City,
                Street = row.Street,
                IssuesQuantity = row.IssuesCount,
                Currency = row.Currency,
                Debt = row.Debt
            }).ToList());
        }
        protected override void ClearSearchCriteria()
        {
            SearchCriteria_IsIndyvidual = false;
            SearchCriteria_DuringCourtProcess = false;

            ClearAllProperties(this); //Czyści tylko stringi i combobox'y

            LoadGridData();
        }
    }
}