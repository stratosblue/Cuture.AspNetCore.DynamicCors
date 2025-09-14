#pragma warning disable IDE0130

using System.ComponentModel;
using Cuture.AspNetCore.DynamicCors;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 动态Cors DI拓展
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class DynamicCorsDIExtensions
{
    #region Public 方法

    /// <summary>
    /// 添加动态Cors服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="setupAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddDynamicCors(this IServiceCollection services,
                                                    Action<DynamicCorsBuilder>? setupAction = null)
    {
        services.AddCors();

        services.TryAddSingleton<DynamicCorsPoliciesContainer>();
        services.TryAddSingleton<IDomainNameCollectionContainer, DefaultDomainNameCollectionContainer>();
        services.TryAddSingleton<IOriginCorsValidatorContainer, DefaultOriginCorsValidatorContainer>();

        services.Replace(ServiceDescriptor.Transient<ICorsPolicyProvider, DynamicCorsPolicyProvider>());
        services.TryAdd(ServiceDescriptor.Transient<DefaultCorsPolicyProvider, DefaultCorsPolicyProvider>());

        if (setupAction is not null)
        {
            var builder = new DynamicCorsBuilder(services);
            setupAction(builder);

            services.Configure(builder.OptionsSetupAction);
        }

        return services;
    }

    #endregion Public 方法
}
