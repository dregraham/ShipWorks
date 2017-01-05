using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using System.Xml;
using Autofac;
using Interapptive.Shared.Tests.Filters;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Utility;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Express1.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Express1.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval;
using ShipWorks.Shipping.Settings;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Carriers.Postal.Usps;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ShipmentProcessorTest : IDisposable
    {
        private ShipmentProcessor testObject;
        private readonly DataContext context;
        private ShipmentEntity shipment;
        private Mock<IUspsWebServiceFactory> webServiceFactory;

        public ShipmentProcessorTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                mock =>
                {
                    webServiceFactory = mock.CreateMock<IUspsWebServiceFactory>();
                    mock.Provide(webServiceFactory.Object);
                });

            context.Mock.SetupDefaultMocksForEnumerable<ILabelRetrievalShipmentValidator>(item =>
                item.Setup(x => x.Validate(It.IsAny<ShipmentEntity>())).Returns(Result.FromSuccess()));

            context.Mock.SetupDefaultMocksForEnumerable<ILabelRetrievalShipmentManipulator>(item =>
                item.Setup(x => x.Manipulate(It.IsAny<ShipmentEntity>())).Returns((ShipmentEntity s) => s));

            context.Mock.Provide(new Control());
            context.Mock.Provide<Func<Control>>(() => new Control());
            context.Mock.Override<ITangoWebClient>();
            context.Mock.Override<IMessageHelper>();

            Modify.Store(context.Store)
                .Set(x => x.Enabled, true)
                .Set(x => x.StoreTypeCode, StoreTypeCode.GenericModule)
                .Set(x => x.License, "I5TKE-5RXP3-NGMEN-ZXHMX-GENERIC-BRIAN@INTERAPPTIVE.COM")
                .Set(x => x.Edition, "QTGyeHaEih1ldH2CBYvYjcUj4YRmteV6oXBCl0s7SwjZ+DtjOHT3JD1Uit/x3sF65o4/EnBuifBA6H1hodXIbIgPMmDQTwXiTIcZj3+53Of8ygIUOvKgurrLmicPNHEmyvwwGLtRRGpkygBh0KCqwVazlsaQ0zFBh34mBLhF3TbVKg8ZYKNzwBIcnXPw1iBLNY3JuuJOd1JOXeC86DGf7ZGlZ5lwQF26Z29Mt6uexBtjAHQup4AX4ORKdjqldEfmiqyh+80AcpMhRPQVeB9gTrWzmVmD+AKuwmdI7j5GrxcKc+1Mmh150RfOhgj8NyR9YKGtbHrrih5D4IuqXUX+BwpuNN5ZjPcOmUrQrjiKP37OlaEdBPRzl5UflPXqBOfSe5iCU/LDKQSbqoxoLu/uC8G/gjvMPavdhAyzOQWwHDOcSNIdIf7QBpPBAkcwucIxcpdRCIgeyc76Tcar7Oc7A+AjjfK/mEZty0ORTDi7WO5k4fPygn5ZK0fbV7D6HF1Rj7rZ0WkHV2zLeSro7ZGIuyz1GN6PMS1uK9cTR/Dm7P/WNeUn9aJ5JaOmqnXOzG+RvG/jrlhc126R5wFg/X/kvfkf9oHn4h72UkLSL3wIj8kiARB8r65qCcw0G0McqXs9WACrQjI+UT12/pZrde8M+D7BvoirfH4GOqEzj7JI8weXiPR62ZzdF4WQ7bYKN/RxLb2KQdH2MMUuU2zSV3Xs/VWGKnmUIXdn7An4pMjhm2WiJLdnQXUjfsHdvOVYTPbZwyFI5vZGX3lDhn4Figoog3potyb+r2HeIJOz2h0NAYJThWhfnQOPqMgc2imFTTjvNnLsVtf0x2dWNECBfY3K0UNC31czVHYJKlxWUS7YqPRP1VtRnPFV4WOfb1WfNC7cQGnpYWZZwItv/8JtINa1J9JxxFKWRGuBZWpZax25M7f4Bd2Ndiil9Rg4Nu+TvpGo5DZ+4yKcGwJOhrnPKVWgEe/xPM5RDNl6lMwZrkMJ/QXebTv3NvY2G97LNWG1SxJc/ywoo8exLWqULu+fbZmysFGiH2Tg2qsN+IjQ/DTh6qPMb40q2Ejl5fARyg++3nP5+bWiDy4vJwmHWX84Sw1XUettUTWmcCEAuuDcuDYrgupkQI0FiRRvu/vq+ZFopJ+TE9lV3lDqA63azhwyboSI9WSSmrjtNqhio28utmp38ohq0WUmwfHN0vNY8JP21vA1g33Jb2t7WG6D+1P26GE9XA/CszrOlVNxF0zd5N7kv1Y88FHl4X5actJh/5sd1pxWrlvN0N6F+dRleaJX7Cua4bRVIJXJW9oN/pnPSUNviY2YFEL89Fbxx3bqvJd44a10Bz25HVTC6ib1QRWiPLruAsDOeAYOuOxz")
                .Save();

            Create.Profile().AsPrimary().AsOther().Save();

            ShippingSettings.MarkAsConfigured(ShipmentTypeCode.Other);

            shipment = Create.Shipment(context.Order)
                .AsOther()
                .Save();
        }

        [Fact]
        public async Task Process_ReturnsErrorResult_WhenShipmentCannotBeProcessed()
        {
            testObject = context.Mock.Create<ShipmentProcessor>();

            var result = await ProcessShipment();

            Assert.Equal(1, result.Count());
            Assert.False(result.Single().IsSuccessful);
            Assert.NotNull(result.Single().Error);
        }

        [Fact]
        public async Task Process_DisplaysMessage_WhenShipmentCannotBeProcessed()
        {
            testObject = context.Mock.Create<ShipmentProcessor>();

            await ProcessShipment();

            context.Mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(It.IsAny<string>()));
        }

        [Fact]
        public async Task Process_DisplaysMoreErrorsMessage_WhenMoreThanThreeShipmentsCannotBeProcessed()
        {
            string errorMessage = string.Empty;
            context.Mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowError(It.IsAny<string>()))
                .Callback((string x) => errorMessage = x);

            testObject = context.Mock.Create<ShipmentProcessor>();

            var shippingManager = IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>();
            await ProcessShipments(Enumerable.Repeat(shipment, 4).Select(shippingManager.CreateShipmentCopy));

            var errorCount = errorMessage.Split('\n')
                .Where(x => x.Contains("No carrier is specified"))
                .Count();

            Assert.Contains("See the shipment list for all errors", errorMessage);
            Assert.Equal(3, errorCount);
        }

        [Fact]
        public async Task Process_ReturnsSuccessResult_WhenShipmentIsProcessed()
        {
            Modify.Entity(shipment.Other)
                .Set(x => x.Carrier, "Foo")
                .Set(x => x.Service, "Bar")
                .Save();

            testObject = context.Mock.Create<ShipmentProcessor>();

            var result = await ProcessShipment();

            Assert.Equal(1, result.Count());
            Assert.True(result.Single().IsSuccessful);
            Assert.Null(result.Single().Error);
        }

        [Fact]
        public async Task Process_MarksShipmentCopyAsProcessed_WhenShipmentIsProcessed()
        {
            context.Mock.Override<IDateTimeProvider>()
                .Setup(x => x.UtcNow).Returns(new DateTime(2016, 1, 20, 10, 30, 0));

            Modify.Entity(shipment.Other)
                .Set(x => x.Carrier, "Foo")
                .Set(x => x.Service, "Bar")
                .Save();

            testObject = context.Mock.Create<ShipmentProcessor>();

            var result = await ProcessShipment();

            Assert.True(result.Single().Shipment.Processed);
            Assert.Equal(new DateTime(2016, 1, 20, 10, 30, 0), result.Single().Shipment.ProcessedDate);
            Assert.Equal(context.User.UserID, result.Single().Shipment.ProcessedUserID);
            Assert.Equal(context.Computer.ComputerID, result.Single().Shipment.ProcessedComputerID);
        }

        [Fact]
        public async Task Process_ShipmentIsSaved_WhenShipmentIsProcessed()
        {
            context.Mock.Override<IDateTimeProvider>()
                .Setup(x => x.UtcNow).Returns(new DateTime(2016, 1, 20, 10, 30, 0));

            Modify.Entity(shipment.Other)
                .Set(x => x.Carrier, "Foo")
                .Set(x => x.Service, "Bar")
                .Save();

            testObject = context.Mock.Create<ShipmentProcessor>();

            await ProcessShipment();

            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                var loadedShipment = new ShipmentEntity(shipment.ShipmentID);
                adapter.FetchEntity(loadedShipment);

                Assert.True(loadedShipment.Processed);
                Assert.Equal(new DateTime(2016, 1, 20, 10, 30, 0), loadedShipment.ProcessedDate);
                Assert.Equal(context.User.UserID, loadedShipment.ProcessedUserID);
                Assert.Equal(context.Computer.ComputerID, loadedShipment.ProcessedComputerID);
            }
        }

        [Fact]
        public async Task Process_DoesNotModifyOriginalShipment_WhenShipmentIsProcessed()
        {
            Modify.Entity(shipment.Other)
                .Set(x => x.Carrier, "Foo")
                .Set(x => x.Service, "Bar")
                .Save();

            testObject = context.Mock.Create<ShipmentProcessor>();

            await ProcessShipment();

            Assert.False(shipment.Processed);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Usps, typeof(UspsSetupWizard))]
        [InlineData(ShipmentTypeCode.Endicia, typeof(EndiciaSetupWizard))]
        [InlineData(ShipmentTypeCode.Express1Usps, typeof(Express1UspsSetupWizard))]
        [InlineData(ShipmentTypeCode.Express1Endicia, typeof(Express1EndiciaSetupWizard))]
        [InlineData(ShipmentTypeCode.FedEx, typeof(FedExSetupWizard))]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, typeof(UpsSetupWizard))]
        [InlineData(ShipmentTypeCode.iParcel, typeof(iParcelSetupWizard))]
        [InlineData(ShipmentTypeCode.OnTrac, typeof(OnTracSetupWizard))]
        public async Task Process_ShowsAddAccountDialog_WhenProcessingWithNoAccount(
            ShipmentTypeCode shipmentType, Type expectedWizardType)
        {
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(shipmentType);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(shipmentType, shipment);

            Func<IForm> dialogCreator = null;
            context.Mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IForm>>()))
                .Callback((Func<IForm> x) => dialogCreator = x);

            testObject = context.Mock.Create<ShipmentProcessor>();

            await ProcessShipment();

            Assert.IsType(expectedWizardType, dialogCreator?.Invoke());
        }

        [Fact]
        public async Task Process_ShowsErrorMessage_WhenTimeoutErrorIsReceived()
        {
            Mock<ISwsimV55> webService = context.Mock.CreateMock<ISwsimV55>(w =>
            {
                UspsTestHelpers.SetupAddressValidationResponse(w);
                w.Setup(x => x.CreateIndicium(It.IsAny<CreateIndiciumParameters>()))
                    .Throws(new WebException("There was an error", WebExceptionStatus.Timeout));
            });

            string errorMessage = string.Empty;
            context.Mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowError(It.IsAny<string>()))
                .Callback((string x) => errorMessage = x);

            webServiceFactory
                .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<LogActionType>()))
                .Returns(webService);

            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.Usps);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.Usps, shipment);

            var account = Create.CarrierAccount<UspsAccountEntity, IUspsAccountEntity>().Save();
            shipment.Postal.Usps.UspsAccountID = account.AccountId;
            shipment.TotalWeight = 3;

            testObject = context.Mock.Create<ShipmentProcessor>();

            await ProcessShipment();

            Assert.Contains("There was an error", errorMessage);
        }

        [Fact]
        public async Task Process_ShowsErrorMessage_WhenLabelIsBad()
        {
            Mock<ISwsimV55> webService = context.Mock.CreateMock<ISwsimV55>(w =>
            {
                UspsTestHelpers.SetupAddressValidationResponse(w);
                w.Setup(x => x.CreateIndicium(It.IsAny<CreateIndiciumParameters>()))
                    .Returns(new CreateIndiciumResult
                    {
                        Rate = new RateV20(),
                        ImageData = new[] { new byte[] { 0x20, 0x20 } },
                    });
            });

            webServiceFactory.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<LogActionType>()))
                .Returns(webService);

            string errorMessage = string.Empty;
            context.Mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowError(It.IsAny<string>()))
                .Callback((string x) => errorMessage = x);

            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.Usps);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.Usps, shipment);

            var account = Create.CarrierAccount<UspsAccountEntity, IUspsAccountEntity>().Save();
            shipment.Postal.Usps.UspsAccountID = account.AccountId;
            shipment.TotalWeight = 3;

            testObject = context.Mock.Create<ShipmentProcessor>();

            await ProcessShipment();

            Assert.Contains("Parameter is not valid", errorMessage);
        }

        [Fact]
        public async Task Process_ShowsErrorMessage_WhenServerReturns500()
        {
            Mock<ISwsimV55> webService = context.Mock.CreateMock<ISwsimV55>(w =>
            {
                XmlDocument details = new XmlDocument();
                details.LoadXml("<error><details code=\"bar\" /></error>");

                UspsTestHelpers.SetupAddressValidationResponse(w);
                w.Setup(x => x.CreateIndicium(It.IsAny<CreateIndiciumParameters>()))
                    .Throws(new SoapException("There was an error", new XmlQualifiedName("abc"), "actor", details));
            });

            webServiceFactory.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<LogActionType>()))
                .Returns(webService);

            string errorMessage = string.Empty;
            context.Mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowError(It.IsAny<string>()))
                .Callback((string x) => errorMessage = x);

            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.Usps);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.Usps, shipment);

            var account = Create.CarrierAccount<UspsAccountEntity, IUspsAccountEntity>().Save();
            shipment.Postal.Usps.UspsAccountID = account.AccountId;
            shipment.TotalWeight = 3;

            testObject = context.Mock.Create<ShipmentProcessor>();

            await ProcessShipment();

            Assert.Contains("There was an error", errorMessage);
        }

        /// <summary>
        /// Tests that trying to process a shipment that is already locked for processing will fail.
        /// </summary>
        [Fact]
        public async Task Process_Fails_WhenEntityLockAlreadyExistsOnDifferentConnection()
        {
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.Usps);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.Usps, shipment);

            using (SqlConnection con = new SqlConnection(SqlAdapter.Default.ConnectionString))
            {
                con.Open();

                using (SqlEntityLock processLock = new SqlEntityLock(con, shipment.ShipmentID, "Process Shipment"))
                {
                    testObject = context.Mock.Create<ShipmentProcessor>();

                    IEnumerable<ProcessShipmentResult> results = await ProcessShipment();

                    Assert.True(results.Any(r => !r.IsSuccessful && r.Error.Message.Contains("The shipment was being processed on another computer.")));
                }
            }
        }

        /// <summary>
        /// Tests that processing a WorldShip shipment adds a row to the WorldShipShipment table but
        /// does not add anything to the action queue or print result tables.
        /// </summary>
        [Fact]
        public async Task Process_SuccessfullyAddsWorldShipShipmentRowsButDoesNotPrintALabel()
        {
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.UpsWorldShip);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.UpsWorldShip, shipment);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.UpsOnLineTools);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.UpsOnLineTools, shipment);

            UpsAccountEntity account = Create.CarrierAccount<UpsAccountEntity, IUpsAccountEntity>().Save();

            Create.Profile().AsPrimary()
                .AsUps(p => p.Set(x => x.UpsAccountID, account.AccountId + 2000))
                .Save();

            shipment = Create.Shipment(context.Order)
                .AsUpsWorldShip(s => s.WithPackage())
                .Save();

            testObject = context.Mock.Create<ShipmentProcessor>();

            shipment.Ups.Packages[0].DimsWidth = 6;
            shipment.Ups.Packages[0].DimsLength = 6;
            shipment.Ups.Packages[0].DimsHeight = 6;
            shipment.Ups.Packages[0].DimsAddWeight = true;
            shipment.Ups.Packages[0].DimsWeight = 10;

            IEnumerable<ProcessShipmentResult> results = await ProcessShipment();

            Assert.True(results.All(r => r.IsSuccessful), $"ProcessShipment failed with error: {results.First().Error?.Message}");

            using (SqlConnection connection = new SqlConnection(SqlAdapter.Default.ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $"select count(*) from WorldShipShipment where ShipmentID = {shipment.ShipmentID}";

                    int count = int.Parse(cmd.ExecuteScalar().ToString());
                    Assert.True(1 == count, "ProcessShipment failed to save the correct number of rows to WorldShipShipment.");
                }

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $"select count(*) from ActionQueue where ObjectID = {shipment.ShipmentID}";

                    int count = int.Parse(cmd.ExecuteScalar().ToString());
                    Assert.True(0 == count, "ProcessShipment added rows to ActionQueue, even though it should not.");
                }

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $"select count(*) from PrintResult where ContextObjectID = {shipment.ShipmentID}";

                    int count = int.Parse(cmd.ExecuteScalar().ToString());
                    Assert.True(0 == count, "ProcessShipment added rows to PrintResult, even though it should not.");
                }
            }
        }

        /// <summary>
        /// Tests that processing a WorldShip shipment adds a row to the WorldShipShipment table but
        /// does not add anything to the action queue or print result tables.
        /// </summary>
        [Fact]
        public async Task FiltersUpdate_WhenShipmentSuccessfullyProcessed()
        {
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.UpsWorldShip);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.UpsWorldShip, shipment);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.UpsOnLineTools);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.UpsOnLineTools, shipment);

            UpsAccountEntity account = Create.CarrierAccount<UpsAccountEntity, IUpsAccountEntity>().Save();

            Create.Profile().AsPrimary()
                .AsUps(p => p.Set(x => x.UpsAccountID, account.AccountId + 2000))
                .Save();

            shipment = Create.Shipment(context.Order)
                .AsUpsWorldShip(s => s.WithPackage())
                .Save();

            testObject = context.Mock.Create<ShipmentProcessor>();

            shipment.Ups.Packages[0].DimsWidth = 6;
            shipment.Ups.Packages[0].DimsLength = 6;
            shipment.Ups.Packages[0].DimsHeight = 6;
            shipment.Ups.Packages[0].DimsAddWeight = true;
            shipment.Ups.Packages[0].DimsWeight = 10;

            FilterDefinition definition = new FilterDefinition(FilterTarget.Shipments);
            ConditionGroup group = definition.RootContainer.FirstGroup;
            group.Conditions.Add(new ShipmentStatusCondition() { Value = ShipmentStatusType.Processed });

            FilterNodeEntity filterNode = FilterTestingHelper.CreateFilterNode(definition);

            FilterTestingHelper.CalculateInitialCounts();

            IEnumerable<ProcessShipmentResult> results = await ProcessShipment();

            Assert.True(results.All(r => r.IsSuccessful), $"ProcessShipment failed with error: {results.First().Error?.Message}");

            FilterContentManager.CalculateUpdateCounts();
            FilterTestingHelper.WaitForFilterCountsToFinish();

            int afterProcessCount = FilterTestingHelper.GetFilterNodeContentCount(filterNode.FilterNodeContentID);

            Assert.True(afterProcessCount != 0, "Processing the shipment did not update filter counts.");
        }

        /// <summary>
        /// Test that a message is displayed when dimensions are missing.
        /// </summary>
        [Fact]
        public async Task Process_DisplaysMessage_WhenShipmentDimensionsAreMissing()
        {
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.UpsWorldShip);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.UpsWorldShip, shipment);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.UpsOnLineTools);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.UpsOnLineTools, shipment);

            UpsAccountEntity account = Create.CarrierAccount<UpsAccountEntity, IUpsAccountEntity>().Save();

            Create.Profile().AsPrimary()
                .AsUps(p => p.Set(x => x.UpsAccountID, account.AccountId + 2000))
                .Save();

            shipment = Create.Shipment(context.Order)
                .AsUpsWorldShip(s => s.WithPackage())
                .Save();

            testObject = context.Mock.Create<ShipmentProcessor>();

            IEnumerable<ProcessShipmentResult> results = await ProcessShipment();

            Assert.True(results.All(r => !r.IsSuccessful), "ProcessShipment succeeded, but shouldn't have.");
            Assert.True(results.All(r => r.Error.Message.Contains("has invalid dimensions", StringComparison.InvariantCultureIgnoreCase)),
                "ProcessShipment succeeded even though it was missing dimensions.");

            context.Mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(It.IsAny<string>()), "ShowError was not called.");
        }

        /// <summary>
        /// Test that a message is displayed when dimensions are invalid.
        /// </summary>
        [Fact]
        public async Task Process_DisplaysMessage_WhenShipmentDimensionsAreIncorrect()
        {
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.UpsWorldShip);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.UpsWorldShip, shipment);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.UpsOnLineTools);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.UpsOnLineTools, shipment);

            UpsAccountEntity account = Create.CarrierAccount<UpsAccountEntity, IUpsAccountEntity>().Save();

            Create.Profile().AsPrimary()
                .AsUps(p => p.Set(x => x.UpsAccountID, account.AccountId + 2000))
                .Save();

            shipment = Create.Shipment(context.Order)
                .AsUpsWorldShip(s => s.WithPackage())
                .Save();

            shipment.Ups.Packages[0].DimsWidth = int.MaxValue;
            shipment.Ups.Packages[0].DimsLength = int.MaxValue;
            shipment.Ups.Packages[0].DimsHeight = int.MaxValue;
            shipment.Ups.Packages[0].DimsAddWeight = true;
            shipment.Ups.Packages[0].DimsWeight = int.MaxValue;

            testObject = context.Mock.Create<ShipmentProcessor>();

            IEnumerable<ProcessShipmentResult> results = await ProcessShipment();

            Assert.True(results.All(r => !r.IsSuccessful), "ProcessShipment succeeded, but shouldn't have.");

            context.Mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(It.IsAny<string>()), "ShowError was not called.");
        }

        /// <summary>
        /// Test that a message is displayed when dimensions are missing.
        /// </summary>
        [Fact]
        public async Task Process_DisplaysMessage_WhenShipmentWeightMissing()
        {
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.UpsWorldShip);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.UpsWorldShip, shipment);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.UpsOnLineTools);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.UpsOnLineTools, shipment);

            UpsAccountEntity account = Create.CarrierAccount<UpsAccountEntity, IUpsAccountEntity>().Save();

            Create.Profile().AsPrimary()
                .AsUps(p => p.Set(x => x.UpsAccountID, account.AccountId + 2000))
                .Save();

            shipment = Create.Shipment(context.Order)
                .AsUpsWorldShip(s => s.WithPackage())
                .Save();

            shipment.Ups.Packages[0].DimsWidth = 6;
            shipment.Ups.Packages[0].DimsLength = 6;
            shipment.Ups.Packages[0].DimsHeight = 6;
            shipment.Ups.Packages[0].DimsAddWeight = true;
            shipment.Ups.Packages[0].DimsWeight = 0;

            shipment.Ups.Packages[0].Weight = 0;
            shipment.TotalWeight = 0;
            shipment.BilledWeight = 0;
            shipment.ContentWeight = 0;

            testObject = context.Mock.Create<ShipmentProcessor>();

            IEnumerable<ProcessShipmentResult> results = await ProcessShipment();

            Assert.True(results.All(r => !r.IsSuccessful), "ProcessShipment succeeded, but shouldn't have due to missing weight.");

            context.Mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(It.IsAny<string>()), "ShowError was not called.");
        }

        /// <summary>
        /// Test that a message is displayed when dimensions are invalid.
        /// </summary>
        [Fact]
        public async Task Process_DisplaysMessage_WhenShipmentWeightIsIncorrect()
        {
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.UpsWorldShip);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.UpsWorldShip, shipment);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>().MarkAsConfigured(ShipmentTypeCode.UpsOnLineTools);
            IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingManager>().ChangeShipmentType(ShipmentTypeCode.UpsOnLineTools, shipment);

            UpsAccountEntity account = Create.CarrierAccount<UpsAccountEntity, IUpsAccountEntity>().Save();

            Create.Profile().AsPrimary()
                .AsUps(p => p.Set(x => x.UpsAccountID, account.AccountId + 2000))
                .Save();

            shipment = Create.Shipment(context.Order)
                .AsUpsWorldShip(s => s.WithPackage())
                .Save();

            shipment.Ups.Packages[0].DimsWidth = 6;
            shipment.Ups.Packages[0].DimsLength = 6;
            shipment.Ups.Packages[0].DimsHeight = 6;
            shipment.Ups.Packages[0].DimsAddWeight = true;
            shipment.Ups.Packages[0].DimsWeight = 0;

            shipment.Ups.Packages[0].Weight = int.MaxValue;
            shipment.TotalWeight = int.MaxValue;
            shipment.BilledWeight = int.MaxValue;
            shipment.ContentWeight = int.MaxValue;

            testObject = context.Mock.Create<ShipmentProcessor>();

            IEnumerable<ProcessShipmentResult> results = await ProcessShipment();

            Assert.True(results.All(r => !r.IsSuccessful), "ProcessShipment succeeded, but shouldn't have due to invalid weight.");

            context.Mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(It.IsAny<string>()), "ShowError was not called.");
        }

        private Task<IEnumerable<ProcessShipmentResult>> ProcessShipment() =>
            ProcessShipments(new[] { shipment });

        private Task<IEnumerable<ProcessShipmentResult>> ProcessShipments(IEnumerable<ShipmentEntity> shipments) =>
            testObject.Process(shipments,
                context.Mock.Create<ICarrierConfigurationShipmentRefresher>(),
                null, null);

        public void Dispose()
        {
            context?.Dispose();
        }
    }
}
