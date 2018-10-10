namespace DriveCentric.Model
{
    public interface IDeal : IBaseModel
    {
        ICustomer Customer { get; set; }
    }
}
