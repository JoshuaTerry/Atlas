using System.Collections.Generic;
using DriveCentric.Core.Models;

namespace DriveCentric.Data.DataRepository.Interfaces
{
    public interface IDriveServerCollection : IDictionary<int, DriveServer>
    {
        string GalaxyConnectionString { get; }
    }
}