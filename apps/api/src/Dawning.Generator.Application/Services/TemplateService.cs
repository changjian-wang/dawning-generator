using Dawning.Generator.Application.Dtos;
using Dawning.Generator.Domain.Enums;

namespace Dawning.Generator.Application.Services;

/// <summary>
/// 模板服务实现
/// </summary>
public class TemplateService : ITemplateService
{
    public TemplateOptionsDto GetOptions()
    {
        return new TemplateOptionsDto
        {
            ProjectTypes =
            [
                new OptionItem { Value = "backend", Label = "后端项目 (API)", IsDefault = true },
                new OptionItem { Value = "frontend", Label = "前端项目 (Web)", IsDefault = false },
                new OptionItem { Value = "fullstack", Label = "全栈项目 (API + Web)", IsDefault = false }
            ],
            DotNetVersions =
            [
                new OptionItem { Value = "net8", Label = ".NET 8.0 (LTS)", IsDefault = true },
                new OptionItem { Value = "net9", Label = ".NET 9.0", IsDefault = false }
            ],
            ArchitectureTypes =
            [
                new OptionItem { Value = "layered", Label = "分层架构", IsDefault = true },
                new OptionItem { Value = "simple", Label = "简单架构", IsDefault = false }
            ],
            FrontendFrameworks =
            [
                new OptionItem { Value = "vue-arco", Label = "Vue 3 + Arco Design", IsDefault = true },
                new OptionItem { Value = "vue-element", Label = "Vue 3 + Element Plus", IsDefault = false },
                new OptionItem { Value = "react-antd", Label = "React + Ant Design", IsDefault = false }
            ],
            DatabaseTypes =
            [
                new OptionItem { Value = "mysql", Label = "MySQL", IsDefault = true },
                new OptionItem { Value = "postgresql", Label = "PostgreSQL", IsDefault = false },
                new OptionItem { Value = "sqlserver", Label = "SQL Server", IsDefault = false },
                new OptionItem { Value = "sqlite", Label = "SQLite", IsDefault = false }
            ],
            AvailableModules =
            [
                new ModuleItem
                {
                    Id = "identity",
                    Name = "Dawning.Identity",
                    Description = "JWT 认证、用户上下文",
                    IsRequired = true,
                    IsDefault = true
                },
                new ModuleItem
                {
                    Id = "core",
                    Name = "Dawning.Core",
                    Description = "统一响应、业务异常",
                    IsRequired = false,
                    IsDefault = true
                },
                new ModuleItem
                {
                    Id = "logging",
                    Name = "Dawning.Logging",
                    Description = "Serilog 结构化日志",
                    IsRequired = false,
                    IsDefault = true
                },
                new ModuleItem
                {
                    Id = "dapper",
                    Name = "Dawning.ORM.Dapper",
                    Description = "Dapper CRUD 扩展",
                    IsRequired = false,
                    IsDefault = true
                },
                new ModuleItem
                {
                    Id = "caching",
                    Name = "Dawning.Caching",
                    Description = "Redis 分布式缓存",
                    IsRequired = false,
                    IsDefault = false
                },
                new ModuleItem
                {
                    Id = "messaging",
                    Name = "Dawning.Messaging",
                    Description = "消息队列集成",
                    IsRequired = false,
                    IsDefault = false
                },
                new ModuleItem
                {
                    Id = "resilience",
                    Name = "Dawning.Resilience",
                    Description = "Polly 重试、熔断",
                    IsRequired = false,
                    IsDefault = false
                }
            ],
            AvailableFeatures =
            [
                new FeatureItem
                {
                    Id = "tests",
                    Name = "单元测试",
                    Description = "包含 xUnit 测试项目",
                    IsDefault = true
                },
                new FeatureItem
                {
                    Id = "docker",
                    Name = "Docker",
                    Description = "包含 Dockerfile",
                    IsDefault = true
                },
                new FeatureItem
                {
                    Id = "swagger",
                    Name = "Swagger",
                    Description = "API 文档",
                    IsDefault = true
                },
                new FeatureItem
                {
                    Id = "healthchecks",
                    Name = "健康检查",
                    Description = "健康检查端点",
                    IsDefault = true
                },
                new FeatureItem
                {
                    Id = "helm",
                    Name = "Helm Chart",
                    Description = "Kubernetes 部署配置",
                    IsDefault = false
                },
                new FeatureItem
                {
                    Id = "github-actions",
                    Name = "GitHub Actions",
                    Description = "CI/CD 工作流",
                    IsDefault = false
                },
                new FeatureItem
                {
                    Id = "redis",
                    Name = "Redis 缓存",
                    Description = "分布式缓存支持",
                    IsDefault = false
                },
                new FeatureItem
                {
                    Id = "signalr",
                    Name = "SignalR",
                    Description = "实时通信支持",
                    IsDefault = false
                },
                new FeatureItem
                {
                    Id = "openiddict",
                    Name = "OpenIddict",
                    Description = "OAuth 2.0 认证服务器",
                    IsDefault = false
                },
                new FeatureItem
                {
                    Id = "echarts",
                    Name = "ECharts 图表",
                    Description = "前端图表库 (仅前端/全栈)",
                    IsDefault = true
                }
            ]
        };
    }
}
