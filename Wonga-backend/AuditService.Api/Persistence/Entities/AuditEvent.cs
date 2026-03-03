namespace AuditService.Api.Persistence.Entities
{
    public class AuditEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string EventType { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
