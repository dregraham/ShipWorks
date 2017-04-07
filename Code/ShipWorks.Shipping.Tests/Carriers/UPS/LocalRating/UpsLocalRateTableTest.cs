using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRateTableTest
    {
        [Fact]
        public void Load_DelegatesToUpsLocalRateTableRepo()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            var rateTableRepo = mock.Mock<IUpsLocalRateTableRepo>();
            UpsAccountEntity upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRateTable>();

            testObject.Load(upsAccount);

            rateTableRepo.Verify(r => r.Get(upsAccount));
        }

        [Fact]
        public void Save_SetsUploadDate()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Mock<IUpsLocalRateTableRepo>();
            UpsAccountEntity upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRateTable>();

            testObject.Save(upsAccount);

            Assert.NotEqual(new DateTime(), testObject.UploadDate);
        }

        [Fact]
        public void Save_DelegatesToUpsLocalRateTableRepo()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            var rateTableRepo = mock.Mock<IUpsLocalRateTableRepo>();
            UpsAccountEntity upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRateTable>();

            testObject.Save(upsAccount);

            rateTableRepo.Verify(r => r.Save(testObject, upsAccount));
        }
    }
}