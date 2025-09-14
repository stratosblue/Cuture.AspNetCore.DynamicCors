using System.Buffers;
using System.Text.RegularExpressions;

namespace Cuture.AspNetCore.DynamicCors.Internal;

internal ref partial struct DomainNameValues(ReadOnlyMemory<char>[] pooledValues, int length) : IDisposable
{
    #region Private 字段

    private readonly int _length = length;

    private ReadOnlyMemory<char>[]? _pooledValues = pooledValues;

    #endregion Private 字段

    #region Public 属性

    public readonly bool IsDefaultOrEmpty => _pooledValues == null || _pooledValues.Length == 0;

    public readonly ReadOnlySpan<ReadOnlyMemory<char>> Values => _pooledValues!.AsSpan(0, _length);

    #endregion Public 属性

    #region Public 方法

    public void Dispose()
    {
        if (_pooledValues is { } pooledValues)
        {
            _pooledValues = null;
            ArrayPool<ReadOnlyMemory<char>>.Shared.Return(pooledValues);
        }
    }

    #region 静态方法

    /// <summary>
    /// 从 <paramref name="domain"/> 创建多段域名值
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    public static DomainNameValues Create(ReadOnlyMemory<char> domain)
    {
        var nodeCount = domain.Span.Count('.') + 1;

        var nodeBuffer = ArrayPool<ReadOnlyMemory<char>>.Shared.Rent(nodeCount);

        if (nodeCount == 0)
        {
            nodeBuffer[0] = domain;
            return new(nodeBuffer, 1);
        }

        var nodeIndex = nodeCount - 1;
        try
        {
            var span = domain.Span;
            while (span.Length > 0)
            {
                var index = span.IndexOf('.');
                if (index < 0)
                {
                    nodeBuffer[0] = domain;
                    break;
                }
                var current = domain.Slice(0, index).Trim();

                if (current.IsEmpty)
                {
                    throw new InvalidOperationException($"The input host \"{domain}\" is invalid");
                }

                nodeBuffer[nodeIndex--] = current;

                domain = domain.Slice(index + 1);
                span = domain.Span;
            }
            return new(nodeBuffer, nodeCount);
        }
        catch
        {
            ArrayPool<ReadOnlyMemory<char>>.Shared.Return(nodeBuffer);
            throw;
        }
    }

    /// <summary>
    /// 将输入 <paramref name="host"/> 解析为多段域名值
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static DomainNameValues Parse(string host)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(host);
        return Create(ExtractDomain(host).AsMemory());
    }

    #endregion 静态方法

    #endregion Public 方法

    #region DomainProcess

    /// <summary>
    /// 从<paramref name="host"/>中提取域名
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string ExtractDomain(string host)
    {
        var match = GetDomainNameMatchRegex().Match(host);
        if (!match.Success)
        {
            var span = host.AsSpan();
            if (span.StartsWith("*.", StringComparison.Ordinal))
            {
                span = span.Slice(2);
            }
            if (GetDomainNameValidRegex().IsMatch(span))
            {
                return host;
            }
            throw new ArgumentException($"The input value \"{host}\" is invalid.");
        }
        return match.Value;
    }

    [GeneratedRegex("(?<=.+?://)(.+?)(?=[:/]|$)")]
    private static partial Regex GetDomainNameMatchRegex();

    [GeneratedRegex("^(?!-)[_a-zA-Z0-9-]+(\\.[_a-zA-Z0-9-]+)*$")]
    private static partial Regex GetDomainNameValidRegex();

    #endregion DomainProcess
}
