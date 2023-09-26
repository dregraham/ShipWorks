using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Platform;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Factory for creating Amazon Buy Shipping rate groups
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IFedExRateGroupFactory))]
    public class FedExRateGroupFactory : ShipEngineRateGroupFactory, IFedExRateGroupFactory
    {
        private readonly List<int> _availableFedExServiceTypeIds;

        public FedExRateGroupFactory(List<int> availableFedExServiceTypeIds)
        {
            _availableFedExServiceTypeIds = availableFedExServiceTypeIds;
        }
        protected override RateResult GetRateResult(Rate apiRate, ShipmentTypeCode shipmentType)
        {
            var rateResult = base.GetRateResult(apiRate, shipmentType);
            var fedexServiceType = GetServiceType(apiRate);
            if (fedexServiceType != null)
            {
                rateResult.Tag = fedexServiceType.Value;
                rateResult.Description = EnumHelper.GetDescription(fedexServiceType.Value);
            }

            switch (apiRate.DeliveryDays)
            {
                case 1:
                    rateResult.ServiceLevel = ServiceLevelType.OneDay;
                    break;
                case 2:
                    rateResult.ServiceLevel = ServiceLevelType.TwoDays;
                    break;
                case 3:
                    rateResult.ServiceLevel = ServiceLevelType.ThreeDays;
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                    rateResult.ServiceLevel = ServiceLevelType.FourToSevenDays;
                    break;
            }

            return rateResult;
        }

        private FedExServiceType? GetServiceType(Rate apiRate)
        {
            var serviceCode = apiRate.ServiceCode;
            FedExServiceType? result = null;
            if (serviceCode != null)
            {
                foreach (var serviceId in _availableFedExServiceTypeIds)
                {
                    var fedexServiceType = (FedExServiceType) serviceId;
                    var apiValue = EnumHelper.GetApiValue(fedexServiceType);
                    if (apiValue != serviceCode)
                    {
                        continue;
                    }
                    if (apiRate.PackageType.EndsWith("_onerate"))
                    {
                        if (FedExUtility.IsOneRateService(fedexServiceType))
                        {
                            return fedexServiceType;
                        }
                    }
                    else
                    {
                        if (!FedExUtility.IsOneRateService(fedexServiceType))
                        {
                            return fedexServiceType;
                        }
                    }
                    result = fedexServiceType;//it is best match if exact match will be not found
                }
            }
            return result;
        }
    }
}
