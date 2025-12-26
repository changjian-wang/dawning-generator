using Dawning.Generator.Application.Services;
using Dawning.Generator.Application.Templates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dawning.Generator.Application;

/// <summary>
/// 应用层依赖注入
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration,
        string? contentRootPath = null
    )
    {
        // 模板引擎
        var templatesPath = configuration.GetValue<string>("Templates:BasePath") ?? "./templates";

        // 如果是相对路径，则基于 contentRootPath 解析
        if (!Path.IsPathRooted(templatesPath) && !string.IsNullOrEmpty(contentRootPath))
        {
            templatesPath = Path.Combine(contentRootPath, templatesPath);
        }

        services.AddSingleton(new TemplateEngine(templatesPath));

        // 服务
        services.AddScoped<IProjectGeneratorService, ProjectGeneratorService>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<IProjectHistoryService, ProjectHistoryService>();

        return services;
    }
}
