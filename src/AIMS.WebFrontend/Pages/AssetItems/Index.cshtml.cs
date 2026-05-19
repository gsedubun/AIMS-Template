using AIMS.Core.Entities;
using AIMS.Infrastructure.Data;
using AIMS.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AIMS.WebFrontend.Pages.AssetItems;

[Authorize]
public class IndexModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly IActivityLogger _activityLogger;
    private const int PageSize = 10;

    public IndexModel(AppDbContext context, IActivityLogger activityLogger)
    {
        _context = context;
        _activityLogger = activityLogger;
    }

    public List<AssetItem> AssetItems { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? TypeFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? PriorityFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? StatusFilter { get; set; }

    public async Task OnGetAsync(int page = 1)
    {
        CurrentPage = page < 1 ? 1 : page;

        var query = _context.AssetItems.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            query = query.Where(a => a.Title.Contains(SearchTerm) ||
                                      a.AssetId.Contains(SearchTerm) ||
                                      a.Description.Contains(SearchTerm) ||
                                      a.Location.Contains(SearchTerm));
        }

        // Apply type filter
        if (!string.IsNullOrEmpty(TypeFilter) && Enum.TryParse<AssetType>(TypeFilter, out var typeEnum))
        {
            query = query.Where(a => a.Type == typeEnum);
        }

        // Apply priority filter
        if (!string.IsNullOrEmpty(PriorityFilter) && Enum.TryParse<AssetPriority>(PriorityFilter, out var priorityEnum))
        {
            query = query.Where(a => a.Priority == priorityEnum);
        }

        // Apply integrity status filter
        if (!string.IsNullOrEmpty(StatusFilter) && Enum.TryParse<IntegrityStatus>(StatusFilter, out var statusEnum))
        {
            query = query.Where(a => a.IntegrityStatus == statusEnum);
        }

        // Get total count for pagination
        int totalCount = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

        // Get the page data
        AssetItems = await query
            .OrderByDescending(a => a.Id)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }
}
