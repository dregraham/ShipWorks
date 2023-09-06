using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Repository for FedEx accounts
    /// </summary>
    public interface IFedExAccountRepository : ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity>
    {
    }
}
