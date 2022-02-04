using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Windykator_PRO.Database.Models
{
    [Table("Adress")]
    public class Adress : BaseModel
    {
        /// <summary>
        /// Miasto
        /// </summary>
        [StringLength(50)]
        public string City { get; set; }

        /// <summary>
        /// Ulica
        /// </summary
        [StringLength(50)]
        public string Street { get; set; }
    }
}