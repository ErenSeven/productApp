
namespace AuthService.API.DTOs
{
    public record RefreshTokenUserRequest(
        string UserId,
        string RefreshToken
    );
}