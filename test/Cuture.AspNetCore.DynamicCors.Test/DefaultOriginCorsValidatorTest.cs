// 代码由 AI 自动生成

using Moq;

namespace Cuture.AspNetCore.DynamicCors.Test;

[TestClass]
public sealed class DefaultOriginCorsValidatorTest
{
    #region Private 字段

    private Mock<IDomainNameCollection> _domainNameCollectionMock = null!;

    private DefaultOriginCorsValidator _validator = null!;

    #endregion Private 字段

    #region 测试初始化

    [TestInitialize]
    public void Initialize()
    {
        _domainNameCollectionMock = new Mock<IDomainNameCollection>();
        _validator = new DefaultOriginCorsValidator(_domainNameCollectionMock.Object);
    }

    #endregion 测试初始化

    #region IsAllowed 测试

    [TestMethod]
    public void Should_Handle_Multiple_Origins_Correctly()
    {
        var allowedOrigin = "http://allowed.com";
        var disallowedOrigin = "http://disallowed.com";

        _domainNameCollectionMock.Setup(m => m.Contains(allowedOrigin)).Returns(true);
        _domainNameCollectionMock.Setup(m => m.Contains(disallowedOrigin)).Returns(false);

        Assert.IsTrue(_validator.IsAllowed(allowedOrigin));
        Assert.IsFalse(_validator.IsAllowed(disallowedOrigin));

        _domainNameCollectionMock.Verify(m => m.Contains(allowedOrigin), Times.Once);
        _domainNameCollectionMock.Verify(m => m.Contains(disallowedOrigin), Times.Once);
    }

    [TestMethod]
    public void Should_Handle_Various_Origin_Formats()
    {
        var origins = new[]
        {
            "http://example.com",
            "https://example.com",
            "http://sub.example.com",
            "https://api.test.com:8080",
            "http://localhost:3000"
        };

        foreach (var origin in origins)
        {
            _domainNameCollectionMock.Setup(m => m.Contains(origin)).Returns(true);
            Assert.IsTrue(_validator.IsAllowed(origin));
            _domainNameCollectionMock.Setup(m => m.Contains(origin)).Returns(false);
            Assert.IsFalse(_validator.IsAllowed(origin));
        }
    }

    [TestMethod]
    public void Should_Return_False_For_Empty_Origin()
    {
        var origin = string.Empty;
        _domainNameCollectionMock.Setup(m => m.Contains(origin)).Returns(false);

        var result = _validator.IsAllowed(origin);

        Assert.IsFalse(result);
        _domainNameCollectionMock.Verify(m => m.Contains(origin), Times.Once);
    }

    [TestMethod]
    public void Should_Return_False_For_Null_Origin()
    {
        _domainNameCollectionMock.Setup(m => m.Contains(null)).Returns(false);

        var result = _validator.IsAllowed(null);

        Assert.IsFalse(result);
        _domainNameCollectionMock.Verify(m => m.Contains(null), Times.Once);
    }

    [TestMethod]
    public void Should_Return_False_When_Origin_Is_Not_Allowed()
    {
        var origin = "http://example.com";
        _domainNameCollectionMock.Setup(m => m.Contains(origin)).Returns(false);

        var result = _validator.IsAllowed(origin);

        Assert.IsFalse(result);
        _domainNameCollectionMock.Verify(m => m.Contains(origin), Times.Once);
    }

    [TestMethod]
    public void Should_Return_True_When_Origin_Is_Allowed()
    {
        var origin = "http://example.com";
        _domainNameCollectionMock.Setup(m => m.Contains(origin)).Returns(true);

        var result = _validator.IsAllowed(origin);

        Assert.IsTrue(result);
        _domainNameCollectionMock.Verify(m => m.Contains(origin), Times.Once);
    }

    #endregion IsAllowed 测试

    #region Constructor 测试

    [TestMethod]
    public void Should_Throw_ArgumentNullException_When_DomainNameCollection_Is_Null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            new DefaultOriginCorsValidator(null!);
        });
    }

    #endregion Constructor 测试

    #region Integration 测试

    [TestMethod]
    public void Should_Integrate_With_Real_DomainNameCollection()
    {
        var realCollection = new DefaultDomainNameCollection();
        realCollection.Add("example.com");
        realCollection.Add("*.test.com");

        var realValidator = new DefaultOriginCorsValidator(realCollection);

        Assert.IsTrue(realValidator.IsAllowed("http://example.com"));
        Assert.IsTrue(realValidator.IsAllowed("https://sub.test.com"));
        Assert.IsFalse(realValidator.IsAllowed("http://other.com"));
        Assert.IsFalse(realValidator.IsAllowed("http://test.com"));
    }

    #endregion Integration 测试
}
