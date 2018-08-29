using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
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
using TechTalk.SpecFlow;
using Xunit;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.Core.Specs.Actions.Tasks
{
    [Binding]
    public class PurgeDatabase_DownloadsSteps
    {
        private const int RetentionPeriodInDays = 180;
        private readonly DateTime cutoffDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(RetentionPeriodInDays));
        private readonly DataContext context;
        private readonly PurgeDatabaseTask testObject;
        private List<DownloadEntity> downloads;

        public PurgeDatabase_DownloadsSteps(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
            });

            testObject = context.Mock.Create<PurgeDatabaseTask>();

            testObject.RetentionPeriodInDays = RetentionPeriodInDays;
        }

        [Given(@"a task with purges \((.*)\)")]
        public void GivenATaskWithPurgesDownloads(IEnumerable<PurgeDatabaseType> purgeTypes) =>
            purgeTypes.ForEach(testObject.Purges.Add);

        [Given(@"the following downloads")]
        public void GivenTheFollowingDownloads(Table table) =>
            downloads = table.Rows
                .Select(CreateDownload)
                .ToList();

        [When(@"I run the purge")]
        public void WhenIRunThePurge() =>
            testObject.Run(new List<long>(), null);

        [Then(@"download \((.*)\) should exist")]
        public void ThenDownloadShouldExist(IEnumerable<int> downloadIndices) =>
            downloadIndices.Select(downloads.ElementAt).ForEach(AssertDownloadExists);

        [Then(@"download \((.*)\) should not exist")]
        public void ThenDownloadShouldNotExist(IEnumerable<int> downloadIndices) =>
            downloadIndices.Select(downloads.ElementAt).ForEach(AssertDownloadDoesNotExist);

        [StepArgumentTransformation]
        public IEnumerable<int> TransformToListOfInts(string commaSeparatedList) =>
            TransformToParsableType(commaSeparatedList, ParseInt);

        [StepArgumentTransformation]
        public IEnumerable<PurgeDatabaseType> TransformToListOfPurgeTypes(string commaSeparatedList) =>
            TransformToParsableType(commaSeparatedList, ParseEnum<PurgeDatabaseType>);

        private static IEnumerable<T> TransformToParsableType<T>(string commaSeparatedList, Func<string, GenericResult<T>> parse) =>
            commaSeparatedList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(parse)
                .Select(x => x.Match(e => e, ex => throw new InvalidCastException()));

        private DownloadEntity CreateDownload(TableRow row)
        {
            var download = Create.Entity<DownloadEntity>()
                   .Set(x => x.ComputerID, context.Computer.ComputerID)
                   .Set(x => x.UserID, context.User.UserID)
                   .Set(x => x.StoreID, context.Store.StoreID)
                   .Set(x => x.Started, GetDateFromRow(row))
                   .Save();

            Create.Entity<DownloadDetailEntity>()
                .Set(x => x.DownloadID, download.DownloadID)
                .Save();

            return download;
        }

        private DateTime GetDateFromRow(TableRow row)
        {
            var started = row["Started"];

            if (started.Equals("now", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.UtcNow;
            }

            var match = Regex.Match(started, "([0-9]+) days? (after|before) cutoff");
            if (match.Success)
            {
                int days = int.Parse(match.Groups[1].Value);
                int multiplier = match.Groups[2]
                        .Value.Equals("after", StringComparison.OrdinalIgnoreCase) ? 1 : -1;

                return cutoffDate.AddDays(days * multiplier);
            }

            throw new InvalidOperationException($"Could not figure out how to create a date from {started}");
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

        private static void AssertDownloadDoesNotExist(DownloadEntity download)
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
    }
}
