using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.DealershipGroup;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DriveCentric.BusinessLogic.CoreTests.DealershipGroup
{
    [TestClass]
    public class DealershipGroupBusinessObjectCoreTests
    {
        private Mock<IContextInfoAccessor> contextInfoAccessorMock;
        private Mock<IDataRepository<IDealershipGroup>> dealershipGroupDataRepositoryMock;
        private DealershipGroupBusinessObject businessObject;
        private Mock<IDealershipGroup> dealershipGroupMock;

        [TestInitialize]
        public void TestInitialize()
        {
            contextInfoAccessorMock = new Mock<IContextInfoAccessor>();
            dealershipGroupDataRepositoryMock = new Mock<IDataRepository<IDealershipGroup>>();

            businessObject =
                new DealershipGroupBusinessObject(contextInfoAccessorMock.Object, dealershipGroupDataRepositoryMock.Object);
            dealershipGroupMock = new Mock<IDealershipGroup>();
        }

        [TestMethod]
        public void GetDealershipGroup_ValidId_ReturnsDealershipGroup()
        {
            dealershipGroupDataRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(dealershipGroupMock.Object);

            var returnedDealershipGroup = businessObject.GetDealershipGroupAsync(1);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetDealershipGroup_KeyNotFound_Throws()
        {
            dealershipGroupDataRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>()))
                .Throws<KeyNotFoundException>();

            var returnedDealershipGroup = await businessObject.GetDealershipGroupAsync(1);
        }

        [TestMethod]
        public async Task Delete_ValidId_ReturnsTrue()
        {
            dealershipGroupDataRepositoryMock.Setup(mock => mock.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            var wasDeleted = await businessObject.DeleteDealershipGroupAsync(1);
            Assert.IsTrue(wasDeleted);
        }

        [TestMethod]
        public async Task Delete_IdNotExists_ReturnsFalse()
        {
            dealershipGroupDataRepositoryMock.Setup(mock => mock.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            var wasDeleted = await businessObject.DeleteDealershipGroupAsync(1);
            Assert.IsFalse(wasDeleted);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Delete_NullReferenceException_Throws()
        {
            dealershipGroupDataRepositoryMock.Setup(mock => mock.DeleteByIdAsync(It.IsAny<int>()))
                .Throws(new NullReferenceException("Test NRE"));
            var wasDeleted = await businessObject.DeleteDealershipGroupAsync(1);
        }
    }
}

