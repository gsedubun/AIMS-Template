using System;
using System.Threading.Tasks;
using AIMS.Core.Entities;
using AIMS.Infrastructure.Data;
using AIMS.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AIMS.Infrastructure.Audit;

/// <summary>
/// Implementation of IActivityLogger that logs user activities to the database.
/// </summary>
public class ActivityLogger : IActivityLogger
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuditUserProvider _auditUserProvider;

    public ActivityLogger(
        AppDbContext context,
        IHttpContextAccessor httpContextAccessor,
        IAuditUserProvider auditUserProvider)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _auditUserProvider = auditUserProvider;
    }

    public async Task LogActivityAsync(
        string activityType,
        string? description = null,
        string? entityName = null,
        string? entityId = null,
        string? result = null,
        string? userId = null,
        string? userName = null)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var auditLog = new AuditLog
        {
            Category = AuditCategory.Activity,
            EntityName = entityName ?? activityType,
            EntityId = entityId ?? string.Empty,
            Action = activityType,
            Description = description,
            UserId = userId ?? _auditUserProvider.GetUserId(),
            UserName = userName ?? _auditUserProvider.GetUserName(),
            IpAddress = _auditUserProvider.GetIpAddress(),
            UserAgent = GetUserAgent(httpContext),
            RequestPath = GetRequestPath(httpContext),
            Result = result ?? "Success",
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task LogSecurityActivityAsync(
        string activityType,
        string? description = null,
        string? result = null,
        string? userId = null,
        string? userName = null)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var auditLog = new AuditLog
        {
            Category = AuditCategory.Security,
            EntityName = "Security",
            EntityId = string.Empty,
            Action = activityType,
            Description = description,
            UserId = userId ?? _auditUserProvider.GetUserId(),
            UserName = userName ?? _auditUserProvider.GetUserName(),
            IpAddress = _auditUserProvider.GetIpAddress(),
            UserAgent = GetUserAgent(httpContext),
            RequestPath = GetRequestPath(httpContext),
            Result = result ?? "Success",
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    private static string? GetUserAgent(HttpContext? httpContext)
    {
        if (httpContext == null) return null;

        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        // Truncate if too long
        return userAgent.Length > 500 ? userAgent.Substring(0, 500) : userAgent;
    }

    private static string? GetRequestPath(HttpContext? httpContext)
    {
        if (httpContext == null) return null;

        var path = httpContext.Request.Path.ToString();
        var queryString = httpContext.Request.QueryString.ToString();
        var fullPath = path + queryString;

        // Truncate if too long
        return fullPath.Length > 500 ? fullPath.Substring(0, 500) : fullPath;
    }
}
