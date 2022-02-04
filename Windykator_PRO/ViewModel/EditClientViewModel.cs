using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using Windykator_PRO.Database;

namespace Windykator_PRO.ViewModel
{
    public class EditClientViewModel : AddNewClientViewModel
    {
        private long CustomerId { get; set; }

        private VindicationDatabase db { get; set; }

        public EditClientViewModel(long customerId)
        {
            this.DisplayName = "Edycja klienta";
            this.CustomerId = customerId;
            this.db = new VindicationDatabase();

            //Załąduj dane klienta
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            var customer = db.Customer.Where(row => row.Id == CustomerId && row.IsEnable).FirstOrDefault();
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
            IsForeignCustomer = customer.IsForeignCustomer;
            IsIndyvidual = customer.IsIndyvidual;
            IsCompany = customer.IsCompany;
            IsLocalCustomer = customer.IsLocalCustomer;
        }

        protected override void Save()
        {
            bool customerAlreadyExists = db.Customer.Where(row => row.Id != CustomerId && row.Name == Name).Count() > 0;
            if (customerAlreadyExists)
            {
                ShowErrorMessageBox("Klient o takiej nazwie już istnieje");
                return;
            }

            var customer = db.Customer.Find(CustomerId);
            if (customer == null) ShowErrorMessageBox("brak klienta o podanym identyfikatorze");
            else
            {
                customer.Name = Name;
                customer.Adress.City = City;
                customer.Adress.Street = Street;
                customer.BankAccount.BankAccountNumber = BankAccountNumber;
                customer.BankAccount.BankName = BankName;
                customer.IsCompany = IsCompany;
                customer.IsLocalCustomer = IsLocalCustomer;
                customer.IsForeignCustomer = IsForeignCustomer;
                customer.IsLocalCustomer = IsLocalCustomer;
                customer.BankAccount.IBAN = IBAN;
                db.SaveChanges();
                Messenger.Default.Send("REFRESH");
                OnRequestClose();
            }
        }
    }
}