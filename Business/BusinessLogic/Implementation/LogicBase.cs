using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using DriveCentric.Model.Interfaces; 

namespace DriveCentric.BusinessLogic.Implementation
{
    public abstract class LogicBase<T> : IContextAccessible where T : IBaseModel
    {
        public IContextInfoAccessor ContextInfoAccessor { get; }
        private IRepository Repository { get; }
        public LogicBase(IContextInfoAccessor contextInfoAccessor, IRepository repository)
        {
            ContextInfoAccessor = contextInfoAccessor;
            Repository = repository;
        }

        public abstract bool ValidateAdd(T entity);
        public abstract bool ValidateUpdate(T entity);
        public abstract bool ValidateDelete(T entity);
        public abstract T FormatGet(T entity);
        public abstract IEnumerable<T> FormatGet(IEnumerable<T> entities);
    }
}
