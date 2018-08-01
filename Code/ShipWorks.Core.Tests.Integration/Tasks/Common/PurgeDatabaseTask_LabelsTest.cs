using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Tasks.Common
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class PurgeDatabaseTask_LabelsTest
    {
        private readonly DataContext context;
        private int RetentionPeriodInDays = 30;
        private readonly List<PurgeTableData> tableDataList = new List<PurgeTableData>();
        private QueryFactory queryFactory = new QueryFactory();

        public PurgeDatabaseTask_LabelsTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
            });
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesLabels_LessThanRetentionDate()
        {
            List<(long ConsumerID, ThermalLanguage LabelFormat, long EntityID, long ResourceID)> resourcesForPurging = new List<(long ConsumerID, ThermalLanguage LabelFormat, long EntityID, long ResourceID)>();
            List<(long ConsumerID, long ResourceID)> resourcesToNotChange = new List<(long ConsumerID, long ResourceID)>();

            CreateEntities(resourcesForPurging, resourcesToNotChange, DateTime.UtcNow.AddDays(-(RetentionPeriodInDays + 1)), DateTime.UtcNow.AddDays(RetentionPeriodInDays + 1));

            LoadInitialTableData();

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(new SqlPurgeScriptRunner(), new DateTimeProvider());
            testObject.Purges.Add(PurgeDatabaseType.Labels);
            testObject.PurgeEmailHistory = false;
            testObject.PurgePrintJobHistory = false;
            testObject.RetentionPeriodInDays = RetentionPeriodInDays;

            // Execute the purge
            testObject.Run(new List<long>(), null);

            UpdateTableDataAfterPurge();

            // First, make sure counts are the same...no deletions happened.
            var resourceTable = tableDataList.First(t => t.TableName == "ResourceEntity");
            var objRefTable = tableDataList.First(t => t.TableName == "ObjectReferenceEntity");
            var shipmentTable = tableDataList.First(t => t.TableName == "ShipmentEntity");

            Assert.Equal(resourceTable.InitialRowCount, resourceTable.AfterPurgeRowCount);
            Assert.Equal(objRefTable.InitialRowCount, objRefTable.AfterPurgeRowCount);
            Assert.Equal(shipmentTable.InitialRowCount, shipmentTable.AfterPurgeRowCount);

            // Make sure no shipment data changed
            Assert.True(shipmentTable.AreTablesEqual());

            // Make sure placeholder rows got added
            int pngPlaceholder = int.Parse(resourceTable.AfterPurgeTable.Select(@"Filename = '__purged_label.png'").First()[0].ToString());
            int eplPlaceholder = int.Parse(resourceTable.AfterPurgeTable.Select(@"Filename = '__purged_label_epl.swr'").First()[0].ToString());
            int zplPlaceholder = int.Parse(resourceTable.AfterPurgeTable.Select(@"Filename = '__purged_label_zpl.swr'").First()[0].ToString());

            // Make sure old rows now point to place holders
            long newResourceID;
            foreach (var resourceForPurging in resourcesForPurging)
            {
                switch (resourceForPurging.LabelFormat)
                {
                    case ThermalLanguage.None:
                        newResourceID = pngPlaceholder;
                        break;
                    case ThermalLanguage.EPL:
                        newResourceID = eplPlaceholder;
                        break;
                    case ThermalLanguage.ZPL:
                        newResourceID = zplPlaceholder;
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
        public async Task PurgeDatabaseTask_PurgesLabels_NoDeletes_WhenDatesGreaterThanRetentionDate()
        {
            RetentionPeriodInDays = 180;

            List<(long ConsumerID, ThermalLanguage LabelFormat, long EntityID, long ResourceID)> resourcesForPurging = new List<(long ConsumerID, ThermalLanguage LabelFormat, long EntityID, long ResourceID)>();
            List<(long ConsumerID, long ResourceID)> resourcesToNotChange = new List<(long ConsumerID, long ResourceID)>();

            CreateEntities(resourcesForPurging, resourcesToNotChange, DateTime.UtcNow, DateTime.UtcNow);

            LoadInitialTableData();

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(new SqlPurgeScriptRunner(), new DateTimeProvider());
            testObject.Purges.Add(PurgeDatabaseType.Labels);
            testObject.PurgeEmailHistory = false;
            testObject.PurgePrintJobHistory = false;
            testObject.RetentionPeriodInDays = RetentionPeriodInDays;

            // Execute the purge
            testObject.Run(new List<long>(), null);

            UpdateTableDataAfterPurge();

            // First, make sure counts are the same...no deletions happened.
            var resourceTable = tableDataList.First(t => t.TableName == "ResourceEntity");
            var objRefTable = tableDataList.First(t => t.TableName == "ObjectReferenceEntity");
            var shipmentTable = tableDataList.First(t => t.TableName == "ShipmentEntity");

            Assert.Equal(resourceTable.InitialRowCount, resourceTable.AfterPurgeRowCount);
            Assert.Equal(objRefTable.InitialRowCount, objRefTable.AfterPurgeRowCount);
            Assert.Equal(shipmentTable.InitialRowCount, shipmentTable.AfterPurgeRowCount);

            // Make sure no shipment data changed
            Assert.True(shipmentTable.AreTablesEqual());

            // Make sure placeholder rows got added
            Assert.True(resourceTable.AfterPurgeTable.Select(@"Filename = '__purged_label.png'").None());
            Assert.True(resourceTable.AfterPurgeTable.Select(@"Filename = '__purged_label_epl.swr'").None());
            Assert.True(resourceTable.AfterPurgeTable.Select(@"Filename = '__purged_label_zpl.swr'").None());

            // Make sure old rows now point to place holders
            long newResourceID;
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
                ShipmentEntity shipment = Create.Shipment(context.Order)
                    .AsFedEx(x => x.WithPackage())
                    .Set(a => a.ProcessedDate = processedDateToPurge)
                    .Set(s => s.Processed = true)
                    .Set(s => s.ActualLabelFormat = (int) labelFormat.Value)
                    .Save();

                ResourceEntity resource = Create.Entity<ResourceEntity>()
                    .Set(r => r.Filename = Guid.NewGuid().ToString())
                    .Set(r => r.Checksum = CalculateChecksum(Guid.NewGuid().ToString()))
                    .Save();

                ObjectReferenceEntity objRef = Create.Entity<ObjectReferenceEntity>()
                    .Set(o => o.ConsumerID = shipment.ShipmentID)
                    .Set(o => o.EntityID = resource.ResourceID)
                    .Save();

                resourcesForPurging.Add((ConsumerID: objRef.ConsumerID, LabelFormat: labelFormat.Value, EntityID: objRef.EntityID, ResourceID: resource.ResourceID));
            }

            // Create labels to keep (not purge)
            foreach (var labelFormat in EnumHelper.GetEnumList<ThermalLanguage>())
            {
                ShipmentEntity shipment = Create.Shipment(context.Order)
                    .AsFedEx(x => x.WithPackage())
                    .Set(a => a.ProcessedDate = processedDateToKeep)
                    .Set(s => s.Processed = true)
                    .Set(s => s.ActualLabelFormat = (int) labelFormat.Value)
                    .Save();

                ResourceEntity resource = Create.Entity<ResourceEntity>()
                    .Set(r => r.Filename = Guid.NewGuid().ToString())
                    .Set(r => r.Checksum = CalculateChecksum(Guid.NewGuid().ToString()))
                    .Save();

                ObjectReferenceEntity objRef = Create.Entity<ObjectReferenceEntity>()
                    .Set(o => o.ConsumerID = shipment.ShipmentID)
                    .Set(o => o.EntityID = resource.ResourceID)
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
            tableDataList.Add(new PurgeTableData(nameof(ShipmentEntity), FetchDataTable(queryFactory.Shipment, ShipmentFields.ShipmentID > 1), null));
        }

        private void UpdateTableDataAfterPurge()
        {
            tableDataList.First(t => t.TableName == nameof(ResourceEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.Resource, ResourceFields.ResourceID > 0));
            tableDataList.First(t => t.TableName == nameof(ObjectReferenceEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.ObjectReference, ObjectReferenceFields.ObjectReferenceID > 0));
            tableDataList.First(t => t.TableName == nameof(ShipmentEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.Shipment, ShipmentFields.ShipmentID > 1));
        }

        private DataTable FetchDataTable<T>(EntityQuery<T> entityQuery, IPredicate predicate) where T : IEntityCore
        {
            DataTable dataTable;

            using (ISqlAdapter sqlAdapter = context.Mock.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                var countOrderQuery = entityQuery
                    .Where(predicate).Select(Projection.Full);

                return sqlAdapter.FetchAsDataTable(countOrderQuery);
            }
        }
    }
}
