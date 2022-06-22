namespace task_manager.Core.Models
{
    public abstract class EntityBase
    {
        public Guid Id { get; }

        protected EntityBase()
        {
            Id = Guid.NewGuid();
        }
    }
}
