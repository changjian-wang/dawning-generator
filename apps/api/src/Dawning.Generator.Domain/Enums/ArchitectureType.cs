using System.Text.Json.Serialization;

namespace Dawning.Generator.Domain.Enums;

/// <summary>
/// 项目架构类型
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ArchitectureType
{
    /// <summary>
    /// 分层架构 (Api/Application/Domain/Infrastructure)
    /// </summary>
    Layered,

    /// <summary>
    /// 整洁架构 (Clean Architecture)
    /// </summary>
    Clean,

    /// <summary>
    /// 简单架构 (单项目)
    /// </summary>
    Simple,
}
