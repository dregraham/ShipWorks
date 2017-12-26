using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Usps
{
    public class UspsLabelServiceTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IUspsShipmentType> uspsShipmentTypeMock;
        private readonly UspsLabelService testObject;
        private readonly ShipmentEntity shipment;


        public UspsLabelServiceTest()
        {

            shipment = new ShipmentEntity
            {
                TotalWeight = 5,
                Postal = new PostalShipmentEntity
                {
                    Service = (int) PostalServiceType.FirstClass,
                    Usps = new UspsShipmentEntity
                    {
                        RateShop = false
                    }
                }
            };

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            uspsShipmentTypeMock = mock.Mock<IUspsShipmentType>();
            Mock<IUspsWebClient> webClientMock = mock.CreateMock<IUspsWebClient>();

            uspsShipmentTypeMock
                .Setup(t => t.CreateWebClient())
                .Returns(webClientMock);

            webClientMock.Setup(w => w.ProcessShipment(shipment))
                .ReturnsAsync(new UspsLabelResponse());

            Mock<IIndex<ShipmentTypeCode, IUspsShipmentType>> shipmentTypeRepo = mock.MockRepository.Create<IIndex<ShipmentTypeCode, IUspsShipmentType>>();
            shipmentTypeRepo.Setup(x => x[ShipmentTypeCode.Usps])
                .Returns(uspsShipmentTypeMock.Object);
            mock.Provide(shipmentTypeRepo.Object);

            testObject = mock.Create<UspsLabelService>();
        }

        [Fact]
        public async Task Create_DelegatesToTermsAndConditions_WhenNotRateShopping()
        {
            await testObject.Create(shipment);

            mock.Mock<IUspsTermsAndConditions>()
                .Verify(tc=>tc.Validate(shipment), Times.Once);
        }
        
    }
}
