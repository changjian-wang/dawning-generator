using Dawning.Generator.Application.Dtos;
using Dawning.Generator.Domain.Enums;
using Dawning.Generator.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Dawning.Generator.Application.Services;

/// <summary>
/// 项目历史服务实现
/// </summary>
public class ProjectHistoryService : IProjectHistoryService
{
    private readonly IProjectHistoryRepository _repository;
    private readonly IProjectGeneratorService _generatorService;
    private readonly ILogger<ProjectHistoryService> _logger;

    public ProjectHistoryService(
        IProjectHistoryRepository repository,
        IProjectGeneratorService generatorService,
        ILogger<ProjectHistoryService> logger)
    {
        _repository = repository;
        _generatorService = generatorService;
        _logger = logger;
    }

    public async Task<IEnumerable<ProjectHistoryDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var histories = await _repository.GetByUserIdAsync(userId, cancellationToken);
        
        return histories.Select(h => new ProjectHistoryDto
        {
            Id = h.Id,
            ProjectName = h.ProjectName,
            NamespacePrefix = h.NamespacePrefix,
            ArchitectureType = h.ArchitectureType.ToString().ToLower(),
            DotNetVersion = h.DotNetVersion.ToFrameworkMoniker(),
            DatabaseType = h.DatabaseType.ToString().ToLower(),
            SelectedModules = h.SelectedModules,
            OptionalFeatures = h.OptionalFeatures,
            ServicePort = h.ServicePort,
            DownloadCount = h.DownloadCount,
            CreatedAt = h.CreatedAt
        });
    }

    public async Task<ProjectHistoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var history = await _repository.GetByIdAsync(id, cancellationToken);
        
        if (history == null) return null;

        return new ProjectHistoryDto
        {
            Id = history.Id,
            ProjectName = history.ProjectName,
            NamespacePrefix = history.NamespacePrefix,
            ArchitectureType = history.ArchitectureType.ToString().ToLower(),
            DotNetVersion = history.DotNetVersion.ToFrameworkMoniker(),
            DatabaseType = history.DatabaseType.ToString().ToLower(),
            SelectedModules = history.SelectedModules,
            OptionalFeatures = history.OptionalFeatures,
            ServicePort = history.ServicePort,
            DownloadCount = history.DownloadCount,
            CreatedAt = history.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }

    public async Task<GenerateProjectResponse> RegenerateAsync(Guid historyId, Guid userId, CancellationToken cancellationToken = default)
    {
        var history = await _repository.GetByIdAsync(historyId, cancellationToken);
        
        if (history == null)
        {
            return new GenerateProjectResponse
            {
                Success = false,
                ErrorMessage = "History record not found"
            };
        }

        // 从历史记录重建请求
        var request = new GenerateProjectRequest
        {
            ProjectName = history.ProjectName,
            NamespacePrefix = history.NamespacePrefix,
            ArchitectureType = history.ArchitectureType,
            DotNetVersion = history.DotNetVersion,
            Database = new DatabaseConfig { Type = history.DatabaseType },
            Modules = history.SelectedModules,
            Features = new FeatureOptions
            {
                IncludeTests = history.OptionalFeatures.Contains("tests"),
                IncludeDocker = history.OptionalFeatures.Contains("docker"),
                IncludeHelmChart = history.OptionalFeatures.Contains("helm"),
                IncludeGitHubActions = history.OptionalFeatures.Contains("github-actions"),
                IncludeSwagger = history.OptionalFeatures.Contains("swagger"),
                IncludeHealthChecks = history.OptionalFeatures.Contains("healthchecks")
            },
            ServicePort = history.ServicePort,
            SaveToHistory = false // 重新生成不再保存新记录
        };

        // 增加下载次数
        await _repository.IncrementDownloadCountAsync(historyId, cancellationToken);

        return await _generatorService.GenerateAsync(request, userId, cancellationToken);
    }
}
