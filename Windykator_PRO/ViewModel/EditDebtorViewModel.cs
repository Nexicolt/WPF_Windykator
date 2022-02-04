using System.Linq;
using Windykator_PRO.Database;

namespace Windykator_PRO.ViewModel
{
    public class EditDebtorViewModel : AddNewDebtorViewModel
    {
        private long DebtorId { get; set; }

        private VindicationDatabase db { get; set; }

        public EditDebtorViewModel(long debtorId)
        {
            this.DisplayName = "Edycja dłużnika";
            this.DebtorId = debtorId;
            this.db = new VindicationDatabase();

            //Załąduj dane klienta
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            var customer = db.Debtor.Where(row => row.Id == DebtorId && row.IsEnable).FirstOrDefault();
            if (customer == null)
            {
                ShowErrorMessageBox("Brak klient,a o podanym identyfikatorze");
                OnRequestClose();
            }

            Name = customer.Name;
            City = customer.Adress.City;
            Street = customer.Adress.Street;
            IBAN = customer.BankAccount.IBAN;
            BankAccountNumber = customer.BankAccount.BankAccountNumber;
            BankName = customer.BankAccount.BankName;
            IsForeignDebtor = customer.IsForeignDebtor;
            IsIndyvidual = customer.IsIndyvidual;
            IsCompany = customer.IsCompany;
            IsLocalDebtor = customer.IsLocalDebtor;
        }

        protected override void Save()
        {
            bool customerAlreadyExists = db.Debtor.Where(row => row.Id != DebtorId && row.Name == Name).Count() > 0;
            if (customerAlreadyExists)
            {
                ShowErrorMessageBox("Dłużnik o takiej nazwie już istnieje");
                return;
            }

            var customer = db.Debtor.Find(DebtorId);
            if (customer == null) ShowErrorMessageBox("Brak dłużnika o podanym identyfikatorze");
            else
            {
                customer.Name = Name;
                customer.Adress.City = City;
                customer.Adress.Street = Street;
                customer.BankAccount.BankAccountNumber = BankAccountNumber;
                customer.BankAccount.BankName = BankName;
                customer.IsCompany = IsCompany;
                customer.IsLocalDebtor = IsLocalDebtor;
                customer.IsForeignDebtor = IsForeignDebtor;
                customer.BankAccount.IBAN = IBAN;
                db.SaveChanges();

                SendRefreshSignal();
                OnRequestClose();
            }
        }
    }
}