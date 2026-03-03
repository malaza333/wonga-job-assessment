using AuditService.Api.Controllers;
using AuditService.Api.Contracts.Requests;
using AuditService.Api.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AuditService.UnitTests.Controllers;

public class AuditControllerTests
{
    private AuditController CreateSut(out AuditDbContext db)
    {
        var options = new DbContextOptionsBuilder<AuditDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        db = new AuditDbContext(options);
        return new AuditController(db);
    }

    [Fact]
    public async Task Create_Should_Save_Audit_Event()
    {
        // Arrange
        var sut = CreateSut(out var db);

        var request = new CreateAuditEventRequest
        {
            EventType = "UserRegistered",
            Email = "test@mail.com",
            UserId = Guid.NewGuid()
        };

        // Act
        var result = await sut.Create(request, CancellationToken.None);

        // Assert (DB)
        var count = await db.AuditEvents.CountAsync();
        count.Should().Be(1);

        var saved = await db.AuditEvents.SingleAsync();
        saved.EventType.Should().Be("UserRegistered");
        saved.Email.Should().Be("test@mail.com");
        saved.UserId.Should().Be(request.UserId);

        // Assert (HTTP result not null)
        result.Should().NotBeNull();
    }
}