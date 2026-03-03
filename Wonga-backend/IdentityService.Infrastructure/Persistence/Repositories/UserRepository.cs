using IdentityService.Application.Users.Interfaces;
using IdentityService.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _db;

        public UserRepository(IdentityDbContext db)
        {
            _db = db;
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
            => _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, ct);

        public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);

        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);
        }
    }
}
