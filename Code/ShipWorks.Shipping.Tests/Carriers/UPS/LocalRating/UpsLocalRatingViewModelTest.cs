using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRatingViewModelTest : IDisposable
    {
        readonly AutoMock mock;

        public UpsLocalRatingViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Load_CorrectlySetsLocalRatingEnabled(bool localRatingEnabled)
        {
            var upsAccount = new UpsAccountEntity()
            {
                LocalRatingEnabled = localRatingEnabled
            };

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount);

            Assert.Equal(localRatingEnabled, testObject.LocalRatingEnabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Save_CorrectlySetsLocalRatingEnabled(bool localRatingEnabled)
        {
            var upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRatingViewModel>();

            testObject.LocalRatingEnabled = localRatingEnabled;

            testObject.Save(upsAccount);

            Assert.Equal(localRatingEnabled, upsAccount.LocalRatingEnabled);
        }

        [Fact]
        public void DownloadSampleFileAccount_ResourceStreamNotAccessed_WhenFileDialogDoesNotReturnOk()
        {
            
        }


        public void Dispose()
        {
            mock.Dispose();
        }

    }
}