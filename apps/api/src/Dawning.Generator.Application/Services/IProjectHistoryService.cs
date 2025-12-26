using Dawning.Generator.Application.Dtos;

namespace Dawning.Generator.Application.Services;

/// <summary>
/// 项目历史服务接口
/// </summary>
public interface IProjectHistoryService
{
    Task<IEnumerable<ProjectHistoryDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ProjectHistoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GenerateProjectResponse> RegenerateAsync(Guid historyId, Guid userId, CancellationToken cancellationToken = default);
}
