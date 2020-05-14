using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TaskManager.Domain;

namespace TaskManager.Web.Models
{
    public class WorkItemUpdateDto
    {
        public int Id { get; set; }
        [DisplayName("Наименование")]
        public string Title { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        [DisplayName("Исполнители")]
        public string Executors { get; set; }
        [DisplayName("Плановая трудоёмкость")]
        public TimeSpan PlannedExecutionTime { get; set; }
        [DisplayName("Фактическое время выполнения")]
        public TimeSpan ActualExecutionTime { get; set; }
        public WorkItemStatus Status { get; set; }
        public IEnumerable<WorkItemStatus> AllowedValues { get; set; }
    }
}