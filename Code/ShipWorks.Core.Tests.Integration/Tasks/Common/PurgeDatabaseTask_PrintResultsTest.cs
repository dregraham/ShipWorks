using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Common;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Templates;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Tasks.Common
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class PurgeDatabaseTask_PrintResultsTest
    {
        private readonly DataContext context;
        private int RetentionPeriodInDays = 30;
        private readonly List<PurgeTableData> tableDataList = new List<PurgeTableData>();
        private readonly QueryFactory queryFactory = new QueryFactory();
        private readonly ComputerEntity computer;

        public PurgeDatabaseTask_PrintResultsTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
            });

            computer = Create.Entity<ComputerEntity>()
                .Set(c => c.Identifier = Guid.NewGuid())
                .Save();
        }

        [Fact]
        public void PurgeDatabaseTask_PrintResultsTest_LessThanRetentionDate_WhenPurgeHistoryIsFalse()
        {
            List<(long ConsumerID, ThermalLanguage LabelFormat, long EntityID, long ResourceID)> resourcesForPurging = new List<(long ConsumerID, ThermalLanguage LabelFormat, long EntityID, long ResourceID)>();
            List<(long ConsumerID, long ResourceID)> resourcesToNotChange = new List<(long ConsumerID, long ResourceID)>();

            CreateEntities(resourcesForPurging, resourcesToNotChange, DateTime.UtcNow.AddDays(-(RetentionPeriodInDays + 1)), DateTime.UtcNow.AddDays(RetentionPeriodInDays + 1));

            LoadInitialTableData();

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(new SqlPurgeScriptRunner(), new DateTimeProvider());
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);
            testObject.PurgeEmailHistory = false;
            testObject.PurgePrintJobHistory = false;
            testObject.RetentionPeriodInDays = RetentionPeriodInDays;

            // Execute the purge
            testObject.Run(new List<long>(), null);

            UpdateTableDataAfterPurge();

            // First, make sure counts are the same...no deletions happened.
            var resourceTable = tableDataList.First(t => t.TableName == "ResourceEntity");
            var objRefTable = tableDataList.First(t => t.TableName == "ObjectReferenceEntity");
            var printResultTable = tableDataList.First(t => t.TableName == "PrintResultEntity");

            Assert.Equal(5, resourceTable.AfterPurgeRowCount);
            Assert.Equal(objRefTable.InitialRowCount, objRefTable.AfterPurgeRowCount);
            Assert.Equal(printResultTable.InitialRowCount, printResultTable.AfterPurgeRowCount);

            // Make sure no printResult data changed
            Assert.True(printResultTable.AreTablesEqual());

            // Make sure placeholder rows got added
            int htmlPlaceholder = int.Parse(resourceTable.AfterPurgeTable.Select(@"Filename = '__purged_print_html.swr'").First()[0].ToString());
            int thermalPlaceholder = int.Parse(resourceTable.AfterPurgeTable.Select(@"Filename = '__purged_print_thermal.swr'").First()[0].ToString());

            // Make sure old rows now point to place holders
            long newResourceID;
            foreach (var resourceForPurging in resourcesForPurging)
            {
                switch (resourceForPurging.LabelFormat)
                {
                    case ThermalLanguage.None:
                        newResourceID = htmlPlaceholder;
                        break;
                    case ThermalLanguage.EPL:
                    case ThermalLanguage.ZPL:
                        newResourceID = thermalPlaceholder;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                long count = GetCounts(queryFactory.ObjectReference,
                    ObjectReferenceFields.ConsumerID == resourceForPurging.ConsumerID & ObjectReferenceFields.EntityID == newResourceID,
                    ObjectReferenceFields.ObjectReferenceID.CountBig());

                Assert.Equal(1, count);
            }

            // Make sure new rows don't change.
            foreach (var resourceToNotChange in resourcesToNotChange)
            {
                long count = GetCounts(queryFactory.ObjectReference,
                    ObjectReferenceFields.ConsumerID == resourceToNotChange.ConsumerID & ObjectReferenceFields.EntityID == resourceToNotChange.ResourceID,
                    ObjectReferenceFields.ObjectReferenceID.CountBig());
                Assert.Equal(1, count);

                // Make sure Resource entry still exists
                count = GetCounts(queryFactory.Resource,
                    ResourceFields.ResourceID == resourceToNotChange.ResourceID,
                    ResourceFields.ResourceID.CountBig());
                Assert.Equal(1, count);
            }
        }

        [Fact]
        public void PurgeDatabaseTask_PrintResultsTest_LessThanRetentionDate_WhenPurgeHistoryIsTrue()
        {
            List<(long ConsumerID, ThermalLanguage LabelFormat, long EntityID, long ResourceID)> resourcesForPurging = new List<(long ConsumerID, ThermalLanguage LabelFormat, long EntityID, long ResourceID)>();
            List<(long ConsumerID, long ResourceID)> resourcesToNotChange = new List<(long ConsumerID, long ResourceID)>();

            CreateEntities(resourcesForPurging, resourcesToNotChange, DateTime.UtcNow.AddDays(-(RetentionPeriodInDays + 1)), DateTime.UtcNow.AddDays(RetentionPeriodInDays + 1));

            LoadInitialTableData();

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(new SqlPurgeScriptRunner(), new DateTimeProvider());
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);
            testObject.PurgeEmailHistory = false;
            testObject.PurgePrintJobHistory = true;
            testObject.RetentionPeriodInDays = RetentionPeriodInDays;

            // Execute the purge
            testObject.Run(new List<long>(), null);

            UpdateTableDataAfterPurge();

            // First, make sure counts are the same...no deletions happened.
            var resourceTable = tableDataList.First(t => t.TableName == "ResourceEntity");
            var objRefTable = tableDataList.First(t => t.TableName == "ObjectReferenceEntity");
            var printResultTable = tableDataList.First(t => t.TableName == "PrintResultEntity");

            Assert.Equal(3, resourceTable.AfterPurgeRowCount);
            Assert.Equal(3, objRefTable.AfterPurgeRowCount);
            Assert.Equal(3, printResultTable.AfterPurgeRowCount);

            // Make sure no printResult data changed
            Assert.True(printResultTable.AreTablesEqual());

            // Make sure placeholder rows got added
            Assert.True(resourceTable.AfterPurgeTable.Select(@"Filename = '__purged_print_html.swr'").None());
            Assert.True(resourceTable.AfterPurgeTable.Select(@"Filename = '__purged_print_thermal.swr'").None());

            // Make sure old rows now point to place holders
            foreach (var resourceForPurging in resourcesForPurging)
            {
                long count = GetCounts(queryFactory.ObjectReference,
                    ObjectReferenceFields.ConsumerID == resourceForPurging.ConsumerID, // & ObjectReferenceFields.EntityID == newResourceID,
                    ObjectReferenceFields.ObjectReferenceID.CountBig());

                Assert.Equal(0, count);
            }

            // Make sure new rows don't change.
            foreach (var resourceToNotChange in resourcesToNotChange)
            {
                long count = GetCounts(queryFactory.ObjectReference,
                    ObjectReferenceFields.ConsumerID == resourceToNotChange.ConsumerID & ObjectReferenceFields.EntityID == resourceToNotChange.ResourceID,
                    ObjectReferenceFields.ObjectReferenceID.CountBig());
                Assert.Equal(1, count);

                // Make sure Resource entry still exists
                count = GetCounts(queryFactory.Resource,
                    ResourceFields.ResourceID == resourceToNotChange.ResourceID,
                    ResourceFields.ResourceID.CountBig());
                Assert.Equal(1, count);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PurgeDatabaseTask_PrintResultsTest_NoDeletes_WhenDatesGreaterThanRetentionDate(bool purgePrintJobHistory)
        {
            RetentionPeriodInDays = 180;

            List<(long ConsumerID, ThermalLanguage LabelFormat, long EntityID, long ResourceID)> resourcesForPurging = new List<(long ConsumerID, ThermalLanguage LabelFormat, long EntityID, long ResourceID)>();
            List<(long ConsumerID, long ResourceID)> resourcesToNotChange = new List<(long ConsumerID, long ResourceID)>();

            CreateEntities(resourcesForPurging, resourcesToNotChange, DateTime.UtcNow, DateTime.UtcNow);

            LoadInitialTableData();

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(new SqlPurgeScriptRunner(), new DateTimeProvider());
            testObject.Purges.Add(PurgeDatabaseType.Labels);
            testObject.PurgeEmailHistory = false;
            testObject.PurgePrintJobHistory = purgePrintJobHistory;
            testObject.RetentionPeriodInDays = RetentionPeriodInDays;

            // Execute the purge
            testObject.Run(new List<long>(), null);

            UpdateTableDataAfterPurge();

            // First, make sure counts are the same...no deletions happened.
            var resourceTable = tableDataList.First(t => t.TableName == "ResourceEntity");
            var objRefTable = tableDataList.First(t => t.TableName == "ObjectReferenceEntity");
            var printResultTable = tableDataList.First(t => t.TableName == "PrintResultEntity");

            Assert.Equal(resourceTable.InitialRowCount, resourceTable.AfterPurgeRowCount);
            Assert.Equal(objRefTable.InitialRowCount, objRefTable.AfterPurgeRowCount);
            Assert.Equal(printResultTable.InitialRowCount, printResultTable.AfterPurgeRowCount);

            // Make sure no printResult data changed
            Assert.True(printResultTable.AreTablesEqual());

            // Make sure placeholder rows got added
            Assert.True(resourceTable.AfterPurgeTable.Select(@"Filename = '__purged_print_html.swr'").None());
            Assert.True(resourceTable.AfterPurgeTable.Select(@"Filename = '__purged_print_thermal.swr'").None());

            // Make sure old rows now point to place holders
            foreach (var resourceForPurging in resourcesForPurging)
            {
                long count = GetCounts(queryFactory.ObjectReference,
                    ObjectReferenceFields.ConsumerID == resourceForPurging.ConsumerID,
                    ObjectReferenceFields.ObjectReferenceID.CountBig());

                Assert.Equal(1, count);
            }

            // Make sure new rows don't change.
            foreach (var resourceToNotChange in resourcesToNotChange)
            {
                long count = GetCounts(queryFactory.ObjectReference,
                    ObjectReferenceFields.ConsumerID == resourceToNotChange.ConsumerID & ObjectReferenceFields.EntityID == resourceToNotChange.ResourceID,
                    ObjectReferenceFields.ObjectReferenceID.CountBig());
                Assert.Equal(1, count);

                // Make sure Resource entry still exists
                count = GetCounts(queryFactory.Resource,
                    ResourceFields.ResourceID == resourceToNotChange.ResourceID,
                    ResourceFields.ResourceID.CountBig());
                Assert.Equal(1, count);
            }
        }

        private void CreateEntities(List<(long ConsumerID, ThermalLanguage LabelFormat, long EntityID, long ResourceID)> resourcesForPurging,
            List<(long ConsumerID, long ResourceID)> resourcesToNotChange, DateTime processedDateToPurge, DateTime processedDateToKeep)
        {
            // Create labels for purging
            foreach (var labelFormat in EnumHelper.GetEnumList<ThermalLanguage>())
            {
                TemplateType templateType = labelFormat.Value == ThermalLanguage.None ? TemplateType.Label : TemplateType.Thermal;

                PrintResultEntity printResult = Create.Entity<PrintResultEntity>()
                    .Set(a => a.PrintDate = processedDateToPurge)
                    .Set(a => a.ComputerID = computer.ComputerID)
                    .Set(a => a.TemplateType = (int) templateType)
                    .Save();

                ResourceEntity resource = Create.Entity<ResourceEntity>()
                    .Set(r => r.Filename = Guid.NewGuid().ToString())
                    .Set(r => r.Checksum = CalculateChecksum(Guid.NewGuid().ToString()))
                    .Save();

                ObjectReferenceEntity objRef = Create.Entity<ObjectReferenceEntity>()
                    .Set(o => o.ConsumerID = printResult.PrintResultID)
                    .Set(o => o.EntityID = resource.ResourceID)
                    .Save();

                Modify.Entity(printResult)
                    .Set(pr => pr.ContentResourceID = objRef.ObjectReferenceID)
                    .Save();

                resourcesForPurging.Add((ConsumerID: objRef.ConsumerID, LabelFormat: labelFormat.Value, EntityID: objRef.EntityID, ResourceID: resource.ResourceID));
            }

            // Create labels to keep (not purge)
            foreach (var labelFormat in EnumHelper.GetEnumList<ThermalLanguage>())
            {
                TemplateType templateType = labelFormat.Value == ThermalLanguage.None ? TemplateType.Label : TemplateType.Thermal;

                PrintResultEntity printResult = Create.Entity<PrintResultEntity>()
                    .Set(a => a.PrintDate = processedDateToKeep)
                    .Set(a => a.ComputerID = computer.ComputerID)
                    .Set(a => a.TemplateType = (int) templateType)
                    .Save();

                ResourceEntity resource = Create.Entity<ResourceEntity>()
                    .Set(r => r.Filename = Guid.NewGuid().ToString())
                    .Set(r => r.Checksum = CalculateChecksum(Guid.NewGuid().ToString()))
                    .Save();

                ObjectReferenceEntity objRef = Create.Entity<ObjectReferenceEntity>()
                    .Set(o => o.ConsumerID = printResult.PrintResultID)
                    .Set(o => o.EntityID = resource.ResourceID)
                    .Save();

                Modify.Entity(printResult)
                    .Set(pr => pr.ContentResourceID = objRef.ObjectReferenceID)
                    .Save();

                resourcesToNotChange.Add((ConsumerID: objRef.ConsumerID, ResourceID: resource.ResourceID));
            }
        }

        private byte[] CalculateChecksum(string dataToCalculate)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(dataToCalculate));
            }
        }

        private long GetCounts<T>(EntityQuery<T> entityQuery, IPredicate predicate, IEntityFieldCore countBigField) where T : IEntityCore
        {
            long counts = 0;

            using (ISqlAdapter sqlAdapter = context.Mock.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                var countOrderQuery = entityQuery
                    .Where(predicate)
                    .Select(countBigField);

                counts = sqlAdapter.FetchScalar<long>(countOrderQuery);
            }

            return counts;
        }

        private void LoadInitialTableData()
        {
            tableDataList.Add(new PurgeTableData(nameof(ResourceEntity), FetchDataTable(queryFactory.Resource, ResourceFields.ResourceID > 0), null));
            tableDataList.Add(new PurgeTableData(nameof(ObjectReferenceEntity), FetchDataTable(queryFactory.ObjectReference, ObjectReferenceFields.ObjectReferenceID > 0), null));
            tableDataList.Add(new PurgeTableData(nameof(PrintResultEntity), FetchDataTable(queryFactory.PrintResult, PrintResultFields.PrintResultID > 1), null));
        }

        private void UpdateTableDataAfterPurge()
        {
            tableDataList.First(t => t.TableName == nameof(ResourceEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.Resource, ResourceFields.ResourceID > 0));
            tableDataList.First(t => t.TableName == nameof(ObjectReferenceEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.ObjectReference, ObjectReferenceFields.ObjectReferenceID > 0));
            tableDataList.First(t => t.TableName == nameof(PrintResultEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.PrintResult, PrintResultFields.PrintResultID > 1));
        }

        private DataTable FetchDataTable<T>(EntityQuery<T> entityQuery, IPredicate predicate) where T : IEntityCore
        {
            using (ISqlAdapter sqlAdapter = context.Mock.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                var countOrderQuery = entityQuery
                    .Where(predicate).Select(Projection.Full);

                return sqlAdapter.FetchAsDataTable(countOrderQuery);
            }
        }
    }
}
