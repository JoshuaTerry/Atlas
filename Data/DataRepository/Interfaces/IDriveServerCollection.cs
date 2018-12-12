using System;
using System.Collections.Generic;
using System.Text;

namespace DriveCentric.Data.DataRepository.Interfaces
{
    public interface IDriveServerCollection
    {
        string GalaxyConnectionString { get; }

        string GetConnectionStringById(int id);
    }
}
