using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TaskManager.Domain
{
    public enum WorkItemStatus
    {
        [DisplayName("Назначена")]
        ASSIGNED,
        [DisplayName("Выполняется")]
        EXECUTING,
        [DisplayName("Приостановлена")]
        STOPPED,
        [DisplayName("Завершена")]
        COMPLETED
    }
}
