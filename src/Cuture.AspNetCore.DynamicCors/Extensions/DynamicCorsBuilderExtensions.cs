﻿#pragma warning disable IDE0130

using System.ComponentModel;
using Cuture.AspNetCore.DynamicCors.Internal;
using Cuture.AspNetCore.DynamicCors.Internal.OriginsSync;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
    /// 添加允许来源同步源
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="syncSourceFactory"></param>
    /// <returns></returns>
    public static DynamicCorsPolicyBuilder AddAllowedOriginsSyncSource(DynamicCorsPolicyBuilder builder, Func<IServiceProvider, IAllowedOriginsSyncSource> syncSourceFactory)
    {
        builder.Services.TryAddSingleton<AllowedOriginsSynchronizer>();

        builder.Services.AddSingleton<IAllowedOriginsSyncSource>(syncSourceFactory);

        if (!builder.Services.Any(static m => m.ServiceType == typeof(IPostConfigureOptions<CorsOptions>) && m.ImplementationType == typeof(AllowedOriginsSynchronizerInitPostConfigureOptions)))
        {
            var descriptor = ServiceDescriptor.Singleton<IPostConfigureOptions<CorsOptions>, AllowedOriginsSynchronizerInitPostConfigureOptions>();
            builder.Services.Add(descriptor);
        }

        return builder;
    }

    /// <summary>
    /// 从 <see cref="IConfiguration"/> 中的指定节点 <paramref name="key"/> 同步允许的来源
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="key"></param>
    public static DynamicCorsPolicyBuilder SyncAllowedOriginsFromConfiguration(this DynamicCorsPolicyBuilder builder, string key)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        return AddAllowedOriginsSyncSource(builder, serviveProvider => new ConfigurationAllowedOriginsSyncSource(builder.PolicyName, serviveProvider.GetRequiredService<IConfiguration>().GetSection(key)));
    }

    /// <summary>
    /// 从选项 <typeparamref name="TOptions"/> 中的指定字段同步允许的来源
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="builder"></param>
    /// <param name="propertySelectAction">字段选择器</param>
    public static DynamicCorsPolicyBuilder SyncAllowedOriginsWithOptions<TOptions>(this DynamicCorsPolicyBuilder builder, Func<TOptions, string?> propertySelectAction)
        where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(propertySelectAction);

        return AddAllowedOriginsSyncSource(builder, serviveProvider => new OptionsMonitorAllowedOriginsSyncSource<TOptions>(builder.PolicyName, serviveProvider.GetRequiredService<IOptionsMonitor<TOptions>>(), propertySelectAction));
    }

    /// <summary>
    /// 从选项 <typeparamref name="TOptions"/> 中的指定字段同步允许的来源
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="builder"></param>
    /// <param name="propertySelectAction">字段选择器</param>
    public static DynamicCorsPolicyBuilder SyncAllowedOriginsWithOptions<TOptions>(this DynamicCorsPolicyBuilder builder, Func<TOptions, IEnumerable<string?>?> propertySelectAction)
        where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(propertySelectAction);

        return AddAllowedOriginsSyncSource(builder, serviveProvider => new OptionsMonitorAllowedOriginsSyncSource<TOptions>(builder.PolicyName, serviveProvider.GetRequiredService<IOptionsMonitor<TOptions>>(), propertySelectAction));
    }

    #endregion Sync

    #endregion Public 方法
}
