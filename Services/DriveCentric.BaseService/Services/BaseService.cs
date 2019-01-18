using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.BaseService.Interfaces;
using DriveCentric.BusinessLogic.Implementation;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using FluentValidation;

namespace DriveCentric.BaseService.Services
{
    public class BaseService<T> : IContextAccessible, IBaseService<T>
        where T : class, IBaseModel, new()
    {
        private const string InsertRuleSet = "Insert";

        private const string UpdateRuleSet = "Update";

        private readonly IContextInfoAccessor contextInfoAccessor;

        private readonly IUnitOfWork unitOfWork;

        private readonly LogicBase<T> logic;

        public BaseService(IContextInfoAccessor contextInfoAccessor, IUnitOfWork unitOfWork, LogicBase<T> logic)
        {
            this.unitOfWork = unitOfWork;
            this.contextInfoAccessor = contextInfoAccessor;
            this.logic = logic;
        }

        public IContextInfoAccessor ContextInfoAccessor
        {
            get
            {
                return contextInfoAccessor;
            }
        }

        public virtual async Task<IDataResponse<object>> GetHealthCheck()
            => new DataResponseBase<object>(new { Databases = await unitOfWork.GetDatabaseHealthCheck() });

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<T>> GetSingleByExpressionAsync(Expression<Func<T, bool>> expression = null, string[] referenceFields = null)
        {
            return new DataResponseBase<T> { Data = await unitOfWork.GetEntity(expression, referenceFields), TotalResults = 1 };
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<IEnumerable<T>>> GetAllByExpressionAsync(Expression<Func<T, bool>> expression = null, IPageable paging = null, string[] referenceFields = null)
        {
            var result = await unitOfWork.GetEntities(expression, paging, referenceFields);
            var count = await unitOfWork.GetCount<T>(expression);

            return new DataResponseBase<IEnumerable<T>> { Data = result, TotalResults = count };
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<long>> InsertAsync(T entity)
        {
            await logic.ValidateAndThrowAsync(entity, InsertRuleSet);
            unitOfWork.Insert(entity);

            return new DataResponseBase<long> { Data = await unitOfWork.SaveChanges() };
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<long>> UpdateAsync(T entity)
        {
            await logic.ValidateAndThrowAsync(entity, UpdateRuleSet);
            unitOfWork.Update(entity);

            return new DataResponseBase<long> { Data = await unitOfWork.SaveChanges() };
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<long>> DeleteAsync(int id)
        {
            unitOfWork.Delete<T>(id);
            return new DataResponseBase<long> { Data = await unitOfWork.SaveChanges() };
        }

        public IDataResponse<T> ProcessIDataResponseException(Exception ex)
        {
            var response = new DataResponseBase<T>
            {
                IsSuccessful = false
            };
            response.ErrorMessages.Add(ex.Message);
            response.VerboseErrorMessages.Add(ex.ToString());

            return response;
        }

        public DataResponseBase<T1> ProcessDataResponseException<T1>(Exception ex)
        {
            var response = new DataResponseBase<T1>
            {
                IsSuccessful = false
            };
            response.ErrorMessages.Add(ex.Message);
            response.VerboseErrorMessages.Add(ex.ToString());

            return response;
        }
    }
}