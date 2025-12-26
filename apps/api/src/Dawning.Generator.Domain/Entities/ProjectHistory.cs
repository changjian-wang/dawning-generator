using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dawning.Generator.Domain.Enums;

namespace Dawning.Generator.Domain.Entities;

/// <summary>
/// 项目生成历史记录
/// </summary>
[Table("project_histories")]
public class ProjectHistory
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("user_id")]
    public Guid UserId { get; set; }
    
    [Column("project_name")]
    public string ProjectName { get; set; } = string.Empty;
    
    [Column("namespace_prefix")]
    public string? NamespacePrefix { get; set; }
    
    [Column("architecture_type")]
    public string ArchitectureTypeValue { get; set; } = "layered";
    
    [NotMapped]
    public ArchitectureType ArchitectureType
    {
        get => Enum.TryParse<ArchitectureType>(ArchitectureTypeValue, true, out var result) ? result : ArchitectureType.Layered;
        set => ArchitectureTypeValue = value.ToString().ToLower();
    }
    
    [Column("dotnet_version")]
    public string DotNetVersionValue { get; set; } = "net8.0";
    
    [NotMapped]
    public DotNetVersion DotNetVersion
    {
        get => DotNetVersionValue == "net9.0" ? DotNetVersion.Net9 : DotNetVersion.Net8;
        set => DotNetVersionValue = value.ToFrameworkMoniker();
    }
    
    [Column("database_type")]
    public string DatabaseTypeValue { get; set; } = "mysql";
    
    [NotMapped]
    public DatabaseType DatabaseType
    {
        get => Enum.TryParse<DatabaseType>(DatabaseTypeValue, true, out var result) ? result : DatabaseType.MySQL;
        set => DatabaseTypeValue = value.ToString().ToLower();
    }
    
    [Column("selected_modules")]
    public string SelectedModulesJson { get; set; } = "[]";
    
    [NotMapped]
    public List<string> SelectedModules
    {
        get => System.Text.Json.JsonSerializer.Deserialize<List<string>>(SelectedModulesJson) ?? [];
        set => SelectedModulesJson = System.Text.Json.JsonSerializer.Serialize(value);
    }
    
    [Column("optional_features")]
    public string OptionalFeaturesJson { get; set; } = "[]";
    
    [NotMapped]
    public List<string> OptionalFeatures
    {
        get => System.Text.Json.JsonSerializer.Deserialize<List<string>>(OptionalFeaturesJson) ?? [];
        set => OptionalFeaturesJson = System.Text.Json.JsonSerializer.Serialize(value);
    }
    
    [Column("service_port")]
    public int ServicePort { get; set; }
    
    [Column("download_count")]
    public int DownloadCount { get; set; } = 1;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
