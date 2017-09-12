using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.RateGroupFilters
{
    /// <summary>
    /// Filters out services user has excluded in AmazonSettings.
    /// </summary>
    public class AmazonAvailableServiceTypeRateGroupFilter : IAmazonRateGroupFilter
    {
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly Lazy<List<string>> availableServices;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentTypeManager"></param>
        public AmazonAvailableServiceTypeRateGroupFilter(IShipmentTypeManager shipmentTypeManager)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            availableServices = new Lazy<List<string>>(GetAvailableServices);
        }

        /// <summary>
        /// Filter out excluded services
        /// </summary>
        public RateGroup Filter(RateGroup rateGroup)
        {
            RateGroup filteredRateGroup = new RateGroup(rateGroup.Rates.Where(IsAvailableService))
            {
                Carrier = rateGroup.Carrier,
                OutOfDate = rateGroup.OutOfDate
            };

            foreach (IRateFootnoteFactory factory in rateGroup.FootnoteFactories)
            {
                filteredRateGroup.AddFootnoteFactory(factory);
            }

            return filteredRateGroup;
        }

        /// <summary>
        /// Is service available
        /// </summary>
        private bool IsAvailableService(RateResult rateResult)
        {
            AmazonRateTag tag = rateResult.Tag as AmazonRateTag;
            return tag != null && availableServices.Value.Contains(tag.ShippingServiceId);
        }

        /// <summary>
        /// Gets list of available services
        /// </summary>
        private List<string> GetAvailableServices()
        {
            ShipmentType amazonShipmentType = shipmentTypeManager.Get(ShipmentTypeCode.Amazon);
            return amazonShipmentType.GetAvailableServiceTypes().Select(service => EnumHelper.GetApiValue((AmazonServiceType) service)).ToList();
        }
    }
}
