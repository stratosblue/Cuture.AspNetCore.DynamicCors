#pragma warning disable IDE0130

using System.ComponentModel;
using Cuture.AspNetCore.DynamicCors.Internal.OriginsSync;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 动态Cors <see cref="IHost"/>拓展
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class DynamicCorsHostExtensions
{
    #region Public 方法

    /// <summary>
    /// 等待动态Cors服务初始化完成
    /// </summary>
    /// <param name="host"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static void WaitDynamicCorsInit(this IHost host, CancellationToken cancellationToken = default)
    {
        host.Services
            .GetRequiredService<AllowedOriginsSynchronizer>()
            .InitTask
            .Wait(cancellationToken);
    }

    /// <summary>
    /// 等待动态Cors服务初始化完成
    /// </summary>
    /// <param name="host"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task WaitDynamicCorsInitAsync(this IHost host, CancellationToken cancellationToken = default)
    {
        await host.Services
                  .GetRequiredService<AllowedOriginsSynchronizer>()
                  .InitTask
                  .WaitAsync(cancellationToken);
    }

    #endregion Public 方法
}
