using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogService.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LogService.Core.Entities;
using LogService.Application.Logs.Queries;

namespace LogService.Application.Logs.Handlers
{
    public class GetLogsQueryHandler : IRequestHandler<GetLogsQuery, IEnumerable<LogEntry>>
    {
        private readonly ILogRepository _repository;

        public GetLogsQueryHandler(ILogRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LogEntry>> Handle(GetLogsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
