using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using Moq;
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
        private Mock<IIndex<UpsRatingMethod, IUpsRateClient>> rateClientFactory;
        private UpsLocalRateValidator testObject;

        public UpsLocalRateValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            resultFactory = mock.Mock<ILocalRateValidationResultFactory>();
            rateClientFactory = new Mock<IIndex<UpsRatingMethod, IUpsRateClient>>();
        }

        [Fact]
        public void Validate_ReturnsZeroDiscrepencies_WhenSnoozing()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment()
            };

            SetupGetLocalRatesToFail();
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Snooze();
            testObject.Validate(shipments);

            resultFactory.Verify(r => r.Create(shipments.Count, 0));
        }


        [Fact]
        public void Validate_ReturnsZeroDiscrepencies_WhenShipmentIsNotUps()
        {
            var shipments = new List<ShipmentEntity>
            {
                new ShipmentEntity()
            };

            SetupGetLocalRatesToFail();
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Validate(shipments);

            resultFactory.Verify(r => r.Create(shipments.Count, 0));
        }

        [Fact]
        public void Validate_DoesNotCountDiscrepency_WhenShipmentUsesThirdPartyBilling()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(0, UpsPayorType.ThirdParty, UpsServiceType.UpsGround)
            };

            SetupGetLocalRatesToFail();
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Validate(shipments);

            resultFactory.Verify(r => r.Create(shipments.Count, 0));
        }

        [Fact]
        public void Validate_DoesNotCountDiscrepency_WhenLocalRatingIsNotEnabledForAccount()
        {
            var shipment = CreateShipment();

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            SetupGetLocalRatesToFail();
            SetupLocalRatingEnabledForAccount(false, shipment);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Validate(shipments);

            resultFactory.Verify(r => r.Create(shipments.Count, 0));
        }

        [Fact]
        public void Validate_ReturnsDiscrepency_WhenNoLocalRateIsFoundForUpsService()
        {
            var shipment = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.WorldwideSaver);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(true, shipment);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Validate(shipments);

            resultFactory.Verify(r => r.Create(shipments.Count, 1));
        }

        [Fact]
        public void Validate_ReturnsDiscrepency_WhenLocalRateDoesNotMatchApiRate()
        {
            var shipment = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(true, shipment);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Validate(shipments);

            resultFactory.Verify(r => r.Create(shipments.Count, 1));
        }

        [Fact]
        public void Validate_ReturnsDiscrepency_WhenGetLocalRatesFails()
        {
            var shipment = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            SetupGetLocalRatesToFail();
            SetupLocalRatingEnabledForAccount(true, shipment);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Validate(shipments);

            resultFactory.Verify(r => r.Create(shipments.Count, 1));
        }

        [Fact]
        public void Validate_ReturnsCorrectNumberOfDiscrepencies_WhenAllLocalRateDoNotMatchApiRates()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(0, UpsPayorType.Sender, UpsServiceType.UpsGround),
                CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir)
            };

            SetupGetLocalRatesToSucceed();

            foreach (var shipment in shipments)
            {
                SetupLocalRatingEnabledForAccount(true, shipment);
            }

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Validate(shipments);

            resultFactory.Verify(r => r.Create(shipments.Count, 2));
        }

        [Fact]
        public void Validate_ReturnsCorrectNumberOfDiscrepencies_WhenSomeLocalRatesMatchApiRates()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(1, UpsPayorType.Sender, UpsServiceType.UpsGround),
                CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir)
            };

            SetupGetLocalRatesToSucceed();

            foreach (var shipment in shipments)
            {
                SetupLocalRatingEnabledForAccount(true, shipment);
            }

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Validate(shipments);

            resultFactory.Verify(r => r.Create(shipments.Count, 1));
        }

        [Fact]
        public void Validate_ReturnsZeroDiscrepencies_WhenAllLocalRatesMatchApiRates()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(1, UpsPayorType.Sender, UpsServiceType.UpsGround),
                CreateShipment(2, UpsPayorType.Sender, UpsServiceType.Ups2DayAir)
            };

            SetupGetLocalRatesToSucceed();

            foreach (var shipment in shipments)
            {
                SetupLocalRatingEnabledForAccount(true, shipment);
            }

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Validate(shipments);

            resultFactory.Verify(r => r.Create(shipments.Count, 0));
        }

        [Fact]
        public void Validate_ReturnsCorrectNumberOfDiscrepancies_WhenSomeAccountsHaveLocalRatingEnabledAndSomeDoNot()
        {
            var shipment1 = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.UpsGround);
            shipment1.Ups.UpsAccountID = 1;

            var shipment2 = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);
            shipment2.Ups.UpsAccountID = 2;

            var shipments = new List<ShipmentEntity>
            {
                shipment1,
                shipment2
            };

            SetupGetLocalRatesToSucceed();

            SetupLocalRatingEnabledForAccount(true, shipment1);
            SetupLocalRatingEnabledForAccount(false, shipment2);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Validate(shipments);

            resultFactory.Verify(r => r.Create(shipments.Count, 0));
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
                ShipmentCost = shipmentCost,
                Ups = new UpsShipmentEntity
                {
                    PayorType = (int)payorType,
                    Service = (int)service
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

        private void SetupLocalRatingEnabledForAccount(bool localRatingEnabled, ShipmentEntity shipmentFromAccount)
        {
            var account = mock.Mock<IUpsAccountEntity>();
            account.Setup(a => a.LocalRatingEnabled).Returns(localRatingEnabled);

            var accountRetriever = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRetriever.Setup(r => r.GetAccountReadOnly(shipmentFromAccount)).Returns(account.Object);
        }
    }
}