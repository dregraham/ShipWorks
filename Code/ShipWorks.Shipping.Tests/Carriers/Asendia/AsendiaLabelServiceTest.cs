using Autofac.Extras.Moq;
using Moq;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Asendia;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.Asendia
{
    public class AsendiaLabelServiceTest
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;
        private readonly Mock<ICarrierShipmentRequestFactory> shipmentRequestFactory;
        private readonly PurchaseLabelRequest request;
        private readonly Label label;

        public AsendiaLabelServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = new ShipmentEntity() { Asendia = new AsendiaShipmentEntity() };
            request = new PurchaseLabelRequest();
            label = new Label();

            shipmentRequestFactory = mock.CreateKeyedMockOf<ICarrierShipmentRequestFactory>().For(ShipmentTypeCode.Asendia);
            shipmentRequestFactory.Setup(f => f.CreatePurchaseLabelRequest(AnyShipment)).Returns(request);
        }

        [Fact]
        public async Task Create_ThrowsShippingExceptionWithPrettyError_WhenAsendiaErrorHasSeOrderIdAboutPostalCode()
        {
            mock.Mock<IAsendiaAccountRepository>().Setup(r => r.GetAccount(shipment)).Returns(new AsendiaAccountEntity() { ShipEngineCarrierId = "se-182974" });

            mock.Mock<IShipEngineWebClient>().Setup(w => w.PurchaseLabel(It.IsAny<PurchaseLabelRequest>(), ApiLogSource.Asendia))
                .Throws(new Exception("Unable to create label. Order ID: se-164205986. \"K1A 0G9\" is an invalid postal code for the country \"US\"."));

            AsendiaLabelService testObject = mock.Create<AsendiaLabelService>();

            var ex = await Assert.ThrowsAsync<ShippingException>(() => testObject.Create(shipment));
            Assert.Equal("\"K1A 0G9\" is an invalid postal code for the country \"US\".", ex.Message);
        }

        [Fact]
        public async Task Create_ThrowsShippingExceptionWithPrettyError_WhenAsendiaErrorHasSeOrderIdAboutCustoms()
        {
            mock.Mock<IAsendiaAccountRepository>().Setup(r => r.GetAccount(shipment)).Returns(new AsendiaAccountEntity() { ShipEngineCarrierId = "se-182974" });

            mock.Mock<IShipEngineWebClient>().Setup(w => w.PurchaseLabel(It.IsAny<PurchaseLabelRequest>(), ApiLogSource.Asendia))
                .Throws(new Exception("Unable to create label. Order ID: se-164554936. Asendia requires a value for all customs items."));

            AsendiaLabelService testObject = mock.Create<AsendiaLabelService>();

            var ex = await Assert.ThrowsAsync<ShippingException>(() => testObject.Create(shipment));
            Assert.Equal("Asendia requires a value for all customs items.", ex.Message);
        }
    }
}
