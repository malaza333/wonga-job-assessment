using FluentAssertions;
using IdentityService.Application.Common.Clients;
using IdentityService.Application.Common.Interfaces;
using IdentityService.Application.Users.Commands;
using IdentityService.Application.Users.Interfaces;
using IdentityService.Application.Users.Services;
using IdentityService.Domain.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.UnitTests.Users.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _repo = new();
        private readonly Mock<IPasswordHasher> _hasher = new();
        private readonly Mock<IJwtTokenGenerator> _jwt = new();
        private readonly Mock<IAuditClient> _audit = new();

        private AuthService CreateSut()
            => new AuthService(
                _repo.Object,
                _hasher.Object,
                _jwt.Object,
                _audit.Object);

        // Register Success
        [Fact]
        public async Task Register_Should_Create_User_When_Email_Not_Exists()
        {
            var command = new RegisterUserCommand
            {
                FirstName = "Thomas",
                LastName = "Selepe",
                Email = "test@mail.com",
                Password = "123"
            };

            _repo.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), default))
                 .ReturnsAsync((User?)null);

            _hasher.Setup(x => x.Hash(command.Password))
                   .Returns("HASH");

            var sut = CreateSut();

            await sut.RegisterAsync(command);

            _repo.Verify(x =>
                x.AddAsync(It.IsAny<User>(), default),
                Times.Once);
        }

        // Duplicate Email
        [Fact]
        public async Task Register_Should_Throw_When_Email_Exists()
        {
            var existingUser =
                new User(Guid.NewGuid(), "A", "B", "test@mail.com", "HASH");

            _repo.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), default))
                 .ReturnsAsync(existingUser);

            var sut = CreateSut();

            var command = new RegisterUserCommand
            {
                FirstName = "Thomas",
                LastName = "Selepe",
                Email = "test@mail.com",
                Password = "123"
            };

            Func<Task> act = () => sut.RegisterAsync(command);

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Email already exists.");
        }

        // Login Success
        [Fact]
        public async Task Login_Should_Return_Token_When_Credentials_Valid()
        {
            var user =
                new User(Guid.NewGuid(), "Thomas", "Selepe", "test@mail.com", "HASH");

            _repo.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), default))
                 .ReturnsAsync(user);

            _hasher.Setup(x => x.Verify("123", "HASH"))
                   .Returns(true);

            _jwt.Setup(x => x.Generate(user))
                .Returns("TOKEN");

            var sut = CreateSut();

            var result = await sut.LoginAsync(
                new LoginUserCommand
                {
                    Email = "test@mail.com",
                    Password = "123"
                });

            result.Should().Be("TOKEN");
        }

        // Wrong Password
        [Fact]
        public async Task Login_Should_Throw_When_Password_Invalid()
        {
            var user =
                new User(Guid.NewGuid(), "Thomas", "Selepe", "test@mail.com", "HASH");

            _repo.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), default))
                 .ReturnsAsync(user);

            _hasher.Setup(x => x.Verify(It.IsAny<string>(), "HASH"))
                   .Returns(false);

            var sut = CreateSut();

            Func<Task> act = () =>
                sut.LoginAsync(new LoginUserCommand
                {
                    Email = "test@mail.com",
                    Password = "wrong"
                });

            await act.Should()
                .ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
