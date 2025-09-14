namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 域名集合
/// <br/>Note: 实现时应当保证线程安全
/// </summary>
public interface IDomainNameCollection
{
    #region Public 方法

    /// <summary>
    /// 添加一个允许的域名
    /// </summary>
    /// <param name="host"></param>
    public void Add(string host);

    /// <summary>
    /// 清空集合的内容
    /// </summary>
    public void Clear();

    /// <summary>
    /// 检查来源 <paramref name="origin"/> 是否在集合中
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public bool Contains(string origin);

    /// <summary>
    /// 移除一个允许的域名
    /// </summary>
    /// <param name="host"></param>
    public bool Remove(string host);

    /// <summary>
    /// 重置集合为 <paramref name="hosts"/>
    /// </summary>
    /// <param name="hosts"></param>
    /// <returns></returns>
    public void Reset(IEnumerable<string> hosts);

    #endregion Public 方法
}
