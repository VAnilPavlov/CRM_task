namespace CRMsystem.Models
{
    public class Task
    {
        public Guid Id { set; get; }
        public Guid EmployeeId { set; get; }
        public Employee? Employee { set; get; }
        public required string Title { set; get; }  
        public required string Description { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public int Percent { set; get; } = 0;
    }
}
