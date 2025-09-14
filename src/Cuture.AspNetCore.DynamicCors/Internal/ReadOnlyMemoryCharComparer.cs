#pragma warning disable IDE0130

namespace Cuture.AspNetCore.DynamicCors.Internal;

internal sealed class ReadOnlyMemoryCharComparer : IEqualityComparer<ReadOnlyMemory<char>>
{
    #region Public 属性

    public static ReadOnlyMemoryCharComparer Shared { get; } = new();

    #endregion Public 属性

    #region Public 方法

    public bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y) => x.Span.SequenceEqual(y.Span);

    public int GetHashCode(ReadOnlyMemory<char> obj) => string.GetHashCode(obj.Span);

    #endregion Public 方法
}
