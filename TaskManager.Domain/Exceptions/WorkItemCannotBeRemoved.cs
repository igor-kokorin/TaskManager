using System;
using System.Runtime.Serialization;

namespace TaskManager.Domain
{
    [Serializable]
    public class WorkItemCannotBeRemoved : Exception
    {
        public WorkItemCannotBeRemoved()
        {
        }

        public WorkItemCannotBeRemoved(string message) : base(message)
        {
        }

        public WorkItemCannotBeRemoved(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WorkItemCannotBeRemoved(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}