namespace IdentityService.Api.Contracts.Responses
{
    public class AuthResponse
    {
        public string Token { get; }

        public AuthResponse(string token)
        {
            Token = token;
        }
    }
}
