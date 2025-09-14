// 代码由 AI 自动生成

namespace Cuture.AspNetCore.DynamicCors.Test;

/// <summary>
/// 测试动态CORS与原始CORS的行为差异
/// 验证README中描述的功能特性
/// </summary>
[TestClass]
public sealed class CorsBehaviorDifferenceTest
{
    #region Private 字段

    private DefaultDomainNameCollection _domainCollection = null!;

    private DefaultOriginCorsValidator _validator = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _domainCollection = new DefaultDomainNameCollection();
        _validator = new DefaultOriginCorsValidator(_domainCollection);
    }

    #endregion 测试初始化

    #region 端口验证差异测试

    /// <summary>
    /// 测试动态CORS不验证端口号的特性
    /// 与原始CORS相比，动态CORS忽略端口号差异
    /// </summary>
    [TestMethod]
    public void Should_Not_Validate_Port_Numbers()
    {
        // 添加一个不带端口号的域名
        _domainCollection.Add("example.com");

        // 验证不同端口号的请求都被允许
        Assert.IsTrue(_validator.IsAllowed("http://example.com"), "80端口应该被允许");
        Assert.IsTrue(_validator.IsAllowed("http://example.com:80"), "显式80端口应该被允许");
        Assert.IsTrue(_validator.IsAllowed("http://example.com:8080"), "8080端口应该被允许");
        Assert.IsTrue(_validator.IsAllowed("http://example.com:3000"), "3000端口应该被允许");
        Assert.IsTrue(_validator.IsAllowed("https://example.com"), "443端口应该被允许");
        Assert.IsTrue(_validator.IsAllowed("https://example.com:443"), "显式443端口应该被允许");
        Assert.IsTrue(_validator.IsAllowed("https://example.com:8443"), "8443端口应该被允许");
    }

    /// <summary>
    /// 测试动态CORS不验证协议的特性
    /// 与原始CORS相比，动态CORS忽略协议差异
    /// </summary>
    [TestMethod]
    public void Should_Not_Validate_Protocol()
    {
        // 添加一个域名
        _domainCollection.Add("example.com");

        // 验证不同协议的请求都被允许
        Assert.IsTrue(_validator.IsAllowed("http://example.com"), "HTTP协议应该被允许");
        Assert.IsTrue(_validator.IsAllowed("https://example.com"), "HTTPS协议应该被允许");
        Assert.IsTrue(_validator.IsAllowed("ws://example.com"), "WebSocket协议应该被允许");
        Assert.IsTrue(_validator.IsAllowed("wss://example.com"), "安全WebSocket协议应该被允许");
    }

    /// <summary>
    /// 测试动态CORS不验证协议和端口的组合特性
    /// </summary>
    [TestMethod]
    public void Should_Not_Validate_Protocol_And_Port_Combinations()
    {
        // 添加一个域名
        _domainCollection.Add("api.example.com");

        // 验证各种协议和端口的组合都被允许
        var testCases = new[]
        {
            "http://api.example.com",
            "https://api.example.com",
            "http://api.example.com:80",
            "https://api.example.com:443",
            "http://api.example.com:8080",
            "https://api.example.com:8443",
            "ws://api.example.com:8080",
            "wss://api.example.com:9090"
        };

        foreach (var origin in testCases)
        {
            Assert.IsTrue(_validator.IsAllowed(origin), $"Origin {origin} 应该被允许");
        }
    }

    #endregion 端口验证差异测试

    #region 泛域名配置测试

    /// <summary>
    /// 测试多级泛域名配置
    /// </summary>
    [TestMethod]
    public void Should_Support_Multi_Level_Wildcard_Domain()
    {
        // 添加多级泛域名
        _domainCollection.Add("*.api.example.com");

        // 验证匹配
        Assert.IsTrue(_validator.IsAllowed("http://v1.api.example.com"), "v1.api子域名应该被允许");
        Assert.IsTrue(_validator.IsAllowed("https://test.v1.api.example.com"), "多级子域名应该被允许");

        // 验证不匹配
        Assert.IsFalse(_validator.IsAllowed("http://api.example.com"), "api.example.com不应该匹配");
        Assert.IsFalse(_validator.IsAllowed("http://example.com"), "example.com不应该匹配");
    }

    /// <summary>
    /// 测试泛域名配置功能
    /// 验证README中描述的泛域名支持
    /// </summary>
    [TestMethod]
    public void Should_Support_Wildcard_Domain_Configuration()
    {
        // 添加泛域名
        _domainCollection.Add("*.example.com");

        // 验证各种子域名都被允许
        Assert.IsTrue(_validator.IsAllowed("http://api.example.com"), "api子域名应该被允许");
        Assert.IsTrue(_validator.IsAllowed("https://admin.example.com"), "admin子域名应该被允许");
        Assert.IsTrue(_validator.IsAllowed("http://sub.api.example.com"), "多级子域名应该被允许");
        Assert.IsTrue(_validator.IsAllowed("https://test.sub.example.com"), "多级子域名应该被允许");

        // 验证不匹配的情况
        Assert.IsFalse(_validator.IsAllowed("http://example.com"), "主域名不应该匹配泛域名");
        Assert.IsFalse(_validator.IsAllowed("http://other.com"), "其他域名不应该被允许");
        Assert.IsFalse(_validator.IsAllowed("http://sub.example.org"), "不同顶级域名不应该被允许");
    }

    #endregion 泛域名配置测试

    #region 统一策略测试

    /// <summary>
    /// 测试策略一致性 - 所有动态域名遵循相同验证规则
    /// </summary>
    [TestMethod]
    public void Should_Maintain_Policy_Consistency_Across_All_Dynamic_Domains()
    {
        // 配置一个泛域名策略
        _domainCollection.Add("*.company.com");

        // 验证所有子域名都遵循相同的策略规则
        var testOrigins = new[]
        {
            "http://hr.company.com",
            "https://finance.company.com",
            "http://dev.company.com:8080",
            "https://staging.company.com:3000"
        };

        foreach (var origin in testOrigins)
        {
            Assert.IsTrue(_validator.IsAllowed(origin), $"Origin {origin} 应该被允许");
        }

        // 验证非公司域名被拒绝
        Assert.IsFalse(_validator.IsAllowed("http://external.com"));
        Assert.IsFalse(_validator.IsAllowed("https://competitor.com"));
    }

    /// <summary>
    /// 测试所有动态CORS域名使用相同策略的特性
    /// 验证README中描述的所有动态CORS域名策略相同的特性
    /// </summary>
    [TestMethod]
    public void Should_Use_Same_Policy_For_All_Dynamic_Cors_Domains()
    {
        // 创建多个域名集合，但使用相同的验证器配置
        var collection1 = new DefaultDomainNameCollection();
        var collection2 = new DefaultDomainNameCollection();

        collection1.Add("app1.example.com");
        collection2.Add("app2.example.com");

        var validator1 = new DefaultOriginCorsValidator(collection1);
        var validator2 = new DefaultOriginCorsValidator(collection2);

        // 验证不同域名集合使用相同的验证逻辑
        Assert.IsTrue(validator1.IsAllowed("http://app1.example.com"));
        Assert.IsFalse(validator1.IsAllowed("http://app2.example.com"));

        Assert.IsTrue(validator2.IsAllowed("http://app2.example.com"));
        Assert.IsFalse(validator2.IsAllowed("http://app1.example.com"));

        // 但验证器的行为是一致的（相同的验证规则）
        Assert.AreEqual(
            validator1.GetType(),
            validator2.GetType(),
            "应该使用相同的验证器类型");
    }

    #endregion 统一策略测试

    #region 边界条件测试

    /// <summary>
    /// 测试空域名配置
    /// </summary>
    [TestMethod]
    public void Should_Handle_Empty_Domain_Configuration()
    {
        // 不添加任何域名
        Assert.IsFalse(_validator.IsAllowed("http://example.com"));
        Assert.IsFalse(_validator.IsAllowed("https://test.com"));
    }

    /// <summary>
    /// 测试特殊域名格式
    /// </summary>
    [TestMethod]
    public void Should_Handle_Special_Domain_Formats()
    {
        // 添加各种特殊格式的域名
        _domainCollection.Add("localhost");
        _domainCollection.Add("*.localhost");
        _domainCollection.Add("192.168.1.1");
        _domainCollection.Add("*.internal.local");

        // 验证特殊域名
        Assert.IsTrue(_validator.IsAllowed("http://localhost"));
        Assert.IsTrue(_validator.IsAllowed("http://app.localhost"));
        Assert.IsTrue(_validator.IsAllowed("http://192.168.1.1"));
        Assert.IsTrue(_validator.IsAllowed("https://service.internal.local"));
    }

    #endregion 边界条件测试

    #region 集成测试

    /// <summary>
    /// 测试完整的动态CORS配置场景
    /// 模拟实际使用中的配置
    /// </summary>
    [TestMethod]
    public void Should_Handle_Complete_Dynamic_Cors_Configuration()
    {
        // 模拟企业应用配置
        _domainCollection.Add("*.company.com");        // 公司所有子域名
        _domainCollection.Add("api.partner.com");      // 合作伙伴API
        _domainCollection.Add("*.staging.company.com"); // 测试环境
        _domainCollection.Add("localhost");              // 本地开发

        // 验证生产环境访问
        Assert.IsTrue(_validator.IsAllowed("https://app.company.com"));
        Assert.IsTrue(_validator.IsAllowed("https://admin.company.com"));
        Assert.IsTrue(_validator.IsAllowed("https://api.company.com:443"));

        // 验证测试环境访问
        Assert.IsTrue(_validator.IsAllowed("http://app.staging.company.com"));
        Assert.IsTrue(_validator.IsAllowed("https://api.staging.company.com:8443"));

        // 验证合作伙伴访问
        Assert.IsTrue(_validator.IsAllowed("https://api.partner.com"));
        Assert.IsTrue(_validator.IsAllowed("http://api.partner.com:8080"));

        // 验证本地开发
        Assert.IsTrue(_validator.IsAllowed("http://localhost"));
        Assert.IsTrue(_validator.IsAllowed("http://localhost:3000"));
        Assert.IsTrue(_validator.IsAllowed("https://localhost:5173"));

        // 验证外部访问被拒绝
        Assert.IsFalse(_validator.IsAllowed("https://external.com"));
        Assert.IsFalse(_validator.IsAllowed("http://malicious.com"));
    }

    #endregion 集成测试
}
