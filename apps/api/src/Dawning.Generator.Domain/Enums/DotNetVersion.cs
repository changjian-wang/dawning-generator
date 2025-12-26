using System.Text.Json.Serialization;

namespace Dawning.Generator.Domain.Enums;

/// <summary>
/// .NET 版本
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DotNetVersion
{
    Net8,
    Net9,
}

public static class DotNetVersionExtensions
{
    public static string ToFrameworkMoniker(this DotNetVersion version)
    {
        return version switch
        {
            DotNetVersion.Net8 => "net8.0",
            DotNetVersion.Net9 => "net9.0",
            _ => "net8.0",
        };
    }
}
