using System;

namespace DriveCentric.Utilities.Configuration
{
    public interface IDriveServerCollection
    {
        string GetConnectionStringById(int id);
    }
}
