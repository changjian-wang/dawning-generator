using Dawning.Generator.Application.Dtos;

namespace Dawning.Generator.Application.Services;

/// <summary>
/// 项目生成器服务接口
/// </summary>
public interface IProjectGeneratorService
{
    /// <summary>
    /// 生成项目
    /// </summary>
    Task<GenerateProjectResponse> GenerateAsync(GenerateProjectRequest request, Guid? userId = null, CancellationToken cancellationToken = default);
}
