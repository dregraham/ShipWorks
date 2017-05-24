using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsImportedRateValidatorTest : IDisposable
    {
        readonly AutoMock mock;

        public UpsImportedRateValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Validate_DoesNotThrow_WhenHasAllPackageRates_OneLetterRate_OnePricePerPound()
        {
            List<IUpsPackageRateEntity> packageRates = GetAllRequiredPackageRates(UpsServiceType.Ups2DayAir, "42");
            List<IUpsLetterRateEntity> letterRates = GetLetterRate(UpsServiceType.Ups2DayAir, "42");
            List<IUpsPricePerPoundEntity> pricesPerPound = GetPricePerPound(UpsServiceType.Ups2DayAir, "42");
            AddValidZoneData(packageRates, letterRates, pricesPerPound, UpsServiceType.Ups2DayAir);

            var testObject = mock.Create<UpsImportedRateValidator>();
            testObject.Validate(packageRates, letterRates, pricesPerPound);
        }

        [Fact]
        public void Validate_DoesNotThrow_WhenHasAllPackageRates_OnePricePerPound_NoLetterRate_Ground()
        {
            List<IUpsPackageRateEntity> packageRates = GetAllRequiredPackageRates(UpsServiceType.UpsGround, "42");
            List <IUpsLetterRateEntity> letterRates = new List<IUpsLetterRateEntity>();
            List<IUpsPricePerPoundEntity> pricesPerPound = GetPricePerPound(UpsServiceType.UpsGround, "42");
            AddValidZoneData(packageRates, letterRates, pricesPerPound, UpsServiceType.Ups2DayAir);

            var testObject = mock.Create<UpsImportedRateValidator>();
            testObject.Validate(packageRates, letterRates, pricesPerPound);
        }

        [Theory]
        [InlineData(UpsServiceType.UpsGround)]
        [InlineData(UpsServiceType.Ups3DaySelect)]
        public void Validate_Throws_WhenHasAllPackageRates_OnePricePerPound_OneLetterRate_IncompatibleLetterService(UpsServiceType upsServiceType)
        {
            List<IUpsPackageRateEntity> packageRates = GetAllRequiredPackageRates(upsServiceType, "42");
            List <IUpsLetterRateEntity> letterRates = GetLetterRate(upsServiceType, "42");
            List <IUpsPricePerPoundEntity> pricesPerPound = GetPricePerPound(upsServiceType, "42");
            AddValidZoneData(packageRates, letterRates, pricesPerPound, UpsServiceType.Ups2DayAir);

            var testObject = mock.Create<UpsImportedRateValidator>();
            var exception = Record.Exception(() => testObject.Validate(packageRates, letterRates, pricesPerPound));
            Assert.IsType<UpsLocalRatingException>(exception);

            string expectedErrorMessage = string.Format(UpsImportedRateValidator.LetterNotValidForServiceErrorMessageFormat,
                EnumHelper.GetDescription(upsServiceType));
            Assert.Contains(expectedErrorMessage, exception.Message);
        }

        [Fact]
        public void Validate_Throws_WhenHasAllPackageRates_OnePricePerPound_NoLetterRate_NotGround()
        {
            List<IUpsPackageRateEntity> packageRates = GetAllRequiredPackageRates(UpsServiceType.Ups2DayAir, "42");
            List <IUpsLetterRateEntity> letterRates = new List<IUpsLetterRateEntity>();
            List<IUpsPricePerPoundEntity> pricesPerPound = GetPricePerPound(UpsServiceType.Ups2DayAir, "42");
            AddValidZoneData(packageRates, letterRates, pricesPerPound, UpsServiceType.Ups2DayAir);

            var testObject = mock.Create<UpsImportedRateValidator>();
            var exception = Record.Exception(() => testObject.Validate(packageRates, letterRates, pricesPerPound));
            Assert.IsType<UpsLocalRatingException>(exception);

            string expectedErrorMessage = string.Format(UpsImportedRateValidator.ServiceMissingLetterErrorMessageFormat, EnumHelper.GetDescription(UpsServiceType.Ups2DayAir));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Fact]
        public void Validate_Throws_WhenMissingPackageWeight()
        {
            List<IUpsPackageRateEntity> packageRates = GetAllRequiredPackageRates(UpsServiceType.Ups2DayAir, "42");
            packageRates.RemoveAt(0);

            List<IUpsLetterRateEntity> letterRates = GetLetterRate(UpsServiceType.Ups2DayAir, "42");
            List <IUpsPricePerPoundEntity> pricesPerPound = GetPricePerPound(UpsServiceType.Ups2DayAir, "42");
            AddValidZoneData(packageRates, letterRates, pricesPerPound, UpsServiceType.Ups2DayAir);

            var testObject = mock.Create<UpsImportedRateValidator>();
            var exception = Record.Exception(() => testObject.Validate(packageRates, letterRates, pricesPerPound));
            Assert.IsType<UpsLocalRatingException>(exception);

            string expectedErrorMessage = string.Format(UpsImportedRateValidator.MissingPackageWeightErrorMessageFormat, EnumHelper.GetDescription(UpsServiceType.Ups2DayAir));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }
        
        [Fact]
        public void Validate_Throws_WhenDuplicatePackageWeight()
        {
            List<IUpsPackageRateEntity> packageRates = GetAllRequiredPackageRates(UpsServiceType.Ups2DayAir, "42");
            ((UpsPackageRateEntity) packageRates.First()).WeightInPounds = 2;

            List<IUpsLetterRateEntity> letterRates = GetLetterRate(UpsServiceType.Ups2DayAir, "42");
            List <IUpsPricePerPoundEntity> pricesPerPound = GetPricePerPound(UpsServiceType.Ups2DayAir, "42");
            AddValidZoneData(packageRates, letterRates, pricesPerPound, UpsServiceType.Ups2DayAir);

            var testObject = mock.Create<UpsImportedRateValidator>();
            var exception = Record.Exception(() => testObject.Validate(packageRates, letterRates, pricesPerPound));
            Assert.IsType<UpsLocalRatingException>(exception);

            string expectedErrorMessage = string.Format(UpsImportedRateValidator.DuplicateWeightDetectedErrorMessageFormat, EnumHelper.GetDescription(UpsServiceType.Ups2DayAir));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(151)]
        [InlineData(-1)]
        public void Validate_Throws_WhenHasWeightOutOfRange(int outOfRangeWeight)
        {
            List<IUpsPackageRateEntity> packageRates = GetAllRequiredPackageRates(UpsServiceType.Ups2DayAir, "42");
            ((UpsPackageRateEntity) packageRates.Skip(10).First()).WeightInPounds = outOfRangeWeight;

            List<IUpsLetterRateEntity> letterRates = GetLetterRate(UpsServiceType.Ups2DayAir, "42");
            List <IUpsPricePerPoundEntity> pricesPerPound = GetPricePerPound(UpsServiceType.Ups2DayAir, "42");
            AddValidZoneData(packageRates, letterRates, pricesPerPound, UpsServiceType.Ups2DayAir);

            var testObject = mock.Create<UpsImportedRateValidator>();
            var exception = Record.Exception(() => testObject.Validate(packageRates, letterRates, pricesPerPound));
            Assert.IsType<UpsLocalRatingException>(exception);

            string expectedErrorMessage = string.Format(UpsImportedRateValidator.PackageWeightOutOfRangeErrorMessageFormat, EnumHelper.GetDescription(UpsServiceType.Ups2DayAir));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Fact]
        public void Validate_Throws_WhenMissingPricePerPound()
        {
            List<IUpsPackageRateEntity> packageRates = GetAllRequiredPackageRates(UpsServiceType.Ups2DayAir, "42");
            List <IUpsLetterRateEntity> letterRates = GetLetterRate(UpsServiceType.Ups2DayAir, "42");
            List <IUpsPricePerPoundEntity> pricesPerPound = new List<IUpsPricePerPoundEntity>();
            AddValidZoneData(packageRates, letterRates, pricesPerPound, UpsServiceType.Ups2DayAir);

            var testObject = mock.Create<UpsImportedRateValidator>();

            var exception = Record.Exception(() => testObject.Validate(packageRates, letterRates, pricesPerPound));
            Assert.IsType<UpsLocalRatingException>(exception);

            string expectedErrorMessage = string.Format(UpsImportedRateValidator.MissingPricePerPoundErrorMessageFormat, EnumHelper.GetDescription(UpsServiceType.Ups2DayAir));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        private void AddValidZoneData(List<IUpsPackageRateEntity> packageRates,
            List<IUpsLetterRateEntity> letterRates,
            List<IUpsPricePerPoundEntity> pricesPerPound,
            UpsServiceType serviceTypeInTest)
        {
            packageRates.AddRange(GetAllRequiredPackageRates(serviceTypeInTest, "22"));
            packageRates.AddRange(GetAllRequiredPackageRates(UpsServiceType.UpsExpress, "45"));


            letterRates.AddRange(GetLetterRate(serviceTypeInTest, "22"));
            letterRates.AddRange(GetLetterRate(UpsServiceType.UpsExpress, "45"));

            pricesPerPound.AddRange(GetPricePerPound(serviceTypeInTest, "22"));
            pricesPerPound.AddRange(GetPricePerPound(UpsServiceType.UpsExpress, "45"));
        }

        private static List<IUpsLetterRateEntity> GetLetterRate(UpsServiceType upsServiceType, string zone)
        {
            return new List<IUpsLetterRateEntity>()
            {
                new UpsLetterRateEntity()
                {
                    Service = (int) upsServiceType,
                    Zone = zone,
                    Rate = 10M
                }
            };
        }

        private static List<IUpsPricePerPoundEntity> GetPricePerPound(UpsServiceType upsServiceType, string zone)
        {
            return new List<IUpsPricePerPoundEntity>()
            {
                new UpsPricePerPoundEntity()
                {
                    Service = (int) upsServiceType,
                    Zone = zone,
                    Rate = 10M
                }
            };
        }

        private static List<IUpsPackageRateEntity> GetAllRequiredPackageRates(UpsServiceType serviceType, string zone)
        {
            List<IUpsPackageRateEntity> upsPackageRateEntities = new List<IUpsPackageRateEntity>();
            for (int weight = 1; weight <= 150; weight++)
            {
                upsPackageRateEntities.Add(new UpsPackageRateEntity()
                {
                    Service = (int) serviceType,
                    Zone = zone,
                    WeightInPounds = weight,
                    Rate = 3.50M
                });
            }

            return upsPackageRateEntities;
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}