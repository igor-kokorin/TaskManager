using Microsoft.EntityFrameworkCore;
using System;

namespace TaskManager.Data
{
    public class TaskManagerContext: DbContext
    {
        public DbSet<WorkItemData> WorkItems { get; set; }

        public TaskManagerContext(DbContextOptions<TaskManagerContext> opts): base(opts)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
