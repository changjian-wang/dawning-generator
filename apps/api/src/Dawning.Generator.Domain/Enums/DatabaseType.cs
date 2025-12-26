using System.Text.Json.Serialization;

namespace Dawning.Generator.Domain.Enums;

/// <summary>
/// 数据库类型
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DatabaseType
{
    MySQL,
    PostgreSQL,
    SqlServer,
    SQLite,
}
