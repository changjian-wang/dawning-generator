using Dawning.Generator.Application.Dtos;

namespace Dawning.Generator.Application.Services;

/// <summary>
/// 模板服务接口
/// </summary>
public interface ITemplateService
{
    /// <summary>
    /// 获取可选配置项
    /// </summary>
    TemplateOptionsDto GetOptions();
}
