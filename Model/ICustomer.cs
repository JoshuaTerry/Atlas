namespace DriveCentric.Model
{
    public interface ICustomer : IBaseModel
    {
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
