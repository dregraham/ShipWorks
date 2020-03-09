using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Factory for a IDhlExpressLabelClient
    /// </summary>
    public interface IDhlExpressLabelClientFactory
    {
        /// <summary>
        /// Create an IDhlExpressLabelClient
        /// </summary>
        IDhlExpressLabelClient Create(ShipmentEntity shipment);
    }
}
