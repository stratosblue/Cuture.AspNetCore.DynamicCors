// 代码由 AI 自动生成

using Microsoft.Extensions.DependencyInjection;

namespace Cuture.AspNetCore.DynamicCors.Test;

[TestClass]
public sealed class DynamicCorsPolicyBuilderTest
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

    #region Constructor 测试

    [TestMethod]
    public void Should_Initialize_With_Empty_PolicyName()
    {
        var builder = new DynamicCorsPolicyBuilder(_services, string.Empty);

        Assert.IsNotNull(builder);
        Assert.AreEqual(_services, builder.Services);
        Assert.AreEqual(string.Empty, builder.PolicyName);
    }

    [TestMethod]
    public void Should_Initialize_With_Null_PolicyName()
    {
        var builder = new DynamicCorsPolicyBuilder(_services, null);

        Assert.IsNotNull(builder);
        Assert.AreEqual(_services, builder.Services);
        Assert.IsNull(builder.PolicyName);
    }

    [TestMethod]
    public void Should_Initialize_With_Services_And_PolicyName()
    {
        var builder = new DynamicCorsPolicyBuilder(_services, "test-policy");

        Assert.IsNotNull(builder);
        Assert.AreEqual(_services, builder.Services);
        Assert.AreEqual("test-policy", builder.PolicyName);
    }

    [TestMethod]
    public void Should_Initialize_With_Services_Only()
    {
        var builder = new DynamicCorsPolicyBuilder(_services, null);

        Assert.IsNotNull(builder);
        Assert.AreEqual(_services, builder.Services);
        Assert.IsNull(builder.PolicyName);
    }

    #endregion Constructor 测试

    #region PolicyName 属性测试

    [TestMethod]
    public void Should_Return_Correct_PolicyName()
    {
        var builder = new DynamicCorsPolicyBuilder(_services, "test-policy");

        Assert.AreEqual("test-policy", builder.PolicyName);
    }

    [TestMethod]
    public void Should_Return_Empty_String_When_Set_To_Empty()
    {
        var builder = new DynamicCorsPolicyBuilder(_services, string.Empty);

        Assert.AreEqual(string.Empty, builder.PolicyName);
    }

    [TestMethod]
    public void Should_Return_Null_For_Default_Policy()
    {
        var builder = new DynamicCorsPolicyBuilder(_services, null);

        Assert.IsNull(builder.PolicyName);
    }

    #endregion PolicyName 属性测试

    #region 记录类型测试

    [TestMethod]
    public void Should_Be_Record_Type()
    {
        var builder1 = new DynamicCorsPolicyBuilder(_services, "test-policy");
        var builder2 = new DynamicCorsPolicyBuilder(_services, "test-policy");
        var builder3 = new DynamicCorsPolicyBuilder(_services, "different-policy");

        Assert.AreEqual(builder1, builder2);
        Assert.AreNotEqual(builder1, builder3);
    }

    [TestMethod]
    public void Should_Support_Value_Equality()
    {
        var builder1 = new DynamicCorsPolicyBuilder(_services, "test-policy");
        var builder2 = new DynamicCorsPolicyBuilder(_services, "test-policy");

        Assert.IsTrue(builder1.Equals(builder2));
        Assert.IsTrue(builder1 == builder2);
        Assert.IsFalse(builder1 != builder2);
    }

    [TestMethod]
    public void Should_Support_With_Expression()
    {
        var builder = new DynamicCorsPolicyBuilder(_services, "test-policy");
        var newBuilder = builder with { PolicyName = "new-policy" };

        Assert.AreEqual("new-policy", newBuilder.PolicyName);
        Assert.AreEqual(builder.Services, newBuilder.Services);
    }

    #endregion 记录类型测试

    #region 使用场景测试

    [TestMethod]
    public void Should_Work_In_Default_Policy_Scenario()
    {
        var builder = new DynamicCorsPolicyBuilder(_services, null);

        Assert.IsNull(builder.PolicyName);
        Assert.AreEqual(_services, builder.Services);
    }

    [TestMethod]
    public void Should_Work_In_Named_Policy_Scenario()
    {
        var policyNames = new[]
        {
            "api-policy",
            "web-policy",
            "mobile-policy",
            "admin-policy"
        };

        foreach (var policyName in policyNames)
        {
            var builder = new DynamicCorsPolicyBuilder(_services, policyName);
            Assert.AreEqual(policyName, builder.PolicyName);
            Assert.AreEqual(_services, builder.Services);
        }
    }

    [TestMethod]
    public void Should_Work_With_Complex_Policy_Names()
    {
        var complexNames = new[]
        {
            "policy-with-dashes",
            "policy_with_underscores",
            "policy.with.dots",
            "policy123",
            "UPPERCASE_POLICY",
            "mixedCase_Policy-123",
            "policy with spaces",
            "policy@with#special$chars"
        };

        foreach (var name in complexNames)
        {
            var builder = new DynamicCorsPolicyBuilder(_services, name);
            Assert.AreEqual(name, builder.PolicyName);
        }
    }

    #endregion 使用场景测试

    #region 性能测试

    [TestMethod]
    public void Should_Handle_Large_Number_Of_Instances()
    {
        var builders = new List<DynamicCorsPolicyBuilder>();

        for (int i = 0; i < 1000; i++)
        {
            var builder = new DynamicCorsPolicyBuilder(_services, $"policy-{i}");
            builders.Add(builder);
        }

        Assert.AreEqual(1000, builders.Count);
        Assert.AreEqual(1000, builders.Select(b => b.PolicyName).Distinct().Count());
    }

    #endregion 性能测试

    #region 内存测试

    [TestMethod]
    public void Should_Not_Hold_References_To_Large_Objects()
    {
        var largeServiceCollection = new ServiceCollection();
        for (int i = 0; i < 1000; i++)
        {
            largeServiceCollection.AddSingleton<object>();
        }

        var builder = new DynamicCorsPolicyBuilder(largeServiceCollection, "test-policy");

        Assert.AreEqual(largeServiceCollection, builder.Services);
        Assert.AreEqual(1000, builder.Services.Count);
    }

    #endregion 内存测试
}
