
namespace AuthService.API.DTOs
{
    public record LoginUserRequest(
        string Email,
        string Password
    );
}