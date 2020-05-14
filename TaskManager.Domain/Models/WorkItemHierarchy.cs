using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TaskManager.Domain
{
    internal class WorkItemHierarchy
    {
        private WorkItem main;
        private List<WorkItem> children;
        public IEnumerable<WorkItem> Children
        {
            get
            {
                return new ReadOnlyCollectionBuilder<WorkItem>(children);
            }
        }

        public WorkItem Parent { get; private set; }

        public WorkItemHierarchy(WorkItem item, WorkItem parent)
        {
            if (item == null)
            {
                throw new ArgumentException("you must provide item");
            }

            if (item == parent)
            {
                throw new ArgumentException("item and parent cannot be the same value");
            }

            main = item;
            Parent = parent;
            children = new List<WorkItem>();
        }

        public void AddChild(WorkItem child)
        {
            if (child != null)
            {
                children.Add(child);
            }
        }

        public void RemoveChild(WorkItem child)
        {
            if (child != null)
            {
                children.Remove(child);
            }
        }

        public IEnumerable<WorkItem> UnwindChildren()
        {
            var childrenFlattened = new List<WorkItem>();

            foreach (var child in children)
            {
                childrenFlattened.Add(child);
                childrenFlattened.AddRange(child.FlattenChildren);
            }

            return childrenFlattened;
        }

        public IEnumerable<WorkItem> Flatten()
        {
            var items = new List<WorkItem>();

            items.Add(main);
            items.AddRange(UnwindChildren());

            return items;
        }

        public void SetParent(WorkItem newParent)
        {
            if (newParent == null)
            {
                return;
            }

            if (Parent != null)
            {
                Parent.RemoveChild(main);
            }

            Parent = newParent;
            Parent.AddChild(main);
        }
    }
}
