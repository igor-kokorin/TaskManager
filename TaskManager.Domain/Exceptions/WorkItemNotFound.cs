using System;
using System.Runtime.Serialization;

namespace TaskManager.Domain
{
    [Serializable]
    public class WorkItemNotFound : Exception
    {
        public WorkItemNotFound()
        {
        }

        public WorkItemNotFound(string message) : base(message)
        {
        }

        public WorkItemNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WorkItemNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}