using System.ComponentModel.DataAnnotations;
using Windykator_PRO.Database.Models.Base;

namespace Windykator_PRO.Database.Models
{
    public class Customer : TimeStampBaseModel
    {
        /// <summary>
        /// Nazwa klienta
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        public virtual Adress Adress { get; set; }

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
        public bool IsLocalCustomer { get; set; }

        /// <summary>
        /// Klient zagraniczny
        /// </summary>
        public bool IsForeignCustomer { get; set; }

        public virtual BankAccount BankAccount { get; set; }
    }
}