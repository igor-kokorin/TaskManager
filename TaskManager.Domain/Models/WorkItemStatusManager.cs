using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TaskManager.Domain
{
    internal class WorkItemStatusManager
    {
        public WorkItemStatus CurrentStatus { get; private set; }

        public WorkItemStatusManager(WorkItemStatus initialStatus)
        {
            CurrentStatus = initialStatus;
        }

        public bool CanTransitTo(WorkItemStatus status)
        {
            if ((status == WorkItemStatus.STOPPED || status == WorkItemStatus.COMPLETED) && CurrentStatus != WorkItemStatus.EXECUTING)
            {
                return false;
            }

            return true;
        }

        public void Assign()
        {
            var newStatus = WorkItemStatus.ASSIGNED;

            if (CanTransitTo(newStatus))
            {
                CurrentStatus = newStatus;
            }
        }

        public void Execute()
        {
            var newStatus = WorkItemStatus.EXECUTING;

            if (CanTransitTo(newStatus))
            {
                CurrentStatus = newStatus;
            }
        }

        public void Stop()
        {
            var newStatus = WorkItemStatus.STOPPED;

            if (!CanTransitTo(newStatus))
            {
                throw new InvalidStateTransition();
            }

            CurrentStatus = newStatus;
        }

        public void Complete()
        {
            var newStatus = WorkItemStatus.COMPLETED;

            if (!CanTransitTo(newStatus))
            {
                throw new InvalidStateTransition();
            }

            CurrentStatus = newStatus;
        }

        [Serializable]
        private class InvalidStateTransition : Exception
        {
            public InvalidStateTransition(): base("Invalid state transition")
            {
            }

            public InvalidStateTransition(string message) : base(message)
            {
            }

            public InvalidStateTransition(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected InvalidStateTransition(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
