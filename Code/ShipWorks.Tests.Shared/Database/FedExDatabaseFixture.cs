using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users;
using System;
using System.Windows.Forms;

namespace ShipWorks.Tests.Shared.Database
{
    /// <summary>
    /// Database fixture to manage localdb.
    /// </summary>
    /// <seealso cref="ShipWorks.Tests.Shared.Database.DatabaseFixture" />
    public class FedExDatabaseFixture : DatabaseFixture
    {
        private DataContext context;

        /// <summary>
        /// Gets the FedEx data context
        /// </summary>
        /// <remarks>
        /// Returns an existing context. If one doesn't exist, it is created.
        /// </remarks>
        public DataContext GetFedExDataContext(Action<IContainer> initializeContainer, Guid instance)
        {
            return context ?? (context = CreateFedExDataContext(initializeContainer, instance));
        }

        /// <summary>
        /// Creates the new reusable data context.
        /// </summary>
        private DataContext CreateFedExDataContext(Action<IContainer> initializeContainer, Guid instance)
        {
            var newContext = base.CreateDataContext(initializeContainer);

            newContext.Mock.Provide<Control>(new Control());
            newContext.Mock.Provide<Func<Control>>(() => new Control());
            newContext.Mock.Override<ITangoWebClient>();
            newContext.Mock.Override<IMessageHelper>();

            ShipWorksSession.Initialize(instance);
            LogSession.Initialize();
            UserSession.InitializeForCurrentDatabase();

            UpdateStore(newContext);

            Create.Profile().AsPrimary().AsFedEx().Set(p => p.RequestedLabelFormat, (int)ThermalLanguage.None).Save();

            GenerateAccounts();
            UpdateSettings();

            return newContext;
        }

