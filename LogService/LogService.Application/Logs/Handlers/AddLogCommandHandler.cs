using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogService.Application.Logs.Commands;
using LogService.Core.Entities;
using LogService.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace LogService.Application.Logs.Commands
{
    public class AddLogCommandHandler : IRequestHandler<AddLogCommand, Guid>
    {
        private readonly ILogRepository _repo;
        private readonly ILogger<AddLogCommandHandler> _logger;

        public AddLogCommandHandler(ILogRepository repo, ILogger<AddLogCommandHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Guid> Handle(AddLogCommand request, CancellationToken cancellationToken)
        {
            // Log nesnesini oluştur
            var log = new LogEntry
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow, // CreatedAt yerine Timestamp kullan
                Message = request.Message,
                Level = request.Level,
                Source = request.Source,      // Service yerine Source kullan
                Exception = request.Exception
            };

            // 1️⃣ DB’ye kaydet
            await _repo.AddAsync(log);

            // 2️⃣ Serilog üzerinden log at
            _logger.LogInformation("Log eklendi {@Log}", log);

            return log.Id;
        }
    }
}
