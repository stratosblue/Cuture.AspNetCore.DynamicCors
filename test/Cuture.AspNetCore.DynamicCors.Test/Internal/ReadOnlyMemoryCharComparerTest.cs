// 代码由 AI 自动生成

using Cuture.AspNetCore.DynamicCors.Internal;

namespace Cuture.AspNetCore.DynamicCors.Test.Internal;

[TestClass]
public sealed class ReadOnlyMemoryCharComparerTest
{
    #region Private 字段

    private ReadOnlyMemoryCharComparer _comparer = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _comparer = ReadOnlyMemoryCharComparer.Shared;
    }

    #endregion 测试初始化

    #region Equals 测试

    [TestMethod]
    public void Should_Return_False_When_Memories_Are_Not_Equal()
    {
        var memory1 = "test1".AsMemory();
        var memory2 = "test2".AsMemory();

        var result = _comparer.Equals(memory1, memory2);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Should_Return_False_When_One_Memory_Is_Empty()
    {
        var memory1 = "test".AsMemory();
        var memory2 = ReadOnlyMemory<char>.Empty;

        var result = _comparer.Equals(memory1, memory2);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Should_Return_True_When_Memories_Are_Empty()
    {
        var memory1 = ReadOnlyMemory<char>.Empty;
        var memory2 = ReadOnlyMemory<char>.Empty;

        var result = _comparer.Equals(memory1, memory2);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Should_Return_True_When_Memories_Are_Equal()
    {
        var memory1 = "test".AsMemory();
        var memory2 = "test".AsMemory();

        var result = _comparer.Equals(memory1, memory2);

        Assert.IsTrue(result);
    }

    #endregion Equals 测试

    #region GetHashCode 测试

    [TestMethod]
    public void Should_Return_Consistent_HashCode_For_Same_Memory_Content()
    {
        var memory = "consistent".AsMemory();
        var hash1 = _comparer.GetHashCode(memory);
        var hash2 = _comparer.GetHashCode(memory);

        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void Should_Return_Different_HashCode_For_Different_Memories()
    {
        var memory1 = "test1".AsMemory();
        var memory2 = "test2".AsMemory();

        var hash1 = _comparer.GetHashCode(memory1);
        var hash2 = _comparer.GetHashCode(memory2);

        Assert.AreNotEqual(hash1, hash2);
    }

    [TestMethod]
    public void Should_Return_Same_HashCode_For_Equal_Memories()
    {
        var memory1 = "test".AsMemory();
        var memory2 = "test".AsMemory();

        var hash1 = _comparer.GetHashCode(memory1);
        var hash2 = _comparer.GetHashCode(memory2);

        Assert.AreEqual(hash1, hash2);
    }

    #endregion GetHashCode 测试
}
