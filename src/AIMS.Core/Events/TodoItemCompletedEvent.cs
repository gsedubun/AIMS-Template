using AIMS.Core.Entities;
using AIMS.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIMS.Core.Events
{
    public class ToDoItemCompletedEvent : BaseDomainEvent
    {
        public ToDoItem CompletedItem { get; set; }

        public ToDoItemCompletedEvent(ToDoItem completedItem)
        {
            CompletedItem = completedItem;
        }
    }
}
