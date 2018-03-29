using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS
{
    public class UpsServicePackageTypeSettingTest
    {
        [Fact]
        public void UpsPackageTypeChecksValidUpsServiceList_ReturnsExpectedResult()
        {
            var actualServiceTypes = UpsServicePackageTypeSetting.ServicePackageValidationSettings.Where(s =>
                s.PackageType == UpsPackagingType.ExpressEnvelope).Select(x => x.ServiceType);

            List<UpsServiceType> expectedServiceTypes = new List<UpsServiceType>
            {
                UpsServiceType.Ups2DayAir,
                UpsServiceType.UpsNextDayAir,
                UpsServiceType.Ups2DayAirAM,
                UpsServiceType.UpsNextDayAirAM,
                UpsServiceType.Ups2DayAirAM,
                UpsServiceType.UpsNextDayAirSaver,
                UpsServiceType.UpsExpress,
                UpsServiceType.UpsExpressEarlyAm,
                UpsServiceType.UpsExpressSaver,
                UpsServiceType.UpsExpedited,
                UpsServiceType.UpsCaWorldWideExpressSaver,
                UpsServiceType.UpsCaWorldWideExpressPlus,
                UpsServiceType.UpsCaWorldWideExpress
            };

            actualServiceTypes.Except(expectedServiceTypes);

            Assert.Equal(0, actualServiceTypes.Except(expectedServiceTypes).Count());
        }
    }
}