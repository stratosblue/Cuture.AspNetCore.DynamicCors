using System.Collections.Concurrent;

namespace Cuture.AspNetCore.DynamicCors.Internal;

/// <summary>
/// 域名匹配器
/// </summary>
internal sealed partial class DomainNameMatcher
{
    #region Private 字段

    private const string WildcardPrefix = "*.";

    #endregion Private 字段

    #region Public 属性

    public ConcurrentDictionary<ReadOnlyMemory<char>, byte> All { get; } = new(ReadOnlyMemoryCharComparer.Shared);

    public ConcurrentDictionary<ReadOnlyMemory<char>, DomainNameMatchNode> Wildcards { get; } = new(ReadOnlyMemoryCharComparer.Shared);

    #endregion Public 属性

    #region Public 方法

    public void AddHost(string host)
    {
        var domain = DomainNameValues.ExtractDomain(host).AsMemory();

        //非泛域名
        if (!domain.Span.StartsWith(WildcardPrefix, StringComparison.Ordinal))
        {
            All.TryAdd(domain, 0);
            return;
        }

        using var domainNameValues = DomainNameValues.Create(domain);
        var values = domainNameValues.Values;

        values = values[..^1];

        lock (Wildcards)
        {
            if (!Wildcards.TryGetValue(values[0], out var matchNode))
            {
                //在所有内容中添加根节点
                matchNode = new DomainNameMatchNode(null, values[0]);
                Wildcards[values[0]] = matchNode;
            }

            DomainNameMatchNode? finalNode = matchNode;
            var children = matchNode.Children;
            for (var i = 1; i < values.Length; i++)
            {
                var current = values[i];
                if (!children.TryGetValue(current, out matchNode))
                {
                    matchNode = new DomainNameMatchNode(finalNode, current);
                    children[current] = matchNode;
                }
                finalNode = matchNode;
                children = matchNode.Children;
            }

            finalNode?.IsEndpoint = true;
        }
    }

    public bool IsMatch(string host)
    {
        ArgumentException.ThrowIfNullOrEmpty(host);

        var domain = DomainNameValues.ExtractDomain(host).AsMemory();

        //非泛域名
        if (All.ContainsKey(domain))
        {
            return true;
        }

        using var domainNameValues = DomainNameValues.Create(domain);
        if (domainNameValues.IsDefaultOrEmpty)
        {
            return false;
        }

        var values = domainNameValues.Values;

        if (!Wildcards.TryGetValue(values[0], out var matchNode))
        {
            return false;
        }

        if (matchNode.IsEndpoint)
        {
            return true;
        }

        var matchIndex = 1;
        var lastIndex = values.Length - 1;
        for (; matchIndex < values.Length; matchIndex++)
        {
            var nodeValue = values[matchIndex];

            if (!matchNode.Children.TryGetValue(nodeValue, out matchNode))
            {
                return false;
            }

            if (matchNode.IsEndpoint)
            {
                //"*.api.example.com" 不应该匹配 http://api.example.com
                return matchIndex != lastIndex;
            }
        }

        return false;
    }

    public bool RemoveHost(string host)
    {
        var domain = DomainNameValues.ExtractDomain(host).AsMemory();

        //非泛域名
        if (!domain.Span.StartsWith(WildcardPrefix, StringComparison.Ordinal))
        {
            return All.TryRemove(domain, out _);
        }

        using var domainNameValues = DomainNameValues.Create(domain);
        var values = domainNameValues.Values;

        values = values[..^1];

        lock (Wildcards)
        {
            if (!Wildcards.TryGetValue(values[0], out var matchNode)) //在所有内容字典中没有找到根节点
            {
                return false;
            }

            var children = matchNode.Children;
            for (var i = 1; i < values.Length; i++)
            {
                var current = values[i];
                if (!children.TryGetValue(current, out matchNode))
                {
                    return false;
                }
                children = matchNode.Children;
            }

            var result = matchNode is not null;

            while (matchNode is not null)
            {
                if (!matchNode.Children.IsEmpty)
                {
                    matchNode.IsEndpoint = false;
                    break;
                }

                if (matchNode.Parent is null)   //已查找到根节点
                {
                    if (matchNode.Children.IsEmpty)
                    {
                        Wildcards.TryRemove(matchNode.Value, out _);
                    }
                    break;
                }

                matchNode.IsEndpoint = false;

                if (matchNode.IsEndpoint)
                {
                    break;
                }
                else
                {
                    matchNode.Parent.Children.TryRemove(matchNode.Value, out _);
                    matchNode = matchNode.Parent;
                }
            }

            return result;
        }
    }

    #endregion Public 方法
}
