// 代码由 AI 自动生成

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace Cuture.AspNetCore.DynamicCors.Test.Extensions;

[TestClass]
public sealed class DynamicCorsBuilderExtensionsTest
{
    #region Private 字段

    private DynamicCorsBuilder _builder = null!;

    private IServiceCollection _services = null!;

    #endregion Private 字段

    #region 测试类

    public class TestOptions
    {
        #region Public 属性

        public string[] Origins { get; set; } = Array.Empty<string>();

        public string SingleOrigin { get; set; } = string.Empty;

        #endregion Public 属性
    }

    #endregion 测试类

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _services = new ServiceCollection();
        _builder = new DynamicCorsBuilder(_services);
    }

    #endregion 测试初始化

    #region SyncAllowedOriginsFromConfiguration 测试

    [TestMethod]
    public void Should_Sync_Allowed_Origins_From_Configuration_With_Default_Policy()
    {
        var configurationMock = new Mock<IConfiguration>();

        var policyBuilder = _builder.AddDefaultPolicy(policy => policy.WithHeaders("*"));

        // 实际的方法是void返回类型，没有返回值
        policyBuilder.SyncAllowedOriginsFromConfiguration("test-key");

        _services.AddSingleton(configurationMock.Object);

        var serviceProvider = _services.BuildServiceProvider();
        var syncSources = serviceProvider.GetServices<IAllowedOriginsSyncSource>();
        Assert.AreEqual(1, syncSources.Count());
    }

    [TestMethod]
    public void Should_Sync_Allowed_Origins_From_Configuration_With_PolicyName()
    {
        var configurationMock = new Mock<IConfiguration>();

        var policyBuilder = _builder.AddPolicy("test-policy", policy => policy.WithHeaders("*"));

        // 实际的方法是void返回类型，没有返回值
        policyBuilder.SyncAllowedOriginsFromConfiguration("test-key");

        _services.AddSingleton(configurationMock.Object);

        // 验证服务是否已注册
        var serviceProvider = _services.BuildServiceProvider();
        var syncSources = serviceProvider.GetServices<IAllowedOriginsSyncSource>();
        Assert.AreEqual(1, syncSources.Count());
    }

    [TestMethod]
    public void Should_Throw_Exception_When_Key_Is_Empty_For_Configuration()
    {
        var policyBuilder = _builder.AddPolicy("test-policy", policy => policy.WithHeaders("*"));

        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            policyBuilder.SyncAllowedOriginsFromConfiguration(string.Empty);
        });
    }

    [TestMethod]
    public void Should_Throw_Exception_When_Key_Is_Null_For_Configuration()
    {
        var policyBuilder = _builder.AddPolicy("test-policy", policy => policy.WithHeaders("*"));

        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            policyBuilder.SyncAllowedOriginsFromConfiguration(null!);
        });
    }

    #endregion SyncAllowedOriginsFromConfiguration 测试

    #region SyncAllowedOriginsWithOptions 测试

    [TestMethod]
    public void Should_Sync_Allowed_Origins_With_Options_With_Default_Policy()
    {
        var policyBuilder = _builder.AddDefaultPolicy(policy => policy.WithHeaders("*"));

        // 实际的方法是void返回类型，没有返回值
        policyBuilder.SyncAllowedOriginsWithOptions<TestOptions>(o => o.Origins);

        _services.AddOptions<TestOptions>();

        var serviceProvider = _services.BuildServiceProvider();
        var syncSources = serviceProvider.GetServices<IAllowedOriginsSyncSource>();
        Assert.AreEqual(1, syncSources.Count());
    }

    [TestMethod]
    public void Should_Sync_Allowed_Origins_With_Options_With_PolicyName()
    {
        var policyBuilder = _builder.AddPolicy("test-policy", policy => policy.WithHeaders("*"));

        // 实际的方法是void返回类型，没有返回值
        policyBuilder.SyncAllowedOriginsWithOptions<TestOptions>(o => o.Origins);

        _services.AddOptions<TestOptions>();

        var serviceProvider = _services.BuildServiceProvider();
        var syncSources = serviceProvider.GetServices<IAllowedOriginsSyncSource>();
        Assert.AreEqual(1, syncSources.Count());
    }

    [TestMethod]
    public void Should_Throw_Exception_When_Property_Selector_Is_Null_For_Options()
    {
        var policyBuilder = _builder.AddPolicy("test-policy", policy => policy.WithHeaders("*"));

        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            policyBuilder.SyncAllowedOriginsWithOptions<TestOptions>((Func<TestOptions, string>)null!);
        });
    }

    #endregion SyncAllowedOriginsWithOptions 测试

    #region 复杂场景测试

    [TestMethod]
    public void Should_Handle_Mixed_Sources()
    {
        var configurationMock = new Mock<IConfiguration>();

        var policyBuilder1 = _builder.AddPolicy("policy1", policy => policy.WithHeaders("*"));
        var policyBuilder2 = _builder.AddPolicy("policy2", policy => policy.WithHeaders("*"));
        var defaultPolicyBuilder = _builder.AddDefaultPolicy(policy => policy.WithHeaders("*"));

        policyBuilder1.SyncAllowedOriginsFromConfiguration("key1");
        policyBuilder2.SyncAllowedOriginsWithOptions<TestOptions>(o => o.Origins);
        defaultPolicyBuilder.SyncAllowedOriginsFromConfiguration("default-key");

        _services.AddOptions<TestOptions>();
        _services.AddSingleton(configurationMock.Object);

        var serviceProvider = _services.BuildServiceProvider();
        var syncSources = serviceProvider.GetServices<IAllowedOriginsSyncSource>();
        Assert.AreEqual(3, syncSources.Count());
    }

    [TestMethod]
    public void Should_Handle_Multiple_Sources_From_Configuration()
    {
        var configurationMock = new Mock<IConfiguration>();

        var policyBuilder1 = _builder.AddPolicy("policy1", policy => policy.WithHeaders("*"));
        var policyBuilder2 = _builder.AddPolicy("policy2", policy => policy.WithHeaders("*"));
        var defaultPolicyBuilder = _builder.AddDefaultPolicy(policy => policy.WithHeaders("*"));

        policyBuilder1.SyncAllowedOriginsFromConfiguration("key1");
        policyBuilder2.SyncAllowedOriginsFromConfiguration("key2");
        defaultPolicyBuilder.SyncAllowedOriginsFromConfiguration("default-key");

        _services.AddSingleton(configurationMock.Object);

        var serviceProvider = _services.BuildServiceProvider();
        var syncSources = serviceProvider.GetServices<IAllowedOriginsSyncSource>();
        Assert.AreEqual(3, syncSources.Count());
    }

    [TestMethod]
    public void Should_Handle_Multiple_Sources_From_Options()
    {
        var configurationMock = new Mock<IConfiguration>();

        var policyBuilder1 = _builder.AddPolicy("policy1", policy => policy.WithHeaders("*"));
        var policyBuilder2 = _builder.AddPolicy("policy2", policy => policy.WithHeaders("*"));
        var defaultPolicyBuilder = _builder.AddDefaultPolicy(policy => policy.WithHeaders("*"));

        policyBuilder1.SyncAllowedOriginsWithOptions<TestOptions>(o => o.Origins);
        policyBuilder2.SyncAllowedOriginsWithOptions<TestOptions>(o => o.SingleOrigin);
        defaultPolicyBuilder.SyncAllowedOriginsWithOptions<TestOptions>(o => o.Origins);

        _services.AddOptions<TestOptions>();
        _services.AddSingleton(configurationMock.Object);

        var serviceProvider = _services.BuildServiceProvider();
        var syncSources = serviceProvider.GetServices<IAllowedOriginsSyncSource>();
        Assert.AreEqual(3, syncSources.Count());
    }

    #endregion 复杂场景测试

    #region 边界条件测试

    [TestMethod]
    public void Should_Handle_Large_Number_Of_Sources()
    {
        var configurationMock = new Mock<IConfiguration>();

        for (int i = 0; i < 100; i++)
        {
            var policyBuilder = _builder.AddPolicy($"policy-{i}", policy => policy.WithOrigins($"http://example{i}.com"));
            policyBuilder.SyncAllowedOriginsFromConfiguration($"key-{i}");
        }

        _services.AddSingleton(configurationMock.Object);

        var serviceProvider = _services.BuildServiceProvider();
        var syncSources = serviceProvider.GetServices<IAllowedOriginsSyncSource>();
        Assert.AreEqual(100, syncSources.Count());
    }

    #endregion 边界条件测试

    #region 服务注册测试

    [TestMethod]
    public void Should_Register_Services_Only_Once()
    {
        var configurationMock = new Mock<IConfiguration>();

        var policyBuilder1 = _builder.AddPolicy("policy1", policy => policy.WithHeaders("*"));
        var policyBuilder2 = _builder.AddPolicy("policy2", policy => policy.WithHeaders("*"));

        policyBuilder1.SyncAllowedOriginsFromConfiguration("key1");
        policyBuilder2.SyncAllowedOriginsWithOptions<TestOptions>(o => o.Origins);

        _services.AddOptions<TestOptions>();
        _services.AddSingleton(configurationMock.Object);

        var serviceProvider = _services.BuildServiceProvider();
        var syncSources = serviceProvider.GetServices<IAllowedOriginsSyncSource>();
        Assert.AreEqual(2, syncSources.Count());

        // 验证配置服务只注册了一次
        var configureOptions = _services.Where(s => s.ServiceType == typeof(IPostConfigureOptions<DynamicCorsOptions>));
        Assert.AreEqual(1, configureOptions.Count());
    }

    #endregion 服务注册测试

    #region 性能测试

    [TestMethod]
    public void Should_Perform_Fast_Registration()
    {
        var configurationMock = new Mock<IConfiguration>();

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        for (int i = 0; i < 1000; i++)
        {
            var policyBuilder = _builder.AddPolicy($"policy-{i}", policy => policy.WithOrigins($"http://example{i}.com"));
            policyBuilder.SyncAllowedOriginsFromConfiguration($"key-{i}");
        }

        stopwatch.Stop();

        Assert.IsTrue(stopwatch.ElapsedMilliseconds < 1000); // 应该在一秒内完成
    }

    #endregion 性能测试
}
