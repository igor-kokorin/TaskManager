using System;

namespace TaskManager.Domain
{
    public class WorkItemSnapshot
    {
        public WorkItemSnapshot()
        {
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Executors { get; set; }
        public int PlannedExecutionTime { get; set; }
        public int ActualExecutionTime { get; set; }
        public WorkItemStatus Status { get; set; }
        public DateTime? EndedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}