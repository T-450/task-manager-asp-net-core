using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using task_manager.Core.Models;

namespace task_manager.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // More info => https://docs.microsoft.com/en-us/ef/core/querying/tracking
            // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<TaskModel> TaskItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var modelProperties = modelBuilder
                .Model
                .GetEntityTypes()
                .SelectMany(e => e.GetProperties())
                .Where(p => p.ClrType == typeof(string));

            var entitiesForeignKeys = modelBuilder
                .Model
                .GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys());

            // Defaults string types to a limit numbers of characters
            foreach (IMutableProperty property in modelProperties) property.SetColumnType("varchar(100)");

            // Disables On Delete Cascade
            foreach (IMutableForeignKey relationship in entitiesForeignKeys)
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EntityBase && e.State is EntityState.Added or EntityState.Modified);

            foreach (EntityEntry entityEntry in entries)
            {
                ((EntityBase) entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added) ((EntityBase) entityEntry.Entity).CreatedAt = DateTime.Now;
            }

            return base.SaveChangesAsync(cancellationToken).GetAwaiter().GetResult();
        }
    }
}
