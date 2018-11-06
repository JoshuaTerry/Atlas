using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.TaskService.Services;
using DriveCentric.Utilities.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DriveCentric.Services.Tests.Services
{
    [TestClass]
    public class TaskServiceTests
    {
        private TaskService.Services.TaskService service;
        private Mock<ITaskLogic> businessLogicMock;
        private Mock<IContextInfoAccessor> contextInfoAccessorMock;
        private Mock<ITask> taskMock;

        [TestInitialize]
        public void TestInitialize()
        {
            businessLogicMock = new Mock<ITaskLogic>();
            contextInfoAccessorMock = new Mock<IContextInfoAccessor>();
            service = new TaskService.Services.TaskService(contextInfoAccessorMock.Object, businessLogicMock.Object);
            taskMock = new Mock<ITask>();
        }

        [TestMethod]
        public async Task GetTask_ValidTask_ReturnsTask()
        {
            businessLogicMock.Setup(mock => mock.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(taskMock.Object);

            var returnedTask = await service.GetAsync(1);

            Assert.AreEqual(taskMock.Object, returnedTask);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetTask_KeyNotFound_Throws()
        {
            businessLogicMock.Setup(mock => mock.GetAsync(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var returnedTask = await service.GetAsync(1);
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

        [TestMethod]
        public async Task InsertTask_ValidTask_ReturnsTaskId()
        {
            businessLogicMock.Setup(mock => mock.InsertAsync(It.IsAny<ITask>()))
                .ReturnsAsync(1234L);

            var returnedTaskId = await service.InsertAsync(taskMock.Object);

            Assert.AreEqual(1234L, returnedTaskId);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task InsertTask_NullReferenceException_Throws()
        {
            businessLogicMock.Setup(mock => mock.InsertAsync(It.IsAny<ITask>()))
                .Throws(new NullReferenceException("Test NRE"));

            var returnedTaskId = await service.InsertAsync(taskMock.Object);
        }

        [TestMethod]
        public async Task UpdateTask_ValidTask_ReturnsTrue()
        {
            businessLogicMock.Setup(mock => mock.UpdateAsync(It.IsAny<ITask>()))
                .ReturnsAsync(true);

            var returnedTask = await service.UpdateAsync(taskMock.Object);

            Assert.IsTrue(returnedTask);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task UpdateTask_NullReferenceException_Throws()
        {
            businessLogicMock.Setup(mock => mock.UpdateAsync(It.IsAny<ITask>()))
                .Throws(new NullReferenceException("Test NRE"));

            var returnedTask = await service.UpdateAsync(taskMock.Object);
        }
    }
}
