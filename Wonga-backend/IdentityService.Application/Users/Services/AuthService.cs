using IdentityService.Application.Common.Interfaces;
using IdentityService.Application.Users.Commands;
using IdentityService.Application.Users.Interfaces;
using IdentityService.Domain.Users;
using IdentityService.Application.Common.Clients;
namespace IdentityService.Application.Users.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenGenerator _jwt;
    private readonly IAuditClient _audit;
    //private readonly ILogger<AuthService> _logger;
    public AuthService(IUserRepository users, IPasswordHasher hasher, IJwtTokenGenerator jwt, IAuditClient audit)
    {
        _users = users;
        _hasher = hasher;
        _jwt = jwt;
        _audit = audit;
 }

    public async Task RegisterAsync(RegisterUserCommand command, CancellationToken ct = default)
    {
        
        var email = command.Email.Trim().ToLowerInvariant();

        var existing = await _users.GetByEmailAsync(email, ct);
        if (existing is not null)
            throw new InvalidOperationException("Email already exists.");

        var hash = _hasher.Hash(command.Password);

        var user = new User(
            Guid.NewGuid(),
            command.FirstName,
            command.LastName,
            email,
            hash
        );

        await _users.AddAsync(user, ct);


        try
        {
            await _audit.LogEventAsync("UserRegistered", user.Id, user.Email, ct);
            //_ = Task.Run(() =>_audit.LogEventAsync("UserRegistered", user.Id, user.Email));
        }
        catch (Exception ex)
        {
            // log and continue (don’t throw)
            //_logger.LogWarning(ex, "Audit logging failed for UserRegistered. Continuing.");
        }
       

    }

    public async Task<string> LoginAsync(LoginUserCommand command, CancellationToken ct = default)
    {
        var email = command.Email.Trim().ToLowerInvariant();

        var user = await _users.GetByEmailAsync(email, ct);
        if (user is null)
            throw new UnauthorizedAccessException("Invalid credentials.");

        if (!_hasher.Verify(command.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");



        try
        {
            await _audit.LogEventAsync("UserLoggedIn", user.Id, user.Email, ct);
            //_ = Task.Run(() => _audit.LogEventAsync("UserLoggedIn", user.Id, user.Email, ct));
        }
        catch (Exception ex)
        {
          //  _logger.LogWarning(ex, "Audit logging failed for UserLoggedIn. Continuing.");
        }



         return _jwt.Generate(user);
    }
}