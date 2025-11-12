// 代码由 AI 自动生成

using System.Collections.Concurrent;
using Cuture.AspNetCore.DynamicCors.Internal;

namespace Cuture.AspNetCore.DynamicCors.Test.Internal;

[TestClass]
public sealed class DomainNameMatchNodeTest
{
    #region Private 字段

    private DomainNameMatchNode _childNode = null!;

    private DomainNameMatchNode _rootNode = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _rootNode = new DomainNameMatchNode(null, "com".AsMemory());
        _childNode = new DomainNameMatchNode(_rootNode, "example".AsMemory());
    }

    #endregion 测试初始化

    #region 构造函数测试

    [TestMethod]
    public void Should_Initialize_With_Empty_Value()
    {
        var emptyNode = new DomainNameMatchNode(null, ReadOnlyMemory<char>.Empty);
        Assert.AreEqual(ReadOnlyMemory<char>.Empty, emptyNode.Value);
    }

    [TestMethod]
    public void Should_Initialize_With_Null_Parent()
    {
        var rootNode = new DomainNameMatchNode(null, "root".AsMemory());
        Assert.IsNull(rootNode.Parent);
        Assert.AreEqual("root".AsMemory().ToString(), rootNode.Value.ToString());
    }

    [TestMethod]
    public void Should_Initialize_With_Parent_And_Value()
    {
        Assert.AreEqual(_rootNode, _childNode.Parent);
        Assert.AreEqual("example".AsMemory().ToString(), _childNode.Value.ToString());
        Assert.IsNotNull(_childNode.Children);
        Assert.IsInstanceOfType<ConcurrentDictionary<ReadOnlyMemory<char>, DomainNameMatchNode>>(_childNode.Children);
        Assert.IsFalse(_childNode.IsEndpoint);
        Assert.IsFalse(_childNode.IsEndpoint);
    }

    #endregion 构造函数测试

    #region 属性测试

    [TestMethod]
    public void Should_Have_ThreadSafe_Children_Collection()
    {
        var children = _childNode.Children;
        Assert.IsNotNull(children);
        Assert.IsEmpty(children);
    }

    [TestMethod]
    public void Should_Set_IsEndpoint_Property()
    {
        _childNode.IsEndpoint = true;
        Assert.IsTrue(_childNode.IsEndpoint);
    }

    [TestMethod]
    public void Should_Set_IsWildcardEndpoint_Property()
    {
        _childNode.IsEndpoint = true;
        Assert.IsTrue(_childNode.IsEndpoint);
    }

    #endregion 属性测试

    #region ToString 测试

    [TestMethod]
    public void Should_Return_String_Representation_For_Complex_Hierarchy()
    {
        var root = new DomainNameMatchNode(null, "com".AsMemory());
        var example = new DomainNameMatchNode(root, "example".AsMemory()) { IsEndpoint = true };
        var www = new DomainNameMatchNode(example, "www".AsMemory()) { IsEndpoint = true };

        var result = www.ToString();
        Assert.AreEqual("*.www.example.com", result);
    }

    [TestMethod]
    public void Should_Return_String_Representation_For_Endpoint_Node()
    {
        var node = new DomainNameMatchNode(_rootNode, "example".AsMemory())
        {
            IsEndpoint = true
        };
        var result = node.ToString();
        Assert.AreEqual("*.example.com", result);
    }

    [TestMethod]
    public void Should_Return_String_Representation_For_Root_Node()
    {
        var root = new DomainNameMatchNode(null, "com".AsMemory());
        var result = root.ToString();
        Assert.AreEqual("com", result);
    }

    [TestMethod]
    public void Should_Return_String_Representation_For_Wildcard_Node()
    {
        var node = new DomainNameMatchNode(_rootNode, "example".AsMemory())
        {
            IsEndpoint = true
        };
        var result = node.ToString();
        Assert.AreEqual("*.example.com", result);
    }

    [TestMethod]
    public void Should_Return_String_Representation_With_Children_Count()
    {
        var node = new DomainNameMatchNode(_rootNode, "example".AsMemory());
        node.Children.TryAdd("sub".AsMemory(), new DomainNameMatchNode(node, "sub".AsMemory()));

        var result = node.ToString();
        Assert.AreEqual("example.com(1)", result);
    }

    #endregion ToString 测试

    #region 复杂场景测试

    [TestMethod]
    public void Should_Handle_Deep_Hierarchy()
    {
        var root = new DomainNameMatchNode(null, "com".AsMemory());
        var example = new DomainNameMatchNode(root, "example".AsMemory());
        var api = new DomainNameMatchNode(example, "api".AsMemory());
        var v1 = new DomainNameMatchNode(api, "v1".AsMemory());

        Assert.AreEqual("v1.api.example.com", v1.ToString());
    }

    [TestMethod]
    public void Should_Handle_Multiple_Children()
    {
        var root = new DomainNameMatchNode(null, "com".AsMemory());

        root.Children.TryAdd("example".AsMemory(), new DomainNameMatchNode(root, "example".AsMemory()));
        root.Children.TryAdd("test".AsMemory(), new DomainNameMatchNode(root, "test".AsMemory()));
        root.Children.TryAdd("demo".AsMemory(), new DomainNameMatchNode(root, "demo".AsMemory()));

        Assert.HasCount(3, root.Children);
        Assert.AreEqual("com(3)", root.ToString());
    }

    [TestMethod]
    public void Should_Handle_Special_Characters_In_Value()
    {
        var node = new DomainNameMatchNode(null, "test-domain".AsMemory());
        Assert.AreEqual("test-domain", node.ToString());
    }

    #endregion 复杂场景测试
}
