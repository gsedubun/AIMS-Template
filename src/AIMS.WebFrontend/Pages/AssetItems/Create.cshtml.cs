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
    private readonly IWebHostEnvironment _env;

    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

    public CreateModel(AppDbContext context, IActivityLogger activityLogger, IWebHostEnvironment env)
    {
        _context = context;
        _activityLogger = activityLogger;
        _env = env;
    }

    [BindProperty]
    public CreateAssetItemInput Input { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        string? picturePath = null;

        if (Input.Picture != null && Input.Picture.Length > 0)
        {
            if (Input.Picture.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Input.Picture", "File size must not exceed 2 MB.");
                return Page();
            }

            var ext = Path.GetExtension(Input.Picture.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
            {
                ModelState.AddModelError("Input.Picture", "Only image files (.jpg, .jpeg, .png, .gif, .webp) are allowed.");
                return Page();
            }

            var dir = Path.Combine(_env.WebRootPath, "asset-pictures");
            Directory.CreateDirectory(dir);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(dir, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await Input.Picture.CopyToAsync(stream);

            picturePath = $"/asset-pictures/{fileName}";
        }

        var assetItem = new AssetItem
        {
            Title = Input.Title,
            AssetId = Input.AssetId,
            Description = Input.Description,
            Type = Input.Type,
            Location = Input.Location,
            Priority = Input.Priority,
            IntegrityStatus = Input.IntegrityStatus,
            PicturePath = picturePath
        };

        _context.AssetItems.Add(assetItem);
        await _context.SaveChangesAsync();

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

        public IFormFile? Picture { get; set; }
    }
}
