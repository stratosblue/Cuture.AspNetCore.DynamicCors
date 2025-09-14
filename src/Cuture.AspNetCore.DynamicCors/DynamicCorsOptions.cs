using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Cuture.AspNetCore.DynamicCors;

// see https://github.com/dotnet/aspnetcore/blob/release/9.0/src/Middleware/CORS/src/Infrastructure/CorsOptions.cs

/// <summary>
/// 动态Cors选项
/// </summary>
public class DynamicCorsOptions
{
    #region Private 字段

    private bool _defaultPolicyAdded = false;

    private string _defaultPolicyName = "__DefaultCorsPolicy";

    #endregion Private 字段

    #region Public 属性

    /// <summary>
    /// 默认策略名称 (默认值与<see cref="CorsOptions.DefaultPolicyName"/>相同
    /// </summary>
    public string DefaultPolicyName
    {
        get => _defaultPolicyName;
        set
        {
            if (_defaultPolicyAdded)
            {
                throw new InvalidOperationException($"Do not change the \"{nameof(DefaultPolicyName)}\" after default policy added");
            }
            _defaultPolicyName = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    #endregion Public 属性

    #region Internal 属性

    internal Dictionary<string, (CorsPolicy policy, Task<CorsPolicy> policyTask)> PolicyMap { get; } = new(StringComparer.Ordinal);

    #endregion Internal 属性

    #region Public 方法

    /// <summary>
    /// 添加默认策略
    /// </summary>
    public void AddDefaultPolicy(CorsPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(policy);

        AddPolicy(DefaultPolicyName, policy);

        _defaultPolicyAdded = true;
    }

    /// <summary>
    /// 添加默认策略
    /// </summary>
    public void AddDefaultPolicy(Action<CorsPolicyBuilder> configurePolicy)
    {
        ArgumentNullException.ThrowIfNull(configurePolicy);

        AddPolicy(DefaultPolicyName, configurePolicy);

        _defaultPolicyAdded = true;
    }

    /// <summary>
    /// 添加策略
    /// </summary>
    public void AddPolicy(string name, CorsPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(policy);

        PolicyMap[name] = (policy, Task.FromResult(policy));
    }

    /// <summary>
    /// 添加策略
    /// </summary>
    public void AddPolicy(string name, Action<CorsPolicyBuilder> configurePolicy)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(configurePolicy);

        var policyBuilder = new CorsPolicyBuilder();
        configurePolicy(policyBuilder);
        var policy = policyBuilder.Build();

        PolicyMap[name] = (policy, Task.FromResult(policy));
    }

    #endregion Public 方法
}
