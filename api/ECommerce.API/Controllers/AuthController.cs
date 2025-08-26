using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.Commands.Users;
using ECommerce.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Cors;

namespace ECommerce.API.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly JwtService _jwtService;
        public AuthController(IMediator mediator, JwtService jwtService, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(new { userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _jwtService.ValidateUserAsync(request.Email, request.Password);
            if (user == null) return Unauthorized();

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}