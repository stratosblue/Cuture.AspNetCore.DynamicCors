// 代码由 AI 自动生成

using Cuture.AspNetCore.DynamicCors.Internal.OriginsSync;
using Microsoft.Extensions.Options;
using Moq;

namespace Cuture.AspNetCore.DynamicCors.Test.Internal.OriginsSync;

[TestClass]
public sealed class OptionsMonitorAllowedOriginsSyncSourceTest
{
    #region Private 字段

    private Mock<IOptionsMonitor<TestOptions>> _optionsMonitorMock = null!;

    private OptionsMonitorAllowedOriginsSyncSource<TestOptions> _source = null!;

    #endregion Private 字段

    #region 测试类

    public class TestOptions
    {
        #region Public 属性

        public string[] Origins { get; set; } = Array.Empty<string>();

        public object? OtherProperty { get; set; }

        public string SingleOrigin { get; set; } = string.Empty;

        #endregion Public 属性
    }

    #endregion 测试类

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _optionsMonitorMock = new Mock<IOptionsMonitor<TestOptions>>();
    }

    #endregion 测试初始化

    #region Constructor 测试

    [TestMethod]
    public void Should_Initialize_With_Null_PolicyName()
    {
        var options = new TestOptions { Origins = ["http://example.com"] };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>(null, _optionsMonitorMock.Object, o => o.Origins);

        Assert.IsNull(_source.PolicyName);
    }

    [TestMethod]
    public void Should_Initialize_With_PolicyName_And_OptionsMonitor_And_PropertySelector()
    {
        var options = new TestOptions { Origins = ["http://example.com"] };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.Origins);

        Assert.AreEqual("test-policy", _source.PolicyName);
    }

    #endregion Constructor 测试

    #region GetOriginsAsync 测试

    [TestMethod]
    public async Task Should_Handle_Complex_Property_Selector()
    {
        var options = new TestOptions { OtherProperty = "http://example.com;https://test.com" };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.OtherProperty?.ToString());

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.Contains("http://example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
    }

    [TestMethod]
    public async Task Should_Handle_Empty_String_In_Array()
    {
        var origins = new[] { "http://example.com", "", "https://test.com" };
        var options = new TestOptions { Origins = origins };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.Origins);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.Contains("http://example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
    }

    [TestMethod]
    public async Task Should_Handle_Null_Elements_In_Array()
    {
        var origins = new[] { "http://example.com", null, "https://test.com" };
        var options = new TestOptions { Origins = origins! };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.Origins);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.Contains("http://example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
    }

    [TestMethod]
    public async Task Should_Handle_Property_Selector_Returning_Null()
    {
        var options = new TestOptions { OtherProperty = null };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.OtherProperty?.ToString());

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public async Task Should_Handle_Semicolon_Separated_Origins()
    {
        var options = new TestOptions { SingleOrigin = "http://example.com;https://test.com" };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.SingleOrigin);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.Contains("http://example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
    }

    [TestMethod]
    public async Task Should_Handle_Wildcard_Origins()
    {
        var origins = new[] { "http://*.example.com", "https://test.com" };
        var options = new TestOptions { Origins = origins };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.Origins);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.Contains("http://*.example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
    }

    [TestMethod]
    public async Task Should_Return_Array_Origins_When_Property_Is_Array()
    {
        var origins = new[] { "http://example.com", "https://test.com" };
        var options = new TestOptions { Origins = origins };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.Origins);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.Contains("http://example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
    }

    [TestMethod]
    public async Task Should_Return_Empty_Array_When_Property_Is_Null()
    {
        var options = new TestOptions { Origins = null! };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.Origins);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public async Task Should_Return_Split_Origins_When_Property_Is_String()
    {
        var options = new TestOptions { SingleOrigin = "http://example.com;https://test.com" };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.SingleOrigin);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.Contains("http://example.com", [.. result]);
        Assert.Contains("https://test.com", [.. result]);
    }

    [TestMethod]
    public async Task Should_Return_Split_Origins_With_Spaces()
    {
        var options = new TestOptions { SingleOrigin = "http://example.com; https://test.com ; https://api.com" };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.SingleOrigin);

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
        var options = new TestOptions { SingleOrigin = "http://example.com;http://example.com;https://test.com" };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.SingleOrigin);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(3, result.Count());
    }

    [TestMethod]
    public async Task Should_Handle_Empty_Property_Value()
    {
        var options = new TestOptions { SingleOrigin = string.Empty };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.SingleOrigin);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public async Task Should_Handle_Single_Origin()
    {
        var options = new TestOptions { SingleOrigin = "http://example.com" };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.SingleOrigin);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("http://example.com", result.First());
    }

    [TestMethod]
    public async Task Should_Handle_Whitespace_Only_Property_Value()
    {
        var options = new TestOptions { SingleOrigin = "   " };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.SingleOrigin);

        var result = await _source.GetOriginsAsync(CancellationToken.None);

        Assert.AreEqual(0, result.Count());
    }

    #endregion 边界条件测试

    #region CancellationToken 测试

    [TestMethod]
    public async Task Should_Handle_Cancellation_Token()
    {
        var origins = new[] { "http://example.com", "https://test.com" };
        var options = new TestOptions { Origins = origins };
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

        _source = new OptionsMonitorAllowedOriginsSyncSource<TestOptions>("test-policy", _optionsMonitorMock.Object, o => o.Origins);

        using var cts = new CancellationTokenSource();
        var result = await _source.GetOriginsAsync(cts.Token);

        Assert.AreEqual(2, result.Count());
    }

    #endregion CancellationToken 测试
}
