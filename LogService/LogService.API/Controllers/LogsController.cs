using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LogService.Application.Logs.Commands;
using LogService.Application.Logs.Queries;
using LogService.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace LogService.API.Controllers
{
    [Authorize(Policy = "AdminOnly")] 
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LogsController> _logger; 

        public LogsController(IMediator mediator, ILogger<LogsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // POST /api/logs
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddLogCommand cmd)
        {
            var id = await _mediator.Send(cmd);

            // 🔹 Burada sadece cmd’yi arg olarak veriyoruz, string interpolation yok
            _logger.LogInformation("Yeni log eklendi: {@Log}", cmd);

            return CreatedAtAction(nameof(GetAll), new { id }, new { id });
        }

        // GET /api/logs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _mediator.Send(new GetLogsQuery());

            // 🔹 Burada da sadece arg olarak Count veriyoruz
            _logger.LogInformation("Tüm loglar getirildi. Toplam: {Count}", logs.Count());

            return Ok(logs);
        }
    }
}
