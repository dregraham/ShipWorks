using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx
{
    public class FedExUtilityTest
    {
        private readonly ITestOutputHelper output;
        private readonly Dictionary<FedExServiceType, List<FedExPackagingType>> expectedPackagingTypes;

        public FedExUtilityTest(ITestOutputHelper output)
        {
            this.output = output;

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

        [Fact]
        public void IsFreightLtlServiceInt_ReturnsTrue_ForFreightValues()
        {
            var invalidResults = LtlFreightServices
                .Cast<int>()
                .Select(x => new { Service = x, Result = FedExUtility.IsFreightLtlService(x) })
                .Where(x => x.Result == false)
                .ToList();

            foreach (var value in invalidResults)
            {
                output.WriteLine("{0} returned false but should have returned true", value.Service);
            }

            Assert.Empty(invalidResults);
        }

        [Fact]
        public void IsFreightLtlServiceInt_ReturnsFalse_ForNonFreightServices()
        {
            var invalidResults = AllServices.Except(LtlFreightServices)
                .Cast<int>()
                .Select(x => new { Service = x, Result = FedExUtility.IsFreightLtlService(x) })
                .Where(x => x.Result == true)
                .ToList();

            foreach (var value in invalidResults)
            {
                output.WriteLine("{0} returned false but should have returned true", value.Service);
            }

            Assert.Empty(invalidResults);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(null)]
        public void IsFreightLtlServiceInt_ReturnsFalse_ForInvalidValues(int? value) =>
            Assert.False(FedExUtility.IsFreightLtlService(value));

        [Fact]
        public void IsFreightLtlService_ReturnsTrue_ForFreightValues()
        {
            var invalidResults = LtlFreightServices
                .Select(x => new { Service = x, Result = FedExUtility.IsFreightLtlService(x) })
                .Where(x => x.Result == false)
                .ToList();

            foreach (var value in invalidResults)
            {
                output.WriteLine("{0} returned false but should have returned true", value.Service);
            }

            Assert.Empty(invalidResults);
        }

        [Fact]
        public void IsFreightLtlService_ReturnsFalse_ForNonFreightServices()
        {
            var invalidResults = AllServices.Except(LtlFreightServices)
                .Select(x => new { Service = x, Result = FedExUtility.IsFreightLtlService(x) })
                .Where(x => x.Result == true)
                .ToList();

            foreach (var value in invalidResults)
            {
                output.WriteLine("{0} returned false but should have returned true", value.Service);
            }

            Assert.Empty(invalidResults);
        }

        [Theory]
        [InlineData((FedExServiceType) int.MinValue)]
        [InlineData(null)]
        public void IsFreightLtlService_ReturnsFalse_ForInvalidValues(FedExServiceType? value) =>
            Assert.False(FedExUtility.IsFreightLtlService(value));

        [Theory]
        [InlineData(FedExServiceType.PriorityOvernight)]
        [InlineData(FedExServiceType.FedEx1DayFreight)]
        [InlineData(FedExServiceType.FirstFreight)]
        [InlineData(FedExServiceType.OneRatePriorityOvernight)]
        [InlineData(FedExServiceType.FedExNextDayAfternoon)]
        [InlineData(FedExServiceType.FedExNextDayEarlyMorning)]
        [InlineData(FedExServiceType.FedExNextDayMidMorning)]
        [InlineData(FedExServiceType.FedExNextDayEndOfDay)]
        [InlineData(FedExServiceType.FedExNextDayFreight)]
        public void CanDeliverOnSaturday_ReturnsTrueForEligible1DayService_WhenDayIsFriday(FedExServiceType serviceType)
        {
            // Can't set day of week directly, so picked a friday date
            Assert.True(FedExUtility.CanDeliverOnSaturday(serviceType, new DateTime(2018, 7, 27)));
        }
        
        [Theory]
        [InlineData(FedExServiceType.PriorityOvernight)]
        [InlineData(FedExServiceType.FedEx1DayFreight)]
        [InlineData(FedExServiceType.FirstFreight)]
        [InlineData(FedExServiceType.OneRatePriorityOvernight)]
        [InlineData(FedExServiceType.FedExNextDayAfternoon)]
        [InlineData(FedExServiceType.FedExNextDayEarlyMorning)]
        [InlineData(FedExServiceType.FedExNextDayMidMorning)]
        [InlineData(FedExServiceType.FedExNextDayEndOfDay)]
        [InlineData(FedExServiceType.FedExNextDayFreight)]
        public void CanDeliverOnSaturday_ReturnsFalseForEligible1DayService_WhenDayIsNotFriday(FedExServiceType serviceType)
        {
            List<DateTime> nonFridayDays = new List<DateTime>
            {
                new DateTime(2018, 7, 22),
                new DateTime(2018, 7, 23),
                new DateTime(2018, 7, 24),
                new DateTime(2018, 7, 25),
                new DateTime(2018, 7, 26),
                new DateTime(2018, 7, 28)
            };

            bool result = false;
            
            foreach (DateTime day in nonFridayDays)
            {
                result = result || FedExUtility.CanDeliverOnSaturday(serviceType, day);
            }
            
            Assert.False(result);
        }
        
        [Theory]
        [InlineData(FedExServiceType.FedEx2Day)]
        [InlineData(FedExServiceType.FedEx2DayAM)]
        [InlineData(FedExServiceType.FedEx2DayFreight)]
        [InlineData(FedExServiceType.OneRate2Day)]
        [InlineData(FedExServiceType.OneRate2DayAM)]
        public void CanDeliverOnSaturday_ReturnsTrueForEligible2DayService_WhenDayIsThursday(FedExServiceType serviceType)
        {
            // Can't set day of week directly, so picked a friday date
            Assert.True(FedExUtility.CanDeliverOnSaturday(serviceType, new DateTime(2018, 7, 26)));
        }
        
        [Theory]
        [InlineData(FedExServiceType.FedEx2Day)]
        [InlineData(FedExServiceType.FedEx2DayAM)]
        [InlineData(FedExServiceType.FedEx2DayFreight)]
        [InlineData(FedExServiceType.OneRate2Day)]
        [InlineData(FedExServiceType.OneRate2DayAM)]
        public void CanDeliverOnSaturday_ReturnsFalseForEligible2DayService_WhenDayIsNotThursday(FedExServiceType serviceType)
        {
            List<DateTime> nonThursdayDays = new List<DateTime>
            {
                new DateTime(2018, 7, 22),
                new DateTime(2018, 7, 23),
                new DateTime(2018, 7, 24),
                new DateTime(2018, 7, 25),
                new DateTime(2018, 7, 27),
                new DateTime(2018, 7, 28)
            };

            bool result = false;
            
            foreach (DateTime day in nonThursdayDays)
            {
                result = result || FedExUtility.CanDeliverOnSaturday(serviceType, day);
            }
            
            Assert.False(result);
        }
        
        [Theory]
        [InlineData(FedExServiceType.InternationalPriority)]
        public void CanDeliverOnSaturday_ReturnsTrueForEligibleInternationalServices_WhenDayIsWednesdayThursdayOrFriday(FedExServiceType serviceType)
        {
            List<DateTime> nonFridayDays = new List<DateTime>
            {
                new DateTime(2018, 7, 25),
                new DateTime(2018, 7, 26),
                new DateTime(2018, 7, 27)
            };

            bool result = true;
            
            foreach (DateTime day in nonFridayDays)
            {
                result = result && FedExUtility.CanDeliverOnSaturday(serviceType, day);
            }
            
            Assert.True(result);
        }
        
        [Theory]
        [InlineData(FedExServiceType.InternationalPriority)]
        public void CanDeliverOnSaturday_ReturnsFalseForEligibleInternationalServices_WhenDayIsNotWednesdayThursdayOrFriday(FedExServiceType serviceType)
        {
            List<DateTime> nonFridayDays = new List<DateTime>
            {
                new DateTime(2018, 7, 22),
                new DateTime(2018, 7, 23),
                new DateTime(2018, 7, 24),
                new DateTime(2018, 7, 28)
            };

            bool result = false;
            
            foreach (DateTime day in nonFridayDays)
            {
                result = result || FedExUtility.CanDeliverOnSaturday(serviceType, day);
            }
            
            Assert.False(result);
        }
        

        /// <summary>
        /// Get all services
        /// </summary>
        private IEnumerable<FedExServiceType> AllServices =>
            Enum.GetValues(typeof(FedExServiceType)).Cast<FedExServiceType>();

        /// <summary>
        /// Get LTL Freight services
        /// </summary>
        private IEnumerable<FedExServiceType> LtlFreightServices =>
            new[] { FedExServiceType.FedExFreightPriority, FedExServiceType.FedExFreightEconomy };
    }
}