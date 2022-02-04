using System.ComponentModel.DataAnnotations.Schema;

namespace Windykator_PRO.Database.Models
{
    [Table("Currency")]
    public class Currency : BaseModel
    {
        public string Name { get; set; }
    }
}