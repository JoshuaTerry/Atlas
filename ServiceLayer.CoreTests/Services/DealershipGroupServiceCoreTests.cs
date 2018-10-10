using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.ServiceLayer.Services;
using DriveCentric.Utilities.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DriveCentric.ServiceLayer.CoreTests.Services
{
    [TestClass]
    public class DealershipGroupServiceCoreTests
    {
        private DealershipGroupService service;
        private Mock<IDealershipGroupLogic> businessLogicMock;
        private Mock<IContextInfoAccessor> contextInfoAccessorMock;
        private Mock<IDealershipGroup> dealershipGroupMock;

        [TestInitialize]
        public void TestInitialize()
        {
            contextInfoAccessorMock = new Mock<IContextInfoAccessor>();
            businessLogicMock = new Mock<IDealershipGroupLogic>();
            service = new DealershipGroupService(contextInfoAccessorMock.Object, businessLogicMock.Object);
            dealershipGroupMock = new Mock<IDealershipGroup>();
        }

        [TestMethod]
        public async Task GetDealershipGroup_ValidId_ReturnsDealershipGroup()
        {
            businessLogicMock.Setup(mock => mock.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(dealershipGroupMock.Object);

            var returnedDealershipGroup = await service.GetAsync(1);

            Assert.AreEqual(dealershipGroupMock.Object, returnedDealershipGroup);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetDealershipGroup_KeyNotFound_Throws()
        {
            businessLogicMock.Setup(mock => mock.GetAsync(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var returnedDealershipGroup = await service.GetAsync(1);
        }

        [TestMethod]
        public async Task Delete_ValidId_ReturnsTrue()
        {
            businessLogicMock.Setup(mock => mock.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            var wasDeleted = await service.DeleteAsync(1);
            Assert.IsTrue(wasDeleted);
        }

        [TestMethod]
        public async Task Delete_IdNotExists_ReturnsFalse()
        {
            businessLogicMock.Setup(mock => mock.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            var wasDeleted = await service.DeleteAsync(1);
            Assert.IsFalse(wasDeleted);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Delete_NullReferenceException_Throws()
        {
            businessLogicMock.Setup(mock => mock.DeleteAsync(It.IsAny<int>()))
                .Throws(new NullReferenceException("Test NRE"));
            var wasDeleted = await service.DeleteAsync(1);
        }
    }
}

