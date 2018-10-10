﻿using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.ServiceLayer.Interfaces;
using DriveCentric.Utilities.Context;

namespace DriveCentric.ServiceLayer.Services
{
    public class DealService : BaseService<IDeal>, IDealService
    {
        public DealService(
            IContextInfoAccessor contextInfoAccessor,
            IDealLogic businessObject
            ) : base(contextInfoAccessor, businessObject)
        { }
    }
}
