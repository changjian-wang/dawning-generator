using System.IO.Compression;
using Dawning.Generator.Application.Dtos;
using Dawning.Generator.Application.Templates;
using Dawning.Generator.Domain.Entities;
using Dawning.Generator.Domain.Enums;
using Dawning.Generator.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Dawning.Generator.Application.Services;

/// <summary>
/// 项目生成器服务实现
/// </summary>
public class ProjectGeneratorService : IProjectGeneratorService
{
    private readonly TemplateEngine _templateEngine;
    private readonly IProjectHistoryRepository _historyRepository;
    private readonly ILogger<ProjectGeneratorService> _logger;

    public ProjectGeneratorService(
        TemplateEngine templateEngine,
        IProjectHistoryRepository historyRepository,
        ILogger<ProjectGeneratorService> logger
    )
    {
        _templateEngine = templateEngine;
        _historyRepository = historyRepository;
        _logger = logger;
    }

    public async Task<GenerateProjectResponse> GenerateAsync(
        GenerateProjectRequest request,
        Guid? userId = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            _logger.LogInformation("开始生成项目: {ProjectName}", request.ProjectName);

            // 构建模板上下文
            var context = BuildTemplateContext(request);

            // 确定模板文件夹
            var templateFolder = request.ArchitectureType switch
            {
                ArchitectureType.Layered => "layered",
                ArchitectureType.Clean => "clean",
                ArchitectureType.Simple => "simple",
                _ => "layered"
            };

            // 获取所有模板文件
            var templateFiles = _templateEngine.GetTemplateFiles(templateFolder).ToList();

            if (templateFiles.Count == 0)
            {
                _logger.LogWarning("模板文件夹为空: {Folder}", templateFolder);
                return new GenerateProjectResponse
                {
                    Success = false,
                    ErrorMessage = $"No templates found in folder: {templateFolder}",
                };
            }

            // 生成 ZIP
            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var templateFile in templateFiles)
                {
                    var renderedContent = await _templateEngine.RenderAsync(templateFile, context);

                    // 处理输出路径 (移除 .scriban 后缀，替换变量)
                    var outputPath = ProcessOutputPath(templateFile, context, templateFolder);

                    var entry = archive.CreateEntry(outputPath);
                    await using var entryStream = entry.Open();
                    await using var writer = new StreamWriter(entryStream);
                    await writer.WriteAsync(renderedContent);
                }

                // 添加共享文件
                await AddSharedFilesAsync(archive, request, context);
            }

            memoryStream.Position = 0;
            var zipContent = memoryStream.ToArray();

            // 保存历史记录
            Guid? historyId = null;
            if (request.SaveToHistory && userId.HasValue)
            {
                historyId = await SaveHistoryAsync(request, userId.Value, cancellationToken);
            }

            _logger.LogInformation(
                "项目生成完成: {ProjectName}, 大小: {Size} bytes",
                request.ProjectName,
                zipContent.Length
            );

            return new GenerateProjectResponse
            {
                Success = true,
                ProjectName = request.ProjectName,
                FileName = $"{request.ProjectName}.zip",
                Content = zipContent,
                HistoryId = historyId,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "生成项目失败: {ProjectName}", request.ProjectName);
            return new GenerateProjectResponse { Success = false, ErrorMessage = ex.Message };
        }
    }

    private object BuildTemplateContext(GenerateProjectRequest request)
    {
        var fullNamespace = string.IsNullOrEmpty(request.NamespacePrefix)
            ? request.ProjectName
            : $"{request.NamespacePrefix}.{request.ProjectName}";

        return new
        {
            // 用于 Scriban 模板的变量 (snake_case 风格)
            project_name = request.ProjectName,
            @namespace = fullNamespace,
            namespace_prefix = request.NamespacePrefix ?? "",
            dotnet_version = request.DotNetVersion.ToFrameworkMoniker(),
            database_type = request.Database.Type.ToString(),
            connection_string = request.Database.ConnectionStringTemplate.Replace(
                "{database}",
                $"{request.ProjectName.ToLower()}_db"
            ),
            service_port = request.ServicePort,

            // 模块标志
            use_identity = request.Modules.Contains("identity"),
            use_core = request.Modules.Contains("core"),
            use_logging = request.Modules.Contains("logging"),
            use_dapper = request.Modules.Contains("dapper"),
            use_caching = request.Modules.Contains("caching"),
            use_messaging = request.Modules.Contains("messaging"),
            use_resilience = request.Modules.Contains("resilience"),

            // 功能标志
            include_tests = request.Features.IncludeTests,
            include_docker = request.Features.IncludeDocker,
            include_helm_chart = request.Features.IncludeHelmChart,
            include_github_actions = request.Features.IncludeGitHubActions,
            include_swagger = request.Features.IncludeSwagger,
            include_healthchecks = request.Features.IncludeHealthChecks,

            // 辅助变量
            year = DateTime.UtcNow.Year,
            generated_at = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
        };
    }

    private string ProcessOutputPath(string templatePath, object context, string templateFolder)
    {
        // 移除模板文件夹前缀
        var path = templatePath;
        if (path.StartsWith(templateFolder + "/") || path.StartsWith(templateFolder + "\\"))
        {
            path = path[(templateFolder.Length + 1)..];
        }

        // 移除 .scriban 后缀
        if (path.EndsWith(".scriban"))
        {
            path = path[..^8];
        }

        // 获取上下文中的变量值
        var contextDict = context
            .GetType()
            .GetProperties()
            .ToDictionary(p => p.Name, p => p.GetValue(context)?.ToString() ?? "");

        // 替换 {{project_name}} 等变量 (snake_case 风格)
        foreach (var (key, value) in contextDict)
        {
            path = path.Replace($"{{{{{key}}}}}", value);
        }

        // 标准化路径分隔符
        path = path.Replace("\\", "/");

        return path;
    }

    private async Task AddSharedFilesAsync(
        ZipArchive archive,
        GenerateProjectRequest request,
        object context
    )
    {
        // 添加 .gitignore
        var gitignoreFiles = _templateEngine
            .GetTemplateFiles("shared")
            .Where(f => f.Contains("gitignore"));
        foreach (var file in gitignoreFiles)
        {
            var content = await _templateEngine.RenderAsync(file, context);
            var entry = archive.CreateEntry(".gitignore");
            await using var entryStream = entry.Open();
            await using var writer = new StreamWriter(entryStream);
            await writer.WriteAsync(content);
        }

        // 添加 Dockerfile (如果需要)
        if (request.Features.IncludeDocker)
        {
            var dockerFiles = _templateEngine
                .GetTemplateFiles("shared")
                .Where(f => f.Contains("docker"));
            foreach (var file in dockerFiles)
            {
                var content = await _templateEngine.RenderAsync(file, context);
                var entry = archive.CreateEntry("Dockerfile");
                await using var entryStream = entry.Open();
                await using var writer = new StreamWriter(entryStream);
                await writer.WriteAsync(content);
            }
        }

        // 添加 GitHub Actions (如果需要)
        if (request.Features.IncludeGitHubActions)
        {
            var actionFiles = _templateEngine
                .GetTemplateFiles("shared")
                .Where(f => f.Contains("github-actions"));
            foreach (var file in actionFiles)
            {
                var content = await _templateEngine.RenderAsync(file, context);
                var entry = archive.CreateEntry(".github/workflows/ci.yml");
                await using var entryStream = entry.Open();
                await using var writer = new StreamWriter(entryStream);
                await writer.WriteAsync(content);
            }
        }
    }

    private async Task<Guid> SaveHistoryAsync(
        GenerateProjectRequest request,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var history = new ProjectHistory
        {
            UserId = userId,
            ProjectName = request.ProjectName,
            NamespacePrefix = request.NamespacePrefix,
            ArchitectureType = request.ArchitectureType,
            DotNetVersion = request.DotNetVersion,
            DatabaseType = request.Database.Type,
            SelectedModules = request.Modules,
            OptionalFeatures = GetFeatureList(request.Features),
            ServicePort = request.ServicePort,
        };

        return await _historyRepository.CreateAsync(history, cancellationToken);
    }

    private static List<string> GetFeatureList(FeatureOptions features)
    {
        var list = new List<string>();
        if (features.IncludeTests)
            list.Add("tests");
        if (features.IncludeDocker)
            list.Add("docker");
        if (features.IncludeHelmChart)
            list.Add("helm");
        if (features.IncludeGitHubActions)
            list.Add("github-actions");
        if (features.IncludeSwagger)
            list.Add("swagger");
        if (features.IncludeHealthChecks)
            list.Add("healthchecks");
        return list;
    }
}
