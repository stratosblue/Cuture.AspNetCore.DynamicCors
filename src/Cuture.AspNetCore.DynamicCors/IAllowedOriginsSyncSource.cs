using Microsoft.Extensions.Primitives;

namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 允许来源同步源
/// </summary>
public interface IAllowedOriginsSyncSource : IDisposable
{
    #region Public 属性

    /// <summary>
    /// 生效的策略名称(值为 <see langword="null"/> 时则为默认策略)
    /// </summary>
    public string? PolicyName { get; }

    #endregion Public 属性

    #region Public 方法

    /// <summary>
    /// 获取来源
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<string>> GetOriginsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// 获取 <see cref="IChangeToken"/>
    /// </summary>
    /// <returns></returns>
    public IChangeToken GetReloadToken();

    #endregion Public 方法
}
