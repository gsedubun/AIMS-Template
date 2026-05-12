using AIMS.Core.Entities;
using AIMS.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace AIMS.WebFrontend.Pages.Admin;

[Authorize(Roles = "Admin,Manager,User")]
public class AuditLogsModel : PageModel
{
    private readonly AppDbContext _context;
    private const int PageSize = 25;

    public AuditLogsModel(AppDbContext context)
    {
        _context = context;
    }

    public List<AuditLog> AuditLogs { get; set; } = new();
    public List<string> AvailableEntities { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public bool CanViewAllLogs { get; set; }

    // Filter properties
    [BindProperty(SupportsGet = true)]
    public string? EntityNameFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ActionFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? UserNameFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? FromDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? ToDate { get; set; }

    public async Task OnGetAsync([FromQuery] int page = 1)
    {
        CurrentPage = page < 1 ? 1 : page;

        // Admin and Manager can view all logs, User can only view their own
        CanViewAllLogs = User.IsInRole("Admin") || User.IsInRole("Manager");
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUserName = User.Identity?.Name;

        // Build query with filters
        var query = _context.AuditLogs.AsQueryable();

        // If user is not Admin or Manager, filter to only their own audit logs
        if (!CanViewAllLogs)
        {
            query = query.Where(a => a.UserId == currentUserId || a.UserName == currentUserName);
        }

        // Get available entity names for filter dropdown (respecting user's access)
        AvailableEntities = await query
            .Select(a => a.EntityName)
            .Distinct()
            .OrderBy(e => e)
            .ToListAsync();

        // Re-apply the base query for filtering
        query = _context.AuditLogs.AsQueryable();
        if (!CanViewAllLogs)
        {
            query = query.Where(a => a.UserId == currentUserId || a.UserName == currentUserName);
        }

        if (!string.IsNullOrEmpty(EntityNameFilter))
        {
            query = query.Where(a => a.EntityName == EntityNameFilter);
        }

        if (!string.IsNullOrEmpty(ActionFilter))
        {
            query = query.Where(a => a.Action == ActionFilter);
        }

        // Only Admin and Manager can filter by other users
        if (!string.IsNullOrEmpty(UserNameFilter) && CanViewAllLogs)
        {
            query = query.Where(a => a.UserName != null && a.UserName.Contains(UserNameFilter));
        }

        if (FromDate.HasValue)
        {
            query = query.Where(a => a.Timestamp >= FromDate.Value.ToUniversalTime());
        }

        if (ToDate.HasValue)
        {
            var toDateEnd = ToDate.Value.AddDays(1).ToUniversalTime();
            query = query.Where(a => a.Timestamp < toDateEnd);
        }

        // Calculate pagination
        var totalCount = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
        TotalPages = TotalPages < 1 ? 1 : TotalPages;
        CurrentPage = CurrentPage > TotalPages ? TotalPages : CurrentPage;

        // Get paginated results
        AuditLogs = await query
            .OrderByDescending(a => a.Timestamp)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Formats JSON string for display.
    /// </summary>
    public string FormatJson(string? json)
    {
        if (string.IsNullOrEmpty(json))
            return string.Empty;

        try
        {
            var doc = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
        }
        catch
        {
            return json;
        }
    }
}
