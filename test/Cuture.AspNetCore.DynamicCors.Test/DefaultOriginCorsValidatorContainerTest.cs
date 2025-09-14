// 代码由 AI 自动生成

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using Moq;

namespace Cuture.AspNetCore.DynamicCors.Test;

[TestClass]
public sealed class DefaultOriginCorsValidatorContainerTest
{
    #region Private 字段

    private Mock<IDomainNameCollectionContainer> _domainNameCollectionContainerMock = null!;

    private DynamicCorsOptions _options = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _domainNameCollectionContainerMock = new Mock<IDomainNameCollectionContainer>();
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

        var domainCollectionMock = new Mock<IDomainNameCollection>();
        _domainNameCollectionContainerMock.Setup(m => m.Get("test-policy"))
                                       .Returns(domainCollectionMock.Object);

        var container = new DefaultOriginCorsValidatorContainer(
            _domainNameCollectionContainerMock.Object,
            Options.Create(_options));

        Assert.IsNotNull(container);
    }

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_DomainNameCollectionContainer_Is_Null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            new DefaultOriginCorsValidatorContainer(
                null!,
                Options.Create(_options)));
    }

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_Options_Is_Null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            new DefaultOriginCorsValidatorContainer(
                _domainNameCollectionContainerMock.Object,
                null!));
    }

    [TestMethod]
    public void Should_Throw_InvalidOperationException_When_DomainCollection_Not_Found()
    {
        var policy = new CorsPolicy();
        _options.PolicyMap["test-policy"] = (policy, Task.FromResult(policy));
        _options.DefaultPolicyName = "test-policy";

        _domainNameCollectionContainerMock.Setup(m => m.Get("test-policy"))
                                       .Returns((IDomainNameCollection?)null);

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            new DefaultOriginCorsValidatorContainer(
                _domainNameCollectionContainerMock.Object,
                Options.Create(_options)));
    }

    #endregion 构造函数测试

    #region Get 测试

    [TestMethod]
    public void Should_Return_Null_When_Policy_Not_Exists()
    {
        var policy = new CorsPolicy();
        _options.PolicyMap["test-policy"] = (policy, Task.FromResult(policy));
        _options.DefaultPolicyName = "test-policy";

        var domainCollectionMock = new Mock<IDomainNameCollection>();
        _domainNameCollectionContainerMock.Setup(m => m.Get("test-policy"))
                                       .Returns(domainCollectionMock.Object);

        var container = new DefaultOriginCorsValidatorContainer(
            _domainNameCollectionContainerMock.Object,
            Options.Create(_options));

        var result = container.Get("non-existent-policy");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Should_Return_Validator_When_Policy_Exists()
    {
        var policy = new CorsPolicy();
        _options.PolicyMap["test-policy"] = (policy, Task.FromResult(policy));
        _options.DefaultPolicyName = "test-policy";

        var domainCollectionMock = new Mock<IDomainNameCollection>();
        _domainNameCollectionContainerMock.Setup(m => m.Get("test-policy"))
                                       .Returns(domainCollectionMock.Object);

        var container = new DefaultOriginCorsValidatorContainer(
            _domainNameCollectionContainerMock.Object,
            Options.Create(_options));

        var result = container.Get("test-policy");

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<DefaultOriginCorsValidator>(result);
    }

    [TestMethod]
    public void Should_Use_Default_Policy_Name_When_PolicyName_Is_Null()
    {
        var policy = new CorsPolicy();
        _options.PolicyMap["default"] = (policy, Task.FromResult(policy));
        _options.DefaultPolicyName = "default";

        var domainCollectionMock = new Mock<IDomainNameCollection>();
        _domainNameCollectionContainerMock.Setup(m => m.Get("default"))
                                       .Returns(domainCollectionMock.Object);

        var container = new DefaultOriginCorsValidatorContainer(
            _domainNameCollectionContainerMock.Object,
            Options.Create(_options));

        var result = container.Get("default");

        Assert.IsNotNull(result);
    }

    #endregion Get 测试
}
