using DriveCentric.BusinessLogic.Implementation;
using DriveCentric.Core.Models;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace DriveCentric.BusinessLogic.CoreTests.Implementation
{
    [TestClass]
    public class TaskValidatorTests
    {
        [TestMethod]
        public async Task TaskValidator_InsertValidation_ReturnsSuccess()
        {
            var validator = new TaskValidator();
            var task = new UserTask { ActionType = Model.Enums.ActionType.SendEmail, UserId = 5810, DateDue = DateTime.Now.AddDays(1) };

            await validator.ValidateAndThrowAsync(task, "Insert");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task TaskValidator_InsertValidation_ThrowsException()
        {
            var validator = new TaskValidator();
            var task = new UserTask { ActionType = Model.Enums.ActionType.SendEmail, UserId = 5810 };

            await validator.ValidateAndThrowAsync(task, "Insert");
        }

        [TestMethod]
        public async Task TaskValidator_UpdateValidation_ReturnsSuccess()
        {
            var validator = new TaskValidator();
            var task = new UserTask { Id = 1, ActionType = Model.Enums.ActionType.SendEmail, UserId = 5810, DateDue = DateTime.Now.AddDays(1) };

            await validator.ValidateAndThrowAsync(task, "Update");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task TaskValidator_UpdateValidation_ThrowsException()
        {
            var validator = new TaskValidator();
            var task = new UserTask { ActionType = Model.Enums.ActionType.SendEmail, UserId = 5810 };

            await validator.ValidateAndThrowAsync(task, "Update");
        }

        [TestMethod]
        public async Task TaskValidator_DeleteValidation_ReturnsSuccess()
        {
            var validator = new TaskValidator();
            var task = new UserTask { Id = 1 };

            await validator.ValidateAndThrowAsync(task, "Delete");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task TaskValidator_DeleteValidation_ThrowsException()
        {
            var validator = new TaskValidator();
            var task = new UserTask();

            await validator.ValidateAndThrowAsync(task, "Delete");
        }
    }
}