// 代码由 AI 自动生成

using Cuture.AspNetCore.DynamicCors.Internal;

namespace Cuture.AspNetCore.DynamicCors.Test.Internal;

[TestClass]
public sealed class NoopDisposableTest
{
    #region Private 字段

    private NoopDisposable _disposable = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _disposable = new NoopDisposable();
    }

    #endregion 测试初始化

    #region Dispose 测试

    [TestMethod]
    public void Should_Dispose_Multiple_Times_Without_Exception()
    {
        _disposable.Dispose();
        _disposable.Dispose();
        _disposable.Dispose();
    }

    [TestMethod]
    public void Should_Dispose_Success_Without_Exception()
    {
        _disposable.Dispose();
    }

    #endregion Dispose 测试
}
