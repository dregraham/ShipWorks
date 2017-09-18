using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using Autofac.Features.Indexed;

namespace ShipWorks.Shipping.Carriers.Amazon.RateGroupFilters
{
    /// <summary>
    /// Filters out services user has excluded in AmazonSettings.
    /// </summary>
    public class AmazonAvailableServiceTypeRateGroupFilter : IAmazonRateGroupFilter
    {
        private readonly IAmazonServiceTypeRepository serviceTypeRepository;
        private readonly Lazy<List<string>> availableServices;
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;

        public IIndex<ShipmentTypeCode, ShipmentType> ShipmentTypeFactory { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonAvailableServiceTypeRateGroupFilter(IAmazonServiceTypeRepository serviceTypeRepository, 
            IExcludedServiceTypeRepository excludedServiceTypeRepository, 
            IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory)
        {
            this.serviceTypeRepository = serviceTypeRepository;
            availableServices = new Lazy<List<string>>(GetAvailableServices);
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
            ShipmentTypeFactory = shipmentTypeFactory;
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
            IEnumerable<int> availableServiceTypes = ShipmentTypeFactory[ShipmentTypeCode.Amazon].GetAvailableServiceTypes();
            return serviceTypeRepository.Get()
                .Where(knownServiceType => availableServiceTypes.Contains(knownServiceType.AmazonServiceTypeID))
                .Select(s => s.ApiValue).ToList();
        }
    }
}
