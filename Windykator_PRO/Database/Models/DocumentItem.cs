using Windykator_PRO.Database.Models.Base;

namespace Windykator_PRO.Database.Models
{
    public class DocumentItem : TimeStampBaseModel
    {
        public virtual DocumentHeader DocumentHeader { get; set; }

        public virtual Service Service { get; set; }

        public decimal Quantity { get; set; }
    }
}