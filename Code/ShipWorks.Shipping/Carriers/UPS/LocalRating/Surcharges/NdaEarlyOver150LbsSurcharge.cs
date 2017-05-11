using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    public class NdaEarlyOver150LbsSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        public NdaEarlyOver150LbsSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            if (serviceRate.Service == UpsServiceType.UpsNextDayAirAM)
            {
                int applicablePackages = shipment.Packages.Count(p=>p.BillableWeight>150);
                if (applicablePackages>0)
                {
                    decimal surcharge = applicablePackages * (decimal) surcharges[UpsSurchargeType.NdaEarlyOver150Lbs];
                    serviceRate.AddAmount(surcharge, $"{applicablePackages} NDA Early package(s) over 150LBS");
                }
            }
        }
    }
}
