using Windykator_PRO.Database.Models.Base;

namespace Windykator_PRO.Database.Models
{
    public class IssueAddnotation : TimeStampBaseModel
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public virtual User Author{ get; set; }
        public virtual  Issue Issue{ get; set; }
    }
}
