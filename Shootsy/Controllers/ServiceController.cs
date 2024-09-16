using Microsoft.AspNetCore.Mvc;
using Shootsy.Database;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Service")]
    public class ServiceController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationContext _context;

        public ServiceController(ApplicationContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        [HttpGet("health")]
        public async Task<IActionResult> HealthCheckAsync(CancellationToken cancellationToken = default)
        {
            var isCanConnectDatabase = await _context.Database.CanConnectAsync(cancellationToken);
            if (!isCanConnectDatabase)
                return StatusCode(500, "Ошибка сервера. Невозможно подключиться к базе данных");
            return StatusCode(200, "heath");
        }

        [HttpPost("ensure-created")]
        public async Task<IActionResult> EnsureCreatedAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.EnsureCreatedAsync(cancellationToken);
            return StatusCode(200);
        }

        [HttpDelete("ensure-deleted")]
        public async Task<IActionResult> EnsureDeletedAsync(CancellationToken cancellationToken = default)
        {
            var isCanConnectDatabase = await _context.Database.CanConnectAsync(cancellationToken);
            if (!isCanConnectDatabase)
                return StatusCode(500, "Ошибка сервера. Невозможно подключиться к базе данных");
            await _context.Database.EnsureDeletedAsync(cancellationToken);
            return StatusCode(200);
        }
    }
}
