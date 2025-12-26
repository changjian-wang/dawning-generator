namespace Dawning.Generator.Application.Dtos;

/// <summary>
/// 生成项目响应
/// </summary>
public class GenerateProjectResponse
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// 项目名称
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
    
    /// <summary>
    /// ZIP 文件名
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    
    /// <summary>
    /// ZIP 文件内容
    /// </summary>
    public byte[] Content { get; set; } = [];
    
    /// <summary>
    /// 历史记录 ID (如果保存)
    /// </summary>
    public Guid? HistoryId { get; set; }
    
    /// <summary>
    /// 错误消息
    /// </summary>
    public string? ErrorMessage { get; set; }
}
