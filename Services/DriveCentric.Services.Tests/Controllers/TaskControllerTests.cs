using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Tasks = System.Threading.Tasks;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Services.Tests.Helpers;
using DriveCentric.TaskService.Controllers;
//using DriveCentric.TaskService.Services;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DriveCentric.Model.Enums;

namespace DriveCentric.Services.Tests.Controllers
{
    //[TestClass]
    //public class TaskControllerTests
    //{
    //    private TaskController controller;
    //    private Mock<ITaskService> taskServiceMock;
    //    private Mock<IHttpContextAccessor> httpContextAccessorMock;
    //    private Mock<IContextInfoAccessor> contextInfoAccessorMock;
    //    private Task testTask;

    //    [TestInitialize]
    //    public void TestInitialize()
    //    {
    //        httpContextAccessorMock = new Mock<IHttpContextAccessor>();
    //        contextInfoAccessorMock = new Mock<IContextInfoAccessor>();
    //        taskServiceMock = new Mock<ITaskService>();
    //        controller = new TaskController(
    //            httpContextAccessorMock.Object,
    //            contextInfoAccessorMock.Object,
    //            taskServiceMock.Object)
    //        { ControllerContext = ControllerContextHelper.CreateControllerContext() };
    //        testTask = new Task { Id = 1, ActionType = ActionType.Reminder };
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Get_ValidTask_ReturnsOk()
    //    {
    //        taskServiceMock.Setup(mock =>
    //            mock.GetSingleByExpressionAsync(It.IsAny<Expression<Func<Task, bool>>>(), It.IsAny<string[]>()))
    //            .ReturnsAsync(new DataResponse<Task> {  Data = testTask, TotalResults = 1, IsSuccessful = true });
    //        var result = await controller.Get(1);
    //        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Get_EntityNotFound_ReturnsNotFound()
    //    {
    //        taskServiceMock.Setup(mock =>
    //            mock.GetSingleByExpressionAsync(It.IsAny<Expression<Func<Task, bool>>>(), It.IsAny<string[]>()))
    //            .ReturnsAsync(new DataResponse<Task> { Data = null, TotalResults = 0, IsSuccessful = true });
    //        var result = await controller.Get(1);

    //        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Get_KeyNotFound_ReturnsObjectResult()
    //    {
    //        taskServiceMock.Setup(mock =>
    //            mock.GetSingleByExpressionAsync(It.IsAny<Expression<Func<Task, bool>>>(), It.IsAny<string[]>()))
    //            .Throws<KeyNotFoundException>();
    //        var result = await controller.Get(1);

    //        Assert.IsInstanceOfType(result, typeof(ObjectResult));
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Get_Exception_ReturnsObjectResult()
    //    {
    //        taskServiceMock.Setup(mock =>
    //            mock.GetSingleByExpressionAsync(It.IsAny<Expression<Func<Task, bool>>>(), It.IsAny<string[]>()))
    //            .Throws<Exception>();
    //        var result = await controller.Get(1);

    //        Assert.IsInstanceOfType(result, typeof(ObjectResult));
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Get_InvalidModelState_ReturnsBadRequestObjectResult()
    //    {
    //        controller.ModelState.AddModelError("test", "Testing invalid Model State.");
    //        var result = await controller.Get(1);
    //        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Delete_ValidTask_ReturnsOk()
    //    {
    //        taskServiceMock.Setup(mock =>
    //            mock.DeleteAsync(It.IsAny<int>()))
    //            .ReturnsAsync(new DataResponse<bool> { Data = true });
    //        var result = await controller.Delete(1);

    //        Assert.IsInstanceOfType(result, typeof(OkObjectResult));

    //        var okResult = result as OkObjectResult;
    //        Assert.IsTrue(((IDataResponse<bool>)okResult.Value).Data);
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Delete_TaskNotExists_ReturnsOk()
    //    {
    //        taskServiceMock.Setup(mock =>
    //            mock.DeleteAsync(It.IsAny<int>()))
    //            .ReturnsAsync(new DataResponse<bool> { Data = false });
    //        var result = await controller.Delete(1);

    //        Assert.IsInstanceOfType(result, typeof(OkObjectResult));

    //        var okResult = result as OkObjectResult;
    //        Assert.IsFalse(((IDataResponse<bool>)okResult.Value).Data);
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Delete_NullReferenceException_Returns()
    //    {
    //        taskServiceMock.Setup(mock =>
    //            mock.DeleteAsync(It.IsAny<int>()))
    //            .Throws<NullReferenceException>();
    //        var result = await controller.Delete(1);

    //        Assert.IsInstanceOfType(result, typeof(ObjectResult));
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Delete_InvalidModelState_ReturnsBadRequestObjectResult()
    //    {
    //        controller.ModelState.AddModelError("test", "Testing invalid Model State.");
    //        var result = await controller.Delete(1);
    //        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Post_ValidTask_ReturnsOk()
    //    {
    //        taskServiceMock.Setup(mock =>
    //            mock.InsertAsync(It.IsAny<Task>()))
    //            .ReturnsAsync(new DataResponse<long> { Data = 1234L });
    //        var result = await controller.Post(new Task { ActionType = 0 });
    //        var okResult = result as OkObjectResult;
    //        Assert.AreEqual(1234L, ((IDataResponse<long>)okResult.Value).Data);
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Post_InvalidModelState_ReturnsBadRequestObjectResult()
    //    {
    //        controller.ModelState.AddModelError("test", "Testing invalid Model State.");
    //        taskServiceMock.Setup(mock =>
    //            mock.InsertAsync(It.IsAny<Task>()))
    //            .ReturnsAsync(new DataResponse<long> { Data = 1234L });
    //        var result = await controller.Post(new Task { ActionType = 0 });

    //        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Post_Exception_ReturnsObjectResult()
    //    {
    //        taskServiceMock.Setup(mock =>
    //            mock.InsertAsync(It.IsAny<Task>()))
    //            .Throws<Exception>();
    //        var result = await controller.Post(new Task { ActionType = 0 });

    //        Assert.IsInstanceOfType(result, typeof(ObjectResult));
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Patch_ValidTask_CallsUpdateWithNewIdOnce()
    //    {
    //        taskServiceMock.Setup(mock =>
    //            mock.GetSingleByExpressionAsync(It.IsAny<Expression<Func<Task, bool>>>(), It.IsAny<string[]>()))
    //            .ReturnsAsync(new DataResponse<Task> { Data = testTask });
    //        taskServiceMock.Setup(mock =>
    //            mock.UpdateAsync(It.IsAny<Task>()))
    //            .ReturnsAsync(new DataResponse<bool> { Data = true });

    //        JsonPatchDocument<Task> patch = new JsonPatchDocument<Task>();
    //        patch.Replace(task => task.UserId, 15);

    //        var result = await controller.Patch(1, patch);

    //        taskServiceMock.Verify(mock => mock.UpdateAsync(It.Is<Task>(task => task.UserId == 15)), Times.Once());
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Patch_InvalidModelState_ReturnsBadRequestObjectResult()
    //    {
    //        controller.ModelState.AddModelError("test", "Testing invalid Model State.");

    //        taskServiceMock.Setup(mock =>
    //            mock.GetSingleByExpressionAsync(It.IsAny<Expression<Func<Task, bool>>>(), It.IsAny<string[]>()))
    //            .ReturnsAsync(new DataResponse<Task> { Data = testTask });
    //        taskServiceMock.Setup(mock =>
    //            mock.UpdateAsync(It.IsAny<Task>()))
    //            .ReturnsAsync(new DataResponse<bool> { Data = true });
    //        JsonPatchDocument<Task> patch = new JsonPatchDocument<Task>();
    //        patch.Replace(task => task.UserId, 15);
    //        var result = await controller.Patch(1, patch);

    //        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    //    }

    //    [TestMethod]
    //    public async Tasks.Task Patch_Exception_ReturnsObjectResult()
    //    {
    //        taskServiceMock.Setup(mock =>
    //            mock.GetSingleByExpressionAsync(It.IsAny<Expression<Func<Task, bool>>>(), It.IsAny<string[]>()))
    //            .ReturnsAsync(new DataResponse<Task> { Data = testTask });
    //        taskServiceMock.Setup(mock =>
    //            mock.UpdateAsync(It.IsAny<Task>()))
    //            .ReturnsAsync(new DataResponse<bool> { Data = true });
    //        JsonPatchDocument<Task> patch = new JsonPatchDocument<Task>();
    //        patch.Replace(task => task.UserId, 15);
    //        var result = await controller.Patch(1, patch);

    //        Assert.IsInstanceOfType(result, typeof(ObjectResult));
    //    }
    //}
}
