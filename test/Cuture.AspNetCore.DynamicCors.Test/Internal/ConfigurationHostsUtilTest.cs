// 代码由 AI 自动生成

using Cuture.AspNetCore.DynamicCors.Internal;

namespace Cuture.AspNetCore.DynamicCors.Test.Internal;

[TestClass]
public sealed class ConfigurationHostsUtilTest
{
    #region SplitAsHosts 测试

    [TestMethod]
    public void Should_Handle_Empty_String()
    {
        var input = "";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void Should_Handle_Hosts_With_Ports()
    {
        var input = "example.com:8080;test.com:443";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(2, result);
        Assert.AreEqual("example.com:8080", result[0]);
        Assert.AreEqual("test.com:443", result[1]);
    }

    [TestMethod]
    public void Should_Handle_Ip_Addresses()
    {
        var input = "192.168.1.1;10.0.0.1";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(2, result);
        Assert.AreEqual("192.168.1.1", result[0]);
        Assert.AreEqual("10.0.0.1", result[1]);
    }

    [TestMethod]
    public void Should_Handle_Localhost()
    {
        var input = "localhost;127.0.0.1";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(2, result);
        Assert.AreEqual("localhost", result[0]);
        Assert.AreEqual("127.0.0.1", result[1]);
    }

    [TestMethod]
    public void Should_Handle_Multiple_Consecutive_Semicolons()
    {
        var input = "example.com;;;test.com";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(2, result);
        Assert.AreEqual("example.com", result[0]);
        Assert.AreEqual("test.com", result[1]);
    }

    [TestMethod]
    public void Should_Handle_Single_Semicolon()
    {
        var input = ";";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void Should_Handle_Subdomains()
    {
        var input = "api.example.com;www.test.com;mail.demo.com";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(3, result);
        Assert.AreEqual("api.example.com", result[0]);
        Assert.AreEqual("www.test.com", result[1]);
        Assert.AreEqual("mail.demo.com", result[2]);
    }

    [TestMethod]
    public void Should_Handle_Urls_With_Protocols()
    {
        var input = "https://example.com;http://test.com";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(2, result);
        Assert.AreEqual("https://example.com", result[0]);
        Assert.AreEqual("http://test.com", result[1]);
    }

    [TestMethod]
    public void Should_Handle_Whitespace_Only()
    {
        var input = "   ";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void Should_Handle_Wildcards()
    {
        var input = "*.example.com;*.test.com";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(2, result);
        Assert.AreEqual("*.example.com", result[0]);
        Assert.AreEqual("*.test.com", result[1]);
    }

    [TestMethod]
    public void Should_Remove_Empty_Entries()
    {
        var input = "example.com;;test.com;";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(2, result);
        Assert.AreEqual("example.com", result[0]);
        Assert.AreEqual("test.com", result[1]);
    }

    [TestMethod]
    public void Should_Split_Multiple_Hosts_With_Semicolon()
    {
        var input = "example.com;test.com;demo.com";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(3, result);
        Assert.AreEqual("example.com", result[0]);
        Assert.AreEqual("test.com", result[1]);
        Assert.AreEqual("demo.com", result[2]);
    }

    [TestMethod]
    public void Should_Split_Single_Host()
    {
        var input = "example.com";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(1, result);
        Assert.AreEqual("example.com", result[0]);
    }

    [TestMethod]
    public void Should_Trim_Whitespace()
    {
        var input = "  example.com  ;  test.com  ";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(2, result);
        Assert.AreEqual("example.com", result[0]);
        Assert.AreEqual("test.com", result[1]);
    }

    #endregion SplitAsHosts 测试

    #region 边界条件测试

    [TestMethod]
    public void Should_Handle_Single_Char_Hosts()
    {
        var input = "a;b;c";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(3, result);
        Assert.AreEqual("a", result[0]);
        Assert.AreEqual("b", result[1]);
        Assert.AreEqual("c", result[2]);
    }

    [TestMethod]
    public void Should_Handle_Special_Characters_In_Hosts()
    {
        var input = "test-domain.com;sub_domain.com;test123.com";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(3, result);
        Assert.AreEqual("test-domain.com", result[0]);
        Assert.AreEqual("sub_domain.com", result[1]);
        Assert.AreEqual("test123.com", result[2]);
    }

    [TestMethod]
    public void Should_Handle_Very_Long_Host_String()
    {
        var longHost = new string('a', 100);
        var input = longHost + ";" + longHost + "2";
        var result = ConfigurationHostsUtil.SplitAsHosts(input);

        Assert.HasCount(2, result);
        Assert.AreEqual(longHost, result[0]);
        Assert.AreEqual(longHost + "2", result[1]);
    }

    #endregion 边界条件测试
}
