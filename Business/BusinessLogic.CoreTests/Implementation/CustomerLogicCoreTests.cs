using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DriveCentric.BusinessLogic.Implementation;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tasks = System.Threading.Tasks;

namespace DriveCentric.BusinessLogic.CoreTests.Implementation
{
    [TestClass]
    public class CustomerLogicCoreTests
    {
        private Mock<IDataRepository<Customer>> customerDataRepositoryMock;
        private Mock<IContextInfoAccessor> contextInfoAccessorMock;
        private CustomerLogic businessLogic;
        private Mock<Customer> customerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            customerDataRepositoryMock = new Mock<IDataRepository<Customer>>();
            contextInfoAccessorMock = new Mock<IContextInfoAccessor>();

            businessLogic = new CustomerLogic(contextInfoAccessorMock.Object, customerDataRepositoryMock.Object);
            customerMock = new Mock<Customer>();
        }

        //Expression<Func<T, bool>> predicate, string[] referenceFields = null
        [TestMethod]
        public async Tasks.Task GetCustomer_ValidId_ReturnsCustomer()
        {
            customerDataRepositoryMock.Setup(mock =>
                mock.GetSingleAsync(It.IsAny<Expression<Func<Core.Models.Customer, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(customerMock.Object);

            var returnedCustomer = await businessLogic.GetSingleAsync(x => x.Id == 1, null);

            Assert.AreEqual(customerMock.Object, returnedCustomer);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Tasks.Task GetCustomer_KeyNotFound_Throws()
        {
            customerDataRepositoryMock.Setup(mock => 
                mock.GetSingleAsync(It.IsAny<Expression<Func<Core.Models.Customer, bool>>>(), It.IsAny<string[]>()))
                .Throws<KeyNotFoundException>();

            var returnedCustomer = await businessLogic.GetSingleAsync(x => x.Id == 1, null);
        }

        [TestMethod]
        public async Tasks.Task Delete_ValidId_ReturnsTrue()
        {
            customerDataRepositoryMock.Setup(mock => mock.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            var wasDeleted = await businessLogic.DeleteAsync(1);
            Assert.IsTrue(wasDeleted);
        }

        [TestMethod]
        public async Tasks.Task Delete_IdNotExists_ReturnsFalse()
        {
            customerDataRepositoryMock.Setup(mock => mock.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            var wasDeleted = await businessLogic.DeleteAsync(1);
            Assert.IsFalse(wasDeleted);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Tasks.Task Delete_NullReferenceException_Throws()
        {
            customerDataRepositoryMock.Setup(mock => mock.DeleteByIdAsync(It.IsAny<int>()))
                .Throws(new NullReferenceException("Test NRE"));
            var wasDeleted = await businessLogic.DeleteAsync(1);
        }
    }
}

