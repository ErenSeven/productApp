using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Core.Entities;

namespace AuthService.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(ApplicationUser user);
        Task<RefreshToken> GenerateRefreshTokenAsync(ApplicationUser user);
        Task<bool> ValidateRefreshTokenAsync(string token, string userId);
    }
}