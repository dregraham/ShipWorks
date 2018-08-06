using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Shared.Database.Tasks
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class PurgeDatabaseTask_Audits_Test
    {
        private const int RetentionPeriodInDays = 180;
        private readonly DataContext context;
        private int transactionID = 1000;
        private readonly DateTime cutoffDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(RetentionPeriodInDays));
        private readonly PurgeDatabaseTask testObject;

        public PurgeDatabaseTask_Audits_Test(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
            });

            testObject = context.Mock.Create<PurgeDatabaseTask>();

            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.RetentionPeriodInDays = RetentionPeriodInDays;
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesAllAudits_WhenAllDownloadsAreOlderThan180Days()
        {
            var audit1 = CreateAudit(cutoffDate.AddDays(-1));
            var audit2 = CreateAudit(cutoffDate.AddDays(-2));
            var audit3 = CreateAudit(cutoffDate.AddDays(-3));
            var audit4 = CreateAudit(cutoffDate.AddDays(-25));

            testObject.Run(new List<long>(), null);

            AssertAuditDoesNotExists(audit1);
            AssertAuditDoesNotExists(audit2);
            AssertAuditDoesNotExists(audit3);
            AssertAuditDoesNotExists(audit4);
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesTwoAudits_WhenHalfDownloadsAreOlderThan180Days()
        {
            var audit1 = CreateAudit(cutoffDate.AddDays(-1));
            var audit2 = CreateAudit(cutoffDate.AddDays(-2));
            var audit3 = CreateAudit(DateTime.UtcNow);
            var audit4 = CreateAudit(cutoffDate.AddDays(25));

            testObject.Run(new List<long>(), null);

            AssertAuditDoesNotExists(audit1);
            AssertAuditDoesNotExists(audit2);
            AssertAuditExists(audit3);
            AssertAuditExists(audit4);
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesNoAudits_WhenNoDownloadsAreOlderThan180Days()
        {
            var audit1 = CreateAudit(cutoffDate.AddDays(1));
            var audit2 = CreateAudit(cutoffDate.AddDays(2));
            var audit3 = CreateAudit(cutoffDate.AddDays(3));
            var audit4 = CreateAudit(cutoffDate.AddDays(25));

            testObject.Run(new List<long>(), null);

            AssertAuditExists(audit1);
            AssertAuditExists(audit2);
            AssertAuditExists(audit3);
            AssertAuditExists(audit4);
        }

        private static void AssertAuditExists(AuditEntity audit)
        {
            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var factory = new QueryFactory();

                var auditQuery = factory.Audit.Where(AuditFields.AuditID == audit.AuditID);
                var entity = sqlAdapter.FetchFirst(auditQuery);
                Assert.NotNull(entity);
            }
        }

        private static void AssertAuditDoesNotExists(AuditEntity audit)
        {
            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var factory = new QueryFactory();

                var auditQuery = factory.Audit.Where(AuditFields.AuditID == audit.AuditID);
                var entity = sqlAdapter.FetchFirst(auditQuery);
                Assert.Null(entity);
            }
        }

        private AuditEntity CreateAudit(DateTime dateTime)
        {
            var audit = Create.Entity<AuditEntity>()
                .Set(x => x.TransactionID, transactionID++)
                .Set(x => x.ComputerID, context.Computer.ComputerID)
                .Set(x => x.UserID, context.User.UserID)
                .Set(x => x.Date, dateTime)
                .Save();

            var auditChange = Create.Entity<AuditChangeEntity>()
                .Set(x => x.AuditID, audit.AuditID)
                .Save();

            Create.Entity<AuditChangeDetailEntity>()
                .Set(x => x.AuditID, audit.AuditID)
                .Set(x => x.AuditChangeID, auditChange.AuditChangeID)
                .Save();

            return audit;
        }
    }
}