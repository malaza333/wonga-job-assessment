using FluentAssertions;
using IdentityService.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace IdentityService.IntegrationTests.Auth;

public class LoginTests
{
    [Fact]
    public async Task Login_Should_Return_Token_After_Register()
    {
        await using var factory = new TestWebApplicationFactory();
        var client = factory.CreateClient();

        var email = $"test_{Guid.NewGuid():N}@mail.com";
        var password = "Password123!";

        // Register
        var regReq = new
        {
            firstName = "Thomas",
            lastName = "Selepe",
            email,
            password
        };

        var regRes = await client.PostAsJsonAsync("/api/auth/register", regReq);
        regRes.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Login
        var loginReq = new { email, password };
        var loginRes = await client.PostAsJsonAsync("/api/auth/login", loginReq);

        loginRes.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await loginRes.Content.ReadFromJsonAsync<LoginResponse>();
        json.Should().NotBeNull();
        json!.Token.Should().NotBeNullOrWhiteSpace();
    }

    private sealed class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}