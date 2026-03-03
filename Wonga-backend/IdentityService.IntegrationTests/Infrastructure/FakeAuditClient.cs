using IdentityService.Application.Common.Clients;

namespace IdentityService.IntegrationTests.Infrastructure;

public class FakeAuditClient : IAuditClient
{
    public Task<IEnumerable<AuditEventResponse>> GetUserLogEventAsync(string email, int take = 20, CancellationToken ct = default)
    {
        // Return empty list (fake response)
        return Task.FromResult<IEnumerable<AuditEventResponse>>(
            Enumerable.Empty<AuditEventResponse>());
    }

    public Task LogEventAsync(
        string eventType,
        Guid userId,
        string email,
        CancellationToken ct = default)
    {
        // do nothing
        return Task.CompletedTask;
    }
}