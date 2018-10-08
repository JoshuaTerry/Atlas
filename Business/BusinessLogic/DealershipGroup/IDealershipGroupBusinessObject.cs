using DriveCentric.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DriveCentric.BusinessLogic.DealershipGroup
{
    public interface IDealershipGroupBusinessObject
    {
        Task<IDealershipGroup> GetDealershipGroupAsync(int id);
        Task<bool> DeleteDealershipGroupAsync(int id);
    }
}
