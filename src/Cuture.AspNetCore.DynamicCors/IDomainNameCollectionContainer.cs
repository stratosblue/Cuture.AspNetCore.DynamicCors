namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 域名集合容器
/// </summary>
public interface IDomainNameCollectionContainer
{
    #region Public 方法

    /// <summary>
    /// 获取策略<paramref name="policyName"/>的域名集合
    /// </summary>
    /// <param name="policyName">策略名</param>
    /// <returns></returns>
    public IDomainNameCollection? Get(string policyName);

    #endregion Public 方法
}
