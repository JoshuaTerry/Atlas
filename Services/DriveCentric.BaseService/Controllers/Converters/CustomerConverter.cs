using System;
using DriveCentric.BaseService.Controllers.BindingModels;
using DriveCentric.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DriveCentric.BaseService.Controllers.Converters
{
    public class CustomerConverter : BaseConverter<ICustomer>
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var item = new CustomerBindingModel();
            serializer.Populate(jsonObject.CreateReader(), item);
            return item;
        }
    }
}
