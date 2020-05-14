using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain;

namespace TaskManager.Web.Models
{
    public class WorkItemListDto
    {
        public string Title { get; set; }
        public WorkItemStatus Status { get; set; }
        public List<WorkItemListDto> Children { get; set; }
        public int Id { get; set; }

        public WorkItemListDto()
        {
            Children = new List<WorkItemListDto>();
        }
    }
}
