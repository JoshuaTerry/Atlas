using DriveCentric.Core.Models;
using System.Collections.Generic;

namespace DriveCentric.Data.DataRepository.Interfaces
{
    public interface IDriveServerCollection : IDictionary<int, DriveServer>
    {
        string GalaxyConnectionString { get; }
    }
}