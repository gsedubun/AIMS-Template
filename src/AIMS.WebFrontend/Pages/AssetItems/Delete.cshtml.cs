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
    private readonly IWebHostEnvironment _env;

    public DeleteModel(AppDbContext context, IActivityLogger activityLogger, IWebHostEnvironment env)
    {
        _context = context;
        _activityLogger = activityLogger;
        _env = env;
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var assetItem = await _context.AssetItems
            .Include(a => a.AssetItemRemarks)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (assetItem == null)
        {
            return NotFound();
        }

        var assetItemTitle = assetItem.Title;

        if (!string.IsNullOrEmpty(assetItem.PicturePath))
        {
            var oldFile = Path.Combine(_env.WebRootPath, assetItem.PicturePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(oldFile))
                System.IO.File.Delete(oldFile);
        }

        _context.AssetRemarks.RemoveRange(assetItem.AssetItemRemarks);
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
