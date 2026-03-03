namespace AuditService.Api.Contracts.Requests
{
    public class CreateAuditEventRequest
    {
        public string EventType { get; init; } = string.Empty;
        public Guid UserId { get; init; }
        public string Email { get; init; } = string.Empty;
    }
}
