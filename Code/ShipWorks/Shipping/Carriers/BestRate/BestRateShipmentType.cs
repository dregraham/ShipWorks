using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public class BestRateShipmentType : ShipmentType
    {
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.BestRate; }
        }
        
        public override Editing.ServiceControlBase CreateServiceControl()
        {
            throw new NotImplementedException();
        }

        public override void LoadShipmentData(Data.Model.EntityClasses.ShipmentEntity shipment, bool refreshIfPresent)
        {
            throw new NotImplementedException();
        }

        public override string GetServiceDescription(Data.Model.EntityClasses.ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }

        public override Insurance.InsuranceChoice GetParcelInsuranceChoice(Data.Model.EntityClasses.ShipmentEntity shipment, int parcelIndex)
        {
            throw new NotImplementedException();
        }

        public override void ProcessShipment(Data.Model.EntityClasses.ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }
    }
}
