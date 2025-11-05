using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// Cors允许来源同步源
/// </summary>
public abstract class AllowedOriginsSyncSource(PolicyName policyName)
    : IAllowedOriginsSyncSource
{
    #region Private 字段

    private ConfigurationReloadToken _changeToken = new();

    private bool _isDisposed;

    #endregion Private 字段

    #region Public 属性

    /// <inheritdoc/>
    public virtual string? PolicyName { get; } = policyName;

    #endregion Public 属性

    #region Public 方法

    /// <inheritdoc/>
    public virtual Task<IEnumerable<string>> GetOriginsAsync(CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        return InnerGetOriginsAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual IChangeToken GetReloadToken()
    {
        ThrowIfDisposed();
        return _changeToken;
    }

    #endregion Public 方法

    #region Protected 方法

    /// <inheritdoc cref="GetOriginsAsync(CancellationToken)"/>
    protected abstract Task<IEnumerable<string>> InnerGetOriginsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// 通知数据源已变更
    /// </summary>
    protected virtual void NotifySourceChanged()
    {
        ThrowIfDisposed();
        var previousToken = Interlocked.Exchange(ref _changeToken, new());
        previousToken.OnReload();
    }

    /// <summary>
    /// 如果已处置则抛出异常
    /// </summary>
    protected void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
    }

    #endregion Protected 方法

    #region Dispose

    /// <summary>
    ///
    /// </summary>
    ~AllowedOriginsSyncSource()
    {
        Dispose(disposing: false);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        _isDisposed = true;
        _changeToken = null!;
    }

    #endregion Dispose
}
