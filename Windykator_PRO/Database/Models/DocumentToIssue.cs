namespace Windykator_PRO.Database.Models
{
    public class DocumentToIssue : BaseModel
    {
        public virtual DocumentHeader DocumentHeader { get; set; }
        public virtual Issue Issue { get; set; }
    }
}