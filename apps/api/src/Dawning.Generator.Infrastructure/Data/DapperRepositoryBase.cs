using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;
using Dapper;

namespace Dawning.Generator.Infrastructure.Data;

/// <summary>
/// Dapper 通用仓储基类，封装常见 CRUD 操作
/// </summary>
public abstract class DapperRepositoryBase<TEntity, TKey> where TEntity : class
{
    protected readonly IDbConnectionFactory ConnectionFactory;
    private readonly string _tableName;
    private readonly string _keyColumn;
    private readonly PropertyInfo[] _properties;

    protected DapperRepositoryBase(IDbConnectionFactory connectionFactory)
    {
        ConnectionFactory = connectionFactory;
        _tableName = GetTableName();
        _keyColumn = GetKeyColumn();
        _properties = typeof(TEntity).GetProperties()
            .Where(p => p.CanRead && p.CanWrite)
            .ToArray();
    }

    /// <summary>
    /// 根据 ID 查询
    /// </summary>
    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        using var connection = ConnectionFactory.CreateConnection();
        var sql = $"SELECT * FROM {_tableName} WHERE {_keyColumn} = @Id";
        return await connection.QueryFirstOrDefaultAsync<TEntity>(sql, new { Id = id });
    }

    /// <summary>
    /// 查询所有
    /// </summary>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = ConnectionFactory.CreateConnection();
        var sql = $"SELECT * FROM {_tableName}";
        return await connection.QueryAsync<TEntity>(sql);
    }

    /// <summary>
    /// 根据条件查询
    /// </summary>
    public virtual async Task<IEnumerable<TEntity>> GetByConditionAsync(
        string whereClause, 
        object? parameters = null,
        string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        using var connection = ConnectionFactory.CreateConnection();
        var sql = $"SELECT * FROM {_tableName} WHERE {whereClause}";
        if (!string.IsNullOrEmpty(orderBy))
        {
            sql += $" ORDER BY {orderBy}";
        }
        return await connection.QueryAsync<TEntity>(sql, parameters);
    }

    /// <summary>
    /// 插入实体
    /// </summary>
    public virtual async Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        using var connection = ConnectionFactory.CreateConnection();
        var columns = GetColumns(excludeKey: false);
        var values = GetColumnParameters(excludeKey: false);
        var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({values})";
        return await connection.ExecuteAsync(sql, entity);
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    public virtual async Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        using var connection = ConnectionFactory.CreateConnection();
        var setClause = GetUpdateSetClause();
        var sql = $"UPDATE {_tableName} SET {setClause} WHERE {_keyColumn} = @{GetKeyPropertyName()}";
        return await connection.ExecuteAsync(sql, entity);
    }

    /// <summary>
    /// 删除实体
    /// </summary>
    public virtual async Task<int> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        using var connection = ConnectionFactory.CreateConnection();
        var sql = $"DELETE FROM {_tableName} WHERE {_keyColumn} = @Id";
        return await connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// 执行自定义 SQL
    /// </summary>
    protected async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        using var connection = ConnectionFactory.CreateConnection();
        return await connection.ExecuteAsync(sql, parameters);
    }

    /// <summary>
    /// 查询自定义 SQL
    /// </summary>
    protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
    {
        using var connection = ConnectionFactory.CreateConnection();
        return await connection.QueryAsync<T>(sql, parameters);
    }

    /// <summary>
    /// 查询单个结果
    /// </summary>
    protected async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        using var connection = ConnectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    #region Private Methods

    private static string GetTableName()
    {
        var tableAttr = typeof(TEntity).GetCustomAttribute<TableAttribute>();
        if (tableAttr != null)
        {
            return tableAttr.Name;
        }
        
        // 默认使用类名的复数形式，转换为 snake_case
        var name = typeof(TEntity).Name;
        return ToSnakeCase(name) + "s";
    }

    private string GetKeyColumn()
    {
        var keyProperty = typeof(TEntity).GetProperties()
            .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
        
        if (keyProperty != null)
        {
            var columnAttr = keyProperty.GetCustomAttribute<ColumnAttribute>();
            return columnAttr?.Name ?? ToSnakeCase(keyProperty.Name);
        }
        
        // 默认使用 "id"
        return "id";
    }

    private string GetKeyPropertyName()
    {
        var keyProperty = typeof(TEntity).GetProperties()
            .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
        return keyProperty?.Name ?? "Id";
    }

    private string GetColumns(bool excludeKey)
    {
        var props = _properties
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null)
            .Select(p =>
            {
                var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                return columnAttr?.Name ?? ToSnakeCase(p.Name);
            });
        return string.Join(", ", props);
    }

    private string GetColumnParameters(bool excludeKey)
    {
        var props = _properties
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null)
            .Select(p => $"@{p.Name}");
        return string.Join(", ", props);
    }

    private string GetUpdateSetClause()
    {
        var props = _properties
            .Where(p => p.GetCustomAttribute<KeyAttribute>() == null)
            .Select(p =>
            {
                var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                var columnName = columnAttr?.Name ?? ToSnakeCase(p.Name);
                return $"{columnName} = @{p.Name}";
            });
        return string.Join(", ", props);
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        
        var sb = new StringBuilder();
        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (char.IsUpper(c))
            {
                if (i > 0) sb.Append('_');
                sb.Append(char.ToLower(c));
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    #endregion
}
