// 代码由 AI 自动生成

namespace Cuture.AspNetCore.DynamicCors.Test;

[TestClass]
public sealed class DefaultDomainNameCollectionTest
{
    #region Private 字段

    private DefaultDomainNameCollection _collection = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _collection = new DefaultDomainNameCollection();
    }

    #endregion 测试初始化

    #region Add 测试

    [TestMethod]
    public void Should_Add_Empty_Host_Without_Exception()
    {
        Assert.ThrowsExactly<ArgumentException>(() => _collection.Add(""));
    }

    [TestMethod]
    public void Should_Add_Host_Success()
    {
        _collection.Add("example.com");
        Assert.IsTrue(_collection.Contains("http://example.com"));
    }

    [TestMethod]
    public void Should_Add_Multiple_Hosts_Success()
    {
        _collection.Add("example.com");
        _collection.Add("test.com");
        _collection.Add("api.example.com");

        Assert.IsTrue(_collection.Contains("http://example.com"));
        Assert.IsTrue(_collection.Contains("http://test.com"));
        Assert.IsTrue(_collection.Contains("http://api.example.com"));
    }

    [TestMethod]
    public void Should_Add_Wildcard_Host_Success()
    {
        _collection.Add("*.example.com");
        Assert.IsTrue(_collection.Contains("http://sub.example.com"));
        Assert.IsTrue(_collection.Contains("http://api.example.com"));
        Assert.IsFalse(_collection.Contains("http://example.com"));
    }

    #endregion Add 测试

    #region Contains 测试

    [TestMethod]
    public void Should_Handle_Wildcard_Match_Correctly()
    {
        _collection.Add("*.example.com");
        Assert.IsTrue(_collection.Contains("http://sub.example.com"));
        Assert.IsTrue(_collection.Contains("https://api.example.com"));
        Assert.IsFalse(_collection.Contains("http://example.com"));
        Assert.IsFalse(_collection.Contains("http://sub.test.com"));
    }

    [TestMethod]
    public void Should_Return_False_For_Non_Existing_Host()
    {
        _collection.Add("example.com");
        Assert.IsFalse(_collection.Contains("http://test.com"));
    }

    [TestMethod]
    public void Should_Return_True_For_Exact_Match()
    {
        _collection.Add("example.com");
        Assert.IsTrue(_collection.Contains("http://example.com"));
        Assert.IsTrue(_collection.Contains("https://example.com"));
    }

    [TestMethod]
    public void Should_Throw_For_Null_Or_Empty_Origin()
    {
        _collection.Add("example.com");
        Assert.ThrowsExactly<ArgumentNullException>(() => _collection.Contains(null!));
        Assert.ThrowsExactly<ArgumentException>(() => _collection.Contains(""));
        Assert.ThrowsExactly<ArgumentException>(() => _collection.Contains("http://"));
    }

    #endregion Contains 测试

    #region Remove 测试

    [TestMethod]
    public void Should_Remove_Existing_Host_Success()
    {
        _collection.Add("example.com");
        Assert.IsTrue(_collection.Contains("http://example.com"));

        var result = _collection.Remove("example.com");
        Assert.IsTrue(result);
        Assert.IsFalse(_collection.Contains("http://example.com"));
    }

    [TestMethod]
    public void Should_Remove_Wildcard_Host_Success()
    {
        _collection.Add("*.example.com");
        Assert.IsTrue(_collection.Contains("http://sub.example.com"));

        var result = _collection.Remove("*.example.com");
        Assert.IsTrue(result);
        Assert.IsFalse(_collection.Contains("http://sub.example.com"));
    }

    [TestMethod]
    public void Should_Return_False_When_Removing_Non_Existing_Host()
    {
        _collection.Add("example.com");
        var result = _collection.Remove("test.com");
        Assert.IsFalse(result);
    }

    #endregion Remove 测试

    #region Clear 测试

    [TestMethod]
    public void Should_Clear_All_Hosts_Success()
    {
        _collection.Add("example.com");
        _collection.Add("test.com");
        Assert.IsTrue(_collection.Contains("http://example.com"));
        Assert.IsTrue(_collection.Contains("http://test.com"));

        _collection.Clear();
        Assert.IsFalse(_collection.Contains("http://example.com"));
        Assert.IsFalse(_collection.Contains("http://test.com"));
    }

    [TestMethod]
    public void Should_Clear_When_Collection_Is_Empty()
    {
        _collection.Clear();
        Assert.IsFalse(_collection.Contains("http://example.com"));
    }

    #endregion Clear 测试

    #region Reset 测试

    [TestMethod]
    public void Should_Reset_With_Empty_List_Success()
    {
        _collection.Add("example.com");
        Assert.IsTrue(_collection.Contains("http://example.com"));

        _collection.Reset([]);
        Assert.IsFalse(_collection.Contains("http://example.com"));
    }

    [TestMethod]
    public void Should_Reset_With_New_Hosts_Success()
    {
        _collection.Add("old1.com");
        _collection.Add("old2.com");
        Assert.IsTrue(_collection.Contains("http://old1.com"));
        Assert.IsTrue(_collection.Contains("http://old2.com"));

        var newHosts = new[] { "new1.com", "new2.com", "new3.com" };
        _collection.Reset(newHosts);

        Assert.IsFalse(_collection.Contains("http://old1.com"));
        Assert.IsFalse(_collection.Contains("http://old2.com"));
        Assert.IsTrue(_collection.Contains("http://new1.com"));
        Assert.IsTrue(_collection.Contains("http://new2.com"));
        Assert.IsTrue(_collection.Contains("http://new3.com"));
    }

    [TestMethod]
    public void Should_Reset_With_Null_List_Without_Exception()
    {
        _collection.Add("example.com");
        Assert.ThrowsExactly<ArgumentNullException>(() => _collection.Reset(null!));
        Assert.IsTrue(_collection.Contains("http://example.com"));
        _collection.Reset([]);
        Assert.IsFalse(_collection.Contains("http://example.com"));
    }

    #endregion Reset 测试

    #region Complex 场景测试

    [TestMethod]
    public void Should_Handle_Complex_Domain_Scenarios()
    {
        // 添加各种域名组合
        _collection.Add("example.com");
        _collection.Add("*.example.com");
        _collection.Add("api.test.com");

        Assert.IsTrue(_collection.Contains("http://example.com"));
        Assert.IsTrue(_collection.Contains("http://sub.example.com"));
        Assert.IsTrue(_collection.Contains("https://api.test.com"));
        Assert.IsFalse(_collection.Contains("http://other.com"));

        // 移除部分域名
        _collection.Remove("example.com");
        Assert.IsFalse(_collection.Contains("http://example.com"));
        Assert.IsTrue(_collection.Contains("http://sub.example.com"));
        Assert.IsTrue(_collection.Contains("https://api.test.com"));
    }

    #endregion Complex 场景测试
}
