using Interapptive.Shared.Business;
using ShipWorks.Shipping;

namespace ShipWorks.Core.Shipping
{
    public interface IShipmentAdapter
    {
        /// <summary>
        /// Selected shipment type code for the current shipment
        /// </summary>
        ShipmentTypeCode ShipmentType { get; set; }
        
        /// <summary>
        /// Is the loaded shipment processed?
        /// </summary>
        bool IsProcessed { get; set; }

        /// <summary>
        /// Origin address type that should be used
        /// </summary>
        long OriginAddressType { get; set; }

        /// <summary>
        /// The origin person adapter
        /// </summary>
        PersonAdapter OriginPersonAdapter { get; set; }

        /// <summary>
        /// The destination person adapter
        /// </summary>
        PersonAdapter DestinationPersonAdapter { get; set; }
    }
}