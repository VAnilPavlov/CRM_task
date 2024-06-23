namespace CRMsystem.Models
{
    public class EmployeeForView
    {
        public Guid Id { set; get; }
        public string? FullName { set; get; }
        public string? Title { set; get; }
        public int? TaskCount { set; get; } = 0;
        public int? TaskDonePercent { set; get; } = 0;
    }
}
