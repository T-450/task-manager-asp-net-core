using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task_manager.Core.Models;

namespace task_manager.Infrastructure.Data.Mappings
{
    public class TaskModelMapping : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder)
        {
            builder.HasKey(taskModel => taskModel.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(1000)");
            builder.Property(p => p.Completed)
                .HasColumnType("BOOLEAN")
                .HasDefaultValue(false)
                .IsRequired();
            builder.ToTable("Tasks");
        }
    }
}
