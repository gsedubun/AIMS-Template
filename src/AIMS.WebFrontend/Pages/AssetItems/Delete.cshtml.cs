using AIMS.Infrastructure.Data;
using AIMS.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AIMS.WebFrontend.Pages.AssetItems;

[Authorize(Roles = "Admin,Manager")]
public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly IActivityLogger _activityLogger;

    public DeleteModel(AppDbContext context, IActivityLogger activityLogger)
    {
        _context = context;
        _activityLogger = activityLogger;
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var assetItem = await _context.AssetItems.FirstOrDefaultAsync(a => a.Id == id);

        if (assetItem == null)
        {
            return NotFound();
        }

        var assetItemTitle = assetItem.Title;
        _context.AssetItems.Remove(assetItem);
        await _context.SaveChangesAsync();

        // Log activity
        await _activityLogger.LogActivityAsync(
            "AssetItemDeleted",
            $"Asset item '{assetItemTitle}' deleted",
            "AssetItem",
            id.ToString()
        );

        return RedirectToPage("Index");
    }
}
