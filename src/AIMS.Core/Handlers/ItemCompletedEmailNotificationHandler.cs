using AIMS.Core.Events;
using AIMS.SharedKernel.Interfaces;
using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AIMS.Core.Handlers
{
    public class ItemCompletedEmailNotificationHandler : IHandle<ToDoItemCompletedEvent>
    {
        public Task Handle(ToDoItemCompletedEvent domainEvent)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));

            // Do Nothing
            return Task.CompletedTask;
        }
    }
}
