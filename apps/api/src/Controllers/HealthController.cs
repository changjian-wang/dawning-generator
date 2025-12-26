using Microsoft.AspNetCore.Mvc;

namespace Dawning.Generator.Api.Controllers;

/// <summary>
/// 健康检查控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// 健康检查
    /// </summary>
    [HttpGet]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            service = "dawning-generator"
        });
    }

    /// <summary>
    /// 就绪检查
    /// </summary>
    [HttpGet("ready")]
    public IActionResult Ready()
    {
        return Ok(new
        {
            status = "ready",
            timestamp = DateTime.UtcNow
        });
    }
}
