using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawning.Generator.Domain.Entities;

/// <summary>
/// 模板收藏
/// </summary>
[Table("template_favorites")]
public class TemplateFavorite
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("user_id")]
    public Guid UserId { get; set; }
    
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    [Column("description")]
    public string? Description { get; set; }
    
    [Column("config")]
    public string ConfigJson { get; set; } = string.Empty;
    
    [Column("is_default")]
    public bool IsDefault { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
