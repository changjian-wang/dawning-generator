namespace Dawning.Generator.Domain.Entities;

/// <summary>
/// 生成统计 (管理员用)
/// </summary>
public class GenerationStats
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public int TotalGenerations { get; set; }
    public int UniqueUsers { get; set; }
    public string? ByArchitectureJson { get; set; }
    public string? ByDatabaseJson { get; set; }
    public string? ByModuleJson { get; set; }
    public DateTime CreatedAt { get; set; }
}
