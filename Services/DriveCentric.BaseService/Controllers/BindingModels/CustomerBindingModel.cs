using DriveCentric.Model;

namespace DriveCentric.BaseService.Controllers.BindingModels
{
    public class CustomerBindingModel : Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
    }
}
