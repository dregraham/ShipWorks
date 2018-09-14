using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using TechTalk.SpecFlow;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Specs.ShippingPanel
{
    [Binding]
    public class BestRateSteps : IDisposable
    {
        private readonly DataContext context;
        private readonly IMessenger messenger;
        private readonly ShippingPanelViewModel shippingPanelViewModel;
        private Mock<ISwsimV69> client;
        private Mock<ILicenseService> licenseService;

        public BestRateSteps(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                client = CreateMockedUspsWebService(mock);
                var uspsWebServiceFactory = mock.CreateMock<IUspsWebServiceFactory>();

                uspsWebServiceFactory.Setup(x => x.Create(AnyString, It.IsAny<LogActionType>()))
                    .Returns(client);
                mock.Provide(uspsWebServiceFactory.Object);
                mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());

                licenseService = mock.CreateMock<ILicenseService>();
                mock.Provide(licenseService.Object);

                mock.Override<IMessageHelper>();
                mock.Override<IMainForm>();
            });

            IoC.UnsafeGlobalLifetimeScope
                .Resolve<IEnumerable<ILabelRetrievalShipmentValidator>>()
                .Select(MockFromObject)
                .ForEach(ReturnTrueForValidation);

            Modify.Store(context.Store)
                .Set(x => x.Enabled, true)
                .Save();

            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "results");

            DataPath.Initialize(
                instancePath: Path.Combine(currentDirectory, "instance"),
                commonSettingsPath: Path.Combine(currentDirectory, "common"),
                tempPath: Path.Combine(currentDirectory, "temp")
            );

            LogSession.Initialize("specs");
            LogSession.Configure(new LogOptions { TraceToConsole = false });

            licenseService.Setup(x => x.HandleRestriction(EditionFeature.SelectionLimit, It.IsAny<object>(), It.IsAny<IWin32Window>()))
                .Returns(true);

            var shippingSettings = IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingSettings>();
            var settings = shippingSettings.Fetch();
            settings.BestRateExcludedTypes = Enum.GetValues(typeof(ShipmentTypeCode))
                .OfType<ShipmentTypeCode>()
                .Where(x => x != ShipmentTypeCode.Usps);
            shippingSettings.Save(settings);

            ShippingSettings.MarkAsConfigured(ShipmentTypeCode.Usps);
            ShippingSettings.MarkAsConfigured(ShipmentTypeCode.BestRate);

            IoC.UnsafeGlobalLifetimeScope
                .Resolve<IEnumerable<IInitializeForCurrentUISession>>()
                .ForEach(x => x.InitializeForCurrentSession());

            messenger = IoC.UnsafeGlobalLifetimeScope
                .Resolve<IMessenger>();

            shippingPanelViewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<ShippingPanelViewModel>();
        }

        [Given(@"a Legacy Tango account")]
        public void GivenALegacyTangoAccount()
        {
            //ScenarioContext.Current.Pending();
        }

        [Given(@"Best Rate is (off|on) in Tango")]
        public void GivenBestRateIsOffInTango(string bestRateSetting) =>
            licenseService.Setup(x => x.CheckRestriction(EditionFeature.ShipmentType, ShipmentTypeCode.BestRate))
                .Returns(bestRateSetting == "on" ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden);

        [Given(@"a Best Rate shipment is loaded in the Shipping Panel")]
        public async Task GivenABestRateShipmentIsLoadedInTheShippingPanel()
        {
            var shipment = Create.Shipment(context.Order)
                .AsBestRate(s =>
                {
                    s.Set(x => x.DimsWeight, 1)
                    .Set(x => x.DimsLength, 1)
                    .Set(x => x.DimsWidth, 1)
                    .Set(x => x.DimsHeight, 1);
                })
                .WithAddress(x => x.OriginPerson, "1 Memorial Dr.", "", "St. Louis", "MO", "63102", "US")
                .WithAddress(x => x.ShipPerson, "1 Memorial Dr.", "", "St. Louis", "MO", "63102", "US")
                .Set(x => x.TotalWeight, 2)
                .Set(x => x.ContentWeight, 2)
                .Save();

            //var shipment = ShippingManager.CreateShipment(context.Order);
            var shipmentAdapter = context.Container.Resolve<ICarrierShipmentAdapterFactory>().Get(shipment);

            var message = new OrderSelectionChangedMessage(
                this,
                new IOrderSelection[]
                {
                    new LoadedOrderSelection(context.Order, new [] { shipmentAdapter }, Enumerable.Empty<KeyValuePair<long, ShippingAddressEditStateType>>())
                },
                null);

            shippingPanelViewModel.LoadOrder(message);
        }

        [Given(@"the only rate is USPS Priority Mail")]
        public void GivenTheOnlyRateIsUSPSPriorityMail()
        {
            var account = Create.CarrierAccount<UspsAccountEntity, IUspsAccountEntity>()
                .WithAddress(x => x.Address, "1 Memorial Dr.", "", "St. Louis", "MO", "63102", "US")
                .Save();
            Create.Profile().AsPrimary().AsPostal(x => x.AsUsps()).Save();

            client.Setup(x => x.GetRates(It.IsAny<Credentials>(), It.IsAny<RateV25>()))
                .Returns(new[] { new RateV25
                {
                    ServiceType = ServiceType.USPM,
                    Amount = 10,
                    AddOns = new AddOnV11[0],
                    DeliverDays = "1"
                    } });
        }

        [When(@"a shipment is loaded")]
        public void WhenAShipmentIsLoaded()
        {
            var shipment = ShippingManager.CreateShipment(context.Order);
            var shipmentAdapter = context.Container.Resolve<ICarrierShipmentAdapterFactory>().Get(shipment);

            var message = new OrderSelectionChangedMessage(
                this,
                new IOrderSelection[]
                {
                    new LoadedOrderSelection(context.Order, new [] { shipmentAdapter }, Enumerable.Empty<KeyValuePair<long, ShippingAddressEditStateType>>())
                },
                null);

            shippingPanelViewModel.LoadOrder(message);
        }

        [When(@"the shipment is processed")]
        public async Task WhenTheShipmentIsProcessed()
        {
            shippingPanelViewModel.CreateLabel();

            var completedTask = Task.WhenAll(
                messenger.OfType<ShipmentChangedMessage>().FirstAsync().ToTask(),
                messenger.OfType<ShipmentsProcessedMessage>().FirstAsync().ToTask());

            var foo = await Task.WhenAny(
                completedTask,
                Task.Delay(TimeSpan.FromSeconds(10))
                    .ContinueWith(t => throw new TimeoutException("Never received ShipmentsProcessedMessage")));
        }

        [Then(@"the user can not access Best Rate")]
        public void ThenTheUserCanNotAccessBestRate() =>
            Assert.DoesNotContain(ShipmentTypeCode.BestRate, shippingPanelViewModel.AvailableProviders);

        [Then(@"the user can access Best Rate")]
        public void ThenTheUserCanAccessBestRate() =>
            Assert.Contains(ShipmentTypeCode.BestRate, shippingPanelViewModel.AvailableProviders);

        [Then(@"the provider is USPS")]
        public Task ThenTheProviderIsUSPS() =>
            RetryAssertion(() => Assert.Equal(ShipmentTypeCode.Usps, shippingPanelViewModel.ShipmentType));

        /// <summary>
        /// Retry a given assertion, since we may need to wait for actions to complete
        /// </summary>
        /// <param name="action"></param>
        private Task RetryAssertion(Action action) =>
            Functional.RetryAsync(() =>
            {
                action();
                return Task.FromResult(Unit.Default);
            }, 5, TimeSpan.FromSeconds(250), ex => true);

        /// <summary>
        /// Create a mocked version of ISwsimV69
        /// </summary>
        private Mock<ISwsimV69> CreateMockedUspsWebService(Autofac.Extras.Moq.AutoMock mock)
        {
            var uspsClient = mock.CreateMock<ISwsimV69>();
            uspsClient.Setup(x => x.GetAccountInfo(It.IsAny<Credentials>()))
                .Returns(new AccountInfoResult(new AccountInfoV27
                {
                    Terms = new Terms
                    {
                        TermsAR = true,
                        TermsGP = true,
                        TermsSL = true
                    }
                }, new Address(), "foo@example.com"));
            uspsClient.Setup(x => x.CleanseAddressAsync(AnyObject, It.IsAny<Address>(), AnyString))
                .Callback((object value, Address address, string thing) => uspsClient.Raise(x => x.CleanseAddressCompleted += null, CreateCleanseAddressResponse(address)));
            uspsClient.Setup(x => x.CreateIndicium(It.IsAny<CreateIndiciumParameters>()))
                .Returns(new CreateIndiciumResult
                {
                    TrackingNumber = "123abc",
                    Rate = new RateV25(),
                    StampsTxID = Guid.NewGuid()
                });
            return uspsClient;
        }

        /// <summary>
        /// Create a cleansed address response
        /// </summary>
        private static CleanseAddressCompletedEventArgs CreateCleanseAddressResponse(Address address)
        {
            var results = new object[]
            {
                "", // Result (string)
                address,  // Address (Address)
                true, // AddressMatch (bool)
                true, // CityStateZipOK (bool)
                ResidentialDeliveryIndicatorType.No,
                false, // IsPOBox
                true, // IsPOBoxSpecified
                new [] { address }, // CandidateAddresses
                new StatusCodes(), // Status codes
                new RateV25[0], // Rates
                "", // AddressCleansingResult
                AddressVerificationLevel.Maximum // Verification Level
            };

            return new CleanseAddressCompletedEventArgs(results, null, false, null);
        }

        /// <summary>
        /// Return true for mocked instances of ILabelRetrievalShipmentValidator
        /// </summary>
        private void ReturnTrueForValidation(GenericResult<Mock<ILabelRetrievalShipmentValidator>> obj) =>
            obj.Map(m => m.Setup(x => x.Validate(AnyShipment)).Returns(Result.FromSuccess()));

        /// <summary>
        /// Try to get a mock from an object
        /// </summary>
        private GenericResult<Mock<ILabelRetrievalShipmentValidator>> MockFromObject(ILabelRetrievalShipmentValidator value) =>
            Functional.Try(() => Mock.Get(value));

        public void Dispose()
        {
            IoC.UnsafeGlobalLifetimeScope
                .Resolve<IEnumerable<IInitializeForCurrentUISession>>()
                .ForEach(x => x.EndSession());

            shippingPanelViewModel.Dispose();
        }
    }
}
