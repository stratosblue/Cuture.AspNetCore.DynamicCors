// 代码由 AI 自动生成

using Cuture.AspNetCore.DynamicCors.Internal.OriginsSync;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Cuture.AspNetCore.DynamicCors.Test.Internal.OriginsSync;

[TestClass]
public sealed class ConfigurationAllowedOriginsSyncSourceTest
{
    #region Private 字段

    private Mock<IConfigurationSection> _configurationMock = null!;

    private ConfigurationAllowedOriginsSyncSource _source = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _configurationMock = new Mock<IConfigurationSection>();
    }

    #endregion 测试初始化

    #region Constructor 测试

    [TestMethod]
    public void Should_Initialize_With_Null_PolicyName()
    {
        _source = new ConfigurationAllowedOriginsSyncSource(null, _configurationMock.Object);

        Assert.IsNull(_source.PolicyName);
    }

    [TestMethod]
    public void Should_Initialize_With_PolicyName_And_Configuration()
    {
        _source = new ConfigurationAllowedOriginsSyncSource("test-policy", _configurationMock.Object);

        Assert.AreEqual("test-policy", _source.PolicyName);
    }

    #endregion Constructor 测试

    #region GetOriginsAsync 测试

    [TestMethod]
    public async Task Should_Handle_Empty_String_In_Array()
    {
        _configurationMock.Setup(m => m.Value)
                          .Returns("http://example.com;;https://test.com");
        _source = new ConfigurationAllowedOriginsSyncSource("test-policy", _configurationMock.Object);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.Contains("http://example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
    }

    [TestMethod]
    public async Task Should_Handle_Semicolon_Separated_Origins()
    {
        _configurationMock.Setup(c => c.Value)
                          .Returns("http://example.com;https://test.com");

        _source = new ConfigurationAllowedOriginsSyncSource("test-policy", _configurationMock.Object);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.Contains("http://example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
    }

    [TestMethod]
    public async Task Should_Handle_Wildcard_Origins()
    {
        _configurationMock.Setup(c => c.Value)
                          .Returns("http://*.example.com;https://test.com");

        _source = new ConfigurationAllowedOriginsSyncSource("test-policy", _configurationMock.Object);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.Contains("http://*.example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
    }

    [TestMethod]
    public async Task Should_Return_Array_Origins_When_Configuration_Is_Array()
    {
        _configurationMock.Setup(c => c.Value)
                          .Returns("http://example.com;https://test.com");
        _source = new ConfigurationAllowedOriginsSyncSource("test-policy", _configurationMock.Object);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.Contains("http://example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
    }

    [TestMethod]
    public async Task Should_Return_Empty_Array_When_Configuration_Is_Null()
    {
        _configurationMock.Setup(c => c.Value)
                          .Returns((string)null!);
        _source = new ConfigurationAllowedOriginsSyncSource("test-policy", _configurationMock.Object);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public async Task Should_Return_Split_Origins_When_Configuration_Is_String()
    {
        _configurationMock.Setup(c => c.Value)
                          .Returns("http://example.com;https://test.com");
        _source = new ConfigurationAllowedOriginsSyncSource("test-policy", _configurationMock.Object);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.Contains("http://example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
    }

    [TestMethod]
    public async Task Should_Return_Split_Origins_With_Spaces()
    {
        _configurationMock.Setup(c => c.Value)
                          .Returns("http://example.com; https://test.com ; https://api.com");
        _source = new ConfigurationAllowedOriginsSyncSource("test-policy", _configurationMock.Object);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(3, result.Count());
        Assert.Contains("http://example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
        Assert.Contains("https://api.com", [.. result]);
    }

    #endregion GetOriginsAsync 测试

    #region 边界条件测试

    [TestMethod]
    public async Task Should_Handle_Duplicate_Origins()
    {
        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(c => c.Value).Returns("http://example.com;http://example.com;https://test.com");

        _source = new ConfigurationAllowedOriginsSyncSource("test-policy", configSectionMock.Object);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(3, result.Count());
    }

    [TestMethod]
    public async Task Should_Handle_Empty_Configuration_Value()
    {
        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(c => c.Value).Returns(string.Empty);

        _source = new ConfigurationAllowedOriginsSyncSource("test-policy", configSectionMock.Object);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public async Task Should_Handle_Single_Origin()
    {
        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(c => c.Value).Returns("http://example.com");

        _source = new ConfigurationAllowedOriginsSyncSource("test-policy", configSectionMock.Object);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("http://example.com", result.First());
    }

    [TestMethod]
    public async Task Should_Handle_Whitespace_Only_Configuration_Value()
    {
        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(c => c.Value).Returns("   ");

        _source = new ConfigurationAllowedOriginsSyncSource("test-policy", configSectionMock.Object);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(0, result.Count());
    }

    #endregion 边界条件测试
}
