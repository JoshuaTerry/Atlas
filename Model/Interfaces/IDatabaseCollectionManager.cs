using System.Collections.Generic;

namespace DriveCentric.Core.Interfaces
{
    public interface IDatabaseCollectionManager
    {
        Dictionary<string, IRepository> Repositories { get; }
    }
}