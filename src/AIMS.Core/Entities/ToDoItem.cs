using AIMS.Core.Events;
using AIMS.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIMS.Core.Entities;

public class ToDoItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }
    public bool IsDone { get; private set; }

    public void MarkComplete()
    {
        IsDone = true;
        Events.Add(new ToDoItemCompletedEvent(this));
    }
}
public enum AssetPriority
{
    Low = 1,
    Medium = 2,
    High = 3
}
public enum AssetType
{
    Pipe = 1,
    PSV = 2,
    PressureTank = 3,
    Other = 4
}
