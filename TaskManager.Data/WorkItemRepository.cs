using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain;

namespace TaskManager.Data
{
    public class WorkItemRepository : IWorkItemRepository
    {
        private readonly TaskManagerContext context;
        public WorkItemRepository(TaskManagerContext context)
        {
            this.context = context;
        }
        public async Task<WorkItem> CreateWorkItem(WorkItem workItem)
        {
            var item = Map(workItem, true);

            context.Add(item);

            await context.SaveChangesAsync();

            return workItem;
        }
        public async Task DeleteWorkItem(int id)
        {
            var item = await context.WorkItems.FindAsync(id);

            context.Remove(item);

            await context.SaveChangesAsync();
        }
        public async Task<WorkItem> UpdateWorkItem(WorkItem workItem)
        {
            var items = await GetSubtree(workItem.Id);

            var hierarchy = workItem.Flatten();

            foreach (var member in hierarchy)
            {
                context.Entry(items.First(i => i.Id == member.Id)).CurrentValues.SetValues(Map(member, false, true));
            }

            await context.SaveChangesAsync();

            return workItem;
        }
        public async Task<WorkItem> GetWorkItem(int id)
        {
            var items = await GetSubtree(id);

            var parent = items.Where(wi => wi.Id == id).FirstOrDefault();

            return Map(items, parent?.ParentId).First();
        }
        public async Task<IEnumerable<WorkItem>> GetWorkItems()
        {
            var items = await context.WorkItems
                .ToListAsync();

            return Map(items);
        }
        private WorkItemData Map(WorkItem workItem, bool mapChildren, bool setId = false)
        {
            var item = new WorkItemData()
            {
                ParentId = workItem.ParentId,
                Title = workItem.Title,
                Description = workItem.Description,
                Executors = workItem.Executors,
                PlannedExecutionTime = workItem.PlannedExecutionTime.ExecutionTimeInSeconds,
                ActualExecutionTime = workItem.ActualExecutionTime.ExecutionTimeInSeconds,
                Status = workItem.Status,
                EndedAt = workItem.EndedAt,
                CreatedAt = workItem.CreatedAt
            };

            if (setId)
            {
                item.Id = workItem.Id;
            }

            if (!mapChildren)
            {
                return item;
            }

            foreach (var child in workItem.Children)
            {
                var childData = Map(child, true);

                childData.Parent = item;
                item.Children.Add(childData);

            }

            return item;
        }
        private WorkItem Map(WorkItemData item)
        {
            if (item == null)
            {
                return null;
            }

            return WorkItem.Create(
                new WorkItemSnapshot()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Executors = item.Executors,
                    Status = item.Status,
                    PlannedExecutionTime = item.PlannedExecutionTime,
                    ActualExecutionTime = item.ActualExecutionTime,
                    ParentId = item.ParentId,
                    EndedAt = item.EndedAt,
                    CreatedAt = item.CreatedAt
                }
            );
        }
        private IEnumerable<WorkItem> Map(IEnumerable<WorkItemData> allItems, int? parentId = null)
        {
            var topLevelItems = allItems.Where(wi => wi.ParentId == parentId).ToList();

            List<WorkItem> items = new List<WorkItem>();

            foreach (var topLevelItem in topLevelItems)
            {
                var item = Map(topLevelItem);

                var children = Map(allItems, topLevelItem.Id);

                foreach (var child in children)
                {
                    child.SetParent(item);
                }

                items.Add(item);
            }

            return items;
        }
        private async Task<IEnumerable<WorkItemData>> GetSubtree(int id)
        {
            var param = new SqlParameter("@WorkItemId", id);

            var items = await context.WorkItems.FromSql("dbo.GetWorkItemsSubtree @WorkItemId", param)
                .ToListAsync();

            return items;
        }
    }
}
