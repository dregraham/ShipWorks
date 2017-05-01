using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.ServiceFilters
{
    public class PackageServiceFilterTest
    {
        private readonly List<UpsServiceType> services = new List<UpsServiceType>
        {
            UpsServiceType.UpsGround,
            UpsServiceType.Ups3DaySelect,
            UpsServiceType.Ups2DayAirAM,
            UpsServiceType.Ups2DayAir,
            UpsServiceType.UpsNextDayAirAM,
            UpsServiceType.UpsNextDayAir,
            UpsServiceType.UpsNextDayAirSaver
        };

        [Theory]
        // Ups Ground
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.Custom, true)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.Letter, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.Tube, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.Pak, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.BoxExpressSmall, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.BoxExpressMedium, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.BoxExpressLarge, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.Box25Kg, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.Box10Kg, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.FirstClassMail, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.PriorityMail, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.BPMFlats, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.BPMParcels, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.Irregulars, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.Machinables, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.MediaMail, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.ParcelPost, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.StandardFlats, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.Flats, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.BPM, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.Parcels, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.BoxExpress, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.ExpressEnvelope, false)]

        // 3 day air select
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.Custom, true)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.Letter, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.Tube, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.Pak, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.BoxExpressSmall, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.BoxExpressMedium, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.BoxExpressLarge, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.Box25Kg, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.Box10Kg, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.FirstClassMail, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.PriorityMail, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.BPMFlats, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.BPMParcels, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.Irregulars, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.Machinables, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.MediaMail, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.ParcelPost, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.StandardFlats, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.Flats, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.BPM, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.Parcels, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.BoxExpress, false)]
        [InlineData(UpsServiceType.Ups3DaySelect, UpsPackagingType.ExpressEnvelope, false)]

        // 2 day air AM
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.Custom, true)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.Letter, true)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.Tube, true)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.Pak, true)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.BoxExpressSmall, true)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.BoxExpressMedium, true)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.BoxExpressLarge, true)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.Box25Kg, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.Box10Kg, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.FirstClassMail, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.PriorityMail, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.BPMFlats, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.BPMParcels, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.Irregulars, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.Machinables, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.MediaMail, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.ParcelPost, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.StandardFlats, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.Flats, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.BPM, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.Parcels, false)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.BoxExpress, true)]
        [InlineData(UpsServiceType.Ups2DayAirAM, UpsPackagingType.ExpressEnvelope, true)]

        // 2 day air
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.Custom, true)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.Letter, true)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.Tube, true)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.Pak, true)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.BoxExpressSmall, true)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.BoxExpressMedium, true)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.BoxExpressLarge, true)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.Box25Kg, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.Box10Kg, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.FirstClassMail, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.PriorityMail, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.BPMFlats, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.BPMParcels, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.Irregulars, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.Machinables, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.MediaMail, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.ParcelPost, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.StandardFlats, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.Flats, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.BPM, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.Parcels, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.BoxExpress, true)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.ExpressEnvelope, true)]

        // Next day air early
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Custom, true)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Letter, true)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Tube, true)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Pak, true)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.BoxExpressSmall, true)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.BoxExpressMedium, true)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.BoxExpressLarge, true)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Box25Kg, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Box10Kg, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.FirstClassMail, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.PriorityMail, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.BPMFlats, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.BPMParcels, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Irregulars, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Machinables, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.MediaMail, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.ParcelPost, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.StandardFlats, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Flats, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.BPM, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Parcels, false)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.BoxExpress, true)]
        [InlineData(UpsServiceType.UpsNextDayAirAM, UpsPackagingType.ExpressEnvelope, true)]

        // Next day air
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.Custom, true)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.Letter, true)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.Tube, true)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.Pak, true)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.BoxExpressSmall, true)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.BoxExpressMedium, true)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.BoxExpressLarge, true)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.Box25Kg, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.Box10Kg, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.FirstClassMail, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.PriorityMail, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.BPMFlats, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.BPMParcels, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.Irregulars, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.Machinables, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.MediaMail, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.ParcelPost, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.StandardFlats, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.Flats, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.BPM, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.Parcels, false)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.BoxExpress, true)]
        [InlineData(UpsServiceType.UpsNextDayAir, UpsPackagingType.ExpressEnvelope, true)]

        // Next day air saver
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Custom, true)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Letter, true)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Tube, true)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Pak, true)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.BoxExpressSmall, true)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.BoxExpressMedium, true)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.BoxExpressLarge, true)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Box25Kg, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Box10Kg, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.FirstClassMail, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.PriorityMail, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.BPMFlats, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.BPMParcels, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Irregulars, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Machinables, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.MediaMail, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.ParcelPost, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.StandardFlats, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Flats, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.BPM, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Parcels, false)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.BoxExpress, true)]
        [InlineData(UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.ExpressEnvelope, true)]
        public void GetEligibleServices_FiltersCorrectlyForSinglePackageShipments(UpsServiceType serviceType, UpsPackagingType packagingType, bool expectedResult)
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var testObject = mock.Create<PackageServiceFilter>();

                var shipment = CreateSinglePackageShipmentWithPackagingType(packagingType);

                var eligibleServices = testObject.GetEligibleServices(shipment, services);
                Assert.Equal(expectedResult, eligibleServices.Contains(serviceType));
            }
        }

        [Theory]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.Custom, UpsPackagingType.Custom, true)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.Custom, UpsPackagingType.BoxExpressMedium, false)]
        [InlineData(UpsServiceType.UpsGround, UpsPackagingType.BoxExpressLarge, UpsPackagingType.BoxExpressMedium, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.Custom, UpsPackagingType.Custom, true)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.BoxExpressMedium, UpsPackagingType.BoxExpressLarge, true)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.BoxExpressLarge, UpsPackagingType.Box10Kg, false)]
        [InlineData(UpsServiceType.Ups2DayAir, UpsPackagingType.ParcelPost, UpsPackagingType.Machinables, false)]
        public void GetEligibleServices_FiltersCorrectlyForMultiPackageShipments(UpsServiceType serviceType, UpsPackagingType packagingType1, UpsPackagingType packagingType2, bool expectedResult)
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var testObject = mock.Create<PackageServiceFilter>();

                var shipment = CreateSinglePackageShipmentWithPackagingType(packagingType1);
                shipment.Packages.Add(new UpsPackageEntity{PackagingType = (int) packagingType2});

                var eligibleServices = testObject.GetEligibleServices(shipment, services);
                Assert.Equal(expectedResult, eligibleServices.Contains(serviceType));
            }
        }

        private static UpsShipmentEntity CreateSinglePackageShipmentWithPackagingType(UpsPackagingType packagingType)
        {
            var shipment = new UpsShipmentEntity
            {
                Packages =
                {
                    new UpsPackageEntity
                    {
                        PackagingType = (int) packagingType
                    }
                }
            };
            return shipment;
        }
    }
}