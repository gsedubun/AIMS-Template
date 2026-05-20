using AIMS.Core.Entities;
using AIMS.Infrastructure.Data;
using AIMS.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AIMS.WebFrontend.Pages.AssetItems;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly IActivityLogger _activityLogger;

    public DetailsModel(AppDbContext context, IActivityLogger activityLogger)
    {
        _context = context;
        _activityLogger = activityLogger;
    }

    public AssetItem AssetItem { get; set; }
    public List<AssetItemRemarks> Remarks { get; set; } = new();

    [BindProperty]
    public AddRemarkInput Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        AssetItem = await _context.AssetItems.FirstOrDefaultAsync(a => a.Id == id);

        if (AssetItem == null)
        {
            return NotFound();
        }

        // Load remarks ordered by most recent first
        Remarks = await _context.AssetRemarks
            .Where(r => r.AssetItem.Id == id)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        if (!User.IsInRole("Admin") && !User.IsInRole("Manager"))
        {
            return Forbid();
        }

        var assetItem = await _context.AssetItems
            .Include(a => a.AssetItemRemarks)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (assetItem == null)
        {
            return NotFound();
        }

        var assetItemTitle = assetItem.Title;
        _context.AssetRemarks.RemoveRange(assetItem.AssetItemRemarks);
        _context.AssetItems.Remove(assetItem);
        await _context.SaveChangesAsync();

        await _activityLogger.LogActivityAsync(
            "AssetItemDeleted",
            $"Asset item '{assetItemTitle}' deleted",
            "AssetItem",
            id.ToString()
        );

        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        // Only Admin and Manager can add remarks, but Users can also add remarks (view-only doesn't mean can't comment)
        if (!User.IsInRole("Admin") && !User.IsInRole("Manager") && !User.IsInRole("User"))
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            // Reload the asset item and remarks on validation failure
            AssetItem = await _context.AssetItems.FirstOrDefaultAsync(a => a.Id == id);
            Remarks = await _context.AssetRemarks
                .Where(r => r.AssetItem.Id == id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return Page();
        }

        var assetItem = await _context.AssetItems.FirstOrDefaultAsync(a => a.Id == id);

        if (assetItem == null)
        {
            return NotFound();
        }

        var remark = new AssetItemRemarks
        {
            Description = Input.Description,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name ?? "Unknown",
            AssetItem = assetItem
        };

        _context.AssetRemarks.Add(remark);
        await _context.SaveChangesAsync();

        // Log activity
        await _activityLogger.LogActivityAsync(
            "AssetItemRemarkAdded",
            $"Remark added to asset item '{assetItem.Title}'",
            "AssetItem",
            assetItem.Id.ToString()
        );

        return RedirectToPage(new { id = assetItem.Id });
    }

    public class AddRemarkInput
    {
        [Required]
        [StringLength(250, MinimumLength = 1)]
        [Display(Name = "Remark")]
        public string Description { get; set; }
    }
}
