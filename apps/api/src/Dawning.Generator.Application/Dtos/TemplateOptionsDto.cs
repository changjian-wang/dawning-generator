using Dawning.Generator.Domain.Enums;

namespace Dawning.Generator.Application.Dtos;

/// <summary>
/// 模板选项 DTO
/// </summary>
public class TemplateOptionsDto
{
    /// <summary>
    /// .NET 版本选项
    /// </summary>
    public List<OptionItem> DotNetVersions { get; set; } = [];
    
    /// <summary>
    /// 架构类型选项
    /// </summary>
    public List<OptionItem> ArchitectureTypes { get; set; } = [];
    
    /// <summary>
    /// 数据库类型选项
    /// </summary>
    public List<OptionItem> DatabaseTypes { get; set; } = [];
    
    /// <summary>
    /// 可用模块
    /// </summary>
    public List<ModuleItem> AvailableModules { get; set; } = [];
    
    /// <summary>
    /// 可选功能
    /// </summary>
    public List<FeatureItem> AvailableFeatures { get; set; } = [];
}

public class OptionItem
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}

public class ModuleItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public bool IsDefault { get; set; }
}

public class FeatureItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}
