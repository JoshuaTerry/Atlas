namespace DriveCentric.Model
{
    public interface IDriveServer : IBaseExternalIdModel
    {
        string ConnectionString { get; set; }
    }
}
