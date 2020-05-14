using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager.Domain
{
    public class WorkItemExecutionTime
    {
        public int ExecutionTimeInSeconds { get; private set; }

        public WorkItemExecutionTime(int executionTimeInSeconds)
        {
            if (executionTimeInSeconds < 0)
            {
                throw new ArgumentException("executionTimeInSeconds must be greater than or equal to 0");
            }

            this.ExecutionTimeInSeconds = executionTimeInSeconds;
        }

        public TimeSpan AsTimeSpan()
        {
            return TimeSpan.FromSeconds(ExecutionTimeInSeconds);
        }
        static public WorkItemExecutionTime FromTimeSpan(TimeSpan ts)
        {
            return new WorkItemExecutionTime(Convert.ToInt32(ts.TotalSeconds));
        }
    }
}
