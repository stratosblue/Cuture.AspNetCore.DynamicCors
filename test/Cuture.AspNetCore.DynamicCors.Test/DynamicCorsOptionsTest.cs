// 代码由 AI 自动生成

using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Cuture.AspNetCore.DynamicCors.Test;

[TestClass]
public sealed class DynamicCorsOptionsTest
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

    #region DefaultPolicyName 测试

    [TestMethod]
    public void Should_Get_Default_DefaultPolicyName()
    {
        Assert.AreEqual("__DefaultCorsPolicy", _options.DefaultPolicyName);
    }

    [TestMethod]
    public void Should_Set_DefaultPolicyName_Success()
    {
        var newName = "custom-policy-name";
        _options.DefaultPolicyName = newName;
        Assert.AreEqual(newName, _options.DefaultPolicyName);
    }

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_Set_DefaultPolicyName_To_Null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            _options.DefaultPolicyName = null!;
        });
    }

    [TestMethod]
    public void Should_Throw_InvalidOperationException_When_Change_DefaultPolicyName_After_Adding_DefaultPolicy()
    {
        var policy = new CorsPolicy();
        _options.AddDefaultPolicy(policy);

        Assert.ThrowsExactly<InvalidOperationException>(() =>
        {
            _options.DefaultPolicyName = "changed-name";
        });
    }

    #endregion DefaultPolicyName 测试

    #region AddDefaultPolicy 测试

    [TestMethod]
    public void Should_Add_DefaultPolicy_With_Configuration_Action()
    {
        _options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("http://example.com");
        });

        Assert.AreEqual(1, _options.PolicyMap.Count);
        Assert.IsTrue(_options.PolicyMap.ContainsKey(_options.DefaultPolicyName));
    }

    [TestMethod]
    public void Should_Add_DefaultPolicy_With_Policy_Object()
    {
        var policy = new CorsPolicy();
        _options.AddDefaultPolicy(policy);

        Assert.AreEqual(1, _options.PolicyMap.Count);
        Assert.IsTrue(_options.PolicyMap.ContainsKey(_options.DefaultPolicyName));
    }

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_AddDefaultPolicy_With_Null_Action()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            _options.AddDefaultPolicy((Action<CorsPolicyBuilder>)null!);
        });
    }

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_AddDefaultPolicy_With_Null_Policy()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            _options.AddDefaultPolicy((CorsPolicy)null!);
        });
    }

    #endregion AddDefaultPolicy 测试

    #region AddPolicy 测试

    [TestMethod]
    public void Should_Add_Policy_With_Name_And_Configuration_Action()
    {
        var policyName = "test-policy";
        _options.AddPolicy(policyName, builder =>
        {
            builder.WithOrigins("http://example.com");
        });

        Assert.AreEqual(1, _options.PolicyMap.Count);
        Assert.IsTrue(_options.PolicyMap.ContainsKey(policyName));
    }

    [TestMethod]
    public void Should_Add_Policy_With_Name_And_Policy_Object()
    {
        var policyName = "test-policy";
        var policy = new CorsPolicy();
        _options.AddPolicy(policyName, policy);

        Assert.AreEqual(1, _options.PolicyMap.Count);
        Assert.IsTrue(_options.PolicyMap.ContainsKey(policyName));
    }

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_AddPolicy_With_Null_Action()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            _options.AddPolicy("test-policy", (Action<CorsPolicyBuilder>)null!);
        });
    }

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_AddPolicy_With_Null_Name()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            _options.AddPolicy(null!, new CorsPolicy());
        });
    }

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_AddPolicy_With_Null_Name_And_Action()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            _options.AddPolicy(null!, (Action<CorsPolicyBuilder>)null!);
        });
    }

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_AddPolicy_With_Null_Policy()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            _options.AddPolicy("test-policy", (CorsPolicy)null!);
        });
    }

    #endregion AddPolicy 测试

    #region PolicyMap 测试

    [TestMethod]
    public void Should_Have_Empty_PolicyMap_Initially()
    {
        Assert.AreEqual(0, _options.PolicyMap.Count);
    }

    [TestMethod]
    public void Should_Override_Existing_Policy_When_Adding_Same_Name()
    {
        var policy1 = new CorsPolicy();
        var policy2 = new CorsPolicy();
        var policyName = "test-policy";

        _options.AddPolicy(policyName, policy1);
        Assert.AreEqual(1, _options.PolicyMap.Count);
        Assert.AreEqual(policy1, _options.PolicyMap[policyName].policy);

        _options.AddPolicy(policyName, policy2);
        Assert.AreEqual(1, _options.PolicyMap.Count);
        Assert.AreEqual(policy2, _options.PolicyMap[policyName].policy);
    }

    #endregion PolicyMap 测试
}
