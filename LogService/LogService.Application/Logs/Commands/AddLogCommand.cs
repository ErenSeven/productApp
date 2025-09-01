using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace LogService.Application.Logs.Commands
{
    public class AddLogCommand : IRequest<Guid>
    {
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Source { get; set; }
        public string? Exception { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}