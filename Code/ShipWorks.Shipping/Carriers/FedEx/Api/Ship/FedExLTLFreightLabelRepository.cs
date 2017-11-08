using System;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship
{
    public class FedExLTLFreightLabelRepository : IFedExLabelRepository
    {
        public void ClearReferences(IShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }

        public void SaveLabels(IShipmentEntity shipment, ProcessShipmentReply reply)
        {
            throw new NotImplementedException();
        }
    }
}
