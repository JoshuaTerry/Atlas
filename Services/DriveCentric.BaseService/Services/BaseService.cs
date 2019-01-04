using DriveCentric.BaseService.Interfaces;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DriveCentric.BaseService.Services
{
    public class BaseService<T> : IContextAccessible, IBaseService<T> where T : class, IBaseModel, new()
    {
        private const string InsertRuleSet = "Insert";
        private const string UpdateRuleSet = "Update";

        protected readonly IContextInfoAccessor contextInfoAccessor;
        protected readonly IUnitOfWork unitOfWork;
        protected readonly AbstractValidator<T> validator;
        public IContextInfoAccessor ContextInfoAccessor { get { return contextInfoAccessor; } }

        public BaseService(IContextInfoAccessor contextInfoAccessor, IUnitOfWork unitOfWork, AbstractValidator<T> validator)
        {
            this.unitOfWork = unitOfWork;
            this.contextInfoAccessor = contextInfoAccessor;
            this.validator = validator;
        }

        public virtual async Task<IDataResponse<object>> GetHealthCheck()
            => new DataResponse<object>(new { Databases = await unitOfWork.GetDatabaseHealthCheck() });

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<T>> GetSingleByExpressionAsync(Expression<Func<T, bool>> expression = null, string[] referenceFields = null)
        {
            return new DataResponse<T> { Data = await unitOfWork.GetEntity(expression, referenceFields), TotalResults = 1 };
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<IEnumerable<T>>> GetAllByExpressionAsync(Expression<Func<T, bool>> expression, IPageable paging = null, string[] referenceFields = null)
        {
            var result = await unitOfWork.GetEntities(expression, paging, referenceFields);
            var count = await unitOfWork.GetCount<T>(expression);

            return new DataResponse<IEnumerable<T>> { Data = result, TotalResults = count };
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<long>> InsertAsync(T entity)
        {
            await validator.ValidateAndThrowAsync(entity, InsertRuleSet);
            unitOfWork.Insert(entity);

            return new DataResponse<long> { Data = await unitOfWork.SaveChanges() };
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<long>> UpdateAsync(T entity)
        {
            await validator.ValidateAndThrowAsync(entity, UpdateRuleSet);
            unitOfWork.Update(entity);

            return new DataResponse<long> { Data = await unitOfWork.SaveChanges() };
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<long>> DeleteAsync(int id)
        {
            unitOfWork.Delete<T>(id);
            return new DataResponse<long> { Data = await unitOfWork.SaveChanges() };
        }

        public IDataResponse<T> ProcessIDataResponseException(Exception ex)
        {
            var response = new DataResponse<T>
            {
                IsSuccessful = false
            };
            response.ErrorMessages.Add(ex.Message);
            response.VerboseErrorMessages.Add(ex.ToString());

            return response;
        }

        public DataResponse<T1> ProcessDataResponseException<T1>(Exception ex)
        {
            var response = new DataResponse<T1>
            {
                IsSuccessful = false
            };
            response.ErrorMessages.Add(ex.Message);
            response.VerboseErrorMessages.Add(ex.ToString());

            return response;
        }
    }
}