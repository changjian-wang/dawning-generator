using System.Text.Json.Serialization;

namespace Dawning.Generator.Domain.Enums;

/// <summary>
/// 前端框架类型
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FrontendFramework
{
    /// <summary>
    /// Vue 3 + Arco Design
    /// </summary>
    VueArco,

    /// <summary>
    /// Vue 3 + Element Plus (未来支持)
    /// </summary>
    VueElement,

    /// <summary>
    /// React + Ant Design (未来支持)
    /// </summary>
    ReactAntd,
}
