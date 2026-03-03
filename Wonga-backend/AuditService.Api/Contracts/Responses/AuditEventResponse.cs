namespace AuditService.Api.Contracts.Responses
{
    public class AuditEventResponse
    {
        public Guid Id { get; init; }
        public string EventType { get; init; } = string.Empty;
        public Guid UserId { get; init; }
        public string Email { get; init; } = string.Empty;
        public DateTime CreatedAtUtc { get; init; }
    }
}
