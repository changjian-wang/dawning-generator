using Dawning.Generator.Domain.Entities;

namespace Dawning.Generator.Domain.Repositories;

/// <summary>
/// 模板收藏仓储接口
/// </summary>
public interface ITemplateFavoriteRepository
{
    Task<TemplateFavorite?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TemplateFavorite>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(TemplateFavorite favorite, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(TemplateFavorite favorite, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SetDefaultAsync(Guid userId, Guid favoriteId, CancellationToken cancellationToken = default);
}
