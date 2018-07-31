﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Common;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Tasks.Common
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class PurgeDatabaseTaskTest
    {
        private readonly DataContext context;
        private const int RetentionPeriodInDays = 30;
        private readonly List<TableData> tableDataList = new List<TableData>();

        public PurgeDatabaseTaskTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
            });

            // Delete the default order and customer
            using (ISqlAdapter sqlAdapter = context.Mock.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                sqlAdapter.DeleteEntity(context.Order);
                sqlAdapter.DeleteEntity(context.Order.Customer);
            }

            BulkOrderCreator bulkOrderCreator = new BulkOrderCreator(context);

            bulkOrderCreator.StoreTypeCodes = EnumHelper.GetEnumList<StoreTypeCode>()
                .Select(e => e.Value)
                .Where(stc => stc != StoreTypeCode.Invalid)
                .Take(2); // TODO: Remove this Take later...just here for speediness of testing during dev.

            bulkOrderCreator.ShipmentTypeCodes = EnumHelper.GetEnumList<ShipmentTypeCode>()
                .Select(e => e.Value)
                .Where(stc => stc != ShipmentTypeCode.None)
                .Take(2); // TODO: Remove this Take later...just here for speediness of testing during dev.

            bulkOrderCreator.OrderDates = new List<DateTime>
                {
                    DateTime.Now.AddDays(-(RetentionPeriodInDays + 1)),
                    DateTime.Now.AddDays(-(RetentionPeriodInDays + 2)),
                    DateTime.Now.AddDays(-1)
                };

            bulkOrderCreator.CreateOrderForAllStores();

            LoadInitialTableData();
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesAllEmailContent_WhenPurgeHistoryTrue()
        {
            PurgeDatabaseTask testObject = new PurgeDatabaseTask(new SqlPurgeScriptRunner(), new DateTimeProvider());

            testObject.Purges.Add(PurgeDatabaseType.Email);

            testObject.PurgeEmailHistory = true;
            testObject.PurgePrintJobHistory = false;

            testObject.RetentionPeriodInDays = -1;
            testObject.Run(new List<long>(), null);

            UpdateTableDataAfterPurge();

            ValidateEmails(testObject);

            // Now validate the others didn't change.
            ValidatePrintResults(testObject);
            ValidateDownloads(testObject);
            ValidateAudits(testObject);
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesAllDownloads()
        {
            PurgeDatabaseTask testObject = new PurgeDatabaseTask(new SqlPurgeScriptRunner(), new DateTimeProvider());

            testObject.Purges.Add(PurgeDatabaseType.Downloads);

            testObject.PurgeEmailHistory = false;
            testObject.PurgePrintJobHistory = false;

            testObject.RetentionPeriodInDays = -1;
            testObject.Run(new List<long>(), null);

            UpdateTableDataAfterPurge();

            ValidateDownloads(testObject);

            // Now validate the others didn't change.
            ValidatePrintResults(testObject);
            ValidateEmails(testObject);
            ValidateAudits(testObject);
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesAllAudits()
        {
            PurgeDatabaseTask testObject = new PurgeDatabaseTask(new SqlPurgeScriptRunner(), new DateTimeProvider());

            testObject.Purges.Add(PurgeDatabaseType.Audit);

            testObject.PurgeEmailHistory = false;
            testObject.PurgePrintJobHistory = false;

            testObject.RetentionPeriodInDays = -1;
            testObject.Run(new List<long>(), null);

            UpdateTableDataAfterPurge();

            ValidateAudits(testObject);

            // Now validate the others didn't change.
            ValidatePrintResults(testObject);
            ValidateEmails(testObject);
            ValidateDownloads(testObject);
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesOrdersAllOrders()
        {
            // Get the total number of orders
            long startingNumberOfOrders = 0;
            var queryFactory = new QueryFactory();
            var countOrderQuery = queryFactory.Order.Select(OrderFields.OrderID.CountBig());
            using (ISqlAdapter sqlAdapter = context.Mock.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                startingNumberOfOrders = sqlAdapter.FetchScalar<long>(countOrderQuery);
            }
            Assert.NotEqual(0, startingNumberOfOrders);

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(new SqlPurgeScriptRunner(), new DateTimeProvider());

            testObject.Purges.Add(PurgeDatabaseType.Orders);

            testObject.PurgeEmailHistory = true;
            testObject.PurgePrintJobHistory = true;

            testObject.RetentionPeriodInDays = -1;
            testObject.Run(new List<long>(), null);

            UpdateTableDataAfterPurge();

            ValidateEmails(testObject);
            ValidatePrintResults(testObject);
            ValidateDownloads(testObject);
            
            // TODO: When auditing is enabled, the deletion of orders (and other entities) will be audited, making counts difficult to check.  Do we care?
            //ValidateAudits(testObject);

            long endingNumberOfOrders = 0;
            using (ISqlAdapter sqlAdapter = context.Mock.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                endingNumberOfOrders = sqlAdapter.FetchScalar<long>(countOrderQuery);
            }

            Assert.Equal(0, endingNumberOfOrders);
        }

        private void ValidateAudits(PurgeDatabaseTask testObject)
        {
            var tables = tableDataList.Where(t => t.TableName.StartsWith("Audit"));

            if (!testObject.Purges.Contains(PurgeDatabaseType.Audit))
            {
                tables.ForEach(p => Assert.Equal(p.InitialRowCount, p.AfterPurgeRowCount));
            }
            else
            {
                Assert.Equal(0, tables.Sum(t => t.AfterPurgeRowCount));
            }
        }

        private void ValidateDownloads(PurgeDatabaseTask testObject)
        {
            var tables = tableDataList.Where(t => t.TableName.StartsWith("Download"));

            if (!testObject.Purges.Contains(PurgeDatabaseType.Downloads))
            {
                tables.ForEach(p => Assert.Equal(p.InitialRowCount, p.AfterPurgeRowCount));
            }
            else
            {
                Assert.Equal(0, tables.Sum(t => t.AfterPurgeRowCount));
            }
        }

        private void ValidateEmails(PurgeDatabaseTask testObject)
        {
            var emailTables = tableDataList.Where(t => t.TableName.StartsWith("EmailOutbound"));
            var objRefTable = tableDataList.First(t => t.TableName.StartsWith("ObjectReference"));

            if (!testObject.Purges.Contains(PurgeDatabaseType.Email))
            {
                // TODO: Validate values too
                emailTables.ForEach(p =>
                {
                    Assert.Equal(p.InitialRowCount, p.AfterPurgeRowCount);
                    Assert.True(p.AreTablesEqual());
                });

                //Assert.True(objRefTable.AreTablesEqual());
            }
            else
            {
                if (testObject.PurgeEmailHistory)
                {
                    Assert.Equal(0, emailTables.Sum(t => t.AfterPurgeRowCount));
                    // TODO: Validate ObjectReferences/Resources
                }
                else
                {
                    Assert.True(emailTables.Sum(t => t.InitialRowCount - t.AfterPurgeRowCount) == 0);

                    // TODO: Validate values too
                }
            }
        }

        private void ValidatePrintResults(PurgeDatabaseTask testObject)
        {
            var printResultTables = tableDataList.Where(t => t.TableName.StartsWith("PrintResult"));

            if (!testObject.Purges.Contains(PurgeDatabaseType.PrintJobs) &&
                !testObject.Purges.Contains(PurgeDatabaseType.Orders))
            {
                printResultTables.ForEach(p => Assert.Equal(p.InitialRowCount, p.AfterPurgeRowCount));
            }
            else
            {
                if (testObject.PurgePrintJobHistory)
                {
                    Assert.Equal(0, printResultTables.Sum(t => t.AfterPurgeRowCount));
                }
                else
                {
                    Assert.True(printResultTables.Sum(t => t.InitialRowCount - t.AfterPurgeRowCount) == 0);
                }
            }
        }

        private void LoadInitialTableData()
        {
            var queryFactory = new QueryFactory();

            tableDataList.Add(new TableData(nameof(ResourceEntity), FetchDataTable(queryFactory.Resource, ResourceFields.ResourceID > 0), null));

            tableDataList.Add(new TableData(nameof(DownloadEntity), FetchDataTable(queryFactory.Download, DownloadFields.DownloadID > 1), null));

            tableDataList.Add(new TableData(nameof(DownloadDetailEntity), FetchDataTable(queryFactory.DownloadDetail, DownloadDetailFields.DownloadedDetailID > 0), null));

            tableDataList.Add(new TableData(nameof(EmailOutboundEntity), FetchDataTable(queryFactory.EmailOutbound, EmailOutboundFields.EmailOutboundID > 0), null));

            tableDataList.Add(new TableData(nameof(EmailOutboundRelationEntity), FetchDataTable(queryFactory.EmailOutboundRelation, EmailOutboundRelationFields.EmailOutboundRelationID > 0), null));

            tableDataList.Add(new TableData(nameof(ObjectReferenceEntity), FetchDataTable(queryFactory.ObjectReference, ObjectReferenceFields.ObjectReferenceID > 0), null));

            tableDataList.Add(new TableData(nameof(ObjectLabelEntity), FetchDataTable(queryFactory.ObjectLabel, ObjectLabelFields.EntityID > 0 & ObjectLabelFields.ObjectType != 5), null));

            tableDataList.Add(new TableData(nameof(PrintResultEntity), FetchDataTable(queryFactory.PrintResult, PrintResultFields.PrintResultID > 1), null));

            tableDataList.Add(new TableData(nameof(AuditChangeEntity), FetchDataTable(queryFactory.AuditChange, AuditChangeFields.AuditID > 1), null));

            tableDataList.Add(new TableData(nameof(OrderEntity), FetchDataTable(queryFactory.Order, OrderFields.OrderID > 1), null));

            tableDataList.Add(new TableData(nameof(ShipmentEntity), FetchDataTable(queryFactory.Shipment, ShipmentFields.ShipmentID > 1), null));
        }

        private void UpdateTableDataAfterPurge()
        {
            var queryFactory = new QueryFactory();

            tableDataList.First(t => t.TableName == nameof(ResourceEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.Resource, ResourceFields.ResourceID > 0));

            tableDataList.First(t => t.TableName == nameof(DownloadDetailEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.DownloadDetail, DownloadDetailFields.DownloadedDetailID > 0));

            tableDataList.First(t => t.TableName == nameof(EmailOutboundEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.EmailOutbound, EmailOutboundFields.EmailOutboundID > 0));

            tableDataList.First(t => t.TableName == nameof(ObjectReferenceEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.ObjectReference, ObjectReferenceFields.ObjectReferenceID > 0));

            tableDataList.First(t => t.TableName == nameof(EmailOutboundRelationEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.EmailOutboundRelation, EmailOutboundRelationFields.EmailOutboundRelationID > 0));

            tableDataList.First(t => t.TableName == nameof(ObjectLabelEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.ObjectLabel, ObjectLabelFields.EntityID > 0 & ObjectLabelFields.ObjectType != 5));

            tableDataList.First(t => t.TableName == nameof(DownloadEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.Download, DownloadFields.DownloadID > 1));

            tableDataList.First(t => t.TableName == nameof(PrintResultEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.PrintResult, PrintResultFields.PrintResultID > 1));

            tableDataList.First(t => t.TableName == nameof(AuditChangeEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.AuditChange, AuditChangeFields.AuditID > 1));

            tableDataList.First(t => t.TableName == nameof(OrderEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.Order, OrderFields.OrderID > 1));

            tableDataList.First(t => t.TableName == nameof(ShipmentEntity)).SetAfterPurgeTable(FetchDataTable(queryFactory.Shipment, ShipmentFields.ShipmentID > 1));
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

        /// <summary>
        /// Helper class for loading/updating/holding table info
        /// </summary>
        private class TableData
        {
            public string TableName;
            public long InitialRowCount = 0;
            public long AfterPurgeRowCount = 0;
            public DataTable InitialTable = null;
            public DataTable AfterPurgeTable = null;

            public TableData(string tableName, DataTable initialTable, DataTable afterPurgeTable)
            {
                TableName = tableName;
                InitialTable = initialTable;
                AfterPurgeTable = afterPurgeTable;
                InitialRowCount = InitialTable?.Rows.Count ?? 0;
                AfterPurgeRowCount = AfterPurgeTable?.Rows.Count ?? 0;
            }

            public void SetAfterPurgeTable(DataTable after)
            {
                if (after != null)
                {
                    AfterPurgeTable = after;
                    AfterPurgeRowCount = AfterPurgeTable.Rows.Count;
                }
            }

            public bool AreTablesEqual() => InitialTable.Rows.Cast<DataRow>()
                                                .Intersect(AfterPurgeTable.Rows.Cast<DataRow>(), DataRowComparer.Default).Count() == AfterPurgeRowCount;
        }
    }
}
