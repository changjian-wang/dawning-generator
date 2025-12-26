using Dawning.Generator.Domain.Entities;
using Dawning.Generator.Domain.Repositories;
using Dawning.Generator.Infrastructure.Data;

namespace Dawning.Generator.Infrastructure.Repositories;

/// <summary>
/// 项目历史仓储实现 (使用 DapperRepositoryBase 简化)
/// </summary>
public class ProjectHistoryRepository
    : DapperRepositoryBase<ProjectHistory, Guid>,
        IProjectHistoryRepository
{
    public ProjectHistoryRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory) { }

    public new async Task<ProjectHistory?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await base.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<ProjectHistory>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        return await GetByConditionAsync(
            "user_id = @UserId",
            new { UserId = userId },
            "created_at DESC",
            cancellationToken
        );
    }

    public async Task<Guid> CreateAsync(
        ProjectHistory history,
        CancellationToken cancellationToken = default
    )
    {
        history.Id = Guid.NewGuid();
        history.CreatedAt = DateTime.UtcNow;
        history.UpdatedAt = DateTime.UtcNow;

        await InsertAsync(history, cancellationToken);
        return history.Id;
    }

    public new async Task<bool> UpdateAsync(
        ProjectHistory history,
        CancellationToken cancellationToken = default
    )
    {
        history.UpdatedAt = DateTime.UtcNow;
        var affected = await base.UpdateAsync(history, cancellationToken);
        return affected > 0;
    }

    public new async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var affected = await base.DeleteAsync(id, cancellationToken);
        return affected > 0;
    }

    public async Task<bool> IncrementDownloadCountAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var affected = await ExecuteAsync(
            "UPDATE project_histories SET download_count = download_count + 1, updated_at = @UpdatedAt WHERE id = @Id",
            new { Id = id, UpdatedAt = DateTime.UtcNow }
        );
        return affected > 0;
    }
}
