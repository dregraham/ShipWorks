using Interapptive.Shared;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Shipping.Carriers.UPS
{
    public static class UpsServiceLevelConverter
    {
        /// <summary>
        /// Gets the service level. serviceRate.GuaranteedDays is preferred, 
        /// but we will use transitTime.BusinessDays if GuaranteedDays isn't available.
        /// </summary>
        public static ServiceLevelType GetServiceLevel(UpsServiceRate serviceRate, UpsTransitTime transitTime)
        {
            int? expectedDays = null;

            if (transitTime != null)
            {
                expectedDays = transitTime.BusinessDays;
            }

            if (serviceRate.GuaranteedDaysToDelivery.HasValue)
            {
                expectedDays = serviceRate.GuaranteedDaysToDelivery;
            }

            return GetServiceLevel(serviceRate.Service, expectedDays);
        }

        /// <summary>
        /// Gets the service level.
        /// </summary>
        [NDependIgnoreComplexMethodAttribute]
        public static ServiceLevelType GetServiceLevel(UpsServiceType upsService, int? guaranteedDaysToDelivery)
        {
            switch (upsService)
            {
                case UpsServiceType.Ups3DaySelect:
                case UpsServiceType.Ups3DaySelectFromCanada:
                    return ServiceLevelType.ThreeDays;

                case UpsServiceType.Ups2ndDayAirIntra:
                case UpsServiceType.Ups2DayAirAM:
                case UpsServiceType.Ups2DayAir:
                    return ServiceLevelType.TwoDays;

                case UpsServiceType.UpsNextDayAir:
                case UpsServiceType.UpsNextDayAirSaver:
                case UpsServiceType.UpsNextDayAirAM:
                case UpsServiceType.UpsExpress:
                case UpsServiceType.UpsExpressEarlyAm:
                case UpsServiceType.UpsExpressSaver:
                    return ServiceLevelType.OneDay;

                case UpsServiceType.UpsMailInnovationsFirstClass:
                case UpsServiceType.UpsMailInnovationsPriority:
                case UpsServiceType.UpsMailInnovationsExpedited:
                case UpsServiceType.UpsMailInnovationsIntEconomy:
                case UpsServiceType.UpsMailInnovationsIntPriority:
                case UpsServiceType.UpsSurePostLessThan1Lb:
                case UpsServiceType.UpsSurePost1LbOrGreater:
                case UpsServiceType.UpsSurePostBoundPrintedMatter:
                case UpsServiceType.UpsSurePostMedia:
                    return ServiceLevelType.Anytime;

                default:
                    if (!guaranteedDaysToDelivery.HasValue || guaranteedDaysToDelivery < 0)
                    {
                        return ServiceLevelType.Anytime;
                    }
                    if (guaranteedDaysToDelivery <= 1)
                    {
                        return ServiceLevelType.OneDay;
                    }
                    if (guaranteedDaysToDelivery == 2)
                    {
                        return ServiceLevelType.TwoDays;
                    }
                    if (guaranteedDaysToDelivery == 3)
                    {
                        return ServiceLevelType.ThreeDays;
                    }
                    if (guaranteedDaysToDelivery <= 7)
                    {
                        return ServiceLevelType.FourToSevenDays;
                    }
                    return ServiceLevelType.Anytime;
            }
        }
    }
}
