using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain.Users
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        private User() { } // EF Core

        public User(Guid id, string firstName, string lastName, string email, string passwordHash)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id is required");
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("FirstName is required");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("LastName is required");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required");
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("PasswordHash is required");

            Id = id;
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim().ToLowerInvariant();
            PasswordHash = passwordHash;
        }
    }
}
