using Dawning.Generator.Domain.Entities;

namespace Dawning.Generator.Domain.Repositories;

/// <summary>
/// 项目历史仓储接口
/// </summary>
public interface IProjectHistoryRepository
{
    Task<ProjectHistory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProjectHistory>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(ProjectHistory history, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(ProjectHistory history, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IncrementDownloadCountAsync(Guid id, CancellationToken cancellationToken = default);
}
