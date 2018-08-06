using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autofac;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Tasks.Common
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class PurgeDatabaseTask_Downloads_Test
    {
        private const int RetentionPeriodInDays = 180;
        private readonly DataContext context;
        private readonly DateTime cutoffDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(RetentionPeriodInDays));
        private readonly PurgeDatabaseTask testObject;

        public PurgeDatabaseTask_Downloads_Test(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
            });

            testObject = context.Mock.Create<PurgeDatabaseTask>();

            testObject.Purges.Add(PurgeDatabaseType.Downloads);
            testObject.RetentionPeriodInDays = RetentionPeriodInDays;
        }

        [Fact]
        public void PurgeDatabaseTask_PurgesTwoDownloads_WhenHalfOfDownloadsAreOlderThan180Days()
        {
            var download1 = CreateDownload(DateTime.UtcNow);
            var download2 = CreateDownload(cutoffDate.AddDays(1));
            var download3 = CreateDownload(cutoffDate.AddDays(-1));
            var download4 = CreateDownload(cutoffDate.AddDays(-1000));

            testObject.Run(new List<long>(), null);

            AssertDownloadExists(download1);
            AssertDownloadExists(download2);
            AssertDownloadDoesNotExists(download3);
            AssertDownloadDoesNotExists(download4);
        }

        [Fact]
        public void PurgeDatabaseTask_PurgesAllDownloads_WhenAllDownloadsAreOlderThan180Days()
        {
            var download1 = CreateDownload(cutoffDate.AddDays(-1));
            var download2 = CreateDownload(cutoffDate.AddDays(-2));
            var download3 = CreateDownload(cutoffDate.AddDays(-3));
            var download4 = CreateDownload(cutoffDate.AddDays(-1000));

            testObject.Run(new List<long>(), null);

            AssertDownloadDoesNotExists(download1);
            AssertDownloadDoesNotExists(download2);
            AssertDownloadDoesNotExists(download3);
            AssertDownloadDoesNotExists(download4);
        }

        [Fact]
        public void PurgeDatabaseTask_PurgesNoDownloads_WhenNoDownloadsAreOlderThan180Days()
        {
            var download1 = CreateDownload(cutoffDate.AddDays(1));
            var download2 = CreateDownload(cutoffDate.AddDays(2));
            var download3 = CreateDownload(cutoffDate.AddDays(3));
            var download4 = CreateDownload(cutoffDate.AddDays(1000));

            testObject.Run(new List<long>(), null);

            AssertDownloadExists(download1);
            AssertDownloadExists(download2);
            AssertDownloadExists(download3);
            AssertDownloadExists(download4);
        }

        private static void AssertDownloadExists(DownloadEntity download)
        {
            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var factory = new QueryFactory();

                var downloadQuery = factory.Download.Where(DownloadFields.DownloadID == download.DownloadID);
                var entity = sqlAdapter.FetchFirst(downloadQuery);
                Assert.NotNull(entity);

                var detailQuery = factory.DownloadDetail.Where(DownloadDetailFields.DownloadID == download.DownloadID);
                var detailEntity = sqlAdapter.FetchFirst(detailQuery);
                Assert.NotNull(detailEntity);
            }
        }

        private static void AssertDownloadDoesNotExists(DownloadEntity download)
        {
            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var factory = new QueryFactory();

                var downloadQuery = factory.Download.Where(DownloadFields.DownloadID == download.DownloadID);
                var entity = sqlAdapter.FetchFirst(downloadQuery);
                Assert.Null(entity);

                var detailQuery = factory.DownloadDetail.Where(DownloadDetailFields.DownloadID == download.DownloadID);
                var detailEntity = sqlAdapter.FetchFirst(detailQuery);
                Assert.Null(detailEntity);
            }
        }

        private DownloadEntity CreateDownload(DateTime dateTime)
        {
            var download = Create.Entity<DownloadEntity>()
                .Set(x => x.ComputerID, context.Computer.ComputerID)
                .Set(x => x.UserID, context.User.UserID)
                .Set(x => x.StoreID, context.Store.StoreID)
                .Set(x => x.Started, dateTime)
                .Save();

            Create.Entity<DownloadDetailEntity>()
                .Set(x => x.DownloadID, download.DownloadID)
                .Save();

            return download;
        }
    }
}
