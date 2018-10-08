using System;
using System.Threading.Tasks;
using DriveCentric.Model;

namespace DriveCentric.ServiceLayer.DealershipGroup
{
    public interface IDealershipGroupService
    {
        Task<IDealershipGroup> GetDealershipGroupAsync(int id);

        Task<bool> DeleteDealershipGroupAsync(int id);
    }
}
