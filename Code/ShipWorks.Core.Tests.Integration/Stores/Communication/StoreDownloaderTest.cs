using System;
using System.Data.Common;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Stores.Communication
{
    [Collection("Database collection")]
    public class StoreDownloaderTest : IDisposable
    {
        private readonly DatabaseFixture db;
        private readonly DataContext context;

        public StoreDownloaderTest(DatabaseFixture db)
        {
            this.db = db;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

       
        [Theory]
        [InlineData(AddressValidationStoreSettingType.ValidateAndNotify, AddressValidationStoreSettingType.ManualValidationOnly, "US", AddressValidationStatusType.Pending)]
        [InlineData(AddressValidationStoreSettingType.ManualValidationOnly, AddressValidationStoreSettingType.ManualValidationOnly, "US", AddressValidationStatusType.NotChecked)]
        [InlineData(AddressValidationStoreSettingType.ManualValidationOnly, AddressValidationStoreSettingType.ValidateAndNotify, "CA", AddressValidationStatusType.Pending)]
        [InlineData(AddressValidationStoreSettingType.ManualValidationOnly, AddressValidationStoreSettingType.ManualValidationOnly, "CA", AddressValidationStatusType.NotChecked)]
        public async Task Download_SavesOrderWithAddressValidationOfNotChecked_WhenStoreSetToDownloadAndNotify(
            AddressValidationStoreSettingType domesticStoreSetting,
            AddressValidationStoreSettingType internationalStoreSetting,
            string countryCode,
            AddressValidationStatusType expectedOrderStatus)
        {
            Modify.Store(context.Store)
                .Set(s => s.DomesticAddressValidationSetting = domesticStoreSetting)
                .Set(s=> s.InternationalAddressValidationSetting = internationalStoreSetting)
                .Save();

            var statusCreator = Create.Entity<StatusPresetEntity>()
                .Set(x => x.StoreID, context.Store.StoreID)
                .Set(x => x.StatusText, "status")
                .Set(x => x.IsDefault, true);
            
            statusCreator.Set(x => x.StatusTarget, 0).Save();
            statusCreator.Set(x => x.StatusTarget, 1).Save();

            StatusPresetManager.CheckForChanges();

            var testObject = context.Mock.Create<FakeStoreDownloader>(
                TypedParameter.From((StoreEntity) context.Store),
                TypedParameter.From(StoreTypeManager.GetType(context.Store)),
                TypedParameter.From(StoreTypeCode.GenericModule),
                TypedParameter.From(countryCode));

            var downloadEntity = Create.Entity<DownloadEntity>()
                .Set(d => d.StoreID, context.Store.StoreID)
                .Set(d => d.ComputerID, context.Computer.ComputerID)
                .Set(d => d.UserID, context.User.UserID)
                .Set(d => d.InitiatedBy, 0)
                .Set(d => d.Started, DateTime.Now)
                .Set(d => d.Result, 0)
                .Save();

            using (DbConnection conn = SqlSession.Current.OpenConnection())
            {
                await testObject.Download(context.Mock.Mock<IProgressReporter>().Object, downloadEntity.DownloadID, conn);
            }

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                var query = new QueryFactory().Order
                    .Select(OrderFields.ShipAddressValidationStatus)
                    .Where(OrderFields.OrderNumber == 42);
                    
                var addressValidationStatus = await sqlAdapter.FetchScalarAsync<int>(query);
                Assert.Equal(expectedOrderStatus, (AddressValidationStatusType) addressValidationStatus);
            }
        }

        public void Dispose() => context.Dispose();
    }

    class FakeStoreDownloader : StoreDownloader
    {
        private readonly string orderCountryCode;

        public FakeStoreDownloader(string orderCountryCode, StoreEntity store, StoreType storeType, IConfigurationData configurationData, ISqlAdapterFactory sqlAdapterFactory) 
            : base(store, storeType, configurationData, sqlAdapterFactory)
        {
            this.orderCountryCode = orderCountryCode;
        }

        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            
            var orderResult = await InstantiateOrder(new OrderNumberIdentifier(42));
            orderResult.Value.OrderTotal = 0;
            orderResult.Value.OrderDate = DateTime.Now;
            orderResult.Value.OnlineLastModified = DateTime.Now;
            orderResult.Value.ShipStreet1 = "123 woop st";
            orderResult.Value.ShipCountryCode = orderCountryCode;

            await SaveDownloadedOrder(orderResult.Value);
        }
    }
}