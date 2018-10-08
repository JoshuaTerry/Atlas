namespace DriveCentric.Model
{
    public interface ICustomer : IBaseExternalIdModel
    {
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
