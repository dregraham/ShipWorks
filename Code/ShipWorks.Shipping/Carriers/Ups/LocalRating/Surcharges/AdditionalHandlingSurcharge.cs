using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Surcharge for additional handling
    /// </summary>
    public class AdditionalHandlingSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="surcharges"></param>
        public AdditionalHandlingSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the additional handling surcharge to the rate if applicable 
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            shipment.Packages.Where(NeedsAdditionalHandling).ForEach(p => Apply(serviceRate));
        }

        /// <summary>
        /// Apply the additional handling surcharge to the service rate
        /// </summary>
        /// <param name="serviceRate"></param>
        private void Apply(IUpsLocalServiceRate serviceRate)
        {
            serviceRate.AddAmount((decimal) surcharges[UpsSurchargeType.AdditionalHandling],
                EnumHelper.GetDescription(UpsSurchargeType.AdditionalHandling));
        }

        /// <summary>
        /// Check to see if the shipment needs additional handling
        /// </summary>
        private static bool NeedsAdditionalHandling(UpsPackageEntity package)
        {
            return (package.AdditionalHandlingEnabled ||
                    package.LongestSide > 48 ||
                    package.SecondLongestSize > 30 ||
                    package.TotalWeight > 70) && 
                    !package.IsLargePackage;
        }
    }
}