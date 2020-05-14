using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TaskManager.Domain;

namespace TaskManager.Data
{
    public class WorkItemData
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        [MaxLength(200)]
        public string Executors { get; set; }
        public WorkItemStatus Status { get; set; }
        public int PlannedExecutionTime { get; set; }
        public int ActualExecutionTime { get; set; }
        public DateTime? EndedAt { get; set; }
        public WorkItemData Parent { get; set; }
        public ICollection<WorkItemData> Children { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }

        public WorkItemData()
        {
            Children = new List<WorkItemData>();
        }
    }
}
