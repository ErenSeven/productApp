using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogService.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogService.Core.Interfaces
{
    public interface ILogRepository
    {
        Task AddAsync(LogEntry log);
        Task<IEnumerable<LogEntry>> GetAllAsync();
    }
}