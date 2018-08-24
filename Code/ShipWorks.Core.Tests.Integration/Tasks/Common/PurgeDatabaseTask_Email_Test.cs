using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Autofac;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Tasks.Common
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class PurgeDatabaseTask_Email_Test
    {
        private const int RetentionPeriodInDays = 180;
        private readonly DataContext context;
        private readonly DateTime cutoffDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(RetentionPeriodInDays));
        private readonly PurgeDatabaseTask testObject;

        public PurgeDatabaseTask_Email_Test(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
            });

            testObject = context.Mock.Create<PurgeDatabaseTask>();
            testObject.RetentionPeriodInDays = RetentionPeriodInDays;
            testObject.Purges.Add(PurgeDatabaseType.Email);
        }

        [Fact]
        public void PurgeDatabaseTask_SoftPurgesTwoEmails_WhenHalfOfEmailsAreOld()
        {
            var email1 = CreateEmail(DateTime.UtcNow, plain: "email1_plain", html: "email1_html");
            var email2 = CreateEmail(cutoffDate.AddDays(1), plain: "email2_plain", html: "email2_html");
            var email3 = CreateEmail(cutoffDate.AddDays(-1), plain: "email3_plain", html: "email3_html");
            var email4 = CreateEmail(cutoffDate.AddDays(-1000), plain: "email4_plain", html: "email4_html");

            testObject.Run(new List<long>(), null);

            AssertEmailResources(email1, plain: "email1_plain", html: "email1_html");
            AssertEmailResources(email2, plain: "email2_plain", html: "email2_html");
            AssertEmailResources(email3, plain: "__purged_email_plain.swr", html: "__purged_email_html_swr");
            AssertEmailResources(email4, plain: "__purged_email_plain.swr", html: "__purged_email_html_swr");
        }

        [Fact]
        public void PurgeDatabaseTask_DoesNotSoftPurgeEmail_WhenNoEmailsAreOld()
        {
            var email1 = CreateEmail(DateTime.UtcNow, plain: "email1_plain", html: "email1_html");
            var email2 = CreateEmail(cutoffDate.AddDays(1), plain: "email2_plain", html: "email2_html");
            var email3 = CreateEmail(cutoffDate.AddDays(2), plain: "email3_plain", html: "email3_html");
            var email4 = CreateEmail(cutoffDate.AddDays(1000), plain: "email4_plain", html: "email4_html");

            testObject.Run(new List<long>(), null);

            AssertEmailResources(email1, plain: "email1_plain", html: "email1_html");
            AssertEmailResources(email2, plain: "email2_plain", html: "email2_html");
            AssertEmailResources(email3, plain: "email3_plain", html: "email3_html");
            AssertEmailResources(email4, plain: "email4_plain", html: "email4_html");
        }

        [Fact]
        public void PurgeDatabaseTask_SoftPurgesAllEmails_WhenAllEmailsAreSentAndOld()
        {
            var email1 = CreateEmail(cutoffDate.AddDays(-1), plain: "email1_plain", html: "email1_html");
            var email2 = CreateEmail(cutoffDate.AddDays(-2), plain: "email2_plain", html: "email2_html");
            var email3 = CreateEmail(cutoffDate.AddDays(-3), plain: "email3_plain", html: "email3_html");
            var email4 = CreateEmail(cutoffDate.AddDays(-1000), plain: "email4_plain", html: "email4_html");

            testObject.Run(new List<long>(), null);

            AssertEmailResources(email1, plain: "__purged_email_plain.swr", html: "__purged_email_html_swr");
            AssertEmailResources(email2, plain: "__purged_email_plain.swr", html: "__purged_email_html_swr");
            AssertEmailResources(email3, plain: "__purged_email_plain.swr", html: "__purged_email_html_swr");
            AssertEmailResources(email4, plain: "__purged_email_plain.swr", html: "__purged_email_html_swr");
        }

        [Fact]
        public void PurgeDatabaseTask_DoesNotPurgeEmail_WhenOldEmailIsNotSent()
        {
            var email1 = CreateEmail(cutoffDate.AddDays(-1), plain: "email1_plain", html: "email1_html", status: EmailOutboundStatus.Ready);

            testObject.Run(new List<long>(), null);

            AssertEmailResources(email1, plain: "email1_plain", html: "email1_html");
        }

        [Fact]
        public void PurgeDatabaseTask_DeletesTwoEmails_WhenHalfOfEmailsAreOld()
        {
            testObject.PurgeEmailHistory = true;
            var email1 = CreateEmail(DateTime.UtcNow, plain: "email1_plain", html: "email1_html");
            var email2 = CreateEmail(cutoffDate.AddDays(1), plain: "email2_plain", html: "email2_html");
            var email3 = CreateEmail(cutoffDate.AddDays(-1), plain: "email3_plain", html: "email3_html");
            var email4 = CreateEmail(cutoffDate.AddDays(-1000), plain: "email4_plain", html: "email4_html");

            testObject.Run(new List<long>(), null);

            AssertEmailResources(email1, plain: "email1_plain", html: "email1_html");
            AssertEmailResources(email2, plain: "email2_plain", html: "email2_html");
            AssertEmailDeleted(email3);
            AssertEmailDeleted(email4);
        }

        [Fact]
        public void PurgeDatabaseTask_DoesNotDeleteEmail_WhenNoEmailsAreOld()
        {
            testObject.PurgeEmailHistory = true;
            var email1 = CreateEmail(DateTime.UtcNow, plain: "email1_plain", html: "email1_html");
            var email2 = CreateEmail(cutoffDate.AddDays(1), plain: "email2_plain", html: "email2_html");
            var email3 = CreateEmail(cutoffDate.AddDays(2), plain: "email3_plain", html: "email3_html");
            var email4 = CreateEmail(cutoffDate.AddDays(1000), plain: "email4_plain", html: "email4_html");

            testObject.Run(new List<long>(), null);

            AssertEmailResources(email1, plain: "email1_plain", html: "email1_html");
            AssertEmailResources(email2, plain: "email2_plain", html: "email2_html");
            AssertEmailResources(email3, plain: "email3_plain", html: "email3_html");
            AssertEmailResources(email4, plain: "email4_plain", html: "email4_html");
        }

        [Fact]
        public void PurgeDatabaseTask_DeletesAllEmails_WhenAllEmailsAreSentAndOld()
        {
            testObject.PurgeEmailHistory = true;
            var email1 = CreateEmail(cutoffDate.AddDays(-1), plain: "email1_plain", html: "email1_html");
            var email2 = CreateEmail(cutoffDate.AddDays(-2), plain: "email2_plain", html: "email2_html");
            var email3 = CreateEmail(cutoffDate.AddDays(-3), plain: "email3_plain", html: "email3_html");
            var email4 = CreateEmail(cutoffDate.AddDays(-1000), plain: "email4_plain", html: "email4_html");

            testObject.Run(new List<long>(), null);

            AssertEmailDeleted(email1);
            AssertEmailDeleted(email2);
            AssertEmailDeleted(email3);
            AssertEmailDeleted(email4);
        }

        [Fact]
        public void PurgeDatabaseTask_DoesNotDeleteEmail_WhenOldEmailIsNotSent()
        {
            testObject.PurgeEmailHistory = true;
            var email1 = CreateEmail(cutoffDate.AddDays(-1), plain: "email1_plain", html: "email1_html", status: EmailOutboundStatus.Ready);

            testObject.Run(new List<long>(), null);

            AssertEmailResources(email1, plain: "email1_plain", html: "email1_html");
        }

        private void AssertEmailDeleted(IEmailOutboundEntity email)
        {
            Assert.Null(GetEmailResource(email.PlainPartResourceID));
            Assert.Null(GetEmailResource(email.HtmlPartResourceID));

            Assert.Null(GetObjectReference(email.PlainPartResourceID));
            Assert.Null(GetObjectReference(email.HtmlPartResourceID));

            Assert.Null(GetObjectReferenceForConsumer(email.EmailOutboundID));
            Assert.Null(GetEmailOutbound(email));
        }

        private void AssertEmailResources(IEmailOutboundEntity email, string plain, string html)
        {
            AssertEmailResource(email, plain, email.PlainPartResourceID);
            AssertEmailResource(email, html, email.HtmlPartResourceID);
        }

        private static void AssertEmailResource(IEmailOutboundEntity email, string text, long? resourceID)
        {
            ResourceEntity resource = GetEmailResource(resourceID);

            Assert.Equal(text, resource.Filename);
            Assert.NotNull(GetEmailOutbound(email));
        }

        private static EmailOutboundEntity GetEmailOutbound(IEmailOutboundEntity email)
        {
            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var factory = new QueryFactory();

                var emailQuery = factory.EmailOutbound.Where(EmailOutboundFields.EmailOutboundID == email.EmailOutboundID);

                return sqlAdapter.FetchFirst(emailQuery);
            }
        }

        private static ObjectReferenceEntity GetObjectReference(long? referenceID)
        {
            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var factory = new QueryFactory();

                var reference = factory.ObjectReference
                    .Where(ObjectReferenceFields.ObjectReferenceID == referenceID);
                return sqlAdapter.FetchFirst(reference);
            }
        }

        private static ObjectReferenceEntity GetObjectReferenceForConsumer(long? consumerID)
        {
            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var factory = new QueryFactory();

                var reference = factory.ObjectReference
                    .Where(ObjectReferenceFields.ConsumerID == consumerID);
                return sqlAdapter.FetchFirst(reference);
            }
        }

        private static ResourceEntity GetEmailResource(long? resourceID)
        {
            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var factory = new QueryFactory();

                var reference = factory.ObjectReference
                    .Where(ObjectReferenceFields.ObjectReferenceID == resourceID)
                    .Select(ObjectReferenceFields.EntityID);
                var query = factory.Resource.Where(ResourceFields.ResourceID.In(reference));
                return sqlAdapter.FetchFirst(query);
            }
        }

        private EmailOutboundEntity CreateEmail(DateTime date, string plain, string html, EmailOutboundStatus status = EmailOutboundStatus.Sent)
        {
            var email = Create.Entity<EmailOutboundEntity>().Save();

            var plainReference = CreateResourceAndReference(plain, email);
            var htmlReference = CreateResourceAndReference(html, email);

            email = Modify.Entity<EmailOutboundEntity>(email)
                .Set(x => x.SentDate, date)
                .Set(x => x.SendStatus, (int) status)
                .Set(x => x.PlainPartResourceID, plainReference.ObjectReferenceID)
                .Set(x => x.HtmlPartResourceID, htmlReference.ObjectReferenceID)
                .Save();

            return email;
        }

        private static ObjectReferenceEntity CreateResourceAndReference(string fileName, EmailOutboundEntity email)
        {
            var resource = Create.Entity<ResourceEntity>()
                .Set(x => x.Filename, fileName)
                .Set(x => x.Checksum = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(fileName)))
                .Save();

            return Create.Entity<ObjectReferenceEntity>()
                .Set(x => x.EntityID, resource.ResourceID)
                .Set(x => x.ConsumerID, email.EmailOutboundID)
                .Set(x => x.ReferenceKey, fileName)
                .Save();
        }
    }
}
