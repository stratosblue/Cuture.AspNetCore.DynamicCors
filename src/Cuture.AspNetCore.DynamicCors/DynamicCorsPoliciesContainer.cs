using System.Collections.Frozen;
using System.ComponentModel;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 动态Cors策略容器
/// </summary>
[EditorBrowsable(EditorBrowsableState.Advanced)]
public class DynamicCorsPoliciesContainer
{
    #region Private 字段

    private static readonly Task<CorsPolicy?> s_nullResult = Task.FromResult<CorsPolicy?>(null);

    private readonly string _defaultPolicyName;

    #endregion Private 字段

    #region Protected 属性

    /// <summary>
    /// 策略任务字典
    /// </summary>
    protected FrozenDictionary<string, Task<CorsPolicy>> Policies { get; }

    #endregion Protected 属性

    #region Public 构造函数

    /// <inheritdoc cref="DynamicCorsPoliciesContainer"/>
    public DynamicCorsPoliciesContainer(IOptions<DynamicCorsOptions> options, IOriginCorsValidatorContainer originCorsValidatorContainer)
    {
        ArgumentNullException.ThrowIfNull(options?.Value);
        ArgumentNullException.ThrowIfNull(originCorsValidatorContainer);

        var policies = options.Value.PolicyMap;

        if (policies.FirstOrDefault(m => m.Value.policy.Origins.Count > 0) is { } hasOriginsPolicy
            && hasOriginsPolicy.Value.policy is not null)
        {
            throw new InvalidOperationException($"Policy \"{hasOriginsPolicy.Key}\" is invalid. DynamicCors cannot include policies with origins settings.");
        }

        //TODO 检测 IsOriginAllowed 是否被设置，并抛出异常

        Policies = policies.ToFrozenDictionary(keySelector: static m => m.Key,
                                               elementSelector: m =>
                                               {
                                                   var policyName = m.Key ?? options.Value.DefaultPolicyName;
                                                   var policy = m.Value.policy;

                                                   var originCorsValidator = originCorsValidatorContainer.Get(policyName)
                                                                             ?? throw new InvalidOperationException($"Cannot get originCorsValidator for policy \"{policyName}\"");

                                                   policy.IsOriginAllowed = originCorsValidator.IsAllowed;

                                                   return m.Value.policyTask;
                                               },
                                               comparer: StringComparer.Ordinal);

        _defaultPolicyName = options.Value.DefaultPolicyName;
    }

    #endregion Public 构造函数

    #region Public 方法

    /// <inheritdoc cref="ICorsPolicyProvider.GetPolicyAsync(HttpContext, string?)"/>
    public virtual Task<CorsPolicy?> GetPolicyAsync(HttpContext context, string? policyName)
    {
        policyName ??= _defaultPolicyName;

        if (Policies.TryGetValue(policyName, out var policyTask))
        {
            return policyTask!;
        }

        return s_nullResult;
    }

    #endregion Public 方法
}
