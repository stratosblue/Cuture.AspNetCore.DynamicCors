// 代码由 AI 自动生成

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cuture.AspNetCore.DynamicCors.Test.Extensions;

[TestClass]
public sealed class DynamicCorsDIExtensionsTest
{
    #region Private 字段

    private IServiceCollection _services = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _services = new ServiceCollection();
    }

    #endregion 测试初始化

    #region AddDynamicCors 测试

    [TestMethod]
    public void Should_Add_DynamicCors_With_Configure_Action()
    {
        var result = _services.AddDynamicCors(builder =>
        {
            builder.AddDefaultPolicy(policy => policy.WithHeaders("*"));
        });

        Assert.IsNotNull(result);
        Assert.AreEqual(_services, result);

        var serviceProvider = _services.BuildServiceProvider();
        Assert.IsNotNull(serviceProvider.GetService<DynamicCorsPoliciesContainer>());
    }

    [TestMethod]
    public void Should_Add_DynamicCors_With_Default_Configuration()
    {
        var result = _services.AddDynamicCors();

        Assert.IsNotNull(result);
        Assert.AreEqual(_services, result);

        var serviceProvider = _services.BuildServiceProvider();

        // 验证核心服务是否已注册
        Assert.IsNotNull(serviceProvider.GetService<DynamicCorsPoliciesContainer>());
        Assert.IsNotNull(serviceProvider.GetService<IDomainNameCollectionContainer>());
    }

    [TestMethod]
    public void Should_Not_Throw_Exception_When_Configure_Action_Is_Null()
    {
        _services.AddDynamicCors((Action<DynamicCorsBuilder>?)null);
    }

    [TestMethod]
    public void Should_Throw_Exception_When_Services_Is_Null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            ((IServiceCollection)null!).AddDynamicCors();
        });
    }

    #endregion AddDynamicCors 测试

    #region 服务注册验证

    [TestMethod]
    public void Should_Register_CorsPolicyProvider_As_Transient()
    {
        _services.AddDynamicCors();

        var serviceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ICorsPolicyProvider));
        Assert.IsNotNull(serviceDescriptor);
        Assert.AreEqual(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
        Assert.AreEqual(typeof(DynamicCorsPolicyProvider), serviceDescriptor.ImplementationType);
    }

    [TestMethod]
    public void Should_Register_DefaultDomainNameCollectionContainer_As_Singleton()
    {
        _services.AddDynamicCors();

        var serviceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IDomainNameCollectionContainer));
        Assert.IsNotNull(serviceDescriptor);
        Assert.AreEqual(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [TestMethod]
    public void Should_Register_DomainNameCollectionContainer_As_Singleton()
    {
        _services.AddDynamicCors();

        var serviceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IDomainNameCollectionContainer));
        Assert.IsNotNull(serviceDescriptor);
        Assert.AreEqual(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
        Assert.AreEqual(typeof(DefaultDomainNameCollectionContainer), serviceDescriptor.ImplementationType);
    }

    [TestMethod]
    public void Should_Register_DynamicCorsPoliciesContainer_As_Singleton()
    {
        _services.AddDynamicCors();

        var serviceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(DynamicCorsPoliciesContainer));
        Assert.IsNotNull(serviceDescriptor);
        Assert.AreEqual(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [TestMethod]
    public void Should_Register_OriginCorsValidator_As_Singleton()
    {
        _services.AddDynamicCors();

        var serviceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IOriginCorsValidatorContainer));
        Assert.IsNotNull(serviceDescriptor);
        Assert.AreEqual(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
        Assert.AreEqual(typeof(DefaultOriginCorsValidatorContainer), serviceDescriptor.ImplementationType);
    }

    #endregion 服务注册验证

    #region 服务替换验证

    [TestMethod]
    public void Should_Not_Replace_When_No_Existing_Provider()
    {
        _services.AddDynamicCors();

        var serviceProvider = _services.BuildServiceProvider();
        var provider = serviceProvider.GetService<ICorsPolicyProvider>();

        Assert.IsInstanceOfType<DynamicCorsPolicyProvider>(provider);
    }

    [TestMethod]
    public void Should_Replace_Existing_CorsPolicyProvider()
    {
        // 先添加一个默认的
        _services.AddSingleton<ICorsPolicyProvider, DefaultCorsPolicyProvider>();

        _services.AddDynamicCors();

        var serviceProvider = _services.BuildServiceProvider();
        var provider = serviceProvider.GetService<ICorsPolicyProvider>();

        Assert.IsInstanceOfType<DynamicCorsPolicyProvider>(provider);
    }

    #endregion 服务替换验证

    #region OptionsSetupAction 验证

    [TestMethod]
    public void Should_Not_Register_OptionsSetupAction_When_No_Policies()
    {
        _services.AddDynamicCors();

        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<DynamicCorsOptions>>();

        Assert.IsNotNull(options);
        Assert.IsNotNull(options.Value);
    }

    [TestMethod]
    public void Should_Register_OptionsSetupAction_For_Each_Policy()
    {
        _services.AddDynamicCors(builder =>
        {
            builder.AddDefaultPolicy(policy => policy.WithHeaders("*"));
            builder.AddPolicy("api-policy", policy => policy.WithHeaders("*"));
        });

        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<DynamicCorsOptions>>();

        Assert.IsNotNull(options);
        Assert.IsNotNull(options.Value);
    }

    [TestMethod]
    public void Should_Register_OptionsSetupAction_For_Single_Policy()
    {
        _services.AddDynamicCors(builder =>
        {
            builder.AddDefaultPolicy(policy => policy.WithHeaders("*"));
        });

        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<DynamicCorsOptions>>();

        Assert.IsNotNull(options);
        Assert.IsNotNull(options.Value);
    }

    #endregion OptionsSetupAction 验证

    #region 复杂场景测试

    [TestMethod]
    public void Should_Handle_Complex_Policy_Configuration()
    {
        _services.AddDynamicCors(builder =>
        {
            builder.AddDefaultPolicy(policy =>
            {
                policy.WithHeaders("*")
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials()
                     .WithExposedHeaders("X-Custom-Header")
                     .SetPreflightMaxAge(TimeSpan.FromMinutes(30));
            });

            builder.AddPolicy("api-policy", policy =>
            {
                policy.WithHeaders("*")
                     .WithMethods("GET", "POST")
                     .WithHeaders("Content-Type", "Authorization")
                     .AllowCredentials();
            });

            builder.AddPolicy("web-policy", policy =>
            {
                policy.WithHeaders("*")
                     .AllowAnyMethod()
                     .WithHeaders("Content-Type")
                     .AllowCredentials();
            });
        });

        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<DynamicCorsOptions>>();

        Assert.IsNotNull(options);
        Assert.IsNotNull(options.Value);
    }

    [TestMethod]
    public void Should_Handle_Empty_Policy_Configuration()
    {
        _services.AddDynamicCors(builder =>
        {
            builder.AddDefaultPolicy(policy => { });
            builder.AddPolicy("empty-policy", policy => { });
        });

        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<DynamicCorsOptions>>();

        Assert.IsNotNull(options);
        Assert.IsNotNull(options.Value);
    }

    [TestMethod]
    public void Should_Handle_Multiple_Policy_Registrations()
    {
        _services.AddDynamicCors(builder =>
        {
            builder.AddDefaultPolicy(policy => policy.WithHeaders("*"));

            for (int i = 0; i < 10; i++)
            {
                builder.AddPolicy($"policy-{i}", policy => policy.WithOrigins($"http://example{i}.com"));
            }
        });

        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<DynamicCorsOptions>>();

        Assert.IsNotNull(options);
        Assert.IsNotNull(options.Value);
    }

    #endregion 复杂场景测试

    #region 边界条件测试

    [TestMethod]
    public void Should_Handle_Large_Number_Of_Policies()
    {
        _services.AddDynamicCors(builder =>
        {
            for (int i = 0; i < 100; i++)
            {
                builder.AddPolicy($"policy-{i}", policy => policy.WithOrigins($"http://example{i}.com"));
            }
        });

        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<DynamicCorsOptions>>();

        Assert.IsNotNull(options);
        Assert.IsNotNull(options.Value);
    }

    [TestMethod]
    public void Should_Handle_Null_ServiceCollection_In_Chain()
    {
        var services = new ServiceCollection();
        var result = services.AddDynamicCors();

        Assert.IsNotNull(result);
        Assert.AreEqual(services, result);
    }

    [TestMethod]
    public void Should_Handle_Special_Policy_Names()
    {
        var specialNames = new[]
        {
            "default",
            "api",
            "web",
            "mobile",
            "admin",
            "policy-with-dashes",
            "policy_with_underscores",
            "policy.with.dots",
            "123policy",
            "UPPERCASE",
            "mixedCase_123"
        };

        _services.AddDynamicCors(builder =>
        {
            foreach (var name in specialNames)
            {
                builder.AddPolicy(name, policy => policy.WithHeaders("*"));
            }
        });

        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<DynamicCorsOptions>>();

        Assert.IsNotNull(options);
        Assert.IsNotNull(options.Value);
    }

    #endregion 边界条件测试

    #region 性能测试

    [TestMethod]
    public void Should_Handle_Large_Number_Of_Policies_Performance()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        _services.AddDynamicCors(builder =>
        {
            for (int i = 0; i < 1000; i++)
            {
                builder.AddPolicy($"policy-{i}", policy => policy.WithOrigins($"http://example{i}.com"));
            }
        });

        stopwatch.Stop();

        Assert.IsLessThan(1000, stopwatch.ElapsedMilliseconds); // 应该在一秒内完成
    }

    [TestMethod]
    public void Should_Perform_Fast_Registration()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        _services.AddDynamicCors();

        stopwatch.Stop();

        Assert.IsLessThan(100, stopwatch.ElapsedMilliseconds); // 应该在100毫秒内完成
    }

    #endregion 性能测试
}
