using IdentityService.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}
