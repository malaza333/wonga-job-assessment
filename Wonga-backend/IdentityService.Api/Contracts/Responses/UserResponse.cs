namespace IdentityService.Api.Contracts.Responses
{
    public class UserResponse
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }

        public UserResponse(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
