#pragma warning disable IDE0130

using System.ComponentModel;
using Cuture.AspNetCore.DynamicCors.Internal.OriginsSync;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 动态Cors构造拓展
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class DynamicCorsBuilderExtensions
{
    #region Public 方法

    #region Sync

    /// <summary>
    /// 从 <see cref="IConfiguration"/> 中的指定节点 <paramref name="key"/> 同步允许的来源
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="key"></param>
    public static void SyncAllowedOriginsFromConfiguration(this DynamicCorsPolicyBuilder builder, string key)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        AddAllowedOriginsSyncSource(builder, serviveProvider => new ConfigurationAllowedOriginsSyncSource(builder.PolicyName, serviveProvider.GetRequiredService<IConfiguration>().GetSection(key)));
    }

    /// <summary>
    /// 从选项 <typeparamref name="TOptions"/> 中的指定字段同步允许的来源
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="builder"></param>
    /// <param name="propertySelectAction">字段选择器</param>
    public static void SyncAllowedOriginsWithOptions<TOptions>(this DynamicCorsPolicyBuilder builder, Func<TOptions, string?> propertySelectAction)
        where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(propertySelectAction);

        AddAllowedOriginsSyncSource(builder, serviveProvider => new OptionsMonitorAllowedOriginsSyncSource<TOptions>(builder.PolicyName, serviveProvider.GetRequiredService<IOptionsMonitor<TOptions>>(), propertySelectAction));
    }

    /// <summary>
    /// 从选项 <typeparamref name="TOptions"/> 中的指定字段同步允许的来源
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="builder"></param>
    /// <param name="propertySelectAction">字段选择器</param>
    public static void SyncAllowedOriginsWithOptions<TOptions>(this DynamicCorsPolicyBuilder builder, Func<TOptions, IEnumerable<string?>?> propertySelectAction)
        where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(propertySelectAction);

        AddAllowedOriginsSyncSource(builder, serviveProvider => new OptionsMonitorAllowedOriginsSyncSource<TOptions>(builder.PolicyName, serviveProvider.GetRequiredService<IOptionsMonitor<TOptions>>(), propertySelectAction));
    }

    #endregion Sync

    #endregion Public 方法

    #region Private 方法

    private static void AddAllowedOriginsSyncSource(DynamicCorsPolicyBuilder builder, Func<IServiceProvider, IAllowedOriginsSyncSource> syncSourceFactory)
    {
        builder.Services.TryAddSingleton<AllowedOriginsSynchronizer>();

        builder.Services.AddSingleton<IAllowedOriginsSyncSource>(syncSourceFactory);

        if (!builder.Services.Any(static m => m.ServiceType == typeof(IPostConfigureOptions<DynamicCorsOptions>) && m.ImplementationType == typeof(AllowedOriginsSynchronizerInitPostConfigureOptions)))
        {
            var descriptor = ServiceDescriptor.Singleton<IPostConfigureOptions<DynamicCorsOptions>, AllowedOriginsSynchronizerInitPostConfigureOptions>();
            builder.Services.Add(descriptor);
        }
    }

    #endregion Private 方法

    #region Private 类

    /// <summary>
    /// 触发 <see cref="AllowedOriginsSynchronizer"/> 初始化的 PostConfigureOptions
    /// </summary>
    private sealed class AllowedOriginsSynchronizerInitPostConfigureOptions(IServiceProvider dependency, ILogger<AllowedOriginsSynchronizer> dependency2)
       : PostConfigureOptions<DynamicCorsOptions, IServiceProvider, ILogger<AllowedOriginsSynchronizer>>(Options.DefaultName, dependency, dependency2, StartRequireAllowedOriginsSynchronizerAsync)
    {
        #region Private 方法

        private static void StartRequireAllowedOriginsSynchronizerAsync(DynamicCorsOptions options, IServiceProvider serviceProvider, ILogger<AllowedOriginsSynchronizer> logger)
        {
            //异步触发不能进行构造函数依赖，构造依赖会造成循环依赖
            Task.Run(async () =>
            {
                await Task.Yield();
                try
                {
                    serviceProvider.GetRequiredService<AllowedOriginsSynchronizer>();

                    logger.LogDebug($"{nameof(AllowedOriginsSynchronizer)} create success.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"{nameof(AllowedOriginsSynchronizer)} create failed. Failed to load the dynamic cors origins.");
                }
            });
        }

        #endregion Private 方法
    }

    #endregion Private 类
}
