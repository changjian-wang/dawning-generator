using System.Text.Json.Serialization;

namespace Dawning.Generator.Domain.Enums;

/// <summary>
/// 项目类型
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProjectType
{
    /// <summary>
    /// 后端项目 (仅 API)
    /// </summary>
    Backend,

    /// <summary>
    /// 前端项目 (仅 Web)
    /// </summary>
    Frontend,

    /// <summary>
    /// 全栈项目 (API + Web)
    /// </summary>
    Fullstack,
}
