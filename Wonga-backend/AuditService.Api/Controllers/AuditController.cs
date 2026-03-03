using AuditService.Api.Contracts.Requests;
using AuditService.Api.Contracts.Responses;
using AuditService.Api.Persistence;
using AuditService.Api.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuditService.Api.Controllers;

[ApiController]
[Route("api/audit")]
public class AuditController : ControllerBase
{
    private readonly AuditDbContext _db;

    public AuditController(AuditDbContext db)
    {
        _db = db;
    }

    // POST /api/audit
    // Called by IdentityService after important events (Register/Login)
    [HttpPost]
    public async Task<ActionResult<AuditEventResponse>> Create([FromBody] CreateAuditEventRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.EventType))
            return BadRequest("EventType is required.");

        if (request.UserId == Guid.Empty)
            return BadRequest("UserId is required.");

        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest("Email is required.");

        var entity = new AuditEvent
        {
            EventType = request.EventType.Trim(),
            UserId = request.UserId,
            Email = request.Email.Trim().ToLowerInvariant(),
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.AuditEvents.Add(entity);
        await _db.SaveChangesAsync(ct);

        var response = new AuditEventResponse
        {
            Id = entity.Id,
            EventType = entity.EventType,
            UserId = entity.UserId,
            Email = entity.Email,
            CreatedAtUtc = entity.CreatedAtUtc
        };

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, response);
    }

    // GET /api/audit/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuditEventResponse>> GetById(Guid id, CancellationToken ct)
    {
        var entity = await _db.AuditEvents.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();

        return Ok(new AuditEventResponse
        {
            Id = entity.Id,
            EventType = entity.EventType,
            UserId = entity.UserId,
            Email = entity.Email,
            CreatedAtUtc = entity.CreatedAtUtc
        });
    }

    // GET /api/audit?take=20
    // Nice for demo/debugging (optional but useful)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditEventResponse>>> List([FromQuery] int take = 20, CancellationToken ct = default)
    {
        take = Math.Clamp(take, 1, 100);

        var items = await _db.AuditEvents.AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(take)
            .Select(x => new AuditEventResponse
            {
                Id = x.Id,
                EventType = x.EventType,
                UserId = x.UserId,
                Email = x.Email,
                CreatedAtUtc = x.CreatedAtUtc
            })
            .ToListAsync(ct);

        return Ok(items);
    }

    // GET /api/audit/{email}?take=20
    [HttpGet("{email}")]
    public async Task<ActionResult<IEnumerable<AuditEventResponse>>> UserEventLogs(string email, [FromQuery] int take = 20, CancellationToken ct = default)
    {
        take = Math.Clamp(take, 1, 100);

        var items = await _db.AuditEvents.AsNoTracking()
            .Where(x => x.Email.ToLower() == email.ToLower())
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(take)
            .Select(x => new AuditEventResponse
            {
                Id = x.Id,
                EventType = x.EventType,
                UserId = x.UserId,
                Email = x.Email,
                CreatedAtUtc = x.CreatedAtUtc
            })
            .ToListAsync(ct);

        return Ok(items);
    }
}