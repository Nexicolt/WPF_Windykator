using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windykator_PRO.Database;
using Windykator_PRO.Database.Models;
using Windykator_PRO.Helpers;
using Windykator_PRO.Validation;

namespace Windykator_PRO.ViewModel
{
    public class AddNewDebtorViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        private VindicationDatabase db = null;

        #region Constructor

        public AddNewDebtorViewModel()
        {
            base.DisplayName = "Nowy dłużnik";
            IsIndyvidual = true;
            IsForeignDebtor = true;

            //Baza danych
            db = new VindicationDatabase();
        }

        #endregion Constructor

        #region InputFields

        /// <summary>
        /// Nazwa klienta
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Miasto
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Ulica
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Czy typ klienta, to osoba fizyczna
        /// </summary>
        public bool IsIndyvidual { get; set; }

        /// <summary>
        /// Czy typ klienta, to firma
        /// </summary>
        public bool IsCompany { get; set; }

        /// <summary>
        /// Klient niezagraniczny
        /// </summary>
        public bool IsLocalDebtor { get; set; }

        /// <summary>
        /// Klient zagraniczny
        /// </summary>
        public bool IsForeignDebtor { get; set; }

        public string IBAN { get; set; }

        public string BankName { get; set; }

        public string BankAccountNumber { get; set; }

        #endregion InputFields

        #region Validation

        public string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case "Name":
                        result = StringValidator.IsEmpty(Name);
                        break;

                    case "City":
                        result = StringValidator.IsEmpty(City);
                        break;

                    case "Street":
                        result = StringValidator.IsEmpty(Street);
                        break;

                    case "IBAN":
                        result = StringValidator.IsEmpty(IBAN);
                        break;

                    case "BankName":
                        result = StringValidator.IsEmpty(BankName);
                        break;

                    case "BankAccountNumber":
                        result = StringValidator.IsEmpty(BankAccountNumber);
                        if (result is null)
                            result = StringValidator.IsNumber(BankAccountNumber);
                        break;
                }

                return result;
            }
        }

        public string Error => null;

        #endregion Validation

        #region Commands

        private BaseCommand _saveDebtor;

        public ICommand SaveDebtor
        {
            get
            {
                if (_saveDebtor == null)
                    _saveDebtor = new BaseCommand(() => this.Save());
                return _saveDebtor;
            }
        }

        #endregion Commands

        #region Functions

        protected virtual void Save()
        {
            if (debtorAlreadyExists(Name))
            {
                ShowErrorMessageBox("Dłużnik o takiej nazwie już istnieje");
                return;
            }

            var newAdressData = new Adress
            {
                City = City,
                Street = Street
            };

            var newBankAccount = new BankAccount
            {
                IBAN = IBAN,
                BankAccountNumber = BankAccountNumber,
                BankName = BankName
            };

            var newClientDto = new Debtor
            {
                Name = Name,
                Adress = newAdressData,
                BankAccount = newBankAccount,
                IsCompany = IsCompany,
                IsLocalDebtor = IsLocalDebtor,
                IsIndyvidual = IsIndyvidual,
                IsForeignDebtor = IsForeignDebtor
            };

            try
            {
                db.Debtor.Add(newClientDto);
                db.SaveChanges();
            }
            catch (System.Exception)
            {
                ShowErrorMessageBox("Wystapił błąd podczas dodawania dłużnika.\nOperacja nieudana");
            }
            SendRefreshSignal();
            this.OnRequestClose();
        }

        /// <summary>
        /// Sprawdza, czy klient o takiej nazwie, nie istnieje już w systemie
        /// </summary>
        private bool debtorAlreadyExists(string customerName)
        {
            return db.Debtor.Where(row => row.IsEnable && row.Name.ToLower() == customerName.ToLower()).Count() > 0;
        }

        #endregion Functions
    }
}