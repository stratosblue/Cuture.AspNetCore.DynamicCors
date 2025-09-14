#pragma warning disable IDE0130

namespace Cuture.AspNetCore.DynamicCors.Internal;

internal sealed class NoopDisposable : IDisposable
{
    #region Public 方法

    public void Dispose()
    { }

    #endregion Public 方法
}
