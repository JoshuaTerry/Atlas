using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriveCentric.Model;
using DriveCentric.RestApi.Controllers;
using DriveCentric.RestApi.Tests.Helpers;
using DriveCentric.ServiceLayer.Interfaces;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DriveCentric.RestApi.Tests.Controllers
{
    [TestClass]
    public class DealershipGroupControllerTests
    {
        private DealershipGroup controller;
        private Mock<IDealershipGroupService> dealershipGroupServiceMock;
        private Mock<IHttpContextAccessor> httpContextAccessorMock;
        private Mock<IContextInfoAccessor> contextInfoAccessorMock;
        private Mock<IDealershipGroup> dealershipGroupMock;

        [TestInitialize]
        public void TestInitialize()
        {
            httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            contextInfoAccessorMock = new Mock<IContextInfoAccessor>();
            dealershipGroupServiceMock = new Mock<IDealershipGroupService>();
            controller = new DealershipGroup(
                httpContextAccessorMock.Object,
                contextInfoAccessorMock.Object,
                dealershipGroupServiceMock.Object)
            { ControllerContext = ControllerContextHelper.CreateControllerContext() };

            dealershipGroupMock = new Mock<IDealershipGroup>();
        }

        [TestMethod]
        public async Task Get_ValidDealershipGroup_ReturnsOk()
        {
            dealershipGroupServiceMock.Setup(mock => mock.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(dealershipGroupMock.Object);
            var result = await controller.Get(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Get_KeyNotFound_ReturnsNotFound()
        {
            dealershipGroupServiceMock.Setup(mock => mock.GetAsync(It.IsAny<int>())).Throws<KeyNotFoundException>();
            var result = await controller.Get(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Get_Exception_ReturnsStatusCode400()
        {
            dealershipGroupServiceMock.Setup(mock => mock.GetAsync(It.IsAny<int>())).Throws<Exception>();
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
        public async Task Delete_ValidDealershipGroup_ReturnsOk()
        {
            dealershipGroupServiceMock.Setup(mock => mock.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            var result = await controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            Assert.IsTrue((bool)okResult.Value);
        }

        [TestMethod]
        public async Task Delete_DealershipGroupNotExists_ReturnsOk()
        {
            dealershipGroupServiceMock.Setup(mock => mock.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            var result = await controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            Assert.IsFalse((bool)okResult.Value);
        }

        [TestMethod]
        public async Task Delete_NullReferenceException_Returns()
        {
            dealershipGroupServiceMock.Setup(mock => mock.DeleteAsync(It.IsAny<int>()))
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
    }
}
