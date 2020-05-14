using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain;

namespace TaskManager.Web.Models
{
    public class WorkItemDisplayDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        [DisplayName("Название")]
        public string Title { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        [DisplayName("Исполнители")]
        public string Executors { get; set; }
        [DisplayName("Плановая трудоёмкость")]
        public TimeSpan PlannedExecutionTime { get; set; }
        [DisplayName("Фактическое время выполнения")]
        public TimeSpan ActualExecutionTime { get; set; }
        [DisplayName("Общяя плановая трудоёмкость")]
        public TimeSpan TotalPlannedExecutionTime { get; set; }
        [DisplayName("Общее фактическое время выполнения")]
        public TimeSpan TotalActualExecutionTime { get; set; }
        [DisplayName("Статус")]
        public WorkItemStatus Status { get; set; }
        [DisplayName("Дата завершения")]
        public DateTime? EndedAt { get; set; }
        [DisplayName("Дата создания")]
        public DateTime CreatedAt { get; set; }
        public bool CanDelete { get; set; }
        public List<WorkItemDisplayDto> Children { get; set; }

        public WorkItemDisplayDto()
        {
            Children = new List<WorkItemDisplayDto>();
        }
    }
}
