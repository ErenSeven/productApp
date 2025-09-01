namespace AuthService.API.DTOs
{
    public record RegisterUserRequest(
        string UserName,
        string Email,
        string Password
    );
}