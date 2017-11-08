using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship
{
    [Component(RegistrationType.Self)]
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
