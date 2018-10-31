using System;
using System.Collections.Generic;
using System.Text;
using DriveCentric.Model;

namespace DriveCentric.BaseService.Controllers.BindingModels
{
    public class UserBindingModel : IUser
    {
        public Guid ExternalId { get; set; }
        public Guid GalaxyUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
    }
}
