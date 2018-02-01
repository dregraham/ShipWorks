using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shipping.Carriers.BestRate.Fake;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    public class BestRateShipmentTypeTest
    {
        private BestRateLabelService labelService;
        
        private ShipmentEntity shipment;
        
        private RateGroup rateGroupWithFooterWithAssociatedAmount;
        private RateGroup rateGroupWithFooterNotAssociatedWithAmount;

        private Mock<IRateFootnoteFactory> associatedWithAmountFooterFootnoteFactory;
        private Mock<IRateFootnoteFactory> notAssociatedWithAmountFooterFootnoteFactory;

        private Mock<IRateGroupFilterFactory> filterFactory;
        private AutoMock mock;


        public BestRateShipmentTypeTest()
        {
            shipment = new ShipmentEntity
            {
                BestRate = new BestRateShipmentEntity()
            };

            labelService = new BestRateLabelService();
            
            mock = AutoMockExtensions.GetLooseThatReturnsMocks(); //AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });
            
            filterFactory = mock.Mock<IRateGroupFilterFactory>();
            filterFactory.Setup(f => f.CreateFilters(It.IsAny<ShipmentEntity>())).Returns(new List<IRateGroupFilter>());
            
            InitializeFootnoteTests();
        }

        [Fact]
        public void GetShipmentInsuranceProvider_ReturnsInvalid_OneBrokersWithNoAccounts()
        {
            var bestRateShipmentType = mock.Create<BestRateShipmentType>();

            Assert.Equal(InsuranceProvider.Invalid, bestRateShipmentType.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [Fact]
        public void SupportsGetRates_ReturnsTrue()
        {
            BestRateShipmentType bestRateShipmentType = mock.Create<BestRateShipmentType>();

            Assert.True(bestRateShipmentType.SupportsGetRates);
        }

        [Fact]
        public void GetShipmentInsuranceProvider_ReturnsShipWorks_TwoBrokersWithAccountsAndShipWorksInsurance()
        {
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.HasAccounts).Returns(true);
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.ShipWorks);
            IBestRateShippingBroker broker = mock.Mock<IBestRateShippingBroker>().Object;
            
            mock.Mock<IBestRateShippingBrokerFactory>().Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new [] { broker, broker });
            
            var bestRateShipmentType = mock.Create<BestRateShipmentType>();

            Assert.Equal(InsuranceProvider.ShipWorks, bestRateShipmentType.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [Fact]
        public void GetShipmentInsuranceProvider_ReturnsInvalid_TwoBrokersWithAccountsAndCarrierInsurance()
        {
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.HasAccounts).Returns(true);
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.Carrier);
            IBestRateShippingBroker broker = mock.Mock<IBestRateShippingBroker>().Object;
            
            mock.Mock<IBestRateShippingBrokerFactory>().Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new List<IBestRateShippingBroker> { broker, broker });

            var bestRateShipmentType = mock.Create<BestRateShipmentType>();

            Assert.Equal(InsuranceProvider.Invalid, bestRateShipmentType.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [Fact]
        public void GetShipmentInsuranceProvider_ReturnsCarrier_TwoBrokersWithAccountsAndCarrierInsurance()
        {
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.HasAccounts).Returns(true);
            mock.Mock<IBestRateShippingBroker>().Setup(b => b.GetInsuranceProvider(It.IsAny<ShippingSettingsEntity>())).Returns(InsuranceProvider.Carrier);
            IBestRateShippingBroker broker = mock.Mock<IBestRateShippingBroker>().Object;

            mock.Mock<IBestRateShippingBrokerFactory>().Setup(f => f.CreateBrokers(It.IsAny<ShipmentEntity>())).Returns(new [] { broker });

            var bestRateShipmentType = mock.Create<BestRateShipmentType>();

            Assert.Equal(InsuranceProvider.Carrier, bestRateShipmentType.GetShipmentInsuranceProvider(new ShipmentEntity()));
        }

        [Fact]
        public async Task ProcessShipment_ThrowsInvalidOperationException()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => labelService.Create(new ShipmentEntity()));
        }

        [Fact]
        public void ApplySelectedShipmentRate_AddsRateSelectedEventToShipment()
        {
            shipment.BestRateEvents = 0;
            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => { } } };
            BestRateShipmentType.ApplySelectedShipmentRate(shipment, rate);

            Assert.Equal((int) BestRateEventTypes.RateSelected, shipment.BestRateEvents);
        }

        [Fact]
        public void ApplySelectedShipmentRate_DoesNotRemoveOtherBestRateEvents()
        {
            shipment.BestRateEvents = (int) BestRateEventTypes.RatesCompared;
            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => { } } };
            BestRateShipmentType.ApplySelectedShipmentRate(shipment, rate);

            Assert.Equal(BestRateEventTypes.RatesCompared, (BestRateEventTypes) shipment.BestRateEvents & BestRateEventTypes.RatesCompared);
        }

        [Fact]
        public void ApplySelectedShipmentRate_CallsSelectActionSetOnTag()
        {
            ShipmentEntity calledShipment = null;
            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => calledShipment = entity } };
            BestRateShipmentType.ApplySelectedShipmentRate(shipment, rate);

            Assert.Equal(shipment, calledShipment);
        }

        [Fact]
        public void ApplySelectedShipmentRate_DoesNotCallSignUpActionOnTag_WhenSignUpActionIsNull()
        {
            ShipmentEntity calledShipment = null;
            bool? signUpActionResult = null;

            RateResult rate = new RateResult("foo", "3") { Tag = new BestRateResultTag { RateSelectionDelegate = entity => calledShipment = entity } };
            BestRateShipmentType.ApplySelectedShipmentRate(shipment, rate);

            Assert.False(signUpActionResult.HasValue);
        }

        private void InitializeFootnoteTests()
        {
            associatedWithAmountFooterFootnoteFactory = new Mock<IRateFootnoteFactory>();
            associatedWithAmountFooterFootnoteFactory.Setup(f => f.CreateFootnote(null)).Returns(new FakeRateFootnoteControl(true));
            
            rateGroupWithFooterWithAssociatedAmount = new RateGroup(new List<RateResult>
            {
                new RateResult("result1", "2") { AmountFootnote = new Bitmap(1, 1), Tag = new BestRateResultTag() { ResultKey = "result1"}},
                new RateResult("result2", "2") {Tag = new BestRateResultTag() { ResultKey = "result2"}}
            });
            rateGroupWithFooterWithAssociatedAmount.AddFootnoteFactory(associatedWithAmountFooterFootnoteFactory.Object);


            notAssociatedWithAmountFooterFootnoteFactory = new Mock<IRateFootnoteFactory>();
            notAssociatedWithAmountFooterFootnoteFactory.Setup(f => f.CreateFootnote(null)).Returns(new FakeRateFootnoteControl(false));

            rateGroupWithFooterNotAssociatedWithAmount = new RateGroup(new List<RateResult>
            {
                new RateResult("result1", "2") { Tag = new BestRateResultTag() { ResultKey = "result1"} },
                new RateResult("result2", "2") { Tag = new BestRateResultTag() { ResultKey = "result2"} }
            });
            rateGroupWithFooterNotAssociatedWithAmount.AddFootnoteFactory(notAssociatedWithAmountFooterFootnoteFactory.Object);
        }
        // IsCustomsRequierd has a hard dependency to the database, so these are no longer testable 
        
        // Helper methods for creating rate results
        private RateResult CreateRateResult(string description, string days, decimal amount, string tagResultKey)
        {
            return new RateResult(description, days, amount, new BestRateResultTag() { ResultKey = tagResultKey });
        }

        [Theory]
        [InlineData(true, 9.99)]
        [InlineData(false, 6.66)]
        public void Insured_ReturnsInsuranceFromShipment(bool insured, decimal insuranceValue)
        {
            shipment = new ShipmentEntity
            {
                Insurance = !insured,
                BestRate = new BestRateShipmentEntity {Insurance = insured, InsuranceValue = insuranceValue }
            };

            BestRateShipmentType bestRateShipmentType = mock.Create<BestRateShipmentType>();
            ShipmentParcel parcel = bestRateShipmentType.GetParcelDetail(shipment, 0);

            Assert.Equal(insured, parcel.Insurance.Insured);
            Assert.Equal(insuranceValue, parcel.Insurance.InsuranceValue);
        }

        [Fact]
        public void GetPackageAdapters_ReturnBestRateValues()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Insurance = false,
                BestRate = new BestRateShipmentEntity()
                {
                    Insurance = true,
                    InsuranceValue = 3
                }
            };

            BestRateShipmentType testObject = mock.Create<BestRateShipmentType>();
            IEnumerable<IPackageAdapter> packageAdapters = testObject.GetPackageAdapters(shipment);
            Assert.True(packageAdapters.First().InsuranceChoice.Insured);

            shipment.Insurance = true;
            shipment.BestRate.Insurance = false;
            packageAdapters = testObject.GetPackageAdapters(shipment);
            Assert.False(packageAdapters.First().InsuranceChoice.Insured);
        }
    }
}
