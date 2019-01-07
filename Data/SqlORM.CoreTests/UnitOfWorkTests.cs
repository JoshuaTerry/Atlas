using DriveCentric.Core.Interfaces;
using DriveCentric.Data.DataRepository;
using DriveCentric.Data.DataRepository.Interfaces;
using DriveCentric.Utilities.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveCentric.Data.SqlORM.CoreTests
{
    [TestClass]
    public class UnitOfWorkTests
    {
        [TestMethod]
        public void UnitOfWork_GetDatabaseHealthCheck_Returns2SuccessfulConnections()
        {
            var contextInfo = new Mock<IContextInfoAccessor>();
            var config = new Mock<IConfiguration>();
            var driveServerCollection = new Mock<IDriveServerCollection>();

            var repo = new Mock<IRepository>();
            repo.Setup(r => r.IsDatabaseAvailable()).Returns(Task.FromResult<bool>(true));

            var repos = new Dictionary<string, IRepository>();
            repos.Add("Galaxy", repo.Object);
            repos.Add("Star", repo.Object);

            var manager = new DatabaseCollectionManager(repos);
            var uow = new UnitOfWork(contextInfo.Object, config.Object, manager);
            var result = uow.GetDatabaseHealthCheck().Result;

            Assert.IsTrue(result.Count(kvp => kvp.Value) == 2);
        }
    }
}