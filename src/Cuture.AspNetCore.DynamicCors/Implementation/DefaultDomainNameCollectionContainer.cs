#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配

using System.Collections.Frozen;
using Microsoft.Extensions.Options;

namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 默认<inheritdoc cref="IDomainNameCollectionContainer"/>
/// </summary>
public class DefaultDomainNameCollectionContainer : IDomainNameCollectionContainer
{
    #region Private 字段

    private readonly string _defaultPolicyName;

    private readonly FrozenDictionary<string, IDomainNameCollection> _domainNameCollections;

    #endregion Private 字段

    #region Public 构造函数

    /// <inheritdoc cref="DefaultDomainNameCollectionContainer"/>
    public DefaultDomainNameCollectionContainer(IOptions<DynamicCorsOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options?.Value);

        var defaultPolicyName = options.Value.DefaultPolicyName;

        _domainNameCollections = options.Value.PolicyMap.Keys.Select(policyName =>
        {
            policyName ??= defaultPolicyName;
            var domainNameCollection = new DefaultDomainNameCollection();

            return (policyName, domainNameCollection as IDomainNameCollection);
        }).ToFrozenDictionary(m => m.policyName, m => m.Item2, StringComparer.Ordinal);

        _defaultPolicyName = defaultPolicyName;
    }

    #endregion Public 构造函数

    #region Public 方法

    /// <inheritdoc/>
    public IDomainNameCollection? Get(string policyName)
    {
        _domainNameCollections.TryGetValue(policyName ?? _defaultPolicyName, out var collection);
        return collection;
    }

    #endregion Public 方法
}
