using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Application.Common.Clients
{
    public interface IAuditClient
    {
        Task LogEventAsync(string eventType, Guid userId, string email, CancellationToken ct = default);
        Task<IEnumerable<AuditEventResponse>> GetUserLogEventAsync(
            string email,
            int take = 20,
            CancellationToken ct = default);
    }
}
