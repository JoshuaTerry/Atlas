using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.BaseService.Interfaces;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;

namespace DriveCentric.BaseService.Services
{
    public abstract class BaseService<T> : BaseWithContextInfoAccessor, IBaseService<T>
        where T : IBaseModel
    { 
        protected readonly IBaseLogic<T> businessLogic;

        protected BaseService(IContextInfoAccessor contextInfoAccessor, IBaseLogic<T> businessLogic) : base(contextInfoAccessor)
        {
            this.businessLogic = businessLogic;
        }

        [MonitorAsyncAspect]
        public virtual Task<bool> DeleteAsync(int id)
        {
            return businessLogic.DeleteAsync(id);
        }
          
        public virtual async Task<IDataResponse<T>> GetSingleByExpressionAsync(Expression<Func<T, bool>> predicate = null, string[] referenceFields = null)
        { 
            var response = new DataResponse<T>();
            try
            {
                var result = await businessLogic.GetSingleAsync(predicate, referenceFields);
                response.TotalResults = 1;
                response.Data = result;
            }
            catch (Exception ex)
            {
                response = ProcessDataResponseException<T>(ex);
            }
            return response;
        }

        [MonitorAsyncAspect]
        public virtual async Task<IDataResponse<IEnumerable<T>>> GetAllByExpressionAsync(Expression<Func<T, bool>> predicate, IPageable paging = null, string[] referenceFields = null)
        {
            var response = new DataResponse<IEnumerable<T>>();
            try
            { 
                var result = await businessLogic.GetAllAsync(predicate, paging, referenceFields);
                response.TotalResults = result.count;
                response.Data = result.data;
            }
            catch (Exception ex)
            {
                response = ProcessDataResponseException<IEnumerable<T>>(ex);
            }
            return response;
        }
          
        [MonitorAsyncAspect]
        public virtual Task<long> InsertAsync(T item)
        {
            return businessLogic.InsertAsync(item);
        }

        [MonitorAsyncAspect]
        public virtual Task<bool> SaveAsync()
        {
            return businessLogic.SaveAsync();
        }

        [MonitorAsyncAspect]
        public virtual Task<bool> UpdateAsync(T item)
        {
            return businessLogic.UpdateAsync(item);
        }

       
        public DataResponse<T1> GetDataResponse<T1>(Func<T1> funcToExecute)
        {
            try
            {
                var result = funcToExecute();
                var response = new DataResponse<T1>
                {
                    Data = result,
                    IsSuccessful = true
                };
                return response;
            }
            catch (Exception ex)
            { 
                return ProcessDataResponseException<T1>(ex);
            }
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
