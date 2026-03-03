using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Application.Common.Clients
{
    public sealed class AuditEventResponse
    {
        public Guid Id { get; init; }
        public string EventType { get; init; } = string.Empty;
        public Guid UserId { get; init; }
        public string Email { get; init; } = string.Empty;
        public DateTime CreatedAtUtc { get; init; }
    }
}
