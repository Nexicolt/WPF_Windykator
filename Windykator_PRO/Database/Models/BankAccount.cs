using System.ComponentModel.DataAnnotations;
using Windykator_PRO.Database.Models.Base;

namespace Windykator_PRO.Database.Models
{
    public class BankAccount : TimeStampBaseModel
    {
        [StringLength(20)]
        public string IBAN { get; set; }

        [StringLength(50)]
        public string BankName { get; set; }

        [StringLength(17)]
        public string BankAccountNumber { get; set; }
    }
}