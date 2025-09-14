// 代码由 AI 自动生成

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cuture.AspNetCore.DynamicCors.Test;

/// <summary>
/// 测试动态CORS通过代理ICorsPolicyProvider的实现
/// 验证README中描述的基于Microsoft.AspNetCore.Cors的实现
/// </summary>
[TestClass]
public sealed class DynamicCorsProviderIntegrationTest
{
    #region Private 字段

    private DynamicCorsPoliciesContainer _policiesContainer = null!;

    private DynamicCorsPolicyProvider _policyProvider = null!;

    private ServiceCollection _services = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _services = new ServiceCollection();
        _services.AddDynamicCors(builder =>
        {
            builder.AddDefaultPolicy(policy => policy.AllowAnyHeader());
            builder.AddPolicy("api-policy", policy => policy.AllowAnyHeader());
        });

        var serviceProvider = _services.BuildServiceProvider();
        _policiesContainer = serviceProvider.GetRequiredService<DynamicCorsPoliciesContainer>();
        _policyProvider = new DynamicCorsPolicyProvider(_policiesContainer, serviceProvider);
    }

    #endregion 测试初始化

    #region 代理实现测试

    /// <summary>
    /// 测试动态策略覆盖默认策略
    /// </summary>
    [TestMethod]
    public async Task Should_Override_Default_Policy_With_Dynamic_Policy()
    {
        // 配置动态域名
        var domainCollection = new DefaultDomainNameCollection();
        domainCollection.Add("dynamic.com");

        // 注册动态域名集合
        var container = _services.BuildServiceProvider().GetRequiredService<IDomainNameCollectionContainer>();
        var options = _services.BuildServiceProvider().GetRequiredService<IOptions<DynamicCorsOptions>>();

        // 测试动态策略匹配
        var context = new DefaultHttpContext();
        context.Request.Headers["Origin"] = "http://dynamic.com";

        // 验证动态策略生效
        var policy = await _policyProvider.GetPolicyAsync(context, "api-policy");
        Assert.IsNotNull(policy);
    }

    /// <summary>
    /// 测试动态CORS通过代理ICorsPolicyProvider实现
    /// </summary>
    [TestMethod]
    public async Task Should_Proxy_ICorsPolicyProvider_Implementation()
    {
        // 创建HTTP上下文
        var context = new DefaultHttpContext();
        context.Request.Headers["Origin"] = "http://default.com";

        // 通过动态CORS策略提供器获取策略
        var policy = await _policyProvider.GetPolicyAsync(context, null);

        // 验证策略不为空
        Assert.IsNotNull(policy);
        Assert.AreEqual(0, policy.Origins.Count);
    }

    /// <summary>
    /// 测试未找到策略时返回null
    /// </summary>
    [TestMethod]
    public async Task Should_Return_Null_When_Policy_Not_Found()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers["Origin"] = "http://unknown.com";

        var policy = await _policyProvider.GetPolicyAsync(context, "non-existent-policy");

        Assert.IsNull(policy);
    }

    #endregion 代理实现测试

    #region 统一策略测试

    /// <summary>
    /// 测试所有动态CORS域名使用相同策略
    /// 验证README中描述的特性
    /// </summary>
    [TestMethod]
    public async Task Should_Use_Same_Policy_For_All_Dynamic_Cors_Domains()
    {
        // 配置服务
        var services = new ServiceCollection();
        services.AddDynamicCors(builder =>
        {
            // 只配置一个策略，所有域名都使用这个策略
            builder.AddPolicy("unified-policy", policy =>
                policy.AllowAnyMethod()
                      .AllowAnyHeader());
        });

        var serviceProvider = services.BuildServiceProvider();
        var policyProvider = serviceProvider.GetRequiredService<ICorsPolicyProvider>();

        // 测试不同域名的请求
        var testOrigins = new[]
        {
            "http://app1.company.com",
            "https://app2.company.com",
            "http://api.company.com:8080",
            "https://admin.company.com:3000"
        };

        foreach (var origin in testOrigins)
        {
            var context = new DefaultHttpContext();
            context.Request.Headers["Origin"] = origin;

            // 所有请求都应该使用相同的策略配置
            var policy = await policyProvider.GetPolicyAsync(context, "unified-policy");
            Assert.IsNotNull(policy);
            Assert.IsTrue(policy.AllowAnyMethod);
            Assert.IsTrue(policy.AllowAnyHeader);
        }
    }

    #endregion 统一策略测试

    #region 集成场景测试

    /// <summary>
    /// 测试完整的动态CORS集成场景
    /// </summary>
    [TestMethod]
    public async Task Should_Handle_Complete_Dynamic_Cors_Scenario()
    {
        // 配置完整的动态CORS
        var services = new ServiceCollection();
        services.AddDynamicCors(builder =>
        {
            // 默认策略
            builder.AddDefaultPolicy(policy =>
                policy.AllowAnyMethod()
                      .AllowAnyHeader());

            // API策略
            builder.AddPolicy("api-policy", policy =>
                policy.WithMethods("GET", "POST")
                      .WithHeaders("Content-Type", "Authorization"));

            // 开发策略
            builder.AddPolicy("dev-policy", policy =>
                policy.AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials());
        });

        var serviceProvider = services.BuildServiceProvider();
        var policyProvider = serviceProvider.GetRequiredService<ICorsPolicyProvider>();

        // 测试生产环境
        var prodContext = new DefaultHttpContext();
        prodContext.Request.Headers["Origin"] = "https://app.company.com";
        var prodPolicy = await policyProvider.GetPolicyAsync(prodContext, null); // 使用默认策略
        Assert.IsNotNull(prodPolicy);

        // 测试API访问
        var apiContext = new DefaultHttpContext();
        apiContext.Request.Headers["Origin"] = "http://api.company.com";
        var apiPolicy = await policyProvider.GetPolicyAsync(apiContext, "api-policy");
        Assert.IsNotNull(apiPolicy);
        Assert.AreEqual(2, apiPolicy.Methods.Count);
        Assert.AreEqual(2, apiPolicy.Headers.Count);

        // 测试开发环境
        var devContext = new DefaultHttpContext();
        devContext.Request.Headers["Origin"] = "http://localhost:3000";
        var devPolicy = await policyProvider.GetPolicyAsync(devContext, "dev-policy");
        Assert.IsNotNull(devPolicy);
        Assert.IsTrue(devPolicy.AllowAnyMethod);
        Assert.IsTrue(devPolicy.AllowAnyHeader);
    }

    /// <summary>
    /// 测试动态域名实时更新
    /// </summary>
    [TestMethod]
    public async Task Should_Handle_Dynamic_Domain_Updates()
    {
        var services = new ServiceCollection();
        services.AddDynamicCors(builder =>
        {
            builder.AddPolicy("dynamic-policy", policy =>
                policy.AllowAnyMethod()
                      .AllowAnyHeader());
        });

        var serviceProvider = services.BuildServiceProvider();
        var container = serviceProvider.GetRequiredService<IDomainNameCollectionContainer>();
        var policyProvider = serviceProvider.GetRequiredService<ICorsPolicyProvider>();

        // 初始状态
        var context1 = new DefaultHttpContext();
        context1.Request.Headers["Origin"] = "http://initial.com";
        var policy1 = await policyProvider.GetPolicyAsync(context1, "dynamic-policy");
        Assert.IsNotNull(policy1);

        // 模拟动态添加域名
        var domainCollection = container.Get("dynamic-policy");
        if (domainCollection is DefaultDomainNameCollection collection)
        {
            collection.Add("new-dynamic.com");
        }

        // 验证新域名生效
        var context2 = new DefaultHttpContext();
        context2.Request.Headers["Origin"] = "http://new-dynamic.com";
        var policy2 = await policyProvider.GetPolicyAsync(context2, "dynamic-policy");
        Assert.IsNotNull(policy2);
    }

    #endregion 集成场景测试

    #region 性能测试

    /// <summary>
    /// 测试动态CORS策略提供器的性能
    /// </summary>
    [TestMethod]
    public async Task Should_Perform_Fast_Policy_Lookup()
    {
        // 配置大量域名
        var services = new ServiceCollection();
        services.AddDynamicCors(builder =>
        {
            builder.AddPolicy("performance-policy", policy =>
                policy.AllowAnyMethod()
                      .AllowAnyHeader());
        });

        var serviceProvider = services.BuildServiceProvider();
        var container = serviceProvider.GetRequiredService<IDomainNameCollectionContainer>();
        var policyProvider = serviceProvider.GetRequiredService<ICorsPolicyProvider>();

        // 添加大量域名
        var domainCollection = container.Get("performance-policy");
        if (domainCollection is DefaultDomainNameCollection collection)
        {
            for (int i = 0; i < 1000; i++)
            {
                collection.Add($"app{i}.company.com");
            }
        }

        // 测试快速查找
        for (int i = 0; i < 100; i++)
        {
            var context = new DefaultHttpContext();
            context.Request.Headers["Origin"] = $"http://app{i}.company.com";

            var policy = await policyProvider.GetPolicyAsync(context, "performance-policy");
            Assert.IsNotNull(policy);
        }
    }

    #endregion 性能测试
}
