using Dawning.Generator.Domain.Enums;

namespace Dawning.Generator.Application.Dtos;

/// <summary>
/// 项目历史 DTO
/// </summary>
public class ProjectHistoryDto
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string? NamespacePrefix { get; set; }
    public string ArchitectureType { get; set; } = string.Empty;
    public string DotNetVersion { get; set; } = string.Empty;
    public string DatabaseType { get; set; } = string.Empty;
    public List<string> SelectedModules { get; set; } = [];
    public List<string> OptionalFeatures { get; set; } = [];
    public int ServicePort { get; set; }
    public int DownloadCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
