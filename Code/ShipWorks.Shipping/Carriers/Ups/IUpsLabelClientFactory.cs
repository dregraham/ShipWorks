using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Factory for creating IUpsLabelClients
    /// </summary>
    public interface IUpsLabelClientFactory
    {
        /// <summary>
        /// Get a label client for the given account
        /// </summary>
        IUpsLabelClient GetClient(IUpsAccountEntity account);
    }
}
