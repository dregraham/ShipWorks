using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx
{
    public class FedExUtilityTest
    {
        Dictionary<FedExServiceType, List<FedExPackagingType>> expectedPackagingTypes;

        public FedExUtilityTest()
        {
            var standard = new List<FedExPackagingType>()
            {
                FedExPackagingType.Envelope,
                FedExPackagingType.Pak,
                FedExPackagingType.Box,
                FedExPackagingType.Tube,
                FedExPackagingType.Custom,
                FedExPackagingType.SmallBox,
                FedExPackagingType.MediumBox,
                FedExPackagingType.LargeBox,
                FedExPackagingType.ExtraLargeBox
        };

            var international = new List<FedExPackagingType>()
            {
                FedExPackagingType.Envelope,
                FedExPackagingType.Pak,
                FedExPackagingType.Box,
                FedExPackagingType.Tube,
                FedExPackagingType.Box10Kg,
                FedExPackagingType.Box25Kg,
                FedExPackagingType.Custom,
                FedExPackagingType.SmallBox,
                FedExPackagingType.MediumBox,
                FedExPackagingType.LargeBox,
                FedExPackagingType.ExtraLargeBox
            };

            var freight = new List<FedExPackagingType>()
            {
                FedExPackagingType.Custom
            };

            var oneRate = new List<FedExPackagingType>()
            {
                FedExPackagingType.Envelope,
                FedExPackagingType.Pak,
                FedExPackagingType.Tube,
                FedExPackagingType.SmallBox,
                FedExPackagingType.MediumBox,
                FedExPackagingType.LargeBox,
                FedExPackagingType.ExtraLargeBox
            };

            var otherPackageTypes = new List<FedExPackagingType>()
            {
                FedExPackagingType.Custom,
                FedExPackagingType.SmallBox,
                FedExPackagingType.MediumBox,
                FedExPackagingType.LargeBox,
                FedExPackagingType.ExtraLargeBox
            };

            expectedPackagingTypes = new Dictionary<FedExServiceType, List<FedExPackagingType>>();

            expectedPackagingTypes.Add(FedExServiceType.FirstOvernight, standard);
            expectedPackagingTypes.Add(FedExServiceType.StandardOvernight, standard);
            expectedPackagingTypes.Add(FedExServiceType.PriorityOvernight, standard);
            expectedPackagingTypes.Add(FedExServiceType.FedEx2Day, standard);
            expectedPackagingTypes.Add(FedExServiceType.FedEx2DayAM, standard);
            expectedPackagingTypes.Add(FedExServiceType.FedExEconomyCanada, standard);
            expectedPackagingTypes.Add(FedExServiceType.InternationalFirst, standard);
            expectedPackagingTypes.Add(FedExServiceType.InternationalEconomy, standard);
            expectedPackagingTypes.Add(FedExServiceType.FedExExpressSaver, standard);

            expectedPackagingTypes.Add(FedExServiceType.FedExGround, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedEx1DayFreight, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedEx2DayFreight, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedEx3DayFreight, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FirstFreight, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.SmartPost, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.GroundHomeDelivery, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.InternationalPriorityFreight, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.InternationalEconomyFreight, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExInternationalGround, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExNextDayAfternoon, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExNextDayEarlyMorning, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExNextDayMidMorning, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExNextDayEndOfDay, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExDistanceDeferred, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExNextDayFreight, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExFimsMailView, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExFimsMailViewLite, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExFimsPremium, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExFimsStandard, otherPackageTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExEuropeFirstInternationalPriority, otherPackageTypes);
            

            expectedPackagingTypes.Add(FedExServiceType.OneRate2Day, oneRate);
            expectedPackagingTypes.Add(FedExServiceType.OneRate2DayAM, oneRate);
            expectedPackagingTypes.Add(FedExServiceType.OneRateExpressSaver, oneRate);
            expectedPackagingTypes.Add(FedExServiceType.OneRateFirstOvernight, oneRate);
            expectedPackagingTypes.Add(FedExServiceType.OneRatePriorityOvernight, oneRate);
            expectedPackagingTypes.Add(FedExServiceType.OneRateStandardOvernight, oneRate);

            expectedPackagingTypes.Add(FedExServiceType.InternationalPriority, international);
            expectedPackagingTypes.Add(FedExServiceType.InternationalPriorityExpress, international);

            expectedPackagingTypes.Add(FedExServiceType.FedExFreightEconomy, freight);
            expectedPackagingTypes.Add(FedExServiceType.FedExFreightPriority, freight);
        }

        [Theory]
        [InlineData(FedExServiceType.FedExFimsMailView)]
        [InlineData(FedExServiceType.FedExFimsMailViewLite)]
        [InlineData(FedExServiceType.FedExFimsPremium)]
        [InlineData(FedExServiceType.FedExFimsStandard)]
        public void IsFimsService_ReturnsTrue_WhenPassedFimsService(FedExServiceType fimsService)
        {
            Assert.True(FedExUtility.IsFimsService(fimsService));
        }

        [Fact]
        public void IsFimsService_ReturnsFalse_WhenPassedNonFimsService()
        {
            Assert.False(FedExUtility.IsFimsService(FedExServiceType.FedExGround));
        }

        [Theory]
        [InlineData(FedExServiceType.FedExGround)]
        [InlineData(FedExServiceType.StandardOvernight)]
        [InlineData(FedExServiceType.FirstOvernight)]
        [InlineData(FedExServiceType.PriorityOvernight)]
        [InlineData(FedExServiceType.FedEx1DayFreight)]
        [InlineData(FedExServiceType.FedEx2DayFreight)]
        [InlineData(FedExServiceType.FedEx3DayFreight)]
        [InlineData(FedExServiceType.SmartPost)]
        [InlineData(FedExServiceType.FedEx2Day)]
        [InlineData(FedExServiceType.FedEx2DayAM)]
        [InlineData(FedExServiceType.GroundHomeDelivery)]
        [InlineData(FedExServiceType.InternationalPriorityFreight)]
        [InlineData(FedExServiceType.InternationalEconomyFreight)]
        [InlineData(FedExServiceType.FedExEconomyCanada)]
        [InlineData(FedExServiceType.InternationalFirst)]
        [InlineData(FedExServiceType.InternationalPriority)]
        [InlineData(FedExServiceType.InternationalPriorityExpress)]
        [InlineData(FedExServiceType.InternationalEconomy)]
        [InlineData(FedExServiceType.FedExEuropeFirstInternationalPriority)]
        [InlineData(FedExServiceType.FedExInternationalGround)]
        [InlineData(FedExServiceType.OneRate2Day)]
        [InlineData(FedExServiceType.OneRate2DayAM)]
        [InlineData(FedExServiceType.OneRateExpressSaver)]
        [InlineData(FedExServiceType.OneRateFirstOvernight)]
        [InlineData(FedExServiceType.OneRatePriorityOvernight)]
        [InlineData(FedExServiceType.OneRateStandardOvernight)]
        [InlineData(FedExServiceType.FedExFreightEconomy)]
        [InlineData(FedExServiceType.FedExFreightPriority)]
        [InlineData(FedExServiceType.FirstFreight)]
        [InlineData(FedExServiceType.FedExNextDayAfternoon)]
        [InlineData(FedExServiceType.FedExNextDayEarlyMorning)]
        [InlineData(FedExServiceType.FedExNextDayMidMorning)]
        [InlineData(FedExServiceType.FedExNextDayEndOfDay)]
        [InlineData(FedExServiceType.FedExDistanceDeferred)]
        [InlineData(FedExServiceType.FedExNextDayFreight)]
        [InlineData(FedExServiceType.FedExFimsMailView)]
        [InlineData(FedExServiceType.FedExFimsMailViewLite)]
        [InlineData(FedExServiceType.FedExFimsPremium)]
        [InlineData(FedExServiceType.FedExFimsStandard)]
        [InlineData(FedExServiceType.FedExExpressSaver)]
        public void GetValidPackageType_ReturnsTrue_WhenCorrectPackageType_IsPassed(FedExServiceType input)
        {
            var expectedPackage = expectedPackagingTypes[input];
            var testObject = FedExUtility.GetValidPackagingTypes(input);

            Assert.Equal(expectedPackage.Count, testObject.Count);

            foreach (var packageType in expectedPackage)
            {
                Assert.Contains(packageType, testObject);
            }
        }
    }
}