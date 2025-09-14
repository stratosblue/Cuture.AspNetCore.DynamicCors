#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配

using System.Collections.Frozen;
using Microsoft.Extensions.Options;

namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 默认<inheritdoc cref="IOriginCorsValidatorContainer"/>
/// </summary>
public class DefaultOriginCorsValidatorContainer : IOriginCorsValidatorContainer
{
    #region Private 字段

    private readonly string _defaultPolicyName;

    private readonly FrozenDictionary<string, IOriginCorsValidator> _validators;

    #endregion Private 字段

    #region Public 构造函数

    /// <inheritdoc cref="DefaultOriginCorsValidatorContainer"/>
    public DefaultOriginCorsValidatorContainer(IDomainNameCollectionContainer domainNameCollectionContainer, IOptions<DynamicCorsOptions> options)
    {
        ArgumentNullException.ThrowIfNull(domainNameCollectionContainer);
        ArgumentNullException.ThrowIfNull(options?.Value);

        var defaultPolicyName = options.Value.DefaultPolicyName;

        _validators = options.Value.PolicyMap.Keys.Select(policyName =>
        {
            policyName ??= defaultPolicyName;
            var domainNameCollection = domainNameCollectionContainer.Get(policyName)
                                       ?? throw new InvalidOperationException($"Cannot get domainNameCollection for policy \"{policyName}\"");

            return (policyName, new DefaultOriginCorsValidator(domainNameCollection) as IOriginCorsValidator);
        }).ToFrozenDictionary(m => m.policyName, m => m.Item2, StringComparer.Ordinal);

        _defaultPolicyName = defaultPolicyName;
    }

    #endregion Public 构造函数

    #region Public 方法

    /// <inheritdoc/>
    public IOriginCorsValidator? Get(string policyName)
    {
        _validators.TryGetValue(policyName ?? _defaultPolicyName, out var collection);
        return collection;
    }

    #endregion Public 方法
}
