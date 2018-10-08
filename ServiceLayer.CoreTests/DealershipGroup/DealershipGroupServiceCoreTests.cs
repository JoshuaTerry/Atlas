using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.DealershipGroup;
using DriveCentric.Model;
using DriveCentric.ServiceLayer.DealershipGroup;
using DriveCentric.Utilities.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DriveCentric.ServiceLayer.CoreTests.DealershipGroup
{
    [TestClass]
    public class DealershipGroupServiceCoreTests
    {
        private DealershipGroupService service;
        private Mock<IDealershipGroupBusinessObject> businessObjectMock;
        private Mock<IContextInfoAccessor> contextInfoAccessorMock;
        private Mock<IDealershipGroup> dealershipGroupMock;

        [TestInitialize]
        public void TestInitialize()
        {
            contextInfoAccessorMock = new Mock<IContextInfoAccessor>();
            businessObjectMock = new Mock<IDealershipGroupBusinessObject>();
            service = new DealershipGroupService(contextInfoAccessorMock.Object, businessObjectMock.Object);
            dealershipGroupMock = new Mock<IDealershipGroup>();
        }

        [TestMethod]
        public async Task GetDealershipGroup_ValidId_ReturnsDealershipGroup()
        {
            businessObjectMock.Setup(mock => mock.GetDealershipGroupAsync(It.IsAny<int>()))
                .ReturnsAsync(dealershipGroupMock.Object);

            var returnedDealershipGroup = await service.GetDealershipGroupAsync(1);

            Assert.AreEqual(dealershipGroupMock.Object, returnedDealershipGroup);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetDealershipGroup_KeyNotFound_Throws()
        {
            businessObjectMock.Setup(mock => mock.GetDealershipGroupAsync(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var returnedDealershipGroup = await service.GetDealershipGroupAsync(1);
        }

        [TestMethod]
        public async Task Delete_ValidId_ReturnsTrue()
        {
            businessObjectMock.Setup(mock => mock.DeleteDealershipGroupAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            var wasDeleted = await service.DeleteDealershipGroupAsync(1);
            Assert.IsTrue(wasDeleted);
        }

        [TestMethod]
        public async Task Delete_IdNotExists_ReturnsFalse()
        {
            businessObjectMock.Setup(mock => mock.DeleteDealershipGroupAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            var wasDeleted = await service.DeleteDealershipGroupAsync(1);
            Assert.IsFalse(wasDeleted);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Delete_NullReferenceException_Throws()
        {
            businessObjectMock.Setup(mock => mock.DeleteDealershipGroupAsync(It.IsAny<int>()))
                .Throws(new NullReferenceException("Test NRE"));
            var wasDeleted = await service.DeleteDealershipGroupAsync(1);
        }
    }
}

