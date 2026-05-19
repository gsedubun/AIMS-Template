using AIMS.Core.Entities;
using AIMS.Infrastructure.Data;
using AIMS.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AIMS.WebFrontend.Pages.AssetItems;

[Authorize(Roles = "Admin,Manager")]
public class CreateModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly IActivityLogger _activityLogger;

    public CreateModel(AppDbContext context, IActivityLogger activityLogger)
    {
        _context = context;
        _activityLogger = activityLogger;
    }

    [BindProperty]
    public CreateAssetItemInput Input { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var assetItem = new AssetItem
        {
            Title = Input.Title,
            AssetId = Input.AssetId,
            Description = Input.Description,
            Type = Input.Type,
            Location = Input.Location,
            Priority = Input.Priority,
            IntegrityStatus = Input.IntegrityStatus
        };

        _context.AssetItems.Add(assetItem);
        await _context.SaveChangesAsync();

        // Log activity
        await _activityLogger.LogActivityAsync(
            "AssetItemCreated",
            $"Asset item '{assetItem.Title}' created",
            "AssetItem",
            assetItem.Id.ToString()
        );

        return RedirectToPage("Index");
    }

    public class CreateAssetItemInput
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string AssetId { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        public AssetType Type { get; set; }

        [StringLength(250)]
        public string Location { get; set; }

        [Required]
        public AssetPriority Priority { get; set; }

        [Required]
        public IntegrityStatus IntegrityStatus { get; set; }
    }
}
