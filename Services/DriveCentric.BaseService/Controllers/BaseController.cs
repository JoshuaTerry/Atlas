using DriveCentric.BaseService.Context;
using DriveCentric.BaseService.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DriveCentric.BaseService.Controllers
{
    public abstract class BaseController<T> : Controller, IContextAccessible where T : class, IBaseModel
    {
        private readonly ResponseReducer responseReducer;
        private readonly IBaseService<T> service;
        protected virtual string FieldsForAll => string.Empty;
        protected virtual string FieldsForSingle => string.Empty;
        protected virtual string FieldsForList => string.Empty;
        protected virtual string[] ReferenceFields => new string[] { };
        protected IBaseService<T> Service => service;

        public IContextInfoAccessor ContextInfoAccessor { get; }

        protected BaseController(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor,
            IBaseService<T> service)
        {
            responseReducer = new ResponseReducer();
            contextInfoAccessor.ContextInfo = new ContextInfo(httpContextAccessor);
            ContextInfoAccessor = contextInfoAccessor;
            this.service = service;
        }
        public virtual async Task<IActionResult> GetAll(Expression<Func<T, bool>> predicate = null, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = null, string fields = null)
        { 
            var search = new PageableSearch(offset, limit, orderBy); 
            var result = await Service.GetAllByExpressionAsync(predicate, search, ReferenceFields);

            return Ok(FinalizeReponse(result, string.IsNullOrWhiteSpace(fields) ? FieldsForList : fields));
        }

        public virtual async Task<IActionResult> GetSingle(Expression<Func<T, bool>> predicate = null, string fields = null)
        { 
            var result = await Service.GetSingleByExpressionAsync(predicate, ReferenceFields); 
            return Ok(FinalizeReponse(result, string.IsNullOrWhiteSpace(fields) ? FieldsForSingle : fields));
        }

        public virtual async Task<IActionResult> Post(T entity)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state adding new {GetType().Name}.");
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await Service.InsertAsync(entity));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        public virtual async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<T> patch)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state patching {GetType().Name}({id}).");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await Service.GetSingleByExpressionAsync(t => t.Id == id);
                patch.ApplyTo(result.Data, ModelState);

                return Ok(await Service.UpdateAsync(result.Data));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state deleting {GetType().Name}({id}).");
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await Service.DeleteAsync(id));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        protected virtual string[] ConvertFieldList(string fields, string defaultFields = "")
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = defaultFields;
            }
            if (string.Compare(fields, "all", true) == 0)
            {
                fields = FieldsForAll;
            }

            return fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
           
        public virtual IActionResult FinalizeReponse<U>(IDataResponse<U> response, string fields = null)
        {
            try
            {
                if (response.Data == null)
                {
                    if (response.ErrorMessages.Count > 0)
                        return BadRequest(string.Join(",", response.ErrorMessages));
                    else
                        return NotFound();
                }

                if (!response.IsSuccessful) 
                    return BadRequest(string.Join(",", response.ErrorMessages)); 

                var dynamicResponse = responseReducer.ToDynamicResponse(response, fields);

                if (!dynamicResponse.IsSuccessful)
                    throw new Exception(string.Join(", ", dynamicResponse.ErrorMessages));

                return Ok(dynamicResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new Exception(ex.Message));
            }
        }
    } 
}
