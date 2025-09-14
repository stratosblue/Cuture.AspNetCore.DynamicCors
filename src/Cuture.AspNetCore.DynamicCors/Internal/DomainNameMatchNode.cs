using System.Collections.Concurrent;
using System.Text;

namespace Cuture.AspNetCore.DynamicCors.Internal;

/// <summary>
/// 域名匹配节点
/// </summary>
/// <param name="parent"></param>
/// <param name="value"></param>
internal sealed class DomainNameMatchNode(DomainNameMatchNode? parent, ReadOnlyMemory<char> value)
{
    #region Public 属性

    /// <summary>
    /// 子级列表
    /// </summary>
    public ConcurrentDictionary<ReadOnlyMemory<char>, DomainNameMatchNode> Children { get; } = new(ReadOnlyMemoryCharComparer.Shared);

    /// <summary>
    /// 当前节点是否为终结点
    /// </summary>
    public bool IsEndpoint { get; set; }

    /// <summary>
    /// 父级
    /// </summary>
    public DomainNameMatchNode? Parent { get; } = parent;

    /// <summary>
    /// 当前节点的值
    /// </summary>
    public ReadOnlyMemory<char> Value { get; } = value;

    #endregion Public 属性

    #region Public 方法

    /// <inheritdoc/>
    public override string ToString()
    {
        var builder = new StringBuilder();

        if (IsEndpoint)
        {
            builder.Append("*.");
        }

        var node = this;
        while (node is not null)
        {
            builder.Append(node.Value);
            builder.Append('.');
            node = node.Parent;
        }

        builder.Remove(builder.Length - 1, 1);

        if (!Children.IsEmpty)
        {
            builder.Append('(');
            builder.Append(Children.Count);
            builder.Append(')');
        }
        return builder.ToString();
    }

    #endregion Public 方法
}
