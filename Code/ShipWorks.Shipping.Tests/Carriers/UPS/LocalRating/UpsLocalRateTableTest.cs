using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.ReadOnlyEntityClasses;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRateTableTest
    {
        [Fact]
        public void Load_DelegatesToUpsLocalRateTableRepo()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            var rateTableRepo = mock.Mock<IUpsLocalRateTableRepository>();
            UpsAccountEntity upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRateTable>();

            testObject.Load(upsAccount);

            rateTableRepo.Verify(r => r.Get(upsAccount));
        }

        [Fact]
        public void Save_SetsUploadDate()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Mock<IUpsLocalRateTableRepository>();
            UpsAccountEntity upsAccount = new UpsAccountEntity();

            var testObject = CreatePopulatedRateTable(mock);
            testObject.Save(upsAccount);

            Assert.NotEqual(new DateTime(), testObject.UploadDate);
        }

        [Fact]
        public void Save_DelegatesToUpsLocalRateTableRepo()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            var rateTableRepo = mock.Mock<IUpsLocalRateTableRepository>();
            UpsAccountEntity upsAccount = new UpsAccountEntity();

            var testObject = CreatePopulatedRateTable(mock);

            testObject.Save(upsAccount);
            rateTableRepo.Verify(r => r.Save(It.IsAny<UpsRateTableEntity>(), upsAccount), Times.Once);
        }

        [Fact]
        public void AddRates_DelegatesToImportedRateValidator()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreatePopulatedRateTable(mock);

                mock.Mock<IUpsImportedRateValidator>()
                    .Verify(v => v.Validate(
                            It.Is<List<IUpsPackageRateEntity>>(
                                r => (r.Single() as ReadOnlyUpsPackageRateEntity).UpsPackageRateID == 2),
                            It.Is<List<IUpsLetterRateEntity>>(
                                r => (r.Single() as ReadOnlyUpsLetterRateEntity).UpsLetterRateID == 3),
                            It.Is<List<IUpsPricePerPoundEntity>>(
                                r => (r.Single() as ReadOnlyUpsPricePerPoundEntity).UpsPricePerPoundID == 4)),
                        Times.Once);
            }
        }

        [Fact]
        public void Save_SavesNewRateTable()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            var rateTableRepo = mock.Mock<IUpsLocalRateTableRepository>();
            rateTableRepo.Setup(r => r.Get(It.IsAny<UpsAccountEntity>())).Returns(new UpsRateTableEntity(42));

            UpsAccountEntity upsAccount = new UpsAccountEntity();

            var testObject = CreatePopulatedRateTable(mock);

            testObject.Load(upsAccount);
            testObject.Save(upsAccount);
            rateTableRepo
                .Verify(r => r.Save(It.Is<UpsRateTableEntity>(t => t.UpsRateTableID == 0), upsAccount), Times.Once);
        }

        private UpsLocalRateTable CreatePopulatedRateTable(AutoMock mock)
        {
            var upsPackageRateEntity = new UpsPackageRateEntity(2);
            var upsLetterRateEntity = new UpsLetterRateEntity(3);
            var upsPricePerPoundEntity = new UpsPricePerPoundEntity(4);
            var upsSurchargeEntity = new UpsRateSurchargeEntity(5);

            List<UpsPackageRateEntity> packageRates = new List<UpsPackageRateEntity> { upsPackageRateEntity };
            List<UpsLetterRateEntity> letterRate = new List<UpsLetterRateEntity> { upsLetterRateEntity };
            List<UpsPricePerPoundEntity> pricesPerPound = new List<UpsPricePerPoundEntity> { upsPricePerPoundEntity };
            List<UpsRateSurchargeEntity> surcharges = new List<UpsRateSurchargeEntity> { upsSurchargeEntity };

            var testObject = mock.Create<UpsLocalRateTable>();
            testObject.ReplaceRates(packageRates, letterRate, pricesPerPound);
            testObject.ReplaceSurcharges(surcharges);

            return testObject;
        }
    }
}