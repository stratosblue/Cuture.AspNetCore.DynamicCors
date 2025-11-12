// 代码由 AI 自动生成

using Cuture.AspNetCore.DynamicCors.Internal;

namespace Cuture.AspNetCore.DynamicCors.Test.Internal;

[TestClass]
public sealed class DomainNameMatcherTest
{
    #region Private 字段

    private DomainNameMatcher _matcher = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _matcher = new DomainNameMatcher();
    }

    #endregion 测试初始化

    #region AddHost 测试

    [TestMethod]
    public void Should_Add_Multiple_Hosts_Success()
    {
        _matcher.AddHost("example.com");
        _matcher.AddHost("test.com");
        _matcher.AddHost("api.example.com");

        Assert.IsTrue(_matcher.IsMatch("http://example.com"));
        Assert.IsTrue(_matcher.IsMatch("http://test.com"));
        Assert.IsTrue(_matcher.IsMatch("http://api.example.com"));
    }

    [TestMethod]
    public void Should_Add_Single_Host_Success()
    {
        _matcher.AddHost("example.com");
        Assert.IsTrue(_matcher.IsMatch("http://example.com"));
    }

    [TestMethod]
    public void Should_Add_Wildcard_Host_Success()
    {
        _matcher.AddHost("*.example.com");
        Assert.IsTrue(_matcher.IsMatch("http://sub.example.com"));
        Assert.IsTrue(_matcher.IsMatch("https://api.example.com"));
        Assert.IsFalse(_matcher.IsMatch("http://example.com"));
    }

    [TestMethod]
    public void Should_AddOrigin_Success_ForMultipleDomains()
    {
        var domains = new[] { "example.com", "test.org", "*.example.net" };

        foreach (var domain in domains)
        {
            _matcher.AddHost(domain);
        }

        Assert.IsTrue(_matcher.IsMatch("example.com"));
        Assert.IsTrue(_matcher.IsMatch("test.org"));
        Assert.IsTrue(_matcher.IsMatch("sub.example.net"));
        Assert.IsFalse(_matcher.IsMatch("notfound.com"));
    }

    [TestMethod]
    public void Should_AddOrigin_Success_ForStandardDomain()
    {
        var domain = "example.com";
        _matcher.AddHost(domain);

        Assert.IsTrue(_matcher.IsMatch(domain));
    }

    [TestMethod]
    public void Should_AddOrigin_Success_ForWildcardDomain()
    {
        var domain = "*.example.com";
        _matcher.AddHost(domain);

        Assert.IsTrue(_matcher.IsMatch("sub.example.com"));
        Assert.IsTrue(_matcher.IsMatch("another.sub.example.com"));
    }

    [TestMethod]
    public void Should_Handle_Complex_Domain_Structures()
    {
        _matcher.AddHost("sub.domain.example.com");
        Assert.IsTrue(_matcher.IsMatch("http://sub.domain.example.com"));
        Assert.IsFalse(_matcher.IsMatch("http://domain.example.com"));
    }

    [TestMethod]
    public void Should_Throw_ArgumentException_WhenDomainIsInvalid()
    {
        Assert.ThrowsExactly<ArgumentException>(() => _matcher.AddHost("invalid domain"));
    }

    [TestMethod]
    public void Should_Throw_Empty_Host()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => _matcher.AddHost(null!));
    }

    #endregion AddHost 测试

    #region IsMatch 测试

    [TestMethod]
    public void Should_Handle_Path_In_Origin()
    {
        _matcher.AddHost("example.com");
        Assert.IsTrue(_matcher.IsMatch("http://example.com/path"));
        Assert.IsTrue(_matcher.IsMatch("https://example.com/api/v1"));
    }

    [TestMethod]
    public void Should_Handle_Port_Numbers_In_Origin()
    {
        _matcher.AddHost("example.com");
        Assert.IsTrue(_matcher.IsMatch("http://example.com:8080"));
        Assert.IsTrue(_matcher.IsMatch("https://example.com:443"));
    }

    [TestMethod]
    public void Should_Handle_Protocol_In_Origin()
    {
        _matcher.AddHost("example.com");
        Assert.IsTrue(_matcher.IsMatch("http://example.com"));
        Assert.IsTrue(_matcher.IsMatch("https://example.com"));
        Assert.IsTrue(_matcher.IsMatch("ftp://example.com"));
    }

    [TestMethod]
    public void Should_Handle_Subdomain_Wildcard_Correctly()
    {
        _matcher.AddHost("*.sub.example.com");
        Assert.IsTrue(_matcher.IsMatch("http://api.sub.example.com"));
        Assert.IsFalse(_matcher.IsMatch("http://sub.example.com"));
        Assert.IsFalse(_matcher.IsMatch("http://example.com"));
    }

    [TestMethod]
    public void Should_Handle_Wildcard_Match_Correctly()
    {
        _matcher.AddHost("*.example.com");
        Assert.IsTrue(_matcher.IsMatch("http://sub.example.com"));
        Assert.IsTrue(_matcher.IsMatch("https://api.example.com"));
        Assert.IsFalse(_matcher.IsMatch("http://example.com"));
        Assert.IsFalse(_matcher.IsMatch("http://sub.test.com"));
    }

    [TestMethod]
    public void Should_Return_False_For_Non_Existing_Host()
    {
        _matcher.AddHost("example.com");
        Assert.IsFalse(_matcher.IsMatch("http://test.com"));
    }

    [TestMethod]
    public void Should_Return_False_ForPartialMatch()
    {
        _matcher.AddHost("partial.match");
        Assert.IsFalse(_matcher.IsMatch("match"));
    }

    [TestMethod]
    public void Should_Return_False_WhenDomainNotAdded()
    {
        Assert.IsFalse(_matcher.IsMatch("notadded.com"));
    }

    [TestMethod]
    public void Should_Return_True_For_Exact_Match()
    {
        _matcher.AddHost("example.com");
        Assert.IsTrue(_matcher.IsMatch("http://example.com"));
        Assert.IsTrue(_matcher.IsMatch("https://example.com"));
    }

    [TestMethod]
    public void Should_Return_True_ForExactMatch()
    {
        _matcher.AddHost("exact.match");
        Assert.IsTrue(_matcher.IsMatch("exact.match"));
    }

    [TestMethod]
    public void Should_Return_True_ForWildcardMatch()
    {
        _matcher.AddHost("*.wildcard.test");
        Assert.IsTrue(_matcher.IsMatch("any.wildcard.test"));
        Assert.IsTrue(_matcher.IsMatch("multiple.sub.wildcard.test"));
    }

    [TestMethod]
    public void Should_Throw_For_Null_Or_Empty_Origin()
    {
        _matcher.AddHost("example.com");
        Assert.ThrowsExactly<ArgumentNullException>(() => _matcher.IsMatch(null!));
        Assert.ThrowsExactly<ArgumentException>(() => _matcher.IsMatch(""));
        Assert.ThrowsExactly<ArgumentException>(() => _matcher.IsMatch("http://"));
    }

    [TestMethod]
    public void Should_Throw_WhenOriginIsEmpty()
    {
        Assert.ThrowsExactly<ArgumentException>(() => _matcher.IsMatch(""));
        Assert.ThrowsExactly<ArgumentException>(() => _matcher.IsMatch(string.Empty));
    }

    [TestMethod]
    public void Should_Throw_WhenOriginIsWhitespace()
    {
        Assert.ThrowsExactly<ArgumentException>(() => _matcher.IsMatch("   "));
    }

    #endregion IsMatch 测试

    #region RemoveHost 测试

    [TestMethod]
    public void Should_Remove_Existing_Host_Success()
    {
        _matcher.AddHost("example.com");
        Assert.IsTrue(_matcher.IsMatch("http://example.com"));

        var result = _matcher.RemoveHost("example.com");
        Assert.IsTrue(result);
        Assert.IsFalse(_matcher.IsMatch("http://example.com"));
    }

    [TestMethod]
    public void Should_Remove_Subdomain_Host_Success()
    {
        _matcher.AddHost("api.example.com");
        _matcher.AddHost("sub.api.example.com");
        Assert.IsTrue(_matcher.IsMatch("http://api.example.com"));
        Assert.IsTrue(_matcher.IsMatch("http://sub.api.example.com"));

        var result = _matcher.RemoveHost("api.example.com");
        Assert.IsTrue(result);
        Assert.IsFalse(_matcher.IsMatch("http://api.example.com"));
        Assert.IsTrue(_matcher.IsMatch("http://sub.api.example.com"));
    }

    [TestMethod]
    public void Should_Remove_Wildcard_Host_Success()
    {
        _matcher.AddHost("*.example.com");
        Assert.IsTrue(_matcher.IsMatch("http://sub.example.com"));

        var result = _matcher.RemoveHost("*.example.com");
        Assert.IsTrue(result);
        Assert.IsFalse(_matcher.IsMatch("http://sub.example.com"));
    }

    [TestMethod]
    public void Should_RemoveOrigin_DoNothing_WhenDomainNotExist()
    {
        var domain = "nonexist.com";
        _matcher.RemoveHost(domain); // 不应抛出异常

        // 确保其他已添加的域名不受影响
        _matcher.AddHost("other.com");
        Assert.IsTrue(_matcher.IsMatch("other.com"));
    }

    [TestMethod]
    public void Should_RemoveOrigin_KeepPartialTree_WhenHasOtherChildren()
    {
        _matcher.AddHost("shared.root.com");
        _matcher.AddHost("shared.root.other.com");

        _matcher.RemoveHost("shared.root.com");

        Assert.IsFalse(_matcher.IsMatch("shared.root.com"));
        Assert.IsTrue(_matcher.IsMatch("shared.root.other.com"));
    }

    [TestMethod]
    public void Should_RemoveOrigin_PartialTree_WhenNoOtherChildren()
    {
        _matcher.AddHost("a.b.c.com");
        _matcher.RemoveHost("a.b.c.com");

        Assert.IsFalse(_matcher.IsMatch("a.b.c.com"));
    }

    [TestMethod]
    public void Should_RemoveOrigin_Success_ForStandardDomain()
    {
        var domain = "toremove.com";
        _matcher.AddHost(domain);
        _matcher.RemoveHost(domain);

        Assert.IsFalse(_matcher.IsMatch(domain));
    }

    [TestMethod]
    public void Should_RemoveOrigin_Success_ForWildcardDomain()
    {
        var domain = "*.toremove.com";
        _matcher.AddHost(domain);
        _matcher.RemoveHost(domain);

        Assert.IsFalse(_matcher.IsMatch("sub.toremove.com"));
    }

    [TestMethod]
    public void Should_Return_False_When_Removing_Non_Existing_Host()
    {
        _matcher.AddHost("example.com");
        var result = _matcher.RemoveHost("test.com");
        Assert.IsFalse(result);
    }

    #endregion RemoveHost 测试

    #region 复杂场景测试

    [TestMethod]
    public void Should_Handle_Complex_Domain_Hierarchy()
    {
        // 构建复杂的域名层级
        _matcher.AddHost("com");
        _matcher.AddHost("example.com");
        _matcher.AddHost("sub.example.com");
        _matcher.AddHost("api.sub.example.com");
        _matcher.AddHost("*.sub.example.com");

        Assert.IsTrue(_matcher.IsMatch("http://com"));
        Assert.IsTrue(_matcher.IsMatch("http://example.com"));
        Assert.IsTrue(_matcher.IsMatch("http://sub.example.com"));
        Assert.IsTrue(_matcher.IsMatch("http://api.sub.example.com"));
        Assert.IsTrue(_matcher.IsMatch("http://test.sub.example.com"));
    }

    [TestMethod]
    public void Should_Handle_Domain_With_Hyphens()
    {
        _matcher.AddHost("sub-domain.example.com");
        _matcher.AddHost("*.example-site.com");

        Assert.IsTrue(_matcher.IsMatch("http://sub-domain.example.com"));
        Assert.IsTrue(_matcher.IsMatch("http://api.example-site.com"));
    }

    [TestMethod]
    public void Should_Handle_Numeric_Domains()
    {
        _matcher.AddHost("123.456.789.012");
        _matcher.AddHost("*.123.com");

        Assert.IsTrue(_matcher.IsMatch("http://123.456.789.012"));
        Assert.IsTrue(_matcher.IsMatch("http://api.123.com"));
    }

    [TestMethod]
    public void Should_Handle_Overlapping_Wildcards()
    {
        _matcher.AddHost("*.com");
        _matcher.AddHost("*.example.com");
        _matcher.AddHost("example.com");

        Assert.IsTrue(_matcher.IsMatch("http://test.com"));
        Assert.IsTrue(_matcher.IsMatch("http://sub.example.com"));
        Assert.IsTrue(_matcher.IsMatch("http://example.com"));
    }

    #endregion 复杂场景测试

    #region 边界条件测试

    [TestMethod]
    public void Should_Handle_Single_Character_Domains()
    {
        _matcher.AddHost("a");
        _matcher.AddHost("*.b");

        Assert.IsTrue(_matcher.IsMatch("http://a"));
        Assert.IsTrue(_matcher.IsMatch("http://c.b"));
    }

    [TestMethod]
    public void Should_Handle_Special_Characters_In_Domain()
    {
        _matcher.AddHost("test-example.com");
        _matcher.AddHost("*.sub-domain.com");

        Assert.IsTrue(_matcher.IsMatch("http://test-example.com"));
        Assert.IsTrue(_matcher.IsMatch("http://api.sub-domain.com"));
    }

    [TestMethod]
    public void Should_Handle_Very_Long_Domain_Names()
    {
        var longDomain = new string('a', 253) + ".com";
        _matcher.AddHost(longDomain);
        Assert.IsTrue(_matcher.IsMatch($"http://{longDomain}"));
    }

    #endregion 边界条件测试

    #region 并发测试

    [TestMethod]
    public void Should_Handle_Concurrent_Add_And_Remove_Operations()
    {
        var tasks = new List<Task>();

        // 并发添加
        for (int i = 0; i < 100; i++)
        {
            var host = $"test{i}.com";
            tasks.Add(Task.Run(() => _matcher.AddHost(host)));
        }

        Task.WaitAll([.. tasks]);

        // 验证所有域名都被添加
        for (int i = 0; i < 100; i++)
        {
            Assert.IsTrue(_matcher.IsMatch($"http://test{i}.com"));
        }
    }

    #endregion 并发测试

    #region 性能测试

    [TestMethod]
    [Timeout(1000, CooperativeCancellation = true)] // 1秒内完成
    public void Should_Perform_Fast_Lookup_With_Large_Dataset()
    {
        // 添加大量域名
        for (int i = 0; i < 1000; i++)
        {
            _matcher.AddHost($"test{i}.com");
        }

        // 快速查找
        for (int i = 0; i < 1000; i++)
        {
            Assert.IsTrue(_matcher.IsMatch($"http://test{i}.com"));
        }
    }

    #endregion 性能测试
}
