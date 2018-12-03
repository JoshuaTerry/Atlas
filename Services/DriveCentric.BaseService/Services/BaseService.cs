using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.BaseService.Interfaces;
using DriveCentric.BusinessLogic.Implementation;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Model.Interfaces;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;

namespace DriveCentric.BaseService.Services
{
    public abstract class BaseService<T> : IContextAccessible, IBaseService<T> where T : class, IBaseModel, new() 
    { 
        protected readonly IContextInfoAccessor contextInfoAccessor;
        protected IUnitOfWork UnitOfWork { get; }

        public IContextInfoAccessor ContextInfoAccessor { get { return contextInfoAccessor; } }

        protected BaseService(IContextInfoAccessor contextInfoAccessor, IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork; 
            this.contextInfoAccessor = contextInfoAccessor;
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<long>> DeleteAsync(int id)
        { 
            UnitOfWork.Delete<T>(id); 
            return new DataResponse<long> { Data = await UnitOfWork.SaveChanges() };
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<T>> GetSingleByExpressionAsync(Expression<Func<T, bool>> expression = null, string[] referenceFields = null)
        { 
            return new DataResponse<T> { Data = await UnitOfWork.GetEntity(expression, referenceFields), TotalResults = 1 };
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<IEnumerable<T>>> GetAllByExpressionAsync(Expression<Func<T, bool>> expression, IPageable paging = null, string[] referenceFields = null) 
        {
            var result = await UnitOfWork.GetEntities(expression, paging, referenceFields);
            var count = await UnitOfWork.GetCount<T>(expression);

            return new DataResponse<IEnumerable<T>> { Data = result, TotalResults = count }; 
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<long>> InsertAsync(T entity)
        {
            UnitOfWork.Insert(entity); 
            return new DataResponse<long> { Data = await UnitOfWork.SaveChanges() }; 
        }
        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<long>> UpdateAsync(T entity)
        {
            UnitOfWork.Update(entity);
            return new DataResponse<long> { Data = await UnitOfWork.SaveChanges() };
        }

        public IDataResponse<T> ProcessIDataResponseException(Exception ex)
        {
            var response = new DataResponse<T>();
            response.IsSuccessful = false;
            response.ErrorMessages.Add(ex.Message);
            response.VerboseErrorMessages.Add(ex.ToString()); 

            return response;
        }

        public DataResponse<T1> ProcessDataResponseException<T1>(Exception ex)
        {
            var response = new DataResponse<T1>();
            response.IsSuccessful = false;
            response.ErrorMessages.Add(ex.Message);
            response.VerboseErrorMessages.Add(ex.ToString()); 

            return response; 
        }
    }
}
