namespace Windykator_PRO.Database.Views
{
    public class Customer_GridView
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public bool IsIndyvidual { get; set; }
        public bool DuringDuringCourtProcess { get; set; }
        public int IssuesCount { get; set; }
        public decimal Due { get; set; }
        public string Currency { get; set; }
    }
}