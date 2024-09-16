using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Service")]
    public class ServiceController : Controller
    {
        private readonly HttpClient _httpClient;

        public ServiceController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return StatusCode(200, "heath");
        }
    }
}
