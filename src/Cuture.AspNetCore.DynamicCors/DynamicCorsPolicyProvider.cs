using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 动态Cors策略提供器
/// </summary>
/// <inheritdoc cref="DynamicCorsPolicyProvider"/>
public class DynamicCorsPolicyProvider(DynamicCorsPoliciesContainer policiesContainer, IServiceProvider serviceProvider)
    : ICorsPolicyProvider
{
    #region Public 方法

    /// <inheritdoc/>
    public virtual async Task<CorsPolicy?> GetPolicyAsync(HttpContext context, string? policyName)
    {
        return await policiesContainer.GetPolicyAsync(context, policyName)
               ?? await serviceProvider.GetRequiredService<DefaultCorsPolicyProvider>().GetPolicyAsync(context, policyName);
    }

    #endregion Public 方法
}
