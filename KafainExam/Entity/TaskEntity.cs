namespace KafainExam.Entity
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskState Status { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
    public enum TaskState
    {
        Progress,
        InProgres,
        Completed
    }
}
