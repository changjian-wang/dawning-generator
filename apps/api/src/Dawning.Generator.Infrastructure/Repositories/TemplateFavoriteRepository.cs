using Dapper;
using Dawning.Generator.Domain.Entities;
using Dawning.Generator.Domain.Repositories;
using Dawning.Generator.Infrastructure.Data;
using Dawning.ORM.Dapper;

namespace Dawning.Generator.Infrastructure.Repositories;

/// <summary>
/// 模板收藏仓储实现 (使用 Dawning.ORM.Dapper)
/// </summary>
public class TemplateFavoriteRepository : ITemplateFavoriteRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TemplateFavoriteRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<TemplateFavorite?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.GetAsync<TemplateFavorite>(id);
    }

    public async Task<IEnumerable<TemplateFavorite>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection
            .Builder<TemplateFavorite>()
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.IsDefault)
            .ThenByDescending(f => f.CreatedAt)
            .AsListAsync();
    }

    public async Task<Guid> CreateAsync(
        TemplateFavorite favorite,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = _connectionFactory.CreateConnection();

        favorite.Id = Guid.NewGuid();
        favorite.CreatedAt = DateTime.UtcNow;
        favorite.UpdatedAt = DateTime.UtcNow;

        await connection.InsertAsync(favorite);
        return favorite.Id;
    }

    public async Task<bool> UpdateAsync(
        TemplateFavorite favorite,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = _connectionFactory.CreateConnection();
        favorite.UpdatedAt = DateTime.UtcNow;
        return await connection.UpdateAsync(favorite);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        var entity = await connection.GetAsync<TemplateFavorite>(id);
        if (entity == null)
            return false;
        return await connection.DeleteAsync(entity);
    }

    public async Task<bool> SetDefaultAsync(
        Guid userId,
        Guid favoriteId,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // 先清除该用户的所有默认标记
            const string clearSql = """
                UPDATE template_favorites
                SET is_default = FALSE, updated_at = @UpdatedAt
                WHERE user_id = @UserId
                """;
            await connection.ExecuteAsync(
                clearSql,
                new { UserId = userId, UpdatedAt = DateTime.UtcNow },
                transaction
            );

            // 设置新的默认
            const string setSql = """
                UPDATE template_favorites
                SET is_default = TRUE, updated_at = @UpdatedAt
                WHERE id = @Id AND user_id = @UserId
                """;
            var affected = await connection.ExecuteAsync(
                setSql,
                new
                {
                    Id = favoriteId,
                    UserId = userId,
                    UpdatedAt = DateTime.UtcNow,
                },
                transaction
            );

            transaction.Commit();
            return affected > 0;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
