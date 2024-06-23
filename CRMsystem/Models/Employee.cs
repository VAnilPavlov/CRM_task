namespace CRMsystem.Models
{
    public class Employee
    {
        public Guid Id { set; get; }
        public required string FullName { set; get; }
        public required string Title { set; get; }
        public ICollection<Task>? Tasks { set; get; }
    }
}
