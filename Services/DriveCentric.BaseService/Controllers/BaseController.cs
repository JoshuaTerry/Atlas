using DriveCentric.BaseService.Context;
using DriveCentric.BaseService.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace DriveCentric.BaseService.Controllers
{
    public abstract class BaseController<T> : Controller, IContextAccessible where T : class, IBaseModel
    {
        private readonly DynamicTransmogrifier dynamicTransmogrifier;
        private readonly IBaseService<T> service;
        protected virtual string FieldsForAll => string.Empty;
        protected virtual string FieldsForSingle => string.Empty;
        protected virtual string FieldsForList => string.Empty;
        protected IBaseService<T> Service => service;

        public IContextInfoAccessor ContextInfoAccessor { get; }

        protected BaseController(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor,
            IBaseService<T> service)
        {
            dynamicTransmogrifier = new DynamicTransmogrifier();
            contextInfoAccessor.ContextInfo = new ContextInfo(httpContextAccessor);
            ContextInfoAccessor = contextInfoAccessor;
            this.service = service;
        }
        public virtual async Task<IActionResult> GetAll(Expression<Func<T, bool>> predicate = null, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = null, string fields = null)
        {
            var selectedFields = ConvertFieldList(fields, FieldsForList);
            var search = new PageableSearch(offset, limit, orderBy); 
            var result = await Service.GetAllByExpressionAsync(predicate, search, selectedFields);
                   
            return Ok(FinalizeReponse(result, fields));
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

        public IActionResult FinalizeResponse<T1>(IDataResponse<IEnumerable<T1>> response, IPageable search, string fields = null)
            where T1 : class
        {
            try
            {
                if (search == null)
                {
                    search = PageableSearch.Default;
                }

                if (!response.IsSuccessful)
                {
                    return BadRequest(string.Join(", ", response.ErrorMessages));
                }

                var totalCount = response.TotalResults;

                var dynamicResponse = dynamicTransmogrifier.ToDynamicResponse(response, fields);
                if (!dynamicResponse.IsSuccessful)
                {
                    throw new Exception(string.Join(", ", dynamicResponse.ErrorMessages));
                }
                return Ok(dynamicResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new Exception(ex.Message));
            }

        }
        public virtual IActionResult FinalizeReponse(IDataResponse<IEnumerable<T>> response, string fields = null)
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
                {
                    return BadRequest(string.Join(",", response.ErrorMessages));
                }

                var dynamicResponse = dynamicTransmogrifier.ToDynamicResponse(response, fields);
                if (!dynamicResponse.IsSuccessful)
                {
                    throw new Exception(string.Join(", ", dynamicResponse.ErrorMessages));
                }

                return Ok(dynamicResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new Exception(ex.Message));
            }
        }

        public virtual IActionResult FinalizeReponse(IDataResponse<T> response, string fields = null)
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
                {
                    return BadRequest(string.Join(",", response.ErrorMessages));
                }

                var dynamicResponse = dynamicTransmogrifier.ToDynamicResponse(response, fields);
                if (!dynamicResponse.IsSuccessful)
                {
                    throw new Exception(string.Join(", ", dynamicResponse.ErrorMessages));
                }

                return Ok(dynamicResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new Exception(ex.Message));
            }
        }
    }

    public class FieldSpecificSerializer : DefaultContractResolver
    { 
        public string[] Fields { get; private set; }
        public FieldSpecificSerializer(string[] fields)
        {
            Fields = fields;
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).Where(p => Fields.Contains(p.PropertyName)).ToList(); 
        } 
    }
}
