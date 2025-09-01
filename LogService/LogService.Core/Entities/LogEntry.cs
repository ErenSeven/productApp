using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogService.Core.Entities
{
    public class LogEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Source { get; set; }
        public string? Exception { get; set; }
    }
}