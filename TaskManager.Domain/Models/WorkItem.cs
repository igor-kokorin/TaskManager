using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TaskManager.Domain
{
    public class WorkItem
    {
        private int? parentId;
        private WorkItemStatusManager statusManager;
        private WorkItemHierarchy hierarchy;
        public int Id { get; private set; }
        public int? ParentId
        {
            get
            {
                return Parent?.Id ?? parentId;
            }
            private set
            {
                parentId = value;
            }
        }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public bool CanDelete()
        {
            return Children.Count() == 0;
        }

        public string Executors { get; private set; }
        public WorkItemStatus Status
        {
            get
            {
                return statusManager.CurrentStatus;
            }
        }
        public WorkItemExecutionTime PlannedExecutionTime { get; private set; }
        public WorkItemExecutionTime ActualExecutionTime { get; private set; }
        public DateTime? EndedAt { get; private set; }
        public WorkItem Parent
        {
            get
            {
                return hierarchy.Parent;
            }
        }
        public IEnumerable<WorkItem> Children
        {
            get
            {
                return hierarchy.Children;
            }
        }
        public IEnumerable<WorkItem> FlattenChildren
        {
            get
            {
                return hierarchy.UnwindChildren();
            }
        }

        public DateTime CreatedAt { get; private set; }

        static public WorkItem Create(WorkItemSnapshot snapshot)
        {
            if (snapshot.Status == WorkItemStatus.COMPLETED && snapshot.EndedAt == null)
            {
                throw new ArgumentException("You must provide endedAt for completed task");
            }

            var workItem = new WorkItem()
            {
                Id = snapshot.Id,
                Title = snapshot.Title,
                Description = snapshot.Description,
                Executors = snapshot.Executors,
                statusManager = new WorkItemStatusManager(snapshot.Status),
                PlannedExecutionTime = new WorkItemExecutionTime(snapshot.PlannedExecutionTime),
                ActualExecutionTime = new WorkItemExecutionTime(snapshot.ActualExecutionTime),
                EndedAt = snapshot.EndedAt,
                CreatedAt = snapshot.CreatedAt
            };

            workItem.ParentId = snapshot.ParentId;

            return workItem;
        }
        private WorkItem()
        {
            hierarchy = new WorkItemHierarchy(this, null);
        }
        internal void RemoveChild(WorkItem child)
        {
            hierarchy.RemoveChild(child);
        }
        internal void AddChild(WorkItem child)
        {
            hierarchy.AddChild(child);
        }
        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("title is required");
            }

            Title = title;
        }

        public IEnumerable<WorkItemStatus> GetAllowedStatuses()
        {
            var statusValues = Enum.GetValues(typeof(WorkItemStatus)).Cast<WorkItemStatus>();

            var allowedStatuses = new List<WorkItemStatus>();

            foreach (var statusValue in statusValues)
            {
                if (CanTransitTo(statusValue))
                {
                    allowedStatuses.Add(statusValue);
                }
            }    

            return allowedStatuses;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("description is required");
            }

            Description = description;
        }
        public void SetExecutors(string executors)
        {
            if (string.IsNullOrWhiteSpace(executors))
            {
                throw new ArgumentException("executors is required");
            }

            Executors = executors;
        }
        public void SetPlannedExecutionTime(WorkItemExecutionTime plannedExecutionTime)
        {
            if (plannedExecutionTime == null)
            {
                throw new ArgumentException("plannedExecutionTime is required");
            }

            PlannedExecutionTime = plannedExecutionTime;
        }
        public void SetActualExecutionTime(WorkItemExecutionTime actualExecutionTime)
        {
            if (actualExecutionTime == null)
            {
                throw new ArgumentException("actualExecutionTime is required");
            }

            ActualExecutionTime = actualExecutionTime;
        }
        public void SetParent(WorkItem parent)
        {
            hierarchy.SetParent(parent);
        }
        public bool CanTransitTo(WorkItemStatus status)
        {
            if (status == WorkItemStatus.COMPLETED)
            {
                return hierarchy.Flatten().All(wi => wi.statusManager.CanTransitTo(status));
            }

            return statusManager.CanTransitTo(status);
        }
        public WorkItemStatus SetStatus(WorkItemStatus status)
        {
            switch (status)
            {
                case WorkItemStatus.ASSIGNED:
                    Assign();
                    break;

                case WorkItemStatus.EXECUTING:
                    Execute();
                    break;

                case WorkItemStatus.STOPPED:
                    Stop();
                    break;

                case WorkItemStatus.COMPLETED:
                    Complete();
                    break;

                default:
                    throw new ArgumentException("unknown status");
            }

            return Status;
        }
        public WorkItemStatus Assign()
        {
            statusManager.Assign();
            return Status;
        }
        public WorkItemStatus Execute()
        {
            statusManager.Execute();
            return Status;
        }
        public WorkItemStatus Stop()
        {
            statusManager.Stop();
            return Status;
        }
        public WorkItemStatus Complete()
        {
            if ((Status != WorkItemStatus.COMPLETED) && CanTransitTo(WorkItemStatus.COMPLETED))
            {
                var endedAt = DateTime.Now;

                foreach (var item in hierarchy.Flatten())
                {
                    item.statusManager.Complete();
                    item.EndedAt = endedAt;
                }
            }

            return Status;
        }
        public WorkItemExecutionTime GetPlannedTotal()
        {
            var items = hierarchy.Flatten();

            var total = items.Sum(wi => wi.PlannedExecutionTime.ExecutionTimeInSeconds);

            return new WorkItemExecutionTime(total);
        }
        public WorkItemExecutionTime GetActualTotal()
        {
            var items = hierarchy.Flatten();

            var total = items.Sum(wi => wi.ActualExecutionTime.ExecutionTimeInSeconds);

            return new WorkItemExecutionTime(total);
        }
        public IEnumerable<WorkItem> Flatten()
        {
            return hierarchy.Flatten();
        }
        public WorkItemSnapshot GetSnapshot()
        {
            return new WorkItemSnapshot()
            {
                Id = Id,
                ParentId = ParentId,
                Title = Title,
                Description = Description,
                Executors = Executors,
                PlannedExecutionTime = PlannedExecutionTime.ExecutionTimeInSeconds,
                ActualExecutionTime = ActualExecutionTime.ExecutionTimeInSeconds,
                Status = Status,
                EndedAt = EndedAt
            };
        }
    }
}
