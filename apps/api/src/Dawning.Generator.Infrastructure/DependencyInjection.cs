using Dawning.Generator.Domain.Repositories;
using Dawning.Generator.Infrastructure.Data;
using Dawning.Generator.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Dawning.Generator.Infrastructure;

/// <summary>
/// 基础设施层依赖注入
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, MySqlConnectionFactory>();
        services.AddScoped<IProjectHistoryRepository, ProjectHistoryRepository>();
        services.AddScoped<ITemplateFavoriteRepository, TemplateFavoriteRepository>();

        return services;
    }
}
