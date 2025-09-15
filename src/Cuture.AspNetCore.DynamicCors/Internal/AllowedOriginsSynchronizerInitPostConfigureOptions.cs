using Cuture.AspNetCore.DynamicCors.Internal.OriginsSync;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cuture.AspNetCore.DynamicCors.Internal;

/// <summary>
/// 触发 <see cref="AllowedOriginsSynchronizer"/> 初始化的 PostConfigureOptions
/// </summary>
internal sealed class AllowedOriginsSynchronizerInitPostConfigureOptions(IServiceProvider dependency, ILogger<AllowedOriginsSynchronizer> dependency2)
   : PostConfigureOptions<CorsOptions, IServiceProvider, ILogger<AllowedOriginsSynchronizer>>(Options.DefaultName, dependency, dependency2, StartRequireAllowedOriginsSynchronizerAsync)
{
    #region Private 方法

    private static void StartRequireAllowedOriginsSynchronizerAsync(CorsOptions options, IServiceProvider serviceProvider, ILogger<AllowedOriginsSynchronizer> logger)
    {
        //异步触发不能进行构造函数依赖，构造依赖会造成循环依赖
        //调整为依赖 CorsOptions 后暂时不会构成循环依赖了， 但此处暂时保持逻辑
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
