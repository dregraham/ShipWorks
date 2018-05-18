using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Represents the Endicia Api Client
    /// </summary>
    [Service]
    public interface IEndiciaApiClient
    {
        /// <summary>
        /// Generate a scan form for the given shipments
        /// </summary>
        SCANResponse GetScanForm(IEndiciaAccountEntity account, IEnumerable<IShipmentEntity> shipments);
    }
}
