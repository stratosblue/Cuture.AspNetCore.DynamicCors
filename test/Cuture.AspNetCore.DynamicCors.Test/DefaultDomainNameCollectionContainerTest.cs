// 代码由 AI 自动生成

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace Cuture.AspNetCore.DynamicCors.Test;

[TestClass]
public sealed class DefaultDomainNameCollectionContainerTest
{
    #region Private 字段

    private DynamicCorsOptions _options = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
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

        var container = new DefaultDomainNameCollectionContainer(
            Options.Create(_options));

        Assert.IsNotNull(container);
    }

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_Options_Is_Null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new DefaultDomainNameCollectionContainer(null!));
    }

    #endregion 构造函数测试

    #region Get 测试

    [TestMethod]
    public void Should_Return_DomainNameCollection_For_Multiple_Policies()
    {
        var policy1 = new CorsPolicy();
        var policy2 = new CorsPolicy();

        _options.PolicyMap["policy1"] = (policy1, Task.FromResult(policy1));
        _options.PolicyMap["policy2"] = (policy2, Task.FromResult(policy2));
        _options.DefaultPolicyName = "policy1";

        var container = new DefaultDomainNameCollectionContainer(
            Options.Create(_options));

        var result1 = container.Get("policy1");
        var result2 = container.Get("policy2");

        Assert.IsNotNull(result1);
        Assert.IsNotNull(result2);
        Assert.AreNotEqual(result1, result2);
    }

    [TestMethod]
    public void Should_Return_DomainNameCollection_When_Policy_Exists()
    {
        var policy = new CorsPolicy();
        _options.PolicyMap["test-policy"] = (policy, Task.FromResult(policy));
        _options.DefaultPolicyName = "test-policy";

        var container = new DefaultDomainNameCollectionContainer(
            Options.Create(_options));

        var result = container.Get("test-policy");

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<DefaultDomainNameCollection>(result);
    }

    [TestMethod]
    public void Should_Return_Null_When_Policy_Not_Exists()
    {
        var policy = new CorsPolicy();
        _options.PolicyMap["test-policy"] = (policy, Task.FromResult(policy));
        _options.DefaultPolicyName = "test-policy";

        var container = new DefaultDomainNameCollectionContainer(
            Options.Create(_options));

        var result = container.Get("non-existent-policy");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Should_Use_Default_Policy_Name_When_PolicyName_Is_Null()
    {
        var policy = new CorsPolicy();
        _options.PolicyMap["default"] = (policy, Task.FromResult(policy));
        _options.DefaultPolicyName = "default";

        var container = new DefaultDomainNameCollectionContainer(
            Options.Create(_options));

        var result = container.Get("default");

        Assert.IsNotNull(result);
    }

    #endregion Get 测试
}
