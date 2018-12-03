namespace DriveCentric.BusinessLogic.CoreTests.Implementation
{
    //[TestClass]
    //public class DealershipGroupLogicCoreTests
    //{
    //    private Mock<IContextInfoAccessor> contextInfoAccessorMock;
    //    private Mock<IDataRepository<DealershipGroup>> dealershipGroupDataRepositoryMock;
    //    private DealershipGroupLogic businessLogic;
    //    private Mock<DealershipGroup> dealershipGroupMock;

    //    [TestInitialize]
    //    public void TestInitialize()
    //    {
    //        contextInfoAccessorMock = new Mock<IContextInfoAccessor>();
    //        dealershipGroupDataRepositoryMock = new Mock<IDataRepository<DealershipGroup>>();

    //        businessLogic =
    //            new DealershipGroupLogic(contextInfoAccessorMock.Object, dealershipGroupDataRepositoryMock.Object);
    //        dealershipGroupMock = new Mock<DealershipGroup>();
    //    }

    //    [TestMethod]
    //    public void GetDealershipGroup_ValidId_ReturnsDealershipGroup()
    //    {
    //        dealershipGroupDataRepositoryMock.Setup(mock =>
    //            mock.GetSingleAsync(It.IsAny<Expression<Func<Core.Models.DealershipGroup, bool>>>(), It.IsAny<string[]>()));

    //        var returnedDealershipGroup = businessLogic.GetSingleAsync(x => x.Id == 1, null);
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(KeyNotFoundException))]
    //    public async Tasks.Task GetDealershipGroup_KeyNotFound_Throws()
    //    {
    //        dealershipGroupDataRepositoryMock.Setup(mock =>
    //            mock.GetSingleAsync(It.IsAny<Expression<Func<Core.Models.DealershipGroup, bool>>>(), It.IsAny<string[]>()))
    //            .Throws<KeyNotFoundException>();

    //        var returnedDealershipGroup = await businessLogic.GetSingleAsync(x => x.Id == 1, null);
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Delete_ValidId_ReturnsTrue()
    //    {
    //        dealershipGroupDataRepositoryMock.Setup(mock => mock.DeleteByIdAsync(It.IsAny<int>()))
    //            .ReturnsAsync(true);
    //        var wasDeleted = await businessLogic.DeleteAsync(1);
    //        Assert.IsTrue(wasDeleted);
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Delete_IdNotExists_ReturnsFalse()
    //    {
    //        dealershipGroupDataRepositoryMock.Setup(mock => mock.DeleteByIdAsync(It.IsAny<int>()))
    //            .ReturnsAsync(false);
    //        var wasDeleted = await businessLogic.DeleteAsync(1);
    //        Assert.IsFalse(wasDeleted);
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(NullReferenceException))]
    //    public async Tasks.Task Delete_NullReferenceException_Throws()
    //    {
    //        dealershipGroupDataRepositoryMock.Setup(mock => mock.DeleteByIdAsync(It.IsAny<int>()))
    //            .Throws(new NullReferenceException("Test NRE"));
    //        var wasDeleted = await businessLogic.DeleteAsync(1);
    //    }
    //}
}