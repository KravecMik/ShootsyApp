using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Database;
using Swashbuckle.AspNetCore.Annotations;

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

        [AllowAnonymous]
        [HttpGet("health")]
        [SwaggerOperation(Summary = "Проверка доступности сервера")]
        public async Task<IActionResult> HealthCheckAsync(CancellationToken cancellationToken = default)
        {
            var isCanConnectDatabase = await _context.Database.CanConnectAsync(cancellationToken);
            if (!isCanConnectDatabase)
                return StatusCode(500, "Ошибка сервера. Невозможно подключиться к базе данных");
            return StatusCode(200, "heath");
        }

        [AllowAnonymous]
        [HttpPost("ensure-created")]
        [SwaggerOperation(Summary = "Создает Postgres базу данных, если она удалена (для миграций)")]
        public async Task<IActionResult> EnsureCreatedAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.EnsureCreatedAsync(cancellationToken);
            return StatusCode(200);
        }

        [AllowAnonymous]
        [HttpDelete("ensure-deleted")]
        [SwaggerOperation(Summary = "Удаляет Postgres таблицы и БД (для миграций)")]
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