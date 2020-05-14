using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain;
using TaskManager.Web.Models;

namespace TaskManager.Web.Utilities
{
    static public class Mappers
    {
        static public IEnumerable<WorkItemListDto> Map(IEnumerable<WorkItem> workItems)
        {
            var list = new List<WorkItemListDto>();

            foreach (var workItem in workItems)
            {
                var workItemDto = new WorkItemListDto()
                {
                    Id = workItem.Id,
                    Title = workItem.Title,
                    Status = workItem.Status
                };

                list.Add(workItemDto);

                workItemDto.Children.AddRange(Map(workItem.Children));
            }

            return list;
        }
        static public WorkItemDisplayDto Map(WorkItem workItem)
        {
            var dto = new WorkItemDisplayDto()
            {
                Id = workItem.Id,
                ParentId = workItem.ParentId,
                Title = workItem.Title,
                Description = workItem.Description,
                Executors = workItem.Executors,
                ActualExecutionTime = workItem.ActualExecutionTime.AsTimeSpan(),
                PlannedExecutionTime = workItem.PlannedExecutionTime.AsTimeSpan(),
                TotalActualExecutionTime = workItem.GetActualTotal().AsTimeSpan(),
                TotalPlannedExecutionTime = workItem.GetPlannedTotal().AsTimeSpan(),
                Status = workItem.Status,
                CanDelete = workItem.CanDelete(),
                CreatedAt = workItem.CreatedAt,
                EndedAt = workItem.EndedAt
            };

            foreach (var child in workItem.FlattenChildren)
            {
                dto.Children.Add(Map(child));
            }

            return dto;
        }
        static public WorkItemSnapshot Map(WorkItemCreateDto workItem)
        {
            return new WorkItemSnapshot()
            {
                ParentId = workItem.ParentId,
                Title = workItem.Title,
                Description = workItem.Description,
                Executors = workItem.Executors,
                PlannedExecutionTime = WorkItemExecutionTime.FromTimeSpan(workItem.PlannedExecutionTime).ExecutionTimeInSeconds,
                ActualExecutionTime = WorkItemExecutionTime.FromTimeSpan(workItem.ActualExecutionTime).ExecutionTimeInSeconds,
                Status = workItem.Status,
                EndedAt = workItem.Status == WorkItemStatus.COMPLETED ? DateTime.Now : (DateTime?)null,
                CreatedAt = DateTime.Now
            };
        }
        static public WorkItemUpdateDto MapForUpdate(WorkItem workItem)
        {
            return new WorkItemUpdateDto()
            {
                Id = workItem.Id,
                Title = workItem.Title,
                Description = workItem.Description,
                Executors = workItem.Executors,
                PlannedExecutionTime = workItem.PlannedExecutionTime.AsTimeSpan(),
                ActualExecutionTime = workItem.ActualExecutionTime.AsTimeSpan(),
                Status = workItem.Status,
                AllowedValues = workItem.GetAllowedStatuses()
            };
        }
    }
}
