using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.BaseService.Interfaces;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;

namespace DriveCentric.BaseService.Services
{
    public abstract class BaseService<T> : IContextAccessible, IBaseService<T>
        where T : IBaseModel
    {
        protected readonly IBaseLogic<T> businessLogic;
        protected readonly IContextInfoAccessor contextInfoAccessor;

        public IContextInfoAccessor ContextInfoAccessor { get { return contextInfoAccessor; } }

        protected BaseService(IContextInfoAccessor contextInfoAccessor, IBaseLogic<T> businessLogic)
        {
            this.businessLogic = businessLogic;
            this.contextInfoAccessor = contextInfoAccessor;
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<bool>> DeleteAsync(int id)
            => new DataResponse<bool> { Data = await businessLogic.DeleteAsync(id) };

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<T>> GetSingleByExpressionAsync(
            Expression<Func<T, bool>> predicate = null,
            string[] referenceFields = null)
            => new DataResponse<T> { Data = await businessLogic.GetSingleAsync(predicate, referenceFields), TotalResults = 1 };

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<IEnumerable<T>>> GetAllByExpressionAsync(
            Expression<Func<T, bool>> predicate,
            IPageable paging = null,
            string[] referenceFields = null)
        {
            var (count, data) = await businessLogic.GetAllAsync(predicate, paging, referenceFields);

            return new DataResponse<IEnumerable<T>> { Data = data, TotalResults = count };
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<long>> InsertAsync(T item)
            => new DataResponse<long> { Data = await businessLogic.InsertAsync(item) };

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<bool>> UpdateAsync(T item)
            => new DataResponse<bool> { Data = await businessLogic.UpdateAsync(item) };

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
