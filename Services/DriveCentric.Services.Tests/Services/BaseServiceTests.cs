using DriveCentric.Core.Models;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DriveCentric.Services.Tests.Services
{
    [TestClass]
    public class BaseServiceTests
    {
        private class TestValidator : AbstractValidator<UserTask>
        {
            public TestValidator()
            {
                RuleFor(x => x != null);
            }
        }

        //[TestMethod]
        //public void BaseService_AddTask_ReturnsSuccess()
        //{
        //    var cia = new Mock<IContextInfoAccessor>();
        //    var uow = new Mock<IUnitOfWork>();
        //    var service = new BaseService<UserTask>(cia.Object, uow.Object, new TestValidator());
        //    var task = new UserTask { ActionType = Model.Enums.ActionType.SendEmail, UserId = 5810, DateDue = DateTime.Now.AddDays(1) };

        //    var result = service.InsertAsync(task);

        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ValidationException))]
        //public async Task BaseService_AddTask_ReturnsError()
        //{
        //    var cia = new Mock<IContextInfoAccessor>();
        //    var uow = new Mock<IUnitOfWork>();
        //    uow.Setup(x => x.Insert(It.IsAny<UserTask>())).Throws(new ValidationException("Test"));
        //    var service = new BaseService<UserTask>(cia.Object, uow.Object, new TestValidator());
        //    var task = new UserTask { ActionType = Model.Enums.ActionType.SendEmail, UserId = 5810 };

        //    var result = await service.InsertAsync(task);
        //}

        //[TestMethod]
        //public void BaseService_UpdateTask_ReturnsSuccess()
        //{
        //    var cia = new Mock<IContextInfoAccessor>();
        //    var uow = new Mock<IUnitOfWork>();
        //    var service = new BaseService<UserTask>(cia.Object, uow.Object, new TestValidator());
        //    var task = new UserTask { ActionType = Model.Enums.ActionType.SendEmail, UserId = 5810, DateDue = DateTime.Now.AddDays(1) };

        //    var result = service.UpdateAsync(task);

        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ValidationException))]
        //public async Task BaseService_UpdateTask_ReturnsError()
        //{
        //    var cia = new Mock<IContextInfoAccessor>();
        //    var uow = new Mock<IUnitOfWork>();
        //    uow.Setup(x => x.Update(It.IsAny<UserTask>())).Throws(new ValidationException("Test"));
        //    var service = new BaseService<UserTask>(cia.Object, uow.Object, new TestValidator());
        //    var task = new UserTask { ActionType = Model.Enums.ActionType.SendEmail, UserId = 5810 };

        //    var result = await service.UpdateAsync(task);
        //}

        //[TestMethod]
        //public void BaseService_DeleteTask_ReturnsSuccess()
        //{
        //    var cia = new Mock<IContextInfoAccessor>();
        //    var uow = new Mock<IUnitOfWork>();
        //    var service = new BaseService<UserTask>(cia.Object, uow.Object, new TestValidator());

        //    var result = service.DeleteAsync(1);

        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ValidationException))]
        //public async Task BaseService_DeleteTask_ReturnsError()
        //{
        //    var cia = new Mock<IContextInfoAccessor>();
        //    var uow = new Mock<IUnitOfWork>();
        //    uow.Setup(x => x.Delete<UserTask>(It.IsAny<int>())).Throws(new ValidationException("Test"));
        //    var service = new BaseService<UserTask>(cia.Object, uow.Object, new TestValidator());

        //    var result = await service.DeleteAsync(1);
        //}
    }
}