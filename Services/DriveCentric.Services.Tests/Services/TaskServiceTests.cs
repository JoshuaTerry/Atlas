using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        private Mock<Core.Models.Task> taskMock;

        [TestInitialize]
        public void TestInitialize()
        {
            businessLogicMock = new Mock<ITaskLogic>();
            contextInfoAccessorMock = new Mock<IContextInfoAccessor>();
            service = new TaskService.Services.TaskService(contextInfoAccessorMock.Object, businessLogicMock.Object);
            taskMock = new Mock<Core.Models.Task>();
        }

        [TestMethod]
        public async Task GetTask_ValidTask_ReturnsTask()
        {
            businessLogicMock.Setup(mock =>
                mock.GetSingleAsync(It.IsAny<Expression<Func<Core.Models.Task, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(taskMock.Object);

            var response = await service.GetSingleByExpressionAsync(x => x.Id == 1, null);

            Assert.AreEqual(taskMock.Object, response.Data);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetTask_KeyNotFound_Throws()
        {
            businessLogicMock.Setup(mock =>
                mock.GetSingleAsync(It.IsAny<Expression<Func<Core.Models.Task, bool>>>(), It.IsAny<string[]>()))
                .Throws<KeyNotFoundException>();

            var response = await service.GetSingleByExpressionAsync(x => x.Id == 1, null);
        }

        [TestMethod]
        public async Task Delete_ValidId_ReturnsTrue()
        {
            businessLogicMock.Setup(mock => mock.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            var response = await service.DeleteAsync(1);
            Assert.IsTrue(response.Data);
        }

        [TestMethod]
        public async Task Delete_IdNotExists_ReturnsFalse()
        {
            businessLogicMock.Setup(mock => mock.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            var response = await service.DeleteAsync(1);
            Assert.IsFalse(response.Data);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Delete_NullReferenceException_Throws()
        {
            businessLogicMock.Setup(mock => mock.DeleteAsync(It.IsAny<int>()))
                .Throws(new NullReferenceException("Test NRE"));
            var response = await service.DeleteAsync(1);
        }

        [TestMethod]
        public async Task InsertTask_ValidTask_ReturnsTaskId()
        {
            businessLogicMock.Setup(mock => mock.InsertAsync(It.IsAny<Core.Models.Task>()))
                .ReturnsAsync(1234L);

            var response = await service.InsertAsync(taskMock.Object);

            Assert.AreEqual(1234L, response.Data);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task InsertTask_NullReferenceException_Throws()
        {
            businessLogicMock.Setup(mock => mock.InsertAsync(It.IsAny<Core.Models.Task>()))
                .Throws(new NullReferenceException("Test NRE"));

            var response = await service.InsertAsync(taskMock.Object);
        }

        [TestMethod]
        public async Task UpdateTask_ValidTask_ReturnsTrue()
        {
            businessLogicMock.Setup(mock => mock.UpdateAsync(It.IsAny<Core.Models.Task>()))
                .ReturnsAsync(true);

            var response = await service.UpdateAsync(taskMock.Object);

            Assert.IsTrue(response.Data);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task UpdateTask_NullReferenceException_Throws()
        {
            businessLogicMock.Setup(mock => mock.UpdateAsync(It.IsAny<Core.Models.Task>()))
                .Throws(new NullReferenceException("Test NRE"));

            var response = await service.UpdateAsync(taskMock.Object);
        }
    }
}
