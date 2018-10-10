namespace DriveCentric.Model
{
    public interface IDriveServer : IBaseModel
    {
        string ConnectionString { get; set; }
    }
}
