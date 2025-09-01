using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogService.Core.Entities;
using MediatR;
using System.Collections.Generic;
namespace LogService.Application.Logs.Queries
{
    public class GetLogsQuery : IRequest<IEnumerable<LogEntry>>
    {
    }
}