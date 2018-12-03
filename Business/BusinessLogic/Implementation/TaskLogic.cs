using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using DriveCentric.Model.Interfaces;
using System;
using System.Collections.Generic;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class TaskLogic : LogicBase<Model.Task>
    {
        public TaskLogic(IContextInfoAccessor contextInfoAccessor, IRepository repository) : base(contextInfoAccessor, repository)
        {
        }

        protected override Model.Task FormatGet(Model.Task entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Model.Task> FormatGet(IEnumerable<Model.Task> entities)
        {
            throw new NotImplementedException();
        }

        protected override bool ValidateAdd(Model.Task entity)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateDelete(Model.Task entity)
        {
            throw new NotImplementedException();
        }

        protected override bool ValidateUpdate(Model.Task entity)
        {
            throw new NotImplementedException();
        }
    }
}
