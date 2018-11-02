using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;
using ServiceStack.Data;

namespace DriveCentric.Data.SqlORM.Repositories
{
    public abstract class BaseDataRepository<T, U> : BaseWithContextInfoAccessor
        where T : IBaseModel where U : T, new()
    {
        protected readonly IDataAccessor dataAccessor;

        protected BaseDataRepository(
            IContextInfoAccessor contextInfoAccessor,
            IDataAccessor dataAccessor
            ) : base(contextInfoAccessor)
        {
            this.dataAccessor = dataAccessor;
        }

        protected U ConvertToDataModel(T item)
        {
            U instance = new U();
            GetAllProperties().ForEach(property => property.SetValue(instance, property.GetValue(item)));
            return instance;
        }

        protected static List<PropertyInfo> GetAllProperties()
        {
            var writableProperties = typeof(T).GetProperties().ToList();
            typeof(T).GetInterfaces().ToList()
                .ForEach(iface => iface.GetProperties().ToList().ForEach(property => writableProperties.Add(property)));
            return writableProperties;
        }
    }
}
