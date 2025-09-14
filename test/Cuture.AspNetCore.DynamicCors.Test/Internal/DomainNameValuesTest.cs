// 代码由 AI 自动生成

using Cuture.AspNetCore.DynamicCors.Internal;

namespace Cuture.AspNetCore.DynamicCors.Test.Internal;

[TestClass]
public sealed class DomainNameValuesTest
{
    #region Parse 测试

    [TestMethod]
    public void Should_Parse_Complex_Domain_Success()
    {
        using var domainNameValues = DomainNameValues.Parse("https://api.sub.example.com:8080/");

        Assert.AreEqual(4, domainNameValues.Values.Length);
        Assert.AreEqual("api", domainNameValues.Values[3].ToString());
        Assert.AreEqual("sub", domainNameValues.Values[2].ToString());
        Assert.AreEqual("example", domainNameValues.Values[1].ToString());
        Assert.AreEqual("com", domainNameValues.Values[0].ToString());
    }

    [TestMethod]
    public void Should_Parse_Domain_With_Protocol_Success()
    {
        using var domainNameValues = DomainNameValues.Parse("http://example.com");

        Assert.AreEqual(2, domainNameValues.Values.Length);
        Assert.AreEqual("example", domainNameValues.Values[1].ToString());
        Assert.AreEqual("com", domainNameValues.Values[0].ToString());
    }

    [TestMethod]
    public void Should_Parse_Simple_Domain_Success()
    {
        using var domainNameValues = DomainNameValues.Parse("example.com");

        Assert.IsFalse(domainNameValues.IsDefaultOrEmpty);
        Assert.AreEqual(2, domainNameValues.Values.Length);
        Assert.AreEqual("example", domainNameValues.Values[1].ToString());
        Assert.AreEqual("com", domainNameValues.Values[0].ToString());
    }

    [TestMethod]
    public void Should_Parse_Single_Label_Domain_Success()
    {
        using var domainNameValues = DomainNameValues.Parse("localhost");

        Assert.AreEqual(1, domainNameValues.Values.Length);
        Assert.AreEqual("localhost", domainNameValues.Values[0].ToString());
    }

    [TestMethod]
    public void Should_Parse_Subdomain_Success()
    {
        using var domainNameValues = DomainNameValues.Parse("api.sub.example.com");

        Assert.AreEqual(4, domainNameValues.Values.Length);
        Assert.AreEqual("api", domainNameValues.Values[3].ToString());
        Assert.AreEqual("sub", domainNameValues.Values[2].ToString());
        Assert.AreEqual("example", domainNameValues.Values[1].ToString());
        Assert.AreEqual("com", domainNameValues.Values[0].ToString());
    }

    #endregion Parse 测试

    #region 边界条件测试

    [TestMethod]
    public void Should_Handle_Domain_With_Hyphens()
    {
        using var domainNameValues = DomainNameValues.Parse("sub-domain.example-site.com");

        Assert.AreEqual(3, domainNameValues.Values.Length);
        Assert.AreEqual("sub-domain", domainNameValues.Values[2].ToString());
        Assert.AreEqual("example-site", domainNameValues.Values[1].ToString());
        Assert.AreEqual("com", domainNameValues.Values[0].ToString());
    }

    [TestMethod]
    public void Should_Handle_Domain_With_Numbers()
    {
        using var domainNameValues = DomainNameValues.Parse("api123.example456.com");

        Assert.AreEqual(3, domainNameValues.Values.Length);
        Assert.AreEqual("api123", domainNameValues.Values[2].ToString());
        Assert.AreEqual("example456", domainNameValues.Values[1].ToString());
        Assert.AreEqual("com", domainNameValues.Values[0].ToString());
    }

    [TestMethod]
    public void Should_Handle_IP_Address()
    {
        using var domainNameValues = DomainNameValues.Parse("192.168.1.1");

        Assert.AreEqual(4, domainNameValues.Values.Length);
        Assert.AreEqual("192", domainNameValues.Values[3].ToString());
        Assert.AreEqual("168", domainNameValues.Values[2].ToString());
        Assert.AreEqual("1", domainNameValues.Values[1].ToString());
        Assert.AreEqual("1", domainNameValues.Values[0].ToString());
    }

    [TestMethod]
    public void Should_Handle_Localhost()
    {
        using var domainNameValues = DomainNameValues.Parse("localhost");

        Assert.AreEqual(1, domainNameValues.Values.Length);
        Assert.AreEqual("localhost", domainNameValues.Values[0].ToString());
    }

    [TestMethod]
    public void Should_Handle_Very_Long_Domain_Name()
    {
        var longDomain = new string('a', 50) + ".com";
        using var domainNameValues = DomainNameValues.Parse(longDomain);

        Assert.AreEqual(2, domainNameValues.Values.Length);
        Assert.AreEqual(new string('a', 50), domainNameValues.Values[1].ToString());
        Assert.AreEqual("com", domainNameValues.Values[0].ToString());
    }

    [TestMethod]
    public void Should_Throw_Whitespace_String()
    {
        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            using var domainNameValues = DomainNameValues.Parse("   ");
        });
        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            using var domainNameValues = DomainNameValues.Parse(null!);
        });

        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            using var domainNameValues = DomainNameValues.Parse("");
        });
    }

    #endregion 边界条件测试

    #region 错误处理测试

    [TestMethod]
    public void Should_Throw_Domain_With_Trailing_Dot()
    {
        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            using var domainNameValues = DomainNameValues.Parse("example.com.");
        });
    }

    [TestMethod]
    public void Should_Throw_InvalidOperationException_For_Invalid_Domain_Format()
    {
        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            using var domainNameValues = DomainNameValues.Parse("..example.com");
        });
    }

    #endregion 错误处理测试

    #region ExtractDomain 测试

    [TestMethod]
    public void Should_Extract_Domain_From_Complex_URL()
    {
        using var domainNameValues = DomainNameValues.Parse("https://api.sub.example.com:8080/");

        Assert.AreEqual(4, domainNameValues.Values.Length);
        Assert.AreEqual("api", domainNameValues.Values[3].ToString());
        Assert.AreEqual("sub", domainNameValues.Values[2].ToString());
        Assert.AreEqual("example", domainNameValues.Values[1].ToString());
        Assert.AreEqual("com", domainNameValues.Values[0].ToString());
    }

    [TestMethod]
    public void Should_Extract_Domain_From_URL_With_Port()
    {
        using var domainNameValues = DomainNameValues.Parse("https://example.com:8080/");

        Assert.AreEqual(2, domainNameValues.Values.Length);
        Assert.AreEqual("example", domainNameValues.Values[1].ToString());
        Assert.AreEqual("com", domainNameValues.Values[0].ToString());
    }

    [TestMethod]
    public void Should_Extract_Domain_From_URL_With_Protocol()
    {
        using var domainNameValues = DomainNameValues.Parse("http://example.com/");

        Assert.AreEqual(2, domainNameValues.Values.Length);
        Assert.AreEqual("example", domainNameValues.Values[1].ToString());
        Assert.AreEqual("com", domainNameValues.Values[0].ToString());
    }

    [TestMethod]
    public void Should_Handle_Wildcard_Domain_With_ExtractDomain()
    {
        using var domainNameValues = DomainNameValues.Parse("*.example.com");

        Assert.AreEqual(3, domainNameValues.Values.Length);
        Assert.AreEqual("*", domainNameValues.Values[2].ToString());
        Assert.AreEqual("example", domainNameValues.Values[1].ToString());
        Assert.AreEqual("com", domainNameValues.Values[0].ToString());
    }

    #endregion ExtractDomain 测试

    #region Dispose 测试

    [TestMethod]
    public void Should_Dispose_Without_Exception()
    {
        var domainNameValues = DomainNameValues.Parse("example.com");
        domainNameValues.Dispose();

        // 不应该抛出异常
    }

    [TestMethod]
    public void Should_Handle_Multiple_Dispose_Calls()
    {
        var domainNameValues = DomainNameValues.Parse("example.com");
        domainNameValues.Dispose();
        domainNameValues.Dispose(); // 第二次调用不应该抛出异常
    }

    #endregion Dispose 测试

    #region Values 属性测试

    [TestMethod]
    public void Should_Return_Valid_Values_Array()
    {
        using var domainNameValues = DomainNameValues.Parse("a.b.c.d.com");

        Assert.AreEqual(5, domainNameValues.Values.Length);
        Assert.AreEqual("a", domainNameValues.Values[4].ToString());
        Assert.AreEqual("b", domainNameValues.Values[3].ToString());
        Assert.AreEqual("c", domainNameValues.Values[2].ToString());
        Assert.AreEqual("d", domainNameValues.Values[1].ToString());
        Assert.AreEqual("com", domainNameValues.Values[0].ToString());
    }

    #endregion Values 属性测试
}
