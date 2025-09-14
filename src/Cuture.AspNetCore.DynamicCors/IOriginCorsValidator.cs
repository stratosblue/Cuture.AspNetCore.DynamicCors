namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 请求来源Cors验证器
/// </summary>
public interface IOriginCorsValidator
{
    #region Public 方法

    /// <summary>
    /// 验证来源 <paramref name="origin"/> 是否是允许的
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    bool IsAllowed(string origin);

    #endregion Public 方法
}
