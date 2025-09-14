namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 请求来源验证器集合
/// </summary>
public interface IOriginCorsValidatorContainer
{
    #region Public 方法

    /// <summary>
    /// 获取策略<paramref name="policyName"/>的请求来源验证器
    /// </summary>
    /// <param name="policyName">策略名</param>
    /// <returns></returns>
    public IOriginCorsValidator? Get(string policyName);

    #endregion Public 方法
}
