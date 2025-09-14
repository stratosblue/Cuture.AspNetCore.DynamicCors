#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配

using Cuture.AspNetCore.DynamicCors.Internal;

namespace Cuture.AspNetCore.DynamicCors;

internal sealed class DefaultDomainNameCollection : IDomainNameCollection
{
    #region Private 字段

    private DomainNameMatcher _domainNameMatcher = new();

    #endregion Private 字段

    #region Public 方法

    public void Add(string host)
    {
        _domainNameMatcher.AddHost(host);
    }

    public void Clear()
    {
        _domainNameMatcher = new();
    }

    public bool Contains(string origin) => _domainNameMatcher.IsMatch(origin);

    public bool Remove(string host)
    {
        return _domainNameMatcher.RemoveHost(host);
    }

    public void Reset(IEnumerable<string> hosts)
    {
        ArgumentNullException.ThrowIfNull(hosts);

        var domainNameMatcher = new DomainNameMatcher();

        foreach (var host in hosts)
        {
            domainNameMatcher.AddHost(host);
        }

        _domainNameMatcher = domainNameMatcher;
    }

    #endregion Public 方法
}
