using Dawning.Generator.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawning.Generator.Api.Controllers;

/// <summary>
/// 项目历史控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class HistoryController : ControllerBase
{
    private readonly IProjectHistoryService _historyService;
    private readonly ILogger<HistoryController> _logger;

    public HistoryController(
        IProjectHistoryService historyService,
        ILogger<HistoryController> logger)
    {
        _historyService = historyService;
        _logger = logger;
    }

    /// <summary>
    /// 获取当前用户的生成历史
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetHistory(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var histories = await _historyService.GetByUserIdAsync(userId.Value, cancellationToken);
        return Ok(histories);
    }

    /// <summary>
    /// 获取单个历史详情
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetHistoryById(Guid id, CancellationToken cancellationToken)
    {
        var history = await _historyService.GetByIdAsync(id, cancellationToken);
        
        if (history == null)
        {
            return NotFound();
        }

        return Ok(history);
    }

    /// <summary>
    /// 删除历史记录
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteHistory(Guid id, CancellationToken cancellationToken)
    {
        var result = await _historyService.DeleteAsync(id, cancellationToken);
        
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// 重新生成项目
    /// </summary>
    [HttpPost("{id:guid}/regenerate")]
    public async Task<IActionResult> Regenerate(Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var result = await _historyService.RegenerateAsync(id, userId.Value, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }

        return File(result.Content, "application/zip", result.FileName);
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
