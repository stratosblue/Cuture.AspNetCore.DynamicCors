// 代码由 AI 自动生成

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace Cuture.AspNetCore.DynamicCors.Test;

[TestClass]
public sealed class DynamicCorsPolicyProviderTest
{
    #region Private 字段

    private IOptions<CorsOptions> _corsOptions = null!;

    private DefaultCorsPolicyProvider _defaultProvider = null!;

    private Mock<DynamicCorsPoliciesContainer> _policiesContainerMock = null!;

    private DynamicCorsPolicyProvider _provider = null!;

    private Mock<IServiceProvider> _serviceProviderMock = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _corsOptions = Options.Create(new CorsOptions());
        var originCorsValidatorContainerMock = new Mock<IOriginCorsValidatorContainer>();

        _policiesContainerMock = new Mock<DynamicCorsPoliciesContainer>(Options.Create(new DynamicCorsOptions()), originCorsValidatorContainerMock.Object);
        _serviceProviderMock = new Mock<IServiceProvider>();
        _defaultProvider = new DefaultCorsPolicyProvider(_corsOptions);

        _serviceProviderMock.Setup(m => m.GetService(typeof(DefaultCorsPolicyProvider)))
                            .Returns(_defaultProvider);

        _provider = new DynamicCorsPolicyProvider(_policiesContainerMock.Object, _serviceProviderMock.Object);
    }

    #endregion 测试初始化

    #region GetPolicyAsync 测试

    [TestMethod]
    public async Task Should_Return_Null_When_PolicyNotFound_Anywhere()
    {
        var context = new DefaultHttpContext();
        var policyName = "non-existent-policy";

        _policiesContainerMock.Setup(m => m.GetPolicyAsync(context, policyName))
                             .ReturnsAsync((CorsPolicy?)null);

        var result = await _provider.GetPolicyAsync(context, policyName);

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task Should_Return_Policy_From_Container_When_PolicyExists()
    {
        var context = new DefaultHttpContext();
        var policyName = "test-policy";
        var expectedPolicy = new CorsPolicy();

        _policiesContainerMock.Setup(m => m.GetPolicyAsync(context, policyName))
                             .ReturnsAsync(expectedPolicy);

        var result = await _provider.GetPolicyAsync(context, policyName);

        Assert.AreEqual(expectedPolicy, result);
        _policiesContainerMock.Verify(m => m.GetPolicyAsync(context, policyName), Times.Once);
    }

    [TestMethod]
    public async Task Should_Return_Policy_From_DefaultProvider_When_PolicyNotInContainer()
    {
        var context = new DefaultHttpContext();
        var policyName = "non-existent-policy";
        var expectedPolicy = new CorsPolicy();

        _policiesContainerMock.Setup(m => m.GetPolicyAsync(context, policyName))
                             .ReturnsAsync((CorsPolicy?)null);

        _corsOptions.Value.AddPolicy(policyName, expectedPolicy);

        var result = await _provider.GetPolicyAsync(context, policyName);

        Assert.AreEqual(expectedPolicy, result);
        _policiesContainerMock.Verify(m => m.GetPolicyAsync(context, policyName), Times.Once);
    }

    [TestMethod]
    public async Task Should_Use_Null_PolicyName_When_PolicyNameIsNull()
    {
        var context = new DefaultHttpContext();

        _policiesContainerMock.Setup(m => m.GetPolicyAsync(context, null))
                             .ReturnsAsync((CorsPolicy?)null);

        await _provider.GetPolicyAsync(context, null);

        _policiesContainerMock.Verify(m => m.GetPolicyAsync(context, null), Times.Once);
    }

    #endregion GetPolicyAsync 测试
}
