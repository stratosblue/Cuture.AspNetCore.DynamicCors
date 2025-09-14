// 代码由 AI 自动生成

using Cuture.AspNetCore.DynamicCors.Internal;

namespace Cuture.AspNetCore.DynamicCors.Test;

[TestClass]
public sealed partial class DomainNameMatcherTest
{
    #region Private 字段

    private DomainNameMatcher _domainNameMatcher = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _domainNameMatcher = new DomainNameMatcher();
    }

    #endregion 测试初始化

    #region AddOrigin 测试

    [TestMethod]
    public void Should_AddOrigin_Success_ForMultipleDomains()
    {
        var domains = new[] { "example.com", "test.org", "*.example.net" };

        foreach (var domain in domains)
        {
            _domainNameMatcher.AddHost(domain);
        }

        Assert.IsTrue(_domainNameMatcher.IsMatch("example.com"));
        Assert.IsTrue(_domainNameMatcher.IsMatch("test.org"));
        Assert.IsTrue(_domainNameMatcher.IsMatch("sub.example.net"));
        Assert.IsFalse(_domainNameMatcher.IsMatch("notfound.com"));
    }

    [TestMethod]
    public void Should_AddOrigin_Success_ForStandardDomain()
    {
        var domain = "example.com";
        _domainNameMatcher.AddHost(domain);

        Assert.IsTrue(_domainNameMatcher.IsMatch(domain));
    }

    [TestMethod]
    public void Should_AddOrigin_Success_ForWildcardDomain()
    {
        var domain = "*.example.com";
        _domainNameMatcher.AddHost(domain);

        Assert.IsTrue(_domainNameMatcher.IsMatch("sub.example.com"));
        Assert.IsTrue(_domainNameMatcher.IsMatch("another.sub.example.com"));
    }

    [TestMethod]
    public void Should_Throw_ArgumentException_WhenDomainIsInvalid()
    {
        Assert.ThrowsExactly<ArgumentException>(() => _domainNameMatcher.AddHost("invalid domain"));
    }

    #endregion AddOrigin 测试

    #region IsMatch 测试

    [TestMethod]
    public void Should_Return_False_ForPartialMatch()
    {
        _domainNameMatcher.AddHost("partial.match");
        Assert.IsFalse(_domainNameMatcher.IsMatch("match"));
    }

    [TestMethod]
    public void Should_Return_False_WhenDomainNotAdded()
    {
        Assert.IsFalse(_domainNameMatcher.IsMatch("notadded.com"));
    }

    [TestMethod]
    public void Should_Return_True_ForExactMatch()
    {
        _domainNameMatcher.AddHost("exact.match");
        Assert.IsTrue(_domainNameMatcher.IsMatch("exact.match"));
    }

    [TestMethod]
    public void Should_Return_True_ForWildcardMatch()
    {
        _domainNameMatcher.AddHost("*.wildcard.test");
        Assert.IsTrue(_domainNameMatcher.IsMatch("any.wildcard.test"));
        Assert.IsTrue(_domainNameMatcher.IsMatch("multiple.sub.wildcard.test"));
    }

    [TestMethod]
    public void Should_Throw_WhenOriginIsEmpty()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => _domainNameMatcher.IsMatch(null!));
        Assert.ThrowsExactly<ArgumentException>(() => _domainNameMatcher.IsMatch(""));
        Assert.ThrowsExactly<ArgumentException>(() => _domainNameMatcher.IsMatch("http://"));
    }

    [TestMethod]
    public void Should_Throw_WhenOriginIsWhitespace()
    {
        Assert.ThrowsExactly<ArgumentException>(() => _domainNameMatcher.IsMatch("   "));
    }

    #endregion IsMatch 测试

    #region RemoveOrigin 测试

    [TestMethod]
    public void Should_RemoveOrigin_DoNothing_WhenDomainNotExist()
    {
        var domain = "nonexist.com";
        _domainNameMatcher.RemoveHost(domain); // 不应抛出异常

        // 确保其他已添加的域名不受影响
        _domainNameMatcher.AddHost("other.com");
        Assert.IsTrue(_domainNameMatcher.IsMatch("other.com"));
    }

    [TestMethod]
    public void Should_RemoveOrigin_KeepPartialTree_WhenHasOtherChildren()
    {
        _domainNameMatcher.AddHost("shared.root.com");
        _domainNameMatcher.AddHost("shared.root.other.com");

        _domainNameMatcher.RemoveHost("shared.root.com");

        Assert.IsFalse(_domainNameMatcher.IsMatch("shared.root.com"));
        Assert.IsTrue(_domainNameMatcher.IsMatch("shared.root.other.com"));
    }

    [TestMethod]
    public void Should_RemoveOrigin_PartialTree_WhenNoOtherChildren()
    {
        _domainNameMatcher.AddHost("a.b.c.com");
        _domainNameMatcher.RemoveHost("a.b.c.com");

        Assert.IsFalse(_domainNameMatcher.IsMatch("a.b.c.com"));
    }

    [TestMethod]
    public void Should_RemoveOrigin_Success_ForStandardDomain()
    {
        var domain = "toremove.com";
        _domainNameMatcher.AddHost(domain);
        _domainNameMatcher.RemoveHost(domain);

        Assert.IsFalse(_domainNameMatcher.IsMatch(domain));
    }

    [TestMethod]
    public void Should_RemoveOrigin_Success_ForWildcardDomain()
    {
        var domain = "*.toremove.com";
        _domainNameMatcher.AddHost(domain);
        _domainNameMatcher.RemoveHost(domain);

        Assert.IsFalse(_domainNameMatcher.IsMatch("sub.toremove.com"));
    }

    #endregion RemoveOrigin 测试
}
