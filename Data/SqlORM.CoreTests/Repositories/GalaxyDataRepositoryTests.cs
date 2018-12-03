namespace DriveCentric.Data.SqlORM.CoreTests.Repositories
{
    //[TestClass]
    //public class GalaxyDataRepositoryTests
    //{
    //    private Mock<IContextInfoAccessor> mockContextInfoAccessor;
    //    private Mock<IDbConnectionFactory> mockDbConnectionFactory;
    //    private Mock<IDbConnection> mockDbConnection;
    //    private GalaxyDataRepository<Task> galaxyDataRepository;

    //    [TestInitialize]
    //    public void TestInitialize()
    //    {
    //        mockContextInfoAccessor = new Mock<IContextInfoAccessor>();
    //        mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
    //        mockDbConnection = new Mock<IDbConnection>();

    //        mockDbConnectionFactory.Setup(mock => mock.OpenDbConnection()).Returns(mockDbConnection.Object);

    //        galaxyDataRepository = new GalaxyDataRepository<Task>(mockContextInfoAccessor.Object, mockDbConnectionFactory.Object);
    //    }

    //    [TestMethod]
    //    public void GetDbFactory_ShouldReturn()
    //    {
    //        var result = galaxyDataRepository.GetDbFactory();
    //        Assert.AreEqual(mockDbConnectionFactory.Object, result);
    //    }
    //}
}