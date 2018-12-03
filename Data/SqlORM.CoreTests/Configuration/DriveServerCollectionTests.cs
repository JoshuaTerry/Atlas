using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Data.SqlORM.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DriveCentric.Data.SqlORM.CoreTests.Configuration
{
    [TestClass]
    public class DriveServerCollectionTests
    {
        private (long Count, IEnumerable<DriveServer> Data) driveServers;
        private Mock<IDataRepository<DriveServer>> dataRepositoryMock;
        private DriveServerCollection driveServerCollection;

        [TestInitialize]
        public void TestInitialize()
        {
            dataRepositoryMock = new Mock<IDataRepository<DriveServer>>();

            List<DriveServer> list = new List<DriveServer>();
            list.Add(new DriveServer { Id = 1, ExternalId = Guid.NewGuid(), ConnectionString = "ConnectionString1" });
            list.Add(new DriveServer { Id = 2, ExternalId = Guid.NewGuid(), ConnectionString = "ConnectionString2" });
            driveServers = (Count: list.Count, Data: list);

            dataRepositoryMock.Setup(mock =>
                mock.GetAllAsync(It.IsAny<Expression<Func<DriveServer, bool>>>(), It.IsAny<IPageable>(), null))
                .ReturnsAsync(driveServers);

        }

        [TestMethod]
        public void DriveServerCollection_ctor_DoesNotThrow()
        {
            driveServerCollection = new DriveServerCollection(dataRepositoryMock.Object);
        }

        [TestMethod]
        public void GetConnectionStringById_ExistingDriveServer_ReturnsConnectionString()
        {
            driveServerCollection = new DriveServerCollection(dataRepositoryMock.Object);
            var result = driveServerCollection.GetConnectionStringById(1);
            Assert.AreEqual("ConnectionString1", result);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetConnectionStringById_NonExistingDriveServer_Throws()
        {
            driveServerCollection = new DriveServerCollection(dataRepositoryMock.Object);
            var result = driveServerCollection.GetConnectionStringById(17);
        }
    }
}
