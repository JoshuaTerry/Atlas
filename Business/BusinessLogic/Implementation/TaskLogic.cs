using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class TaskLogic : LogicBase<Task>
    {
        public TaskLogic(IContextInfoAccessor contextInfoAccessor, IRepository repository) : base(contextInfoAccessor, repository)
        {
        }

        public override Task FormatGet(Task entity)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Task> FormatGet(IEnumerable<Task> entities)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateAdd(Task entity)
        {
            ICollection<ValidationResult> results = new List<ValidationResult>();
            var context = new ValidationContext(entity);
            return Validator.TryValidateObject(entity, context, results, true);
        }

        public bool ValidateAdd(Task entity, out ICollection<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);
        }

        public override bool ValidateDelete(Task entity)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUpdate(Task entity)
        {
            throw new NotImplementedException();
        }
    }
}