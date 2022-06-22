namespace task_manager.Core.Models
{
    public class TaskModel : EntityBase
    {
        public string Name { get; set; }
        public bool Completed { get; set; } = false;
    }
}
