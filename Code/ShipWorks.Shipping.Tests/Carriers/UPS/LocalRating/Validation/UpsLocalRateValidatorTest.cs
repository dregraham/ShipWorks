using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
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
        private readonly UpsAccountEntity account;

        public UpsLocalRateValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            resultFactory = mock.Mock<ILocalRateValidationResultFactory>();
            rateClientFactory = mock.MockRepository.Create<IIndex<UpsRatingMethod, IUpsRateClient>>();
            account = new UpsAccountEntity(0);
        }

        #region ValidateShipments
        [Fact]
        public void ValidateShipments_ReturnsZeroDiscrepancies_WhenSnoozing()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment()
            };

            SetupGetLocalRatesToSucceed();
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.Snooze();

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0), 0, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void ValidateShipments_ReturnsZeroDiscrepancies_WhenShipmentIsNotUps()
        {
            var shipments = new List<ShipmentEntity>
            {
                new ShipmentEntity()
            };

            SetupGetLocalRatesToSucceed();
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0), 0, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void ValidateShipments_DoesNotCountDiscrepancy_WhenShipmentUsesThirdPartyBilling()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(0, UpsPayorType.ThirdParty, UpsServiceType.UpsGround)
            };

            SetupGetLocalRatesToSucceed();
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0), 1, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void ValidateShipments_DoesNotCountDiscrepancy_WhenLocalRatingIsNotEnabledForAccount()
        {
            var shipment = CreateShipment();

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(false, shipment.Ups.UpsAccountID);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0), 1, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void ValidateShipments_ReturnsDiscrepancy_WhenNoLocalRateIsFoundForUpsService()
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
        public void ValidateShipments_ReturnsDiscrepancy_WhenLocalRateDoesNotMatchShipmentCost()
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
        public void ValidateShipments_ReturnsDiscrepancy_WhenGetLocalRatesFails()
        {
            var shipment = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            resultFactory.Verify(
                r => r.Create(It.Is<IEnumerable<UpsLocalRateDiscrepancy>>(d => !d.Any()), 1, It.IsAny<Action>()),
                Times.Once);
        }

        [Fact]
        public void ValidateShipments_DoesNotReturnDiscrepancy_WhenGetLocalRatesFails()
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
        public void ValidateShipments_ReturnsCorrectNumberOfDiscrepancies_WhenAllLocalRateDoNotMatchShipmentCost()
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
        public void ValidateShipments_ReturnsCorrectNumberOfDiscrepancies_WhenSomeLocalRatesMatchShipmentCost()
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
        public void ValidateShipments_ReturnsZeroDiscrepancies_WhenAllLocalRatesMatchShipmentCost()
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
        public void ValidateShipments_LogsDiscrepancies_WhenRatesDoNotMatch()
        {
            var shipment = CreateShipment(42, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);
            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            var logger = mock.Mock<IApiLogEntry>();
            var logFactory = mock.MockRepository.Create<Func<ApiLogSource, string, IApiLogEntry>>();
            logFactory.Setup(f => f(ApiLogSource.UpsLocalRating, UpsLocalRateValidator.UploadRatesLogFileName)).Returns(logger);

            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateShipments(shipments);

            logger.Verify(l => l.LogResponse(It.IsAny<string>(), "txt"), Times.Once);
        }

        #endregion

        #region ValidateRecentShipments

        [Fact]
        public void ValidateRecentShipments_ReturnsNoDiscrepancies_WhenShipmentIsNotUps()
        {
            var shipments = new List<ShipmentEntity>
            {
                new ShipmentEntity()
            };

            SetupGetRecentShipments(shipments);
            SetupGetLocalRatesToSucceed();
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateRecentShipments(account);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0)), Times.Once());
        }

        [Fact]
        public void ValidateRecentShipments_DoesNotCountDiscrepancy_WhenShipmentUsesThirdPartyBilling()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(0, UpsPayorType.ThirdParty, UpsServiceType.UpsGround)
            };

            SetupGetRecentShipments(shipments);
            SetupGetLocalRatesToSucceed();
            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateRecentShipments(account);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0)), Times.Once());
        }

        [Fact]
        public void ValidateRecentShipments_ReturnsDiscrepancy_WhenLocalRatingIsNotEnabledForAccount()
        {
            var shipment = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            var apiRates = new List<UpsServiceRate>
            {
                new UpsServiceRate(UpsServiceType.Ups2DayAir, 5, true, 1)
            };

            SetupGetRecentShipments(shipments);
            SetupGetLocalRatesToSucceed();
            SetupApiRateClient(apiRates);
            SetupLocalRatingEnabledForAccount(false, shipment.Ups.UpsAccountID);

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateRecentShipments(account);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 1)), Times.Once());
        }

        [Fact]
        public void ValidateRecentShipments_ReturnsNoDiscrepancies_WhenNoLocalRateIsFoundForUpsService()
        {
            var shipment = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.WorldwideSaver);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            var apiRates = new List<UpsServiceRate>
            {
                new UpsServiceRate(UpsServiceType.UpsGround, 1, true, 1),
                new UpsServiceRate(UpsServiceType.WorldwideSaver, 2, true, 1)
            };

            SetupGetRecentShipments(shipments);
            SetupApiRateClient(apiRates);
            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateRecentShipments(account);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0)), Times.Once());
        }

        [Fact]
        public void ValidateRecentShipments_ReturnsNoDiscrepancies_WhenNoApiRateIsFoundForUpsService()
        {
            var shipment = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            var apiRates = new List<UpsServiceRate>
            {
                new UpsServiceRate(UpsServiceType.UpsGround, 1, true, 1),
                new UpsServiceRate(UpsServiceType.UpsCaWorldWideExpress, 2, true, 1)
            };

            SetupGetRecentShipments(shipments);
            SetupApiRateClient(apiRates);
            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateRecentShipments(account);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0)), Times.Once());
        }

        [Fact]
        public void ValidateRecentShipments_ReturnsNoDiscrepancies_WhenNoLocalOrApiRateIsFoundForUpsService()
        {
            var shipment = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.WorldwideSaver);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            var apiRates = new List<UpsServiceRate>
            {
                new UpsServiceRate(UpsServiceType.UpsGround, 1, true, 1),
                new UpsServiceRate(UpsServiceType.Ups2DayAir, 2, true, 1)
            };

            SetupGetRecentShipments(shipments);
            SetupApiRateClient(apiRates);
            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateRecentShipments(account);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 0)), Times.Once());
        }

        [Fact]
        public void ValidateRecentShipments_ReturnsDiscrepancy_WhenLocalRateDoesNotMatchApiRate()
        {
            var shipment = CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            var apiRates = new List<UpsServiceRate>
            {
                new UpsServiceRate(UpsServiceType.Ups2DayAir, 5, true, 1)
            };

            SetupGetRecentShipments(shipments);
            SetupGetLocalRatesToSucceed();
            SetupApiRateClient(apiRates);
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateRecentShipments(account);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 1)), Times.Once());
        }

        [Fact]
        public void ValidateRecentShipments_ReturnsCorrectNumberOfDiscrepancies_WhenAllLocalRateDoNotMatchApiRate()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(0, UpsPayorType.Sender, UpsServiceType.UpsGround),
                CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir)
            };

            var apiRates = new List<UpsServiceRate>
            {
                new UpsServiceRate(UpsServiceType.UpsGround, 9, true, 1),
                new UpsServiceRate(UpsServiceType.Ups2DayAir, 7, true, 1)
            };

            SetupGetRecentShipments(shipments);
            SetupApiRateClient(apiRates);
            SetupGetLocalRatesToSucceed();

            foreach (var shipment in shipments)
            {
                SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);
            }

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateRecentShipments(account);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 2)), Times.Once());
        }

        [Fact]
        public void ValidateRecentShipments_ReturnsCorrectNumberOfDiscrepancies_WhenSomeLocalRatesMatchApiRates()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(1, UpsPayorType.Sender, UpsServiceType.UpsGround),
                CreateShipment(0, UpsPayorType.Sender, UpsServiceType.Ups2DayAir)
            };

            var apiRates = new List<UpsServiceRate>
            {
                new UpsServiceRate(UpsServiceType.UpsGround, 5, true, 1),
                new UpsServiceRate(UpsServiceType.Ups2DayAir, 2, true, 1)
            };

            SetupGetRecentShipments(shipments);
            SetupApiRateClient(apiRates);
            SetupGetLocalRatesToSucceed();

            foreach (var shipment in shipments)
            {
                SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);
            }

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateRecentShipments(account);

            resultFactory.Verify(r => r.Create(It.Is<List<UpsLocalRateDiscrepancy>>(d => d.Count == 1)), Times.Once());
        }

        [Fact]
        public void ValidateRecentShipments_ReturnsNoDiscrepancies_WhenAllLocalRatesMatchApiRates()
        {
            var shipments = new List<ShipmentEntity>
            {
                CreateShipment(1, UpsPayorType.Sender, UpsServiceType.UpsGround),
                CreateShipment(2, UpsPayorType.Sender, UpsServiceType.Ups2DayAir)
            };

            var apiRates = new List<UpsServiceRate>
            {
                new UpsServiceRate(UpsServiceType.UpsGround, 1, true, 1),
                new UpsServiceRate(UpsServiceType.Ups2DayAir, 2, true, 1)
            };

            SetupGetRecentShipments(shipments);
            SetupApiRateClient(apiRates);
            SetupGetLocalRatesToSucceed();

            foreach (var shipment in shipments)
            {
                SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);
            }

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateRecentShipments(account);

            resultFactory.Verify(r => r.Create(It.Is<IEnumerable<UpsLocalRateDiscrepancy>>(d => !d.Any())), Times.Once());
        }

        [Fact]
        public void ValidateRecentShipments_LogsDiscrepancies_WhenRatesDoNotMatch()
        {
            var shipment = CreateShipment(2, UpsPayorType.Sender, UpsServiceType.Ups2DayAir);

            var shipments = new List<ShipmentEntity>
            {
                shipment
            };

            var apiRates = new List<UpsServiceRate>
            {
                new UpsServiceRate(UpsServiceType.UpsGround, 5, true, 1),
                new UpsServiceRate(UpsServiceType.Ups2DayAir, 6, true, 1)
            };

            SetupGetRecentShipments(shipments);
            SetupApiRateClient(apiRates);

            var logger = mock.Mock<IApiLogEntry>();
            var logFactory = mock.MockRepository.Create<Func<ApiLogSource, string, IApiLogEntry>>();
            logFactory.Setup(f => f(ApiLogSource.UpsLocalRating, UpsLocalRateValidator.UploadRatesLogFileName)).Returns(logger);

            SetupGetLocalRatesToSucceed();
            SetupLocalRatingEnabledForAccount(true, shipment.Ups.UpsAccountID);

            testObject = mock.Create<UpsLocalRateValidator>(new TypedParameter(typeof(IIndex<UpsRatingMethod, IUpsRateClient>), rateClientFactory.Object));

            testObject.ValidateRecentShipments(account);

            logger.Verify(l => l.LogResponse(It.IsAny<string>(), "txt"), Times.Once);
        }
        #endregion

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
            var localRateClient = mock.Mock<IUpsRateClient>();
            localRateClient.Setup(r => r.GetRates(It.IsAny<ShipmentEntity>())).Returns(() => GenericResult.FromError<List<UpsServiceRate>>("Error getting local rates"));

            rateClientFactory.Setup(x => x[UpsRatingMethod.LocalOnly]).Returns(localRateClient.Object);
        }

        private void SetupGetLocalRatesToSucceed()
        {
            var rates = new List<UpsServiceRate>
            {
                new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 1, "1"),
                new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, "2", 2, "1")
            };

            var localRateClient = mock.CreateMock<IUpsRateClient>();
            localRateClient.Setup(r => r.GetRates(It.IsAny<ShipmentEntity>())).Returns(() => GenericResult.FromSuccess(rates));

            rateClientFactory.Setup(x => x[UpsRatingMethod.LocalOnly]).Returns(localRateClient.Object);
        }

        private void SetupLocalRatingEnabledForAccount(bool localRatingEnabled, long accountID)
        {
            var account = mock.Mock<IUpsAccountEntity>();
            account.Setup(a => a.LocalRatingEnabled).Returns(localRatingEnabled);

            var accountRetriever = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRetriever.Setup(r => r.GetAccountReadOnly(It.Is<ShipmentEntity>(s => s.Ups.UpsAccountID == accountID))).Returns(account.Object);
        }

        private void SetupGetRecentShipments(IEnumerable<ShipmentEntity> shipments)
        {
            mock.Mock<IShippingManager>()
                .Setup(m => m.GetShipments(It.IsAny<RelationPredicateBucket>(), It.IsAny<ISortExpression>(), It.IsAny<int>()))
                .Returns(shipments);
        }

        private void SetupApiRateClient(List<UpsServiceRate> rates)
        {
            var apiRateClient = mock.CreateMock<IUpsRateClient>();
            apiRateClient.Setup(r => r.GetRates(It.IsAny<ShipmentEntity>())).Returns(() => GenericResult.FromSuccess(rates));

            rateClientFactory.Setup(x => x[UpsRatingMethod.ApiOnly]).Returns(apiRateClient.Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}