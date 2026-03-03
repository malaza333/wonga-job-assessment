using IdentityService.Api.Clients;
using IdentityService.Api.Contracts.Requests;
using IdentityService.Api.Contracts.Responses;
using IdentityService.Application.Common.Clients;
using IdentityService.Application.Users.Commands;
using IdentityService.Application.Users.Interfaces;
using IdentityService.Application.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityService.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly IUserRepository _users;
    private readonly IAuditClient _auditClient;

    public AuthController(IAuthService auth, IUserRepository users, IAuditClient auditClient)
    {
        _auth = auth;
        _users = users;
        _auditClient = auditClient;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var cmd = new RegisterUserCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password
        };

        await _auth.RegisterAsync(cmd, ct);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var cmd = new LoginUserCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        var token = await _auth.LoginAsync(cmd, ct);
        return Ok(new AuthResponse (token));
    }

    // user details endpoint (authorized)
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserResponse>> Me(CancellationToken ct)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? User.FindFirstValue(ClaimTypes.Name)
                          ?? User.FindFirstValue("sub");

        if (!Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var user = await _users.GetByIdAsync(userId, ct);
        if (user is null) return NotFound();

        return Ok(new UserResponse
        (
             user.FirstName,
             user.LastName,
             user.Email
        ));
    }
   
    [Authorize]
    [HttpGet("me/audit-logs")]
    public async Task<ActionResult<IEnumerable<AuditEventResponse>>> GetMyAuditLogs(
    [FromQuery] int take = 20,
    CancellationToken ct = default)
    {
        var email =
            User.FindFirstValue(ClaimTypes.Email)
            ?? User.FindFirstValue(ClaimTypes.Name)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(email))
            return Unauthorized();

        var logs = await _auditClient.GetUserLogEventAsync(email, take, ct);

        return Ok(logs);
    }
}