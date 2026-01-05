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
            _logger.LogInformation("开始生成项目: {ProjectName}, 类型: {ProjectType}", request.ProjectName, request.ProjectType);

            // 构建模板上下文
            var context = BuildTemplateContext(request);

            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                // 根据项目类型生成不同的内容
                switch (request.ProjectType)
                {
                    case ProjectType.Backend:
                        await GenerateBackendAsync(archive, request, context);
                        break;
                    case ProjectType.Frontend:
                        await GenerateFrontendAsync(archive, request, context);
                        break;
                    case ProjectType.Fullstack:
                        await GenerateFullstackAsync(archive, request, context);
                        break;
                }
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

    private async Task GenerateBackendAsync(ZipArchive archive, GenerateProjectRequest request, object context)
    {
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
            throw new InvalidOperationException($"No templates found in folder: {templateFolder}");
        }

        foreach (var templateFile in templateFiles)
        {
            var renderedContent = await _templateEngine.RenderAsync(templateFile, context);
            var outputPath = ProcessOutputPath(templateFile, context, templateFolder);

            var entry = archive.CreateEntry(outputPath);
            await using var entryStream = entry.Open();
            await using var writer = new StreamWriter(entryStream);
            await writer.WriteAsync(renderedContent);
        }

        // 添加共享文件
        await AddSharedFilesAsync(archive, request, context);
    }

    private async Task GenerateFrontendAsync(ZipArchive archive, GenerateProjectRequest request, object context)
    {
        // 确定前端模板文件夹
        var templateFolder = request.FrontendFramework switch
        {
            FrontendFramework.VueArco => "frontend/vue-arco",
            FrontendFramework.VueElement => "frontend/vue-element",
            FrontendFramework.ReactAntd => "frontend/react-antd",
            _ => "frontend/vue-arco"
        };

        var templateFiles = _templateEngine.GetTemplateFiles(templateFolder).ToList();

        if (templateFiles.Count == 0)
        {
            _logger.LogWarning("前端模板文件夹为空: {Folder}", templateFolder);
            throw new InvalidOperationException($"No templates found in folder: {templateFolder}");
        }

        foreach (var templateFile in templateFiles)
        {
            var renderedContent = await _templateEngine.RenderAsync(templateFile, context);
            var outputPath = ProcessOutputPath(templateFile, context, templateFolder);

            var entry = archive.CreateEntry(outputPath);
            await using var entryStream = entry.Open();
            await using var writer = new StreamWriter(entryStream);
            await writer.WriteAsync(renderedContent);
        }
    }

    private async Task GenerateFullstackAsync(ZipArchive archive, GenerateProjectRequest request, object context)
    {
        // 生成后端到 apps/api 目录
        var backendFolder = request.ArchitectureType switch
        {
            ArchitectureType.Layered => "layered",
            ArchitectureType.Clean => "clean",
            ArchitectureType.Simple => "simple",
            _ => "layered"
        };

        var backendFiles = _templateEngine.GetTemplateFiles(backendFolder).ToList();
        foreach (var templateFile in backendFiles)
        {
            var renderedContent = await _templateEngine.RenderAsync(templateFile, context);
            var outputPath = "apps/api/" + ProcessOutputPath(templateFile, context, backendFolder);

            var entry = archive.CreateEntry(outputPath);
            await using var entryStream = entry.Open();
            await using var writer = new StreamWriter(entryStream);
            await writer.WriteAsync(renderedContent);
        }

        // 生成前端到 apps/web 目录
        var frontendFolder = request.FrontendFramework switch
        {
            FrontendFramework.VueArco => "frontend/vue-arco",
            FrontendFramework.VueElement => "frontend/vue-element",
            FrontendFramework.ReactAntd => "frontend/react-antd",
            _ => "frontend/vue-arco"
        };

        var frontendFiles = _templateEngine.GetTemplateFiles(frontendFolder).ToList();
        foreach (var templateFile in frontendFiles)
        {
            var renderedContent = await _templateEngine.RenderAsync(templateFile, context);
            var outputPath = "apps/web/" + ProcessOutputPath(templateFile, context, frontendFolder);

            var entry = archive.CreateEntry(outputPath);
            await using var entryStream = entry.Open();
            await using var writer = new StreamWriter(entryStream);
            await writer.WriteAsync(renderedContent);
        }

        // 添加全栈共享文件 (docker-compose, CI/CD, README 等)
        var fullstackFiles = _templateEngine.GetTemplateFiles("fullstack").ToList();
        foreach (var templateFile in fullstackFiles)
        {
            var renderedContent = await _templateEngine.RenderAsync(templateFile, context);
            var outputPath = ProcessOutputPath(templateFile, context, "fullstack");

            var entry = archive.CreateEntry(outputPath);
            await using var entryStream = entry.Open();
            await using var writer = new StreamWriter(entryStream);
            await writer.WriteAsync(renderedContent);
        }
    }

    private object BuildTemplateContext(GenerateProjectRequest request)
    {
        // 将项目名称转换为 PascalCase (如 dawning.store -> Dawning.Store)
        var pascalProjectName = ToPascalCase(request.ProjectName);
        
        var fullNamespace = string.IsNullOrEmpty(request.NamespacePrefix)
            ? pascalProjectName
            : $"{request.NamespacePrefix}.{pascalProjectName}";

        var appTitle = string.IsNullOrEmpty(request.Frontend.AppTitle)
            ? pascalProjectName
            : request.Frontend.AppTitle;

        return new
        {
            // 用于 Scriban 模板的变量
            project_name = pascalProjectName,  // 后端使用 PascalCase
            project_name_lower = request.ProjectName.ToLower(),  // 前端使用小写
            @namespace = fullNamespace,
            namespace_prefix = request.NamespacePrefix ?? "",
            dotnet_version = request.DotNetVersion.ToFrameworkMoniker(),
            database_type = request.Database.Type.ToString().ToLower(),
            connection_string = request.Database.ConnectionStringTemplate.Replace(
                "{database}",
                $"{request.ProjectName.ToLower()}_db"
            ),
            service_port = request.ServicePort,
            frontend_port = request.FrontendPort,

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
            use_redis = request.Features.UseRedis,
            use_signalr = request.Features.UseSignalR,
            use_openiddict = request.Features.UseOpenIddict,

            // 前端配置
            use_echarts = request.Frontend.UseECharts,
            api_url = request.Frontend.ApiBaseUrl,
            app_title = appTitle,

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

    /// <summary>
    /// 将字符串转换为 PascalCase (如 dawning.store -> Dawning.Store)
    /// </summary>
    private static string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // 按点号分割，每个部分首字母大写
        var parts = input.Split('.');
        for (var i = 0; i < parts.Length; i++)
        {
            if (!string.IsNullOrEmpty(parts[i]))
            {
                parts[i] = char.ToUpper(parts[i][0]) + parts[i][1..].ToLower();
            }
        }
        return string.Join(".", parts);
    }
}
