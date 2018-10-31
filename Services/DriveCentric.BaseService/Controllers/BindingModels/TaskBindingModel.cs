using System;
using System.ComponentModel.DataAnnotations;
using DriveCentric.BaseService.Controllers.Converters;
using DriveCentric.Model;
using DriveCentric.Model.Enums;
using Newtonsoft.Json;

namespace DriveCentric.BaseService.Controllers.BindingModels
{
    public class TaskBindingModel : ITask
    {
        [JsonConverter(typeof(CustomerConverter))]
        public ICustomer Customer { get; set; }

        [JsonConverter(typeof(UserConverter))]
        public IUser CreatedByUser { get; set; }

        [JsonConverter(typeof(UserConverter))]
        public IUser User { get; set; }

        [Required]
        public ActionType ActionType { get; set; }

        public DateTime? DateDue { get; set; }
        public string Notes { get; set; }
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
    }
}
