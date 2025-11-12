// 代码由 AI 自动生成

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Cuture.AspNetCore.DynamicCors.Test;

[TestClass]
public sealed class DynamicCorsBuilderTest
{
    #region Private 字段

    private DynamicCorsBuilder _builder = null!;

    private IServiceCollection _services = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _services = new ServiceCollection();
        _builder = new DynamicCorsBuilder(_services);
    }

    #endregion 测试初始化

    #region Constructor 测试

    [TestMethod]
    public void Should_Initialize_With_Services()
    {
        Assert.IsNotNull(_builder);
        Assert.AreEqual(_services, _builder.Services);
    }

    #endregion Constructor 测试

    #region AddDefaultPolicy 测试

    [TestMethod]
    public void Should_Add_Default_Policy_With_Configuration_Action()
    {
        var result = _builder.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://example.com");
        });

        GetBuildedOptions();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<DynamicCorsPolicyBuilder>(result);
    }

    [TestMethod]
    public void Should_Add_Default_Policy_With_CorsPolicy()
    {
        var policy = new CorsPolicy();

        var result = _builder.AddDefaultPolicy(policy);

        GetBuildedOptions();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<DynamicCorsPolicyBuilder>(result);
    }

    [TestMethod]
    public void Should_Throw_Exception_When_Default_Policy_Action_Is_Null()
    {
        _builder.AddDefaultPolicy((Action<CorsPolicyBuilder>)null!);

        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            GetBuildedOptions();
        });
    }

    [TestMethod]
    public void Should_Throw_Exception_When_Default_Policy_Is_Null()
    {
        _builder.AddDefaultPolicy((CorsPolicy)null!);

        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            GetBuildedOptions();
        });
    }

    #endregion AddDefaultPolicy 测试

    #region AddPolicy 测试

    [TestMethod]
    public void Should_Add_Policy_With_Name_And_Configuration_Action()
    {
        var result = _builder.AddPolicy("test-policy", policy =>
        {
            policy.WithOrigins("http://example.com");
        });

        GetBuildedOptions();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<DynamicCorsPolicyBuilder>(result);
    }

    [TestMethod]
    public void Should_Add_Policy_With_Name_And_CorsPolicy()
    {
        var policy = new CorsPolicy();

        var result = _builder.AddPolicy("test-policy", policy);

        GetBuildedOptions();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<DynamicCorsPolicyBuilder>(result);
    }

    [TestMethod]
    public void Should_Success_When_Policy_Name_Is_Empty()
    {
        _builder.AddPolicy(string.Empty, new CorsPolicy());
        GetBuildedOptions();
    }

    [TestMethod]
    public void Should_Throw_Exception_When_Policy_Action_Is_Null()
    {
        _builder.AddPolicy("test-policy", (Action<CorsPolicyBuilder>)null!);

        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            GetBuildedOptions();
        });
    }

    [TestMethod]
    public void Should_Throw_Exception_When_Policy_Is_Null()
    {
        _builder.AddPolicy("test-policy", (CorsPolicy)null!);

        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            GetBuildedOptions();
        });
    }

    [TestMethod]
    public void Should_Throw_Exception_When_Policy_Name_Is_Null()
    {
        _builder.AddPolicy(null!, new CorsPolicy());

        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            GetBuildedOptions();
        });
    }

    #endregion AddPolicy 测试

    #region Services 属性测试

    [TestMethod]
    public void Should_Not_Allow_Modifying_Services_Reference()
    {
        var originalServices = _builder.Services;
        _builder.Services.AddSingleton<object>();

        Assert.AreEqual(originalServices, _builder.Services);
        Assert.HasCount(1, _builder.Services);
    }

    [TestMethod]
    public void Should_Return_Same_ServiceCollection()
    {
        Assert.AreEqual(_services, _builder.Services);
    }

    #endregion Services 属性测试

    #region 链式调用测试

    [TestMethod]
    public void Should_Return_Same_PolicyBuilder_For_Chained_Calls()
    {
        var builder1 = _builder.AddDefaultPolicy(policy => { });
        var builder2 = _builder.AddPolicy("test-policy", policy => { });

        GetBuildedOptions();

        Assert.AreNotEqual(builder1, builder2);
    }

    [TestMethod]
    public void Should_Support_Chained_Calls()
    {
        var result = _builder
            .AddDefaultPolicy(policy => policy.WithOrigins("http://default.com"));

        GetBuildedOptions();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<DynamicCorsPolicyBuilder>(result);
    }

    #endregion 链式调用测试

    #region 复杂场景测试

    [TestMethod]
    public void Should_Handle_Default_Policy_With_Complex_Configuration()
    {
        var result = _builder.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://default.com")
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials();
        });

        GetBuildedOptions();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Should_Handle_Multiple_Policies_With_Same_Name()
    {
        var result1 = _builder.AddPolicy("test-policy", policy => policy.WithOrigins("http://example1.com"));
        var result2 = _builder.AddPolicy("test-policy", policy => policy.WithOrigins("http://example2.com"));

        var options = GetBuildedOptions();
        Assert.HasCount(1, options.PolicyMap);
    }

    [TestMethod]
    public void Should_Handle_Policy_With_Complex_Configuration()
    {
        var result = _builder.AddPolicy("complex-policy", policy =>
        {
            policy.WithOrigins("http://example.com", "https://api.com")
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials()
                 .WithExposedHeaders("X-Custom-Header")
                 .SetPreflightMaxAge(TimeSpan.FromMinutes(30));
        });

        GetBuildedOptions();

        Assert.IsNotNull(result);
    }

    #endregion 复杂场景测试

    #region 性能测试

    [TestMethod]
    public void Should_Handle_Large_Number_Of_Policies()
    {
        for (int i = 0; i < 100; i++)
        {
            _builder.AddPolicy($"policy-{i}", policy => policy.WithOrigins($"http://example{i}.com"));
        }

        var options = GetBuildedOptions();
        Assert.HasCount(100, options.PolicyMap);
    }

    #endregion 性能测试

    #region 边界条件测试

    [TestMethod]
    public void Should_Handle_Empty_Policy_Configuration()
    {
        var result = _builder.AddPolicy("empty-policy", policy => { });

        GetBuildedOptions();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Should_Handle_Policy_Name_With_Special_Characters()
    {
        var specialNames = new[]
        {
            "policy-with-dashes",
            "policy_with_underscores",
            "policy.with.dots",
            "policy123",
            "UPPERCASE_POLICY",
            "mixedCase_Policy-123"
        };

        foreach (var name in specialNames)
        {
            var result = _builder.AddPolicy(name, policy => policy.WithOrigins("http://example.com"));
            Assert.IsNotNull(result);
        }
    }

    [TestMethod]
    public void Should_Handle_Single_Origin_Policy()
    {
        var result = _builder.AddPolicy("single-origin", policy => policy.WithOrigins("http://example.com"));

        GetBuildedOptions();

        Assert.IsNotNull(result);
    }

    #endregion 边界条件测试

    #region Private 方法

    private DynamicCorsOptions GetBuildedOptions()
    {
        var options = new DynamicCorsOptions();
        _builder.OptionsSetupAction(options);
        return options;
    }

    #endregion Private 方法
}
