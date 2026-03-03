using IdentityService.Application.Users.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Application.Users.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterUserCommand command, CancellationToken ct = default);
        Task<string> LoginAsync(LoginUserCommand command, CancellationToken ct = default);
    }
}
