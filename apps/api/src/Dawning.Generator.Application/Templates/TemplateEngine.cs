using Scriban;
using Scriban.Runtime;

namespace Dawning.Generator.Application.Templates;

/// <summary>
/// Scriban 模板引擎封装
/// </summary>
public class TemplateEngine
{
    private readonly string _templatesBasePath;

    public TemplateEngine(string templatesBasePath)
    {
        _templatesBasePath = templatesBasePath;
        Console.WriteLine($"[TemplateEngine] Initialized with base path: {_templatesBasePath}");
        Console.WriteLine($"[TemplateEngine] Directory exists: {Directory.Exists(_templatesBasePath)}");
    }

    /// <summary>
    /// 渲染模板
    /// </summary>
    public async Task<string> RenderAsync(string templatePath, object model)
    {
        var fullPath = Path.Combine(_templatesBasePath, templatePath);
        
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"Template not found: {templatePath}", fullPath);
        }

        var templateContent = await File.ReadAllTextAsync(fullPath);
        var template = Template.Parse(templateContent);
        
        if (template.HasErrors)
        {
            var errors = string.Join(Environment.NewLine, template.Messages.Select(m => m.Message));
            throw new InvalidOperationException($"Template parsing error in {templatePath}: {errors}");
        }

        var scriptObject = new ScriptObject();
        scriptObject.Import(model);
        
        var context = new TemplateContext();
        context.PushGlobal(scriptObject);
        
        return await template.RenderAsync(context);
    }

    /// <summary>
    /// 渲染模板字符串
    /// </summary>
    public async Task<string> RenderStringAsync(string templateContent, object model)
    {
        var template = Template.Parse(templateContent);
        
        if (template.HasErrors)
        {
            var errors = string.Join(Environment.NewLine, template.Messages.Select(m => m.Message));
            throw new InvalidOperationException($"Template parsing error: {errors}");
        }

        var scriptObject = new ScriptObject();
        scriptObject.Import(model);
        
        var context = new TemplateContext();
        context.PushGlobal(scriptObject);
        
        return await template.RenderAsync(context);
    }

    /// <summary>
    /// 获取所有模板文件
    /// </summary>
    public IEnumerable<string> GetTemplateFiles(string subFolder)
    {
        var folderPath = Path.Combine(_templatesBasePath, subFolder);
        Console.WriteLine($"[TemplateEngine] GetTemplateFiles - Looking in: {folderPath}");
        Console.WriteLine($"[TemplateEngine] Folder exists: {Directory.Exists(folderPath)}");
        
        if (!Directory.Exists(folderPath))
        {
            return [];
        }

        var files = Directory.GetFiles(folderPath, "*.scriban", SearchOption.AllDirectories)
            .Select(f => Path.GetRelativePath(_templatesBasePath, f))
            .ToList();
        
        Console.WriteLine($"[TemplateEngine] Found {files.Count} template files");
        return files;
    }
}
