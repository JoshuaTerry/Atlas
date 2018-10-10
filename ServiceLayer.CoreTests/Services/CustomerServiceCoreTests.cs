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
    public class CustomerServiceCoreTests
    {
        private CustomerService service;
        private Mock<ICustomerLogic> businessLogicMock;
        private Mock<IContextInfoAccessor> contextInfoAccessorMock;
        private Mock<ICustomer> customerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            businessLogicMock = new Mock<ICustomerLogic>();
            contextInfoAccessorMock = new Mock<IContextInfoAccessor>();
            service = new CustomerService(contextInfoAccessorMock.Object, businessLogicMock.Object);
            customerMock = new Mock<ICustomer>();
        }

        [TestMethod]
        public async Task GetCustomer_ValidCustomer_ReturnsCustomer()
        {
            businessLogicMock.Setup(mock => mock.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(customerMock.Object);

            var returnedCustomer = await service.GetAsync(1);

            Assert.AreEqual(customerMock.Object, returnedCustomer);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetCustomer_KeyNotFound_Throws()
        {
            businessLogicMock.Setup(mock => mock.GetAsync(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var returnedCustomer = await service.GetAsync(1);
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

