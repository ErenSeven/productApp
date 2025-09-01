using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogService.Core.Entities;
using System.Threading.Tasks;

namespace LogService.Core.Interfaces
{
    public interface ILogService
    {
        Task LogInfoAsync(string message, string? source = null);
        Task LogErrorAsync(string message, string? source = null, string? exception = null);
    }
}
