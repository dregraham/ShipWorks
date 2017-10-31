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
            var standardPackagingTypes = new List<FedExPackagingType>()
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

            var internationalPackagingTypes = new List<FedExPackagingType>()
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

            var freightPackagingTypes = new List<FedExPackagingType>()
            {
                FedExPackagingType.Custom
            };

            var oneRatePackagingTypes = new List<FedExPackagingType>()
            {
                FedExPackagingType.Envelope,
                FedExPackagingType.Pak,
                FedExPackagingType.Tube,
                FedExPackagingType.SmallBox,
                FedExPackagingType.MediumBox,
                FedExPackagingType.LargeBox,
                FedExPackagingType.ExtraLargeBox
            };

            var otherPackagingTypes = new List<FedExPackagingType>()
            {
                FedExPackagingType.Custom,
                FedExPackagingType.SmallBox,
                FedExPackagingType.MediumBox,
                FedExPackagingType.LargeBox,
                FedExPackagingType.ExtraLargeBox
            };

            expectedPackagingTypes = new Dictionary<FedExServiceType, List<FedExPackagingType>>();

            expectedPackagingTypes.Add(FedExServiceType.FirstOvernight, standardPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.StandardOvernight, standardPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.PriorityOvernight, standardPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedEx2Day, standardPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedEx2DayAM, standardPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExEconomyCanada, standardPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.InternationalFirst, standardPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.InternationalEconomy, standardPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExExpressSaver, standardPackagingTypes);

            expectedPackagingTypes.Add(FedExServiceType.FedExGround, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedEx1DayFreight, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedEx2DayFreight, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedEx3DayFreight, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FirstFreight, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.SmartPost, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.GroundHomeDelivery, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.InternationalPriorityFreight, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.InternationalEconomyFreight, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExInternationalGround, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExNextDayAfternoon, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExNextDayEarlyMorning, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExNextDayMidMorning, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExNextDayEndOfDay, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExDistanceDeferred, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExNextDayFreight, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExFimsMailView, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExFimsMailViewLite, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExFimsPremium, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExFimsStandard, otherPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExEuropeFirstInternationalPriority, otherPackagingTypes);

            expectedPackagingTypes.Add(FedExServiceType.OneRate2Day, oneRatePackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.OneRate2DayAM, oneRatePackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.OneRateExpressSaver, oneRatePackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.OneRateFirstOvernight, oneRatePackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.OneRatePriorityOvernight, oneRatePackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.OneRateStandardOvernight, oneRatePackagingTypes);

            expectedPackagingTypes.Add(FedExServiceType.InternationalPriority, internationalPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.InternationalPriorityExpress, internationalPackagingTypes);

            expectedPackagingTypes.Add(FedExServiceType.FedExFreightEconomy, freightPackagingTypes);
            expectedPackagingTypes.Add(FedExServiceType.FedExFreightPriority, freightPackagingTypes);
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