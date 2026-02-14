namespace EmployeeTaskManager.Models
{
    public class TaskAudit
    {
        public int Id { get; set; }

        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; }

        public int UserId { get; set; }

        public string Action { get; set; }

        public DateTime ActionDate { get; set; } = DateTime.Now;
    }
}
