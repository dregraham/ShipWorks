﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Usps
{
    public class UspsLabelServiceTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IUspsShipmentType> uspsShipmentTypeMock;
        private readonly ShipmentEntity shipment;

        public UspsLabelServiceTest()
        {
            shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity
                {
                    Usps = new UspsShipmentEntity()
                }
            };

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            uspsShipmentTypeMock = mock.Mock<IUspsShipmentType>();
            Mock<IUspsWebClient> webClientMock = mock.CreateMock<IUspsWebClient>();

            uspsShipmentTypeMock
                .Setup(t => t.CreateWebClient())
                .Returns(webClientMock);

            TelemetricResult<StampsLabelResponse> telemetricResult = new TelemetricResult<StampsLabelResponse>(TelemetricResultBaseName.ApiResponseTimeInMilliseconds);
            telemetricResult.SetValue(new StampsLabelResponse());
            webClientMock.Setup(w => w.ProcessShipment(AnyShipment))
                         .ReturnsAsync(telemetricResult);

            Mock<IIndex<ShipmentTypeCode, IUspsShipmentType>> shipmentTypeRepo = mock.MockRepository.Create<IIndex<ShipmentTypeCode, IUspsShipmentType>>();
            shipmentTypeRepo.Setup(x => x[ShipmentTypeCode.Usps])
                .Returns(uspsShipmentTypeMock.Object);
            mock.Provide(shipmentTypeRepo.Object);
        }

        [Fact]
        public async Task Create_DelegatesToTermsAndConditions_WhenNotRateShopping()
        {
            UspsLabelService testObject = mock.Create<UspsLabelService>();
            await testObject.Create(shipment);

            mock.Mock<IUspsTermsAndConditions>().Verify(tc => tc.Validate(shipment), Times.Once);
        }

        [Fact]
        public async Task Create_DelagatesToTermsAndConditions_WhenRateShopping_AndBestRateIsUsps()
        {
            uspsShipmentTypeMock.Setup(t => t.ShouldRateShop(shipment))
                .Returns(true);

            var postalRateSelectionMock = mock.CreateMock<IUspsPostalRateSelection>();
            postalRateSelectionMock
                .Setup(s => s.IsRateFor(shipment))
                .Returns(true);
            postalRateSelectionMock
                .Setup(s => s.Accounts)
                .Returns(new List<IUspsAccountEntity>
                {
                    new UspsAccountEntity
                    {
                        UspsReseller = (int) UspsResellerType.None
                    }
                });

            RateResult rateResult = new RateResult("rate", "5", 5, postalRateSelectionMock.Object);

            mock.Mock<IUspsRatingService>()
                .Setup(s => s.GetRates(shipment, It.IsAny<TelemetricResult<IDownloadedLabelData>>()))
                .Returns(new RateGroup(new[] { rateResult }));

            UspsLabelService testObject = mock.Create<UspsLabelService>();
            await testObject.Create(shipment);

            mock.Mock<IUspsTermsAndConditions>().Verify(tc => tc.Validate(shipment), Times.Once);
        }

        [Fact]
        public async Task Create_DoesNotDelagateToTermsAndConditions_WhenRateShopping_AndBestRateIsExpress1()
        {
            uspsShipmentTypeMock.Setup(t => t.ShouldRateShop(shipment))
                .Returns(true);

            var postalRateSelectionMock = mock.CreateMock<IUspsPostalRateSelection>();
            postalRateSelectionMock
                .Setup(s => s.IsRateFor(shipment))
                .Returns(true);
            postalRateSelectionMock
                .Setup(s => s.Accounts)
                .Returns(new List<IUspsAccountEntity>
                {
                    new UspsAccountEntity
                    {
                        UspsReseller = (int) UspsResellerType.Express1
                    }
                });

            RateResult rateResult = new RateResult("rate", "5", 5, postalRateSelectionMock.Object);

            mock.Mock<IUspsRatingService>()
                .Setup(s => s.GetRates(shipment, It.IsAny<TelemetricResult<IDownloadedLabelData>>()))
                .Returns(new RateGroup(new[] { rateResult }));

            var labelServiceMock = mock.CreateMock<ILabelService>();
            var telemetricResult = new TelemetricResult<IDownloadedLabelData>("");
            telemetricResult.SetValue(mock.Mock<IDownloadedLabelData>().Object);
            labelServiceMock.Setup(s => s.Create(AnyShipment)).ReturnsAsync(telemetricResult);

            Mock<IIndex<ShipmentTypeCode, ILabelService>> labelServicesRepo = mock.MockRepository.Create<IIndex<ShipmentTypeCode, ILabelService>>();
            labelServicesRepo.Setup(x => x[ShipmentTypeCode.Express1Usps])
                .Returns(labelServiceMock.Object);
            mock.Provide(labelServicesRepo.Object);

            UspsLabelService testObject = mock.Create<UspsLabelService>();
            await testObject.Create(shipment);

            mock.Mock<IUspsTermsAndConditions>().Verify(tc => tc.Validate(shipment), Times.Never);
        }
    }
}
