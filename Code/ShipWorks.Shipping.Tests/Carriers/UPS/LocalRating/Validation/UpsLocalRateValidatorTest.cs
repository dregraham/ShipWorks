using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Validation
{
    public class UpsLocalRateValidatorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<ILocalRateValidationResultFactory> resultFactory;
        private readonly Mock<IIndex<UpsRatingMethod, IUpsRateClient>> rateClientFactory;
        private UpsLocalRateValidator testObject;

        public UpsLocalRateValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            resultFactory = mock.Mock<ILocalRateValidationResultFactory>();
            rateClientFactory = new Mock<IIndex<UpsRatingMethod, IUpsRateClient>>();
        }

        [Fact]
        public void Validate_ReturnsZeroDiscrepancies_WhenSnoozing()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment()
            };

            SetupGetLocalRatesToFail();
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Snooze();

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0), 0, It.IsAny<Action>()), Times.Once());
        }


        [Fact]
        public void Validate_ReturnsZeroDiscrepancies_WhenShipmentIsNotUps()
        {
            var shipments = new List<ShipmentEntity>
            {
                new ShipmentEntity()
            };

            SetupGetLocalRatesToFail();
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0), 0, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void Validate_DoesNotCountDiscrepancy_WhenShipmentUsesThirdPartyBilling()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(0, UpsPayorType.ThirdParty, UpsServiceType.UpsGround)
            };

            SetupGetLocalRatesToFail();
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0), 1, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void Validate_DoesNotCountDiscrepancy_WhenLocalRatingIsNotEnabledForAccount()
        {
            var shipment = CreateShipment();

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            SetupGetLocalRatesToFail();
            SetupLocalRatingEnabledForAccount(false, shipment.Ups.UpsAccountID);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0), 1, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void Validate_ReturnsDiscrepancy_WhenNoLocalRateIsFoundForUpsService()
        {
            var shipment = CreateShipment(6, UpsPayorType.Sender, UpsServiceType.WorldwideSaver);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 1), 1, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void Validate_ReturnsDiscrepancy_WhenLocalRateDoesNotMatchApiRate()
        {
            var shipment = CreateShipment(6, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 1), 1, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void Validate_ReturnsNoDiscrepancies_WhenLocalRateDoesNotMatchApiRate_ButApiRateWasZero()
        {
            var shipment = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Validate(shipments);

            resultFactory.Verify(
                r => r.Create(It.Is<IEnumerable<UpsLocalRateDiscrepancy>>(d => !d.Any()), 1, It.IsAny<Action>()),
                Times.Once);
        }

        [Fact]
        public void Validate_DoesNotReturnDiscrepancy_WhenGetLocalRatesFails()
        {
            var shipment = CreateShipment(42, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            SetupGetLocalRatesToFail();
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0), 1, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void Validate_ReturnsCorrectNumberOfDiscrepancies_WhenAllLocalRateDoNotMatchApiRates()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(3, UpsPayorType.Sender, UpsServiceType.UpsGround),
                CreateShipment(3, UpsPayorType.Sender, UpsServiceType.Ups2DayAir)
            };

            SetupGetLocalRatesToSucceed();

            foreach (var shipment in shipments)
            {
                SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);
            }

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 2), 2, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void Validate_ReturnsCorrectNumberOfDiscrepancies_WhenSomeLocalRatesMatchApiRates()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(1, UpsPayorType.Sender, UpsServiceType.UpsGround),
                CreateShipment(2, UpsPayorType.Sender, UpsServiceType.UpsGround)
            };

            SetupGetLocalRatesToSucceed();

            foreach (var shipment in shipments)
            {
                SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);
            }

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 1), 2, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void Validate_ReturnsZeroDiscrepancies_WhenAllLocalRatesMatchApiRates()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(1, UpsPayorType.Sender, UpsServiceType.UpsGround),
                CreateShipment(2, UpsPayorType.Sender, UpsServiceType.Ups2DayAir)
            };

            SetupGetLocalRatesToSucceed();

            foreach (var shipment in shipments)
            {
                SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);
            }

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0), 2, It.IsAny<Action>()), Times.Once());
        }
        
        [Fact]
        public void Validate_LogsDiscrepancies_WhenRatesDoNotMatch()
        {
            var shipment = CreateShipment(42, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);
            shipment.Ups.UpsAccountID = 1;
            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            var logger = mock.Mock<IApiLogEntry>();
            var logFactory = mock.MockRepository.Create<Func<ApiLogSource, string, IApiLogEntry>>();
            logFactory.Setup(f => f(ApiLogSource.UpsLocalRating, "Rate Discrepancies")).Returns(logger);

            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            logger.Verify(l => l.LogResponse(It.IsAny<string>(), "txt"));
        }

        [Fact]
        public void Validate_DoesNotLogDiscrepancies_WhenLocalRatesNotFound()
        {
            var shipment = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            var logger = mock.Mock<IApiLogEntry>();
            var logFactory = mock.MockRepository.Create<Func<ApiLogSource, string, IApiLogEntry>>();
            logFactory.Setup(f => f(ApiLogSource.UpsLocalRating, "Rate Discrepancies")).Returns(logger);

            SetupGetLocalRatesToFail();
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            logger.Verify(l => l.LogResponse(It.IsAny<string>(), "txt"), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        private ShipmentEntity CreateShipment()
        {
            return CreateShipment(1, UpsPayorType.Sender, UpsServiceType.UpsGround);
        }

        private ShipmentEntity CreateShipment(decimal shipmentCost, UpsPayorType payorType, UpsServiceType service)
        {
            return new ShipmentEntity
            {
                Processed = true,
                ShipmentCost = shipmentCost,
                Ups = new UpsShipmentEntity
                {
                    UpsAccountID = 0,
                    PayorType = (int)payorType,
                    Service = (int)service
                }, 
                Order = new OrderEntity()
                {
                    OrderNumber = 42
                }
            };
        }

        private void SetupGetLocalRatesToFail()
        {
            var rateClient = mock.Mock<IUpsRateClient>();
            rateClient.Setup(r => r.GetRates(It.IsAny<ShipmentEntity>())).Returns(() => GenericResult.FromError<List<UpsServiceRate>>("Error getting local rates"));

            rateClientFactory.Setup(x => x[UpsRatingMethod.LocalOnly]).Returns(rateClient.Object);
        }

        private void SetupGetLocalRatesToSucceed()
        {
            var rates = new List<UpsServiceRate>
            {
                new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1"),
                new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, "2", 2, "1")
            };

            var rateClient = mock.Mock<IUpsRateClient>();
            rateClient.Setup(r => r.GetRates(It.IsAny<ShipmentEntity>())).Returns(() => GenericResult.FromSuccess(rates));

            rateClientFactory.Setup(x => x[UpsRatingMethod.LocalOnly]).Returns(rateClient.Object);
        }

        private void SetupLocalRatingEnabledForAccount(bool localRatingEnabled, long accountID)
        {
            var account = mock.Mock<IUpsAccountEntity>();
            account.Setup(a => a.LocalRatingEnabled).Returns(localRatingEnabled);

            var accountRetriever = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRetriever.Setup(r => r.GetAccountReadOnly(It.Is<ShipmentEntity>(s => s.Ups.UpsAccountID == accountID))).Returns(account.Object);
        }
    }
}