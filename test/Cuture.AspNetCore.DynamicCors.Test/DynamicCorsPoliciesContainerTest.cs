// 代码由 AI 自动生成

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace Cuture.AspNetCore.DynamicCors.Test;

[TestClass]
public sealed class DynamicCorsPoliciesContainerTest
{
    #region Private 字段

    private DynamicCorsOptions _options = null!;

    private Mock<IOriginCorsValidatorContainer> _validatorContainerMock = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _validatorContainerMock = new Mock<IOriginCorsValidatorContainer>();
        _options = new DynamicCorsOptions();
    }

    #endregion 测试初始化

    #region 构造函数测试

    [TestMethod]
    public void Should_Initialize_Success_With_Valid_Options()
    {
        var policy = new CorsPolicy();
        _options.PolicyMap["test-policy"] = (policy, Task.FromResult(policy));
        _options.DefaultPolicyName = "test-policy";

        var validatorMock = new Mock<IOriginCorsValidator>();
        _validatorContainerMock.Setup(m => m.Get("test-policy"))
                             .Returns(validatorMock.Object);

        var container = new DynamicCorsPoliciesContainer(
            Options.Create(_options),
            _validatorContainerMock.Object);

        Assert.IsNotNull(container);
    }

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_Options_Is_Null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            new DynamicCorsPoliciesContainer(
                null!,
                _validatorContainerMock.Object));
    }

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_ValidatorContainer_Is_Null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            new DynamicCorsPoliciesContainer(
                Options.Create(_options),
                null!));
    }

    [TestMethod]
    public void Should_Throw_InvalidOperationException_When_Policy_Has_Origins()
    {
        var policy = new CorsPolicy();
        policy.Origins.Add("http://example.com");
        _options.PolicyMap["test-policy"] = (policy, Task.FromResult(policy));

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            new DynamicCorsPoliciesContainer(
                Options.Create(_options),
                _validatorContainerMock.Object));
    }

    [TestMethod]
    public void Should_Throw_InvalidOperationException_When_Validator_Not_Found()
    {
        var policy = new CorsPolicy();
        _options.PolicyMap["test-policy"] = (policy, Task.FromResult(policy));
        _options.DefaultPolicyName = "test-policy";

        _validatorContainerMock.Setup(m => m.Get("test-policy"))
                             .Returns((IOriginCorsValidator?)null);

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            new DynamicCorsPoliciesContainer(
                Options.Create(_options),
                _validatorContainerMock.Object));
    }

    #endregion 构造函数测试

    #region GetPolicyAsync 测试

    [TestMethod]
    public async Task Should_Return_Null_When_Policy_Not_Exists()
    {
        var policy = new CorsPolicy();
        _options.PolicyMap["test-policy"] = (policy, Task.FromResult(policy));
        _options.DefaultPolicyName = "test-policy";

        var validatorMock = new Mock<IOriginCorsValidator>();
        _validatorContainerMock.Setup(m => m.Get("test-policy"))
                             .Returns(validatorMock.Object);

        var container = new DynamicCorsPoliciesContainer(
            Options.Create(_options),
            _validatorContainerMock.Object);

        var context = new DefaultHttpContext();
        var result = await container.GetPolicyAsync(context, "non-existent-policy");

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task Should_Return_Policy_When_Policy_Exists()
    {
        var policy = new CorsPolicy();
        _options.PolicyMap["test-policy"] = (policy, Task.FromResult(policy));
        _options.DefaultPolicyName = "test-policy";

        var validatorMock = new Mock<IOriginCorsValidator>();
        _validatorContainerMock.Setup(m => m.Get("test-policy"))
                             .Returns(validatorMock.Object);

        var container = new DynamicCorsPoliciesContainer(
            Options.Create(_options),
            _validatorContainerMock.Object);

        var context = new DefaultHttpContext();
        var result = await container.GetPolicyAsync(context, "test-policy");

        Assert.AreEqual(policy, result);
    }

    [TestMethod]
    public async Task Should_Use_Default_Policy_Name_When_PolicyName_Is_Null()
    {
        var policy = new CorsPolicy();
        _options.PolicyMap["default"] = (policy, Task.FromResult(policy));
        _options.DefaultPolicyName = "default";

        var validatorMock = new Mock<IOriginCorsValidator>();
        _validatorContainerMock.Setup(m => m.Get("default"))
                             .Returns(validatorMock.Object);

        var container = new DynamicCorsPoliciesContainer(
            Options.Create(_options),
            _validatorContainerMock.Object);

        var context = new DefaultHttpContext();
        var result = await container.GetPolicyAsync(context, null);

        Assert.AreEqual(policy, result);
    }

    #endregion GetPolicyAsync 测试
}
