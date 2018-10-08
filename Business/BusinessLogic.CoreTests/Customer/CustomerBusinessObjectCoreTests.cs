using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.Customer;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DriveCentric.BusinessLogic.CoreTests.Customer
{
    [TestClass]
    public class CustomerBusinessObjectCoreTests
    {
        private Mock<IDataRepository<ICustomer>> customerDataRepositoryMock;
        private Mock<IContextInfoAccessor> contextInfoAccessorMock;
        private CustomerBusinessObject businessObject;
        private Mock<ICustomer> customerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            customerDataRepositoryMock = new Mock<IDataRepository<ICustomer>>();
            contextInfoAccessorMock = new Mock<IContextInfoAccessor>();

            businessObject = new CustomerBusinessObject(contextInfoAccessorMock.Object, customerDataRepositoryMock.Object);
            customerMock = new Mock<ICustomer>();
        }

        [TestMethod]
        public async Task GetCustomer_ValidId_ReturnsCustomer()
        {
            customerDataRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(customerMock.Object);

            var returnedCustomer = await businessObject.GetCustomerAsync(1);

            Assert.AreEqual(customerMock.Object, returnedCustomer);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetCustomer_KeyNotFound_Throws()
        {
            customerDataRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>()))
                .Throws<KeyNotFoundException>();

            var returnedCustomer = await businessObject.GetCustomerAsync(1);
        }

        [TestMethod]
        public async Task Delete_ValidId_ReturnsTrue()
        {
            customerDataRepositoryMock.Setup(mock => mock.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            var wasDeleted = await businessObject.DeleteCustomerAsync(1);
            Assert.IsTrue(wasDeleted);
        }

        [TestMethod]
        public async Task Delete_IdNotExists_ReturnsFalse()
        {
            customerDataRepositoryMock.Setup(mock => mock.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            var wasDeleted = await businessObject.DeleteCustomerAsync(1);
            Assert.IsFalse(wasDeleted);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Delete_NullReferenceException_Throws()
        {
            customerDataRepositoryMock.Setup(mock => mock.DeleteByIdAsync(It.IsAny<int>()))
                .Throws(new NullReferenceException("Test NRE"));
            var wasDeleted = await businessObject.DeleteCustomerAsync(1);
        }
    }
}

