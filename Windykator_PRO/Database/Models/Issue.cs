using System.ComponentModel.DataAnnotations.Schema;
using Windykator_PRO.Database.Models.Base;

namespace Windykator_PRO.Database.Models
{
    [Table("Issue")]
    public class Issue : TimeStampBaseModel
    {
        public string InternalNo { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual Debtor Debtor { get; set; }

        public decimal Cost { get; set; }

        public virtual Currency Currency { get; set; }

        public bool IsFinished { get; set; }
    }
}