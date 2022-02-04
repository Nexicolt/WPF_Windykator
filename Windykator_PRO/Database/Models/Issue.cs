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
    }
}