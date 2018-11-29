using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using DriveCentric.Model.Interfaces;
using DriveCentric.Data.SqlORM.Repositories;

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

        protected abstract bool ValidateAdd(T entity);
        protected abstract bool ValidateUpdate(T entity);
        protected abstract bool ValidateDelete(T entity);
        protected abstract T FormatGet(T entity);
        protected abstract IEnumerable<T> FormatGet(IEnumerable<T> entities);
    }
}
