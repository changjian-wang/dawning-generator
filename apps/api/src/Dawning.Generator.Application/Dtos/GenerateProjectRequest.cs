using Dawning.Generator.Domain.Enums;

namespace Dawning.Generator.Application.Dtos;

/// <summary>
/// 生成项目请求
/// </summary>
public class GenerateProjectRequest
{
    /// <summary>
    /// 项目名称
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
    
    /// <summary>
    /// 命名空间前缀 (可选)
    /// </summary>
    public string? NamespacePrefix { get; set; }
    
    /// <summary>
    /// 项目类型
    /// </summary>
    public ProjectType ProjectType { get; set; } = ProjectType.Backend;
    
    /// <summary>
    /// 架构类型 (后端适用)
    /// </summary>
    public ArchitectureType ArchitectureType { get; set; } = ArchitectureType.Layered;
    
    /// <summary>
    /// 前端框架 (前端/全栈适用)
    /// </summary>
    public FrontendFramework FrontendFramework { get; set; } = FrontendFramework.VueArco;
    
    /// <summary>
    /// .NET 版本
    /// </summary>
    public DotNetVersion DotNetVersion { get; set; } = DotNetVersion.Net8;
    
    /// <summary>
    /// 数据库配置
    /// </summary>
    public DatabaseConfig Database { get; set; } = new();
    
    /// <summary>
    /// 选择的模块
    /// </summary>
    public List<string> Modules { get; set; } = ["identity", "core", "logging"];
    
    /// <summary>
    /// 可选功能
    /// </summary>
    public FeatureOptions Features { get; set; } = new();
    
    /// <summary>
    /// 前端配置 (前端/全栈适用)
    /// </summary>
    public FrontendConfig Frontend { get; set; } = new();
    
    /// <summary>
    /// 服务端口
    /// </summary>
    public int ServicePort { get; set; } = 5000;
    
    /// <summary>
    /// 前端端口
    /// </summary>
    public int FrontendPort { get; set; } = 3000;
    
    /// <summary>
    /// 是否保存到历史记录
    /// </summary>
    public bool SaveToHistory { get; set; } = true;
}

public class DatabaseConfig
{
    public DatabaseType Type { get; set; } = DatabaseType.MySQL;
    public string ConnectionStringTemplate { get; set; } = "Server=localhost;Database={database};Uid=root;Pwd=password;";
}

public class FeatureOptions
{
    public bool IncludeTests { get; set; } = true;
    public bool IncludeDocker { get; set; } = true;
    public bool IncludeHelmChart { get; set; } = false;
    public bool IncludeGitHubActions { get; set; } = false;
    public bool IncludeSwagger { get; set; } = true;
    public bool IncludeHealthChecks { get; set; } = true;
    public bool UseRedis { get; set; } = false;
    public bool UseSignalR { get; set; } = false;
    public bool UseOpenIddict { get; set; } = false;
}

/// <summary>
/// 前端配置
/// </summary>
public class FrontendConfig
{
    /// <summary>
    /// 是否使用 ECharts 图表
    /// </summary>
    public bool UseECharts { get; set; } = true;
    
    /// <summary>
    /// API 基础路径
    /// </summary>
    public string ApiBaseUrl { get; set; } = "/api";
    
    /// <summary>
    /// 是否使用 SignalR (前端 WebSocket)
    /// </summary>
    public bool UseSignalR { get; set; } = false;
    
    /// <summary>
    /// 应用标题
    /// </summary>
    public string AppTitle { get; set; } = "";
}
