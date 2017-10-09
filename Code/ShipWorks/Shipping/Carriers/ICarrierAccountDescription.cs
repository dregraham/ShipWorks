using ShipWorks.Data.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Interface that represents a class for getting ICarrierAccount descriptions
    /// </summary>
    public interface ICarrierAccountDescription
    {
        /// <summary>
        /// Get the default accounts description
        /// </summary>
        string GetDefaultAccountDescription(ICarrierAccount account);
    }
}