        /// <summary>
        /// Updates the settings with API username and encrypted password
        /// </summary>
        private static void UpdateSettings()
        {
            var settings = ShippingSettings.Fetch();
            settings.ShipSenseEnabled = false;
            settings.ConfiguredTypes = new[] { ShipmentTypeCode.FedEx };
            settings.ActivatedTypes = new[] { ShipmentTypeCode.FedEx };
            settings.FedExUsername = "7c9NbKsT8K3hUNuf";
            settings.FedExPassword = "JOuIZJZw4EsKW5+YqqpUCmhtumtiZnEUcZWKsHMgtu8=";
            settings.FedExThermalDocTab = true;
            settings.FedExThermalDocTabType = 0; // Leading
            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Updates the store so it is a usable ShipWorks store
        /// </summary>
        private static void UpdateStore(DataContext newContext)
        {
            Modify.Store(newContext.Store)
                .Set(x => x.Enabled, true)
                .Set(x => x.StoreTypeCode, StoreTypeCode.GenericModule)
                .Set(x => x.License, "I5TKE-5RXP3-NGMEN-ZXHMX-GENERIC-BRIAN@INTERAPPTIVE.COM")
                .Set(x => x.Edition,
                    "QTGyeHaEih1ldH2CBYvYjcUj4YRmteV6oXBCl0s7SwjZ+DtjOHT3JD1Uit/x3sF65o4/EnBuifBA6H1hodXIbIgPMmDQTwXiTIcZj3+53Of8ygIUOvKgurrLmicPNHEmyvwwGLtRRGpkygBh0KCqwVazlsaQ0zFBh34mBLhF3TbVKg8ZYKNzwBIcnXPw1iBLNY3JuuJOd1JOXeC86DGf7ZGlZ5lwQF26Z29Mt6uexBtjAHQup4AX4ORKdjqldEfmiqyh+80AcpMhRPQVeB9gTrWzmVmD+AKuwmdI7j5GrxcKc+1Mmh150RfOhgj8NyR9YKGtbHrrih5D4IuqXUX+BwpuNN5ZjPcOmUrQrjiKP37OlaEdBPRzl5UflPXqBOfSe5iCU/LDKQSbqoxoLu/uC8G/gjvMPavdhAyzOQWwHDOcSNIdIf7QBpPBAkcwucIxcpdRCIgeyc76Tcar7Oc7A+AjjfK/mEZty0ORTDi7WO5k4fPygn5ZK0fbV7D6HF1Rj7rZ0WkHV2zLeSro7ZGIuyz1GN6PMS1uK9cTR/Dm7P/WNeUn9aJ5JaOmqnXOzG+RvG/jrlhc126R5wFg/X/kvfkf9oHn4h72UkLSL3wIj8kiARB8r65qCcw0G0McqXs9WACrQjI+UT12/pZrde8M+D7BvoirfH4GOqEzj7JI8weXiPR62ZzdF4WQ7bYKN/RxLb2KQdH2MMUuU2zSV3Xs/VWGKnmUIXdn7An4pMjhm2WiJLdnQXUjfsHdvOVYTPbZwyFI5vZGX3lDhn4Figoog3potyb+r2HeIJOz2h0NAYJThWhfnQOPqMgc2imFTTjvNnLsVtf0x2dWNECBfY3K0UNC31czVHYJKlxWUS7YqPRP1VtRnPFV4WOfb1WfNC7cQGnpYWZZwItv/8JtINa1J9JxxFKWRGuBZWpZax25M7f4Bd2Ndiil9Rg4Nu+TvpGo5DZ+4yKcGwJOhrnPKVWgEe/xPM5RDNl6lMwZrkMJ/QXebTv3NvY2G97LNWG1SxJc/ywoo8exLWqULu+fbZmysFGiH2Tg2qsN+IjQ/DTh6qPMb40q2Ejl5fARyg++3nP5+bWiDy4vJwmHWX84Sw1XUettUTWmcCEAuuDcuDYrgupkQI0FiRRvu/vq+ZFopJ+TE9lV3lDqA63azhwyboSI9WSSmrjtNqhio28utmp38ohq0WUmwfHN0vNY8JP21vA1g33Jb2t7WG6D+1P26GE9XA/CszrOlVNxF0zd5N7kv1Y88FHl4X5actJh/5sd1pxWrlvN0N6F+dRleaJX7Cua4bRVIJXJW9oN/pnPSUNviY2YFEL89Fbxx3bqvJd44a10Bz25HVTC6ib1QRWiPLruAsDOeAYOuOxz")
                .Set(x => x.Company, "ShipWorks")
                .Set(x => x.ManualOrderPostfix, "-M")
                .Set(x => x.ModuleUsername = "kevin")
                .Set(x => x.ModulePassword, "hhWfRUrMqjk=")
                .Set(x => x.ModuleUrl, "http://devsandbox:8880/api/kevin2")
                .Set(x => x.ModuleVersion, "3.10.0")
                .Set(x => x.ModulePlatform, "second")
                .Set(x => x.ModuleDeveloper, "ShipWorks")
                .Set(x => x.ModuleStatusCodes,
                    "<StatusCodes><StatusCode><Code>5</Code><Name>Blah</Name></StatusCode><StatusCode><Code>1</Code><Name>Shipped</Name></StatusCode></StatusCodes>")
                .Set(x => x.ModuleDownloadPageSize, 50)
                .Set(x => x.ModuleRequestTimeout, 60)
                .Set(x => x.ModuleDownloadStrategy = (int)GenericStoreDownloadStrategy.ByModifiedTime)
                .Set(x => x.ModuleOnlineStatusSupport, 2)
                .Set(x => x.ModuleOnlineStatusDataType, 0)
                .Set(x => x.ModuleOnlineCustomerSupport, false)
                .Set(x => x.ModuleOnlineCustomerDataType, 0)
                .Set(x => x.ModuleOnlineStatusDataType, 1)
                .Set(x => x.ModuleHttpExpect100Continue, true)
                .Set(x => x.ModuleResponseEncoding, 0)
                .Set(x => x.SchemaVersion, "1.0.0")
                .Save();
        }

        /// <summary>
        /// Generates the FedEx accounts.
        /// </summary>
        private static void GenerateAccounts()
        {
            Create.CarrierAccount<FedExAccountEntity, IFedExAccountEntity>()
                .Set(x => x.Description, "630148367 - Pickup SmartPost")
                .Set(x => x.AccountNumber, "630148367")
                .Set(x => x.SignatureRelease, "")
                .Set(x => x.MeterNumber, "118753757")
                .Set(x => x.SmartPostHubList, "<Root><HubID>5531</HubID></Root>")
                .Set(x => x.FirstName, "FedEx")
                .Set(x => x.MiddleName, "")
                .Set(x => x.LastName, "SmartPost")
                .Set(x => x.Company, "ShipWorks")
                .Set(x => x.Street1, "10 FedEx Pkwy")
                .Set(x => x.Street2, "")
                .Set(x => x.City, "Collierville")
                .Set(x => x.StateProvCode, "TN")
                .Set(x => x.PostalCode, "38017")
                .Set(x => x.CountryCode, "US")
                .Set(x => x.Phone, "3145551212")
                .Set(x => x.Email, "me@shipworks.com")
                .Set(x => x.Website, "www.shipworks.com")
                .Save();

            Create.CarrierAccount<FedExAccountEntity, IFedExAccountEntity>()
                .Set(x => x.Description, "510158040 - SP Returns")
                .Set(x => x.AccountNumber, "510158040")
                .Set(x => x.SignatureRelease, "")
                .Set(x => x.MeterNumber, "118752826")
                .Set(x => x.SmartPostHubList, "<Root><HubID>5531</HubID></Root>")
                .Set(x => x.FirstName, "SP")
                .Set(x => x.MiddleName, "")
                .Set(x => x.LastName, "Returns")
                .Set(x => x.Company, "ShipWorks")
                .Set(x => x.Street1, "10 FED EX PKWY")
                .Set(x => x.Street2, "")
                .Set(x => x.City, "Collierville")
                .Set(x => x.StateProvCode, "TN")
                .Set(x => x.PostalCode, "38017")
                .Set(x => x.CountryCode, "US")
                .Set(x => x.Phone, "3145551212")
                .Set(x => x.Email, "me@shipworks.com")
                .Set(x => x.Website, "www.shipworks.com")
                .Save();

            Create.CarrierAccount<FedExAccountEntity, IFedExAccountEntity>()
                .Set(x => x.Description, "612480567 - US Test Account")
                .Set(x => x.AccountNumber, "612480567")
                .Set(x => x.SignatureRelease, "")
                .Set(x => x.MeterNumber, "118752839")
                .Set(x => x.SmartPostHubList, "<Root />")
                .Set(x => x.FirstName, "US")
                .Set(x => x.MiddleName, "Test")
                .Set(x => x.LastName, "Account")
                .Set(x => x.Company, "ShipWorks")
                .Set(x => x.Street1, "One Memorial Drive")
                .Set(x => x.Street2, "")
                .Set(x => x.City, "St. Louis")
                .Set(x => x.StateProvCode, "MO")
                .Set(x => x.PostalCode, "63102")
                .Set(x => x.CountryCode, "US")
                .Set(x => x.Phone, "3145551212")
                .Set(x => x.Email, "me@shipworks.com")
                .Set(x => x.Website, "www.shipworks.com")
                .Save();

            Create.CarrierAccount<FedExAccountEntity, IFedExAccountEntity>()
                .Set(x => x.Description, "612365903 - CA Test Account")
                .Set(x => x.AccountNumber, "612365903")
                .Set(x => x.SignatureRelease, "")
                .Set(x => x.MeterNumber, "118752841")
                .Set(x => x.SmartPostHubList, "<Root />")
                .Set(x => x.FirstName, "CA")
                .Set(x => x.MiddleName, "Test")
                .Set(x => x.LastName, "Account")
                .Set(x => x.Company, "ShipWorks")
                .Set(x => x.Street1, "5985 EXPLORER DR")
                .Set(x => x.Street2, "")
                .Set(x => x.City, "MISSISSAUGAl")
                .Set(x => x.StateProvCode, "ON")
                .Set(x => x.PostalCode, "L4W5K6")
                .Set(x => x.CountryCode, "CA")
                .Set(x => x.Phone, "3145551212")
                .Set(x => x.Email, "me@shipworks.com")
                .Set(x => x.Website, "www.shipworks.com")
                .Save();

            Create.CarrierAccount<FedExAccountEntity, IFedExAccountEntity>()
                .Set(x => x.Description, "222326460 - ECOD")
                .Set(x => x.AccountNumber, "222326460")
                .Set(x => x.SignatureRelease, "")
                .Set(x => x.MeterNumber, "118752842")
                .Set(x => x.SmartPostHubList, "<Root />")
                .Set(x => x.FirstName, "ECOD")
                .Set(x => x.MiddleName, "")
                .Set(x => x.LastName, "Account")
                .Set(x => x.Company, "ShipWorks")
                .Set(x => x.Street1, "500 THORNHILL LN")
                .Set(x => x.Street2, "")
                .Set(x => x.City, "Aurora")
                .Set(x => x.StateProvCode, "OH")
                .Set(x => x.PostalCode, "44202")
                .Set(x => x.CountryCode, "US")
                .Set(x => x.Phone, "3145551212")
                .Set(x => x.Email, "me@shipworks.com")
                .Set(x => x.Website, "www.shipworks.com")
                .Save();

            Create.CarrierAccount<FedExAccountEntity, IFedExAccountEntity>()
                .Set(x => x.Description, "510051408, 2000 ARKANSAS 7, 72602")
                .Set(x => x.AccountNumber, "510051408")
                .Set(x => x.SignatureRelease, "")
                .Set(x => x.MeterNumber, "118752843")
                .Set(x => x.SmartPostHubList, "<Root />")
                .Set(x => x.FirstName, "Third")
                .Set(x => x.MiddleName, "")
                .Set(x => x.LastName, "Party")
                .Set(x => x.Company, "ShipWorks")
                .Set(x => x.Street1, "2000 ARKANSAS 7")
                .Set(x => x.Street2, "")
                .Set(x => x.City, "Harrison")
                .Set(x => x.StateProvCode, "AR")
                .Set(x => x.PostalCode, "72602")
                .Set(x => x.CountryCode, "US")
                .Set(x => x.Phone, "3145551212")
                .Set(x => x.Email, "k.croke@shipworks.com")
                .Set(x => x.Website, "www.shipworks.com")
                .Save();
        }
    }
}