using System.ComponentModel.DataAnnotations.Schema;

namespace Windykator_PRO.Database.Models
{
    [Table("PaymentMethod")]
    public class PaymentMethod : BaseModel
    {
        public string Name { get; set; }
    }
}