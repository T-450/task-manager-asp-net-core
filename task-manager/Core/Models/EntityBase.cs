namespace task_manager.Core.Models
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
