using FluentAssertions;
using IdentityService.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace IdentityService.IntegrationTests.Auth;

public class RegisterTests
{
    private readonly ITestOutputHelper _output;

    public RegisterTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public async Task Register_Should_Return_NoContent_And_Duplicate_Should_Return_Conflict()
    {
        await using var factory = new TestWebApplicationFactory();
        var client = factory.CreateClient();

        var email = $"test_{Guid.NewGuid():N}@mail.com";

        var request = new
        {
            firstName = "Thomas",
            lastName = "Selepe",
            email,
            password = "Password123!"
        };

        var res1 = await client.PostAsJsonAsync("/api/auth/register", request);
        _output.WriteLine($"First: {(int)res1.StatusCode} {res1.StatusCode}");
        _output.WriteLine(await res1.Content.ReadAsStringAsync());

        res1.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var res2 = await client.PostAsJsonAsync("/api/auth/register", request);
        _output.WriteLine($"Second: {(int)res2.StatusCode} {res2.StatusCode}");
        _output.WriteLine(await res2.Content.ReadAsStringAsync());

        res2.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}