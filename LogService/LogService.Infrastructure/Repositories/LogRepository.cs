using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogService.Core.Entities;
using LogService.Core.Interfaces;
using LogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogService.Infrastructure.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly LogDbContext _context;

        public LogRepository(LogDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LogEntry log)
        {
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LogEntry>> GetAllAsync()
        {
            return await _context.Logs.AsNoTracking().ToListAsync();
        }
    }
}
