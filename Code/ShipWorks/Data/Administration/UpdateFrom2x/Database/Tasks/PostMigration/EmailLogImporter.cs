using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Common.Threading;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email;
using ShipWorks.Templates;
using ShipWorks.Email.Accounts;
using System.Data;
using Interapptive.Shared;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration
{
    /// <summary>
    /// Utility class for importing the 2x EmailLog into the 3x database
    /// </summary>
    public static class EmailLogImporter
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EmailLogImporter));

        static long fakeBodyReferenceID = -1;

        /// <summary>
        /// Migrate all the email history from ShipWorks 2x
        /// </summary>
        [NDependIgnoreLongMethod]
        public static void ImportEmailHistory(ProgressItem progress)
        {
            progress.Starting();

            int expectedCount;
            int completedCount = 0;

            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                expectedCount = (int) SqlCommandProvider.ExecuteScalar(con, "SELECT COUNT(EmailLogID) FROM v2m_EmailLog");

                while (true)
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT TOP(1000) * FROM v2m_EmailLog", con);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        break;
                    }

                    // Keep going until done
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (progress.IsCancelRequested)
                        {
                            throw new OperationCanceledException();
                        }

                        using (SqlAdapter adapter = new SqlAdapter(true))
                        {
                            int emailLogID = (int) row["EmailLogID"];
                            int oldOrderID = (int) row["OrderID"];
                            int oldCustomerID = (int) row["CustomerID"];
                            DateTime sentDate = (DateTime) row["SentDate"];
                            string from = (string) row["EmailFrom"];
                            string toList = (string) row["EmailTo"];
                            string ccList = (string) row["Cc"];
                            string bccList = (string) row["Bcc"];
                            string subject = (string) row["Subject"];
                            string oldTemplateName = (string) row["Template"];
                            int oldResult = (int) row["Result"];
                            string errorMessage = (string) row["ErrorMessage"];

                            if (oldResult == 0)
                            {
                                long? contextID = DetermineContextID(oldOrderID, oldCustomerID);
                                if (contextID != null)
                                {
                                    int contextType = EntityUtility.GetEntitySeed(contextID.Value);

                                    // Create the new email entry
                                    EmailOutboundEntity emailOutbound = new EmailOutboundEntity();
                                    emailOutbound.Visibility = (int) EmailOutboundVisibility.Visible;

                                    // Context/Related to
                                    emailOutbound.ContextID = contextID.Value;
                                    emailOutbound.ContextType = contextType;

                                    // TODO
                                    emailOutbound.TemplateID = GetTemplateID(oldTemplateName);
                                    emailOutbound.AccountID = GetEmailAccount(from);

                                    // Email header
                                    emailOutbound.FromAddress = from;
                                    emailOutbound.ToList = toList;
                                    emailOutbound.CcList = ccList;
                                    emailOutbound.BccList = bccList;
                                    emailOutbound.Subject = subject;

                                    // Body
                                    emailOutbound.HtmlPartResourceID = null;
                                    emailOutbound.PlainPartResourceID = GetFakeEmailBodyReferenceID();
                                    emailOutbound.Encoding = "utf-8";

                                    // Dates
                                    emailOutbound.ComposedDate = sentDate;
                                    emailOutbound.SentDate = sentDate;
                                    emailOutbound.DontSendBefore = null;

                                    // Status
                                    emailOutbound.SendStatus = (oldResult == 0) ? (int) EmailOutboundStatus.Sent : (int) EmailOutboundStatus.Retry;
                                    emailOutbound.SendAttemptCount = 1;
                                    emailOutbound.SendAttemptLastError = (oldResult == 0) ? "" : (oldResult == 1) ? "Canceled" : errorMessage;

                                    // Create the Relations which will link the order\customer to the email 
                                    emailOutbound.RelatedObjects.Add(new EmailOutboundRelationEntity { ObjectID = contextID.Value, RelationType = (int) EmailOutboundRelationType.ContextObject });
                                    emailOutbound.RelatedObjects.Add(new EmailOutboundRelationEntity { ObjectID = contextID.Value, RelationType = (int) EmailOutboundRelationType.RelatedObject });

                                    // Save
                                    adapter.SaveEntity(emailOutbound);
                                }
                                else
                                {
                                    log.WarnFormat("Skipping EmailLog import due to not found contextID ({0}, {1})", oldOrderID, oldCustomerID);
                                }
                            }
                            else
                            {
                                log.WarnFormat("Skipping email import due to non-success ({0}, {1})", oldOrderID, oldCustomerID);
                            }

                            // Delete the original entry to mark it as done
                            using (SqlConnection transCon = SqlSession.Current.OpenConnection())
                            {
                                SqlCommand deleteCmd = SqlCommandProvider.Create(transCon, "DELETE v2m_EmailLog WHERE EmailLogID = @emailLogID");
                                deleteCmd.Parameters.AddWithValue("@emailLogID", emailLogID);
                                deleteCmd.ExecuteNonQuery();
                            }

                            adapter.Commit();
                        }

                        completedCount++;
                        progress.PercentComplete = (completedCount * 100) / Math.Max(expectedCount, 1);
                        progress.Detail = string.Format("Importing {0:#,##0} of {1:#,##0}...", completedCount, expectedCount);
                    }
                }
            }

            progress.Completed();
            progress.PercentComplete = 100;
            progress.Detail = "Done";
        }

        /// <summary>
        /// Try to find an email account that corresponds to the given "From" address
        /// </summary>
        private static long GetEmailAccount(string from)
        {
            EmailAccountEntity account = EmailAccountManager.EmailAccounts.FirstOrDefault(a => from.Contains(a.DisplayName) || from.Contains(a.EmailAddress));
            if (account != null)
            {
                return account.EmailAccountID;
            }

            return -1;
        }

        /// <summary>
        /// Get the TemplateID in 3x for the given 2x template name
        /// </summary>
        private static long? GetTemplateID(string oldTemplateName)
        {
            TemplateEntity template = TemplateManager.Tree.FindTemplate(string.Format(@"{0}\{1}", ImportTemplatesWizardPage.ImportRootFolderName, oldTemplateName));
            if (template != null)
            {
                return template.TemplateID;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the ResourceID of the fake email content to display for imported v2 email messages since we don't import their actual content.
        /// </summary>
        private static long GetFakeEmailBodyReferenceID()
        {
            if (fakeBodyReferenceID < 0)
            {
                fakeBodyReferenceID = DataResourceManager.CreateFromText("This message was imported from ShipWorks 2.", 0).ReferenceID;
            }

            return fakeBodyReferenceID;
        }

        /// <summary>
        /// Determine the ContextID to use based on the 2x order and customer ids
        /// </summary>
        private static long? DetermineContextID(int oldOrderID, int oldCustomerID)
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                try
                {
                    if (oldOrderID > 0)
                    {
                        return MigrationRowKeyTranslator.TranslateKeyToV3(oldOrderID, MigrationRowKeyType.Order, con);
                    }
                    else
                    {
                        return MigrationRowKeyTranslator.TranslateKeyToV3(oldCustomerID, MigrationRowKeyType.Customer, con);
                    }
                }
                catch (NotFoundException)
                {
                    return null;
                }
            }
        }
    }
}
