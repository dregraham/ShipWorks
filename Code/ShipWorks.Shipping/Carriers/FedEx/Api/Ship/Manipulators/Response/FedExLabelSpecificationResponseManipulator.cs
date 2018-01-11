using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Response
{
    /// <summary>
    /// Set the label type on the shipment
    /// </summary>
    public class FedExLabelSpecificationResponseManipulator : IFedExShipResponseManipulator
    {
        /// <summary>
        /// Manipulate the given shipment
        /// </summary>
        public GenericResult<ShipmentEntity> Manipulate(ProcessShipmentReply response, ProcessShipmentRequest request, ShipmentEntity shipment)
        {
            shipment.FedEx.Shipment.ActualLabelFormat = 
                shipment.RequestedLabelFormat == (int) ThermalLanguage.None || 
                FedExUtility.IsFreightLtlService(shipment.FedEx.Service) ? 
                    null : (int?) shipment.RequestedLabelFormat;
            return shipment;
        }
    }
}
