using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo.RateFootnotes
{
    public class UpsPromoFootnoteFactoryTest
    {
        [Fact]
        public void ShipmentTypeCode_MatchesAccount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Provide(new UpsAccountEntity());
                var testObject = mock.Create<UpsPromoFootnoteFactory>();

                Assert.Equal(ShipmentTypeCode.UpsOnLineTools, testObject.ShipmentTypeCode);
            }
        }

        [Fact]
        public void AllowedForBestRate_ReturnsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Provide(new UpsAccountEntity());
                var testObject = mock.Create<UpsPromoFootnoteFactory>();

                Assert.False(testObject.AllowedForBestRate);
            }
        }

        [Fact]
        public void CreateFootnote_UpsPromoFootnoteReturned()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Provide(new UpsAccountEntity());

                var testObject = mock.Create<UpsPromoFootnoteFactory>();

                var parameters = mock.MockRepository.Create<IFootnoteParameters>();

                var returnedFootnoteControl = testObject.CreateFootnote(parameters.Object);

                Assert.IsType<UpsPromoFootnote>(returnedFootnoteControl);
            }
        }
    }
}
