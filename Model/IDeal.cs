namespace DriveCentric.Model
{
    public interface IDeal : IBaseModel
    {
        Customer Customer { get; set; }
    }
}
