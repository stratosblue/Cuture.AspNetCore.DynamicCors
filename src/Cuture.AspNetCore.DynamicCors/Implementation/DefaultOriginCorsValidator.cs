#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配

namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 默认的验证器
/// </summary>
/// <param name="domainNameCollection"></param>
public sealed class DefaultOriginCorsValidator(IDomainNameCollection domainNameCollection)
    : IOriginCorsValidator
{
    #region Private 字段

    private readonly IDomainNameCollection _domainNameCollection = domainNameCollection ?? throw new ArgumentNullException(nameof(domainNameCollection));

    #endregion Private 字段

    #region Public 方法

    /// <inheritdoc/>
    public bool IsAllowed(string origin)
    {
        return _domainNameCollection.Contains(origin);
    }

    #endregion Public 方法
}
