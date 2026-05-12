using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AIMS.Infrastructure.Audit;

/// <summary>
/// Represents a pending audit entry before it's saved to the database.
/// This is used to capture the entity state before SaveChanges completes.
/// </summary>
public class AuditEntry
{
    public EntityEntry Entry { get; }
    public string EntityName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? IpAddress { get; set; }
    public Dictionary<string, object?> OldValues { get; } = new();
    public Dictionary<string, object?> NewValues { get; } = new();
    public List<string> ChangedColumns { get; } = new();
    public List<PropertyEntry> TemporaryProperties { get; } = new();

    public AuditEntry(EntityEntry entry)
    {
        Entry = entry;
    }

    /// <summary>
    /// Indicates if this entry has temporary properties that need to be resolved after SaveChanges.
    /// This happens for auto-generated primary keys.
    /// </summary>
    public bool HasTemporaryProperties => TemporaryProperties.Count > 0;
}
