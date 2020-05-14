using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain
{
    public interface IWorkItemRepository
    {
        Task<WorkItem> GetWorkItem(int id);
        Task<IEnumerable<WorkItem>> GetWorkItems();
        Task<WorkItem> CreateWorkItem(WorkItem workItem);
        Task<WorkItem> UpdateWorkItem(WorkItem workItem);
        Task DeleteWorkItem(int id);
    }
}
