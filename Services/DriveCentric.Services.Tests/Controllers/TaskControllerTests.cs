using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriveCentric.BaseService.Controllers.BindingModels;
using DriveCentric.Model;
using DriveCentric.Services.Tests.Helpers;
using DriveCentric.TaskService.Controllers;
using DriveCentric.TaskService.Services;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DriveCentric.Services.Tests.Controllers
{
    [TestClass]
    public class TaskControllerTests
    {
        private TaskController controller;
        private Mock<ITaskService> taskServiceMock;
        private Mock<IHttpContextAccessor> httpContextAccessorMock;
        private Mock<IContextInfoAccessor> contextInfoAccessorMock;
        private Mock<ITask> taskMock;

        [TestInitialize]
        public void TestInitialize()
        {
            httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            contextInfoAccessorMock = new Mock<IContextInfoAccessor>();
            taskServiceMock = new Mock<ITaskService>();
            controller = new TaskController(
                httpContextAccessorMock.Object,
                contextInfoAccessorMock.Object,
                taskServiceMock.Object)
            { ControllerContext = ControllerContextHelper.CreateControllerContext() };
            taskMock = new Mock<ITask>();
        }

        [TestMethod]
        public async Task Get_ValidTask_ReturnsOk()
        {
            taskServiceMock.Setup(mock => mock.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(taskMock.Object);
            var result = await controller.Get(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Get_KeyNotFound_ReturnsNotFound()
        {
            taskServiceMock.Setup(mock => mock.GetAsync(It.IsAny<int>())).Throws<KeyNotFoundException>();
            var result = await controller.Get(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Get_Exception_ReturnsObjectResult()
        {
            taskServiceMock.Setup(mock => mock.GetAsync(It.IsAny<int>())).Throws<Exception>();
            var result = await controller.Get(1);

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
        }

        [TestMethod]
        public async Task Get_InvalidModelState_ReturnsBadRequestObjectResult()
        {
            controller.ModelState.AddModelError("test", "Testing invalid Model State.");
            var result = await controller.Get(1);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Delete_ValidTask_ReturnsOk()
        {
            taskServiceMock.Setup(mock => mock.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            var result = await controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            Assert.IsTrue((bool)okResult.Value);
        }

        [TestMethod]
        public async Task Delete_TaskNotExists_ReturnsOk()
        {
            taskServiceMock.Setup(mock => mock.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            var result = await controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            Assert.IsFalse((bool)okResult.Value);
        }

        [TestMethod]
        public async Task Delete_NullReferenceException_Returns()
        {
            taskServiceMock.Setup(mock => mock.DeleteAsync(It.IsAny<int>()))
                .Throws<NullReferenceException>();
            var result = await controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
        }

        [TestMethod]
        public async Task Delete_InvalidModelState_ReturnsBadRequestObjectResult()
        {
            controller.ModelState.AddModelError("test", "Testing invalid Model State.");
            var result = await controller.Delete(1);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Post_ValidTask_ReturnsOk()
        {
            taskServiceMock.Setup(mock => mock.InsertAsync(It.IsAny<ITask>()))
                .ReturnsAsync(1234L);
            var result = await controller.Post(new TaskBindingModel { ActionType = 0 });
            var okResult = result as OkObjectResult;
            Assert.AreEqual(1234L, okResult.Value);
        }

        [TestMethod]
        public async Task Post_InvalidModelState_ReturnsBadRequestObjectResult()
        {
            controller.ModelState.AddModelError("test", "Testing invalid Model State.");
            taskServiceMock.Setup(mock => mock.InsertAsync(It.IsAny<ITask>()))
                .ReturnsAsync(1234L);
            var result = await controller.Post(new TaskBindingModel { ActionType = 0 });

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Post_Exception_ReturnsObjectResult()
        {
            taskServiceMock.Setup(mock => mock.InsertAsync(It.IsAny<ITask>())).Throws<Exception>();
            var result = await controller.Post(new TaskBindingModel { ActionType = 0 });

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
        }

        [TestMethod]
        public async Task Patch_ValidTask_CallsUpdateWithNewIdOnce()
        {
            taskMock.SetupProperty(task => task.UserId, 10);
            taskServiceMock.Setup(mock => mock.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(taskMock.Object);
            taskServiceMock.Setup(mock => mock.UpdateAsync(It.IsAny<ITask>()))
                .ReturnsAsync(true);

            JsonPatchDocument<ITask> patch = new JsonPatchDocument<ITask>();
            patch.Replace(task => task.UserId, 15);

            var result = await controller.Patch(1, patch);

            taskServiceMock.Verify(mock => mock.UpdateAsync(It.Is<ITask>(task => task.UserId == 15)), Times.Once());
        }

        [TestMethod]
        public async Task Patch_InvalidModelState_ReturnsBadRequestObjectResult()
        {
            controller.ModelState.AddModelError("test", "Testing invalid Model State.");

            taskMock.SetupProperty(task => task.UserId, 10);
            taskServiceMock.Setup(mock => mock.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(taskMock.Object);
            taskServiceMock.Setup(mock => mock.UpdateAsync(It.IsAny<ITask>()))
                .ReturnsAsync(true);
            JsonPatchDocument<ITask> patch = new JsonPatchDocument<ITask>();
            patch.Replace(task => task.UserId, 15);
            var result = await controller.Patch(1, patch);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Patch_Exception_ReturnsObjectResult()
        {
            taskMock.SetupProperty(task => task.UserId, 10);
            taskServiceMock.Setup(mock => mock.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(taskMock.Object);
            taskServiceMock.Setup(mock => mock.UpdateAsync(It.IsAny<ITask>()))
                .Throws<Exception>();
            JsonPatchDocument<ITask> patch = new JsonPatchDocument<ITask>();
            patch.Replace(task => task.UserId, 15);
            var result = await controller.Patch(1, patch);

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
        }
    }
}
