using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.Customer;
using DriveCentric.Model;
using DriveCentric.ServiceLayer.Customer;
using DriveCentric.Utilities.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DriveCentric.ServiceLayer.CoreTests.Customer
{
    [TestClass]
    public class CustomerServiceCoreTests
    {
        private CustomerService service;
        private Mock<ICustomerBusinessObject> businessObjectMock;
        private Mock<IContextInfoAccessor> contextInfoAccessorMock;
        private Mock<ICustomer> customerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            businessObjectMock = new Mock<ICustomerBusinessObject>();
            contextInfoAccessorMock = new Mock<IContextInfoAccessor>();
            service = new CustomerService(contextInfoAccessorMock.Object, businessObjectMock.Object);
            customerMock = new Mock<ICustomer>();
        }

        [TestMethod]
        public async Task GetCustomer_ValidCustomer_ReturnsCustomer()
        {
            businessObjectMock.Setup(mock => mock.GetCustomerAsync(It.IsAny<int>()))
                .ReturnsAsync(customerMock.Object);

            var returnedCustomer = await service.GetCustomerAsync(1);

            Assert.AreEqual(customerMock.Object, returnedCustomer);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetCustomer_KeyNotFound_Throws()
        {
            businessObjectMock.Setup(mock => mock.GetCustomerAsync(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var returnedCustomer = await service.GetCustomerAsync(1);
        }

        [TestMethod]
        public async Task Delete_ValidId_ReturnsTrue()
        {
            businessObjectMock.Setup(mock => mock.DeleteCustomerAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            var wasDeleted = await service.DeleteCustomerAsync(1);
            Assert.IsTrue(wasDeleted);
        }

        [TestMethod]
        public async Task Delete_IdNotExists_ReturnsFalse()
        {
            businessObjectMock.Setup(mock => mock.DeleteCustomerAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            var wasDeleted = await service.DeleteCustomerAsync(1);
            Assert.IsFalse(wasDeleted);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Delete_NullReferenceException_Throws()
        {
            businessObjectMock.Setup(mock => mock.DeleteCustomerAsync(It.IsAny<int>()))
                .Throws(new NullReferenceException("Test NRE"));
            var wasDeleted = await service.DeleteCustomerAsync(1);
        }
    }
}

