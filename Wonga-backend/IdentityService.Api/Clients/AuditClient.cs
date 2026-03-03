using IdentityService.Api.Contracts.Responses;
using IdentityService.Application.Common.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace IdentityService.Api.Clients;

public class AuditClient : IAuditClient
{
    private readonly HttpClient _http;
    private readonly AuditOptions _options;

    public AuditClient(HttpClient http, IOptions<AuditOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    public async Task LogEventAsync(string eventType, Guid userId, string email, CancellationToken ct = default)
    {
        var payload = new
        {
            eventType,
            userId,
            email
        };

        var url = $"{_options.BaseUrl.TrimEnd('/')}/api/audit";
        var res = await _http.PostAsJsonAsync(url, payload, ct);

        // don’t crash the main flow if audit fails (microservice should be non-blocking)
        if (!res.IsSuccessStatusCode)
        {
            // optional: log it, but keep it simple
        }
    }

    public async Task<IEnumerable<AuditEventResponse>> GetUserLogEventAsync(
            string email,
            int take = 20,
            CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Enumerable.Empty<AuditEventResponse>();

        take = Math.Clamp(take, 1, 100);

        var normalizedEmail = email.Trim().ToLowerInvariant();

        var baseUrl = _options.BaseUrl.TrimEnd('/');
        var url = $"{baseUrl}/api/audit/{Uri.EscapeDataString(normalizedEmail)}?take={take}";

        using var res = await _http.GetAsync(url, ct);

        // Non-blocking behavior: if audit service fails, return empty collection
        if (!res.IsSuccessStatusCode)
            return Enumerable.Empty<AuditEventResponse>();

        var items = await res.Content.ReadFromJsonAsync<List<AuditEventResponse>>(cancellationToken: ct);

        return items ?? Enumerable.Empty<AuditEventResponse>();
    }
}