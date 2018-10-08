namespace DriveCentric.Model
{
    public interface IDeal : IBaseExternalIdModel
    {
        ICustomer Customer { get; set; }
    }
}
