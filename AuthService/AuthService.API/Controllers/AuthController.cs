using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.API.DTOs;
using AuthService.Core.Entities;
using AuthService.Core.Events;
using AuthService.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMessagePublisher _publisher;

        public AuthController(IUserRepository userRepository, ITokenService tokenService, IMessagePublisher publisher)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _publisher = publisher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already in use.");
            }
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                UserType = "User"
            };

            await _userRepository.CreateAsync(user, request.Password);

            var evt = new UserRegisteredEvent
            {
                UserId = user.Id,
                Email = user.Email,
                RegisteredAt = DateTime.UtcNow
            };
            await _publisher.PublishAsync("user.registered", evt);

            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !await _userRepository.CheckPasswordAsync(user, request.Password))
                return Unauthorized("Invalid credentials.");

            var jwt = _tokenService.GenerateJwtToken(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);

            var evt = new UserLoggedInEvent
            {
                UserId = user.Id,
                Email = user.Email,
                LoggedInAt = DateTime.UtcNow
            };


            await _publisher.PublishAsync("user.loggedin", evt);
            
            return Ok(new
            {
                token = jwt,
                refreshToken = refreshToken.Token
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenUserRequest request)
        {
            var valid = await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken, request.UserId);
            if (!valid)
                return Unauthorized("Invalid refresh token.");

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return NotFound("User not found.");

            var newJwt = _tokenService.GenerateJwtToken(user);

            return Ok(new { token = newJwt });
        }
    }
}