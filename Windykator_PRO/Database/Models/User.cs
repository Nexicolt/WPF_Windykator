using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Windykator_PRO.Database.Models.Base;

namespace Windykator_PRO.Database.Models
{
    [Table("User")]
    public class User : TimeStampBaseModel
    {
        [StringLength(50)]
        public string Login { get; set; }

        [StringLength(100)]
        public string Password { get; set; }
    }
}