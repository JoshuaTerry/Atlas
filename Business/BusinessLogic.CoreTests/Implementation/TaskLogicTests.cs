using System;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.Implementation;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DriveCentric.BusinessLogic.CoreTests.Implementation
{
    [TestClass]
    public class TaskLogicTests
    {
        [TestMethod]
        public async Task TaskValidator_InsertValidation_ReturnsSuccess()
        {
            var uow = new Mock<IReadOnlyUnitOfWork>();
            var logic = new TaskLogic(uow.Object);
            var task = new UserTask { ActionType = Model.Enums.ActionType.SendEmail, UserId = 5810, DateDue = DateTime.Now.AddDays(1) };

            await logic.ValidateAndThrowAsync(task, "Insert");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task TaskValidator_InsertValidation_ThrowsException()
        {
            var uow = new Mock<IReadOnlyUnitOfWork>();
            var logic = new TaskLogic(uow.Object);
            var task = new UserTask { ActionType = Model.Enums.ActionType.SendEmail, UserId = 5810 };

            await logic.ValidateAndThrowAsync(task, "Insert");
        }

        [TestMethod]
        public async Task TaskValidator_UpdateValidation_ReturnsSuccess()
        {
            var uow = new Mock<IReadOnlyUnitOfWork>();
            var logic = new TaskLogic(uow.Object);
            var task = new UserTask { Id = 1, ActionType = Model.Enums.ActionType.SendEmail, UserId = 5810, DateDue = DateTime.Now.AddDays(1) };

            await logic.ValidateAndThrowAsync(task, "Update");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task TaskValidator_UpdateValidation_ThrowsException()
        {
            var uow = new Mock<IReadOnlyUnitOfWork>();
            var logic = new TaskLogic(uow.Object);
            var task = new UserTask { ActionType = Model.Enums.ActionType.SendEmail, UserId = 5810 };

            await logic.ValidateAndThrowAsync(task, "Update");
        }

        [TestMethod]
        public async Task TaskValidator_DeleteValidation_ReturnsSuccess()
        {
            var uow = new Mock<IReadOnlyUnitOfWork>();
            var logic = new TaskLogic(uow.Object);
            var task = new UserTask { Id = 1 };

            await logic.ValidateAndThrowAsync(task, "Delete");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task TaskValidator_DeleteValidation_ThrowsException()
        {
            var uow = new Mock<IReadOnlyUnitOfWork>();
            var logic = new TaskLogic(uow.Object);
            var task = new UserTask();

            await logic.ValidateAndThrowAsync(task, "Delete");
        }
    }
}