namespace CRMsystem.Models
{
    public class ReportData
    {
        public string? FullName{ get; set; }
        public string? Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Percent { get; set; }
        public int Overdue { get; set; }
    }
}
