using Dawning.Generator.Application.Dtos;
using Dawning.Generator.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawning.Generator.Api.Controllers;

/// <summary>
/// 项目生成控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GeneratorController : ControllerBase
{
    private readonly IProjectGeneratorService _generatorService;
    private readonly ITemplateService _templateService;
    private readonly ILogger<GeneratorController> _logger;

    public GeneratorController(
        IProjectGeneratorService generatorService,
        ITemplateService templateService,
        ILogger<GeneratorController> logger)
    {
        _generatorService = generatorService;
        _templateService = templateService;
        _logger = logger;
    }

    /// <summary>
    /// 生成项目
    /// </summary>
    [HttpPost("generate")]
    [AllowAnonymous]
    public async Task<IActionResult> Generate([FromBody] GenerateProjectRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _generatorService.GenerateAsync(request, userId, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }

        return File(result.Content, "application/zip", result.FileName);
    }

    /// <summary>
    /// 获取可选配置项
    /// </summary>
    [HttpGet("options")]
    public IActionResult GetOptions()
    {
        var options = _templateService.GetOptions();
        return Ok(options);
    }

    private Guid? GetUserId()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("id");
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }
        return null;
    }
}
