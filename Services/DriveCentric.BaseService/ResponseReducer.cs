using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DriveCentric.Services.Tests")]

namespace DriveCentric.BaseService
{
    public class ResponseReducer
    {
        private const int MAX_RECURSION_DEPTH = 10;

        /* Fields is a comma delimited list of property names or paths (e.g. State.County) that should be included in the response.
         * Fields can be excluded:  This can be useful to exclude properties like Region.ParentRegion or Region.ChildRegions (because they are recursive.)
         * To exclude a field, prefix it with "^": "^ParentRegion,^ChildRegions".
         * If a field list is nothing but excludes, all other fields will be included.
         * To exclude a field from a referenced entity: "ChildRegion.^ParentRegion"
         * To force all fields to be included:  "*,ChildRegion.Code"
         */

        internal IDataResponse ToDynamicResponse<T>(IDataResponse<T> response, string fields = null)
        {
            var dynamicResponse = new DataResponse<dynamic>
            {
                ErrorMessages = response.ErrorMessages,
                IsSuccessful = response.IsSuccessful,
                TotalResults = response.TotalResults,
                VerboseErrorMessages = response.VerboseErrorMessages
            };

            if (response.Data == null)
            {
                dynamicResponse.Data = null;
            }
            else if (response.Data is IEnumerable<IBaseModel>)
            {
                dynamicResponse.Data = ToDynamicList(response.Data as IEnumerable<IBaseModel>, fields);
            }
            else if (response.Data is IBaseModel)
            {
                dynamicResponse.Data = ToDynamicObject(response.Data as IBaseModel, fields);
            }
            else if (IsSimple(response.Data.GetType()))
            {
                dynamicResponse.Data = response.Data;
            }

            return dynamicResponse;
        }

        internal bool IsSimple(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal);
        }

        internal dynamic ToDynamicList<T>(IEnumerable<T> data, string fields = null)
            where T : IBaseModel
        {
            fields = string.IsNullOrWhiteSpace(fields) ? null : fields;

            if (fields == null)
                return data;

            string upperCaseFields = fields?.ToUpper();
            List<string> listOfFields = upperCaseFields?.Split(',').ToList() ?? new List<string>();

            return RecursivelyReduce(data, listOfFields);
        }

        internal dynamic ToDynamicObject<T>(T data, string fields = null)
            where T : IBaseModel
        {
            fields = string.IsNullOrWhiteSpace(fields) ? null : fields;

            if (fields == null)
                return data;

            string upperCaseFields = fields?.ToUpper();
            List<string> listOfFields = upperCaseFields?.Split(',').ToList() ?? new List<string>();

            return RecursivelyReduce(data, listOfFields);
        }

        private dynamic RecursivelyReduce<T>(T data, List<string> fieldsToInclude = null, IEnumerable<int> visited = null, int level = 0)
            where T : IBaseModel
        {
            if (level >= MAX_RECURSION_DEPTH)
                return null;

            dynamic returnObject = new ExpandoObject();
            if (visited == null)
            {
                visited = new List<int> { data.Id };
            }
            else if (visited.Contains(data.Id))
            {
                // To avoid infinite loops, if we have processed this object before, just return the Id
                ((IDictionary<string, object>)returnObject)["Id"] = data.Id;
                return returnObject;
            }
            else
            {
                visited = visited.Union(new int[] { data.Id });
            }

            PropertyInfo[] properties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return ProcessProperties(data, properties, fieldsToInclude, visited, level);
        }

        private ExpandoObject ProcessProperties<T>(T data, PropertyInfo[] properties, List<string> fieldsToInclude = null, IEnumerable<int> visited = null, int level = 0)
        {
            dynamic item = new ExpandoObject();
            // Determine if there are any fields being excluded.
            int excludesCount = fieldsToInclude.Count(p => p.Contains(PathHelper.FieldExcludePrefix));
            bool hasExcludes = (excludesCount > 0);

            // Include all fields if field list is empty, or field list contains only fields to exclude.
            bool includeEverything =
                fieldsToInclude == null || fieldsToInclude.Count == 0 || excludesCount == fieldsToInclude.Count || fieldsToInclude.Contains(PathHelper.IncludeEverythingField);

            foreach (PropertyInfo property in properties)
            {
                object fieldValue = property.GetValue(data, null);
                string propertyNameUppercased = property.Name.ToUpper();
                string propertyNameExclude = PathHelper.FieldExcludePrefix + propertyNameUppercased;
                string propertyNameAsAccessor = propertyNameUppercased + ".";

                if (fieldValue is IEnumerable<IBaseModel> && ((includeEverything && (!hasExcludes && fieldsToInclude.Contains(propertyNameExclude)))
                                          || fieldsToInclude.Any(a => a.StartsWith(propertyNameAsAccessor))))
                {
                    var strippedFieldList = StripFieldList(fieldsToInclude, propertyNameUppercased);
                    ((IDictionary<string, object>)item)[property.Name] = RecursivelyReduce(fieldValue as IEnumerable<IBaseModel>, strippedFieldList, visited, level + 1);
                }
                else if (fieldValue is IBaseModel && ((includeEverything && (!hasExcludes && fieldsToInclude.Contains(propertyNameExclude)))
                                          || fieldsToInclude.Any(a => a.StartsWith(propertyNameAsAccessor))))
                {
                    var strippedFieldList = StripFieldList(fieldsToInclude, propertyNameUppercased);
                    ((IDictionary<string, object>)item)[property.Name] = RecursivelyReduce(fieldValue as IBaseModel, strippedFieldList, visited, level + 1);
                }
                else if ((includeEverything && !(hasExcludes && fieldsToInclude.Contains(propertyNameExclude)))
                                          || fieldsToInclude.Contains(propertyNameUppercased))
                {
                    ((IDictionary<string, object>)item)[property.Name] = fieldValue;
                }
            }

            return item;
        }

        private List<string> StripFieldList(List<string> fieldsToInclude, string propertyNameUppercased)
        {
            if (fieldsToInclude.Count == 0)
            {
                return fieldsToInclude;
            }
            string currentProperty = $"{propertyNameUppercased}.";
            int length = currentProperty.Length;
            return fieldsToInclude.Where(a => a.StartsWith(currentProperty)).Select(a => a.Substring(length)).ToList();
        }

        private dynamic RecursivelyReduce<T>(IEnumerable<T> entities, List<string> fieldsToInclude = null, IEnumerable<int> visited = null, int level = 0)
            where T : IBaseModel
        {
            if (level >= MAX_RECURSION_DEPTH)
            {
                return null;
            }

            var list = new List<ExpandoObject>();

            foreach (var item in entities)
            {
                list.Add(RecursivelyReduce(item, fieldsToInclude, visited, level + 1));
            }

            return list;
        }
    }
}