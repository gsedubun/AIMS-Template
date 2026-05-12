using System.Linq;
using System.Security.Claims;
using AIMS.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AIMS.Infrastructure.Audit;

/// <summary>
/// Provides current user information from HttpContext for audit trail purposes.
/// </summary>
public class HttpContextAuditUserProvider : IAuditUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextAuditUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public string? GetUserName()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }

    public string? GetIpAddress()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            return null;

        // Check for forwarded IP first (for reverse proxy scenarios)
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',').First().Trim();
        }

        return context.Connection.RemoteIpAddress?.ToString();
    }
}
