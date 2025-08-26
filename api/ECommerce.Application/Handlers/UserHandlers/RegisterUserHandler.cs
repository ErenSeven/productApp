using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.Commands.Users;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using MediatR;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Handlers.UserHandlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Guid>
    {   
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(IUserRepository userRepository, ILogger<RegisterUserHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kullanıcı kayıt isteği alındı. Email: {Email}", request.Email);
            using var sha256 = SHA256.Create();
            var passwordHash = Convert.ToBase64String(
                sha256.ComputeHash(Encoding.UTF8.GetBytes(request.Password))
            );

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = passwordHash
            };

            await _userRepository.AddAsync(user);
            _logger.LogInformation("Kullanıcı başarıyla kaydedildi. UserId: {UserId}, Email: {Email}", user.Id, user.Email);
            
            return user.Id;
        }
    }
}