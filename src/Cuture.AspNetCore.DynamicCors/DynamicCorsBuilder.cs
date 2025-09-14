#pragma warning disable IDE0130

using System.ComponentModel;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 动态Cors构造器
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public record class DynamicCorsBuilder(IServiceCollection Services)
{
    //用于简化构建操作

    /// <summary>
    /// 选项设置委托(构造器记录所有操作，然后延时操作)
    /// </summary>
    internal Action<DynamicCorsOptions> OptionsSetupAction { get; private set; } = static _ => { };

    #region ConfigureOptions

    /// <inheritdoc cref="DynamicCorsOptions.AddDefaultPolicy(CorsPolicy)"/>
    public DynamicCorsPolicyBuilder AddDefaultPolicy(CorsPolicy policy)
    {
        OptionsSetupAction += options => options.AddDefaultPolicy(policy);

        return new(Services, null);
    }

    /// <inheritdoc cref="DynamicCorsOptions.AddDefaultPolicy(Action{CorsPolicyBuilder})"/>
    public DynamicCorsPolicyBuilder AddDefaultPolicy(Action<CorsPolicyBuilder> configurePolicy)
    {
        OptionsSetupAction += options => options.AddDefaultPolicy(configurePolicy);

        return new(Services, null);
    }

    /// <inheritdoc cref="DynamicCorsOptions.AddPolicy(string, CorsPolicy)"/>
    public DynamicCorsPolicyBuilder AddPolicy(string name, CorsPolicy policy)
    {
        OptionsSetupAction += options => options.AddPolicy(name, policy);

        return new(Services, name);
    }

    /// <inheritdoc cref="DynamicCorsOptions.AddPolicy(string, Action{CorsPolicyBuilder})"/>
    public DynamicCorsPolicyBuilder AddPolicy(string name, Action<CorsPolicyBuilder> configurePolicy)
    {
        OptionsSetupAction += options => options.AddPolicy(name, configurePolicy);

        return new(Services, name);
    }

    #endregion ConfigureOptions
}
