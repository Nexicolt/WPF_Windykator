using Windykator_PRO.Database.Models.Base;

namespace Windykator_PRO.Database.Models
{
    public class DocumentHeader : TimeStampBaseModel
    {
        public string InternalNo { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual User Author { get; set; }

        public virtual DocumentType DocumentType { get; set; }
    }
}