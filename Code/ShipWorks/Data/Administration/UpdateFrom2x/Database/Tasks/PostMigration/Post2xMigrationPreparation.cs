using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Users.Security;
using ShipWorks.Users;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Stores.Platforms.Ebay;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using System.Transactions;
using Interapptive.Shared;
using ShipWorks.Users.Audit;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration
{
    /// <summary>
    /// Utility class to do some initial Post2x migration steps right after the v3 schema is updated - but before the UI steps\pages are shown.
    /// </summary>
    public static class Post2xMigrationPreparation
    {
        /// <summary>
        /// Prepare for the final 2x migration wizard pages to be shown right after the v3 schema is updated - but before the UI steps\pages are shown.
        /// </summary>
        public static void PrepareForFinalStepsAfter3xSchemaUpdate(ProgressProvider progressProvider)
        {
            // Create the progress item
            ProgressItem progress = new ProgressItem("Final Steps");
            progressProvider.ProgressItems.Add(progress);

            // Starting
            progress.Starting();

            // Do the steps
            ConfigureInitialData(progress);
            ValidateModuleUrls(progress);
            UpdateEbayEffectiveFields(progress);
            UpdateExpress1Data(progress);
            UpdateMivaItemAttributes(progress);

            if (progressProvider.CancelRequested)
            {
                throw new OperationCanceledException();
            }

            // Mark this phase as complete
            Post2xMigrationUtility.MarkStepComplete(Post2xMigrationStep.PostMigrationPreparation);

            // Done
            progress.Detail = "Done";
            progress.PercentComplete = 100;
            progress.Completed();
        }


        /// <summary>
        /// Configure all existing 2x users with 3x settings, create the SuperUser, and register the computer
        /// </summary>
        private static void ConfigureInitialData(ProgressItem progress)
        {
            if (Post2xMigrationUtility.IsStepComplete(Post2xMigrationStep.ConfigureInitialData))
            {
                return;
            }

            progress.Detail = "Configuring initial data...";

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Create all core required data
                InitialDataLoader.CreateCoreRequiredData();

                UserCollection users = UserCollection.Fetch(adapter, UserFields.UserID != SuperUser.UserID);

                // Configure existing users
                foreach (UserEntity user in users)
                {
                    UserUtility.ConfigureNewUser(adapter, user);
                }

                // Create the SuperUser
                SuperUser.Create(adapter);

                // Get the computer registered
                ComputerManager.RegisterThisComputer();

                // Not sure where else to put it, so ill put this here.  In v2 the UPS "AccessKey" was not encrypted, but in V3 it is.
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    using (SqlCommand cmd = SqlCommandProvider.Create(con, "SELECT TOP(1) UpsAccessKey FROM v2m_UpsAccessKey"))
                    {
                        string accessKey = (string) cmd.ExecuteScalar();

                        if (!string.IsNullOrWhiteSpace(accessKey))
                        {
                            // Needed to avoid DTC below
                            con.Close();

                            ShippingSettings.InitializeForCurrentDatabase();
                            ShippingSettingsEntity settings = ShippingSettings.Fetch();
                            settings.UpsAccessKey = SecureText.Encrypt(accessKey, "UPS");
                            ShippingSettings.Save(settings);
                        }
                    }
                }
                
                // This step is now complete
                Post2xMigrationUtility.MarkStepComplete(Post2xMigrationStep.ConfigureInitialData);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Update all the EffectiveXXX fields for eBay
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void UpdateEbayEffectiveFields(ProgressItem progress)
        {
            if (Post2xMigrationUtility.IsStepComplete(Post2xMigrationStep.UpdateEbayEffectiveFields))
            {
                return;
            }

            progress.Detail = "Upgrading eBay items...";

            // configure the calculations
            EbayOrderItemEntity.SetEffectiveCheckoutStatusAlgorithm(e => (int) EbayUtility.GetEffectivePaymentStatus(e));
            EbayOrderItemEntity.SetEffectivePaymentMethodAlgorithm(e => (int) EbayUtility.GetEffectivePaymentMethod(e));

            int expectedCount = 0;
            int completedCount = 0;

            // Determine how many we think there will be, for progress purposes
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con, "SELECT COUNT(OrderItemID) FROM dbo.EbayOrderItem WHERE EffectiveCheckoutStatus = -1"))
                {
                    expectedCount = (int) cmd.ExecuteScalar();
                }
            }

            try
            {
                OrderItemEntity.Is2xUpgraderUpdatingEffectiveEbayFields = true;

                // Put the SuperUser in scope, and don't audit
                using (AuditBehaviorScope scope = new AuditBehaviorScope(AuditBehaviorUser.SuperUser, new AuditReason(AuditReasonType.Default), AuditState.Disabled))
                {
                    while (true)
                    {
                        // touch every eBayOrderItem so their Effective* fields get recalculated
                        using (SqlAdapter adapter = new SqlAdapter())
                        {
                            var itemsToProcess = new EbayOrderItemCollection();
                            adapter.FetchEntityCollection(itemsToProcess, new RelationPredicateBucket(EbayOrderItemFields.EffectiveCheckoutStatus == -1), 1000);

                            if (itemsToProcess.Count == 0)
                            {
                                break;
                            }

                            foreach (var eBayItem in itemsToProcess)
                            {
                                // just setting this will force an update
                                eBayItem.EffectiveCheckoutStatus = 0;

                                // save
                                adapter.SaveEntity(eBayItem);

                                completedCount++;
                                progress.PercentComplete = (completedCount * 100) / expectedCount;
                                progress.Detail = string.Format("Upgrading eBay items ({0:#,##0} of {1:#,##0})...", completedCount, expectedCount);

                                if (progress.IsCancelRequested)
                                {
                                    return;
                                }
                            };
                        }
                    }
                }
            }
            finally
            {
                OrderItemEntity.Is2xUpgraderUpdatingEffectiveEbayFields = false;
            }

            // This step is now complete
            Post2xMigrationUtility.MarkStepComplete(Post2xMigrationStep.UpdateEbayEffectiveFields);
        }

        /// <summary>
        /// Validate the URL's of modules.  In v2 we did not save the http or https necessarily as apart of the URL.  v3 assumes its there.
        /// </summary>
        private static void ValidateModuleUrls(ProgressItem progress)
        {
            if (Post2xMigrationUtility.IsStepComplete(Post2xMigrationStep.ValidateModuleUrls))
            {
                return;
            }

            progress.Detail = "Validating module endpoints...";

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                foreach (GenericModuleStoreEntity store in GenericModuleStoreCollection.Fetch(adapter, null))
                {
                    if (store.ModuleUrl.IndexOf(Uri.SchemeDelimiter) == -1)
                    {
                        store.ModuleUrl = "https://" + store.ModuleUrl;

                        adapter.SaveAndRefetch(store);
                    }
                }

                // This step is now complete
                Post2xMigrationUtility.MarkStepComplete(Post2xMigrationStep.ValidateModuleUrls);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Since Express1 only existed after the migrater's v3 schema was fixed to 3.0.0, we have to handle fields added after 3.0.0 differently.
        /// This will update EndiciaAccount.EndiciaReseller for Express1 accounts as flagged in v2m_Express1Account
        /// </summary>
        private static void UpdateExpress1Data(ProgressItem progress)
        {
            // if we've already done this, move on
            if (Post2xMigrationUtility.IsStepComplete(Post2xMigrationStep.UpdateExpress1Data))
            {
                return;
            }

            progress.Detail = "Initializing Express1 data...";

            // Not sure where else to put it, so ill put this here.  In v2 the UPS "AccessKey" was not encrypted, but in V3 it is.
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con, "UPDATE EndiciaAccount SET EndiciaReseller = 1 WHERE EndiciaAccountID in (SELECT EndiciaAccountID FROM v2m_Express1Account)"))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // this step is now complete
            Post2xMigrationUtility.MarkStepComplete(Post2xMigrationStep.UpdateExpress1Data);
        }

        /// <summary>
        /// Updates data in MivaItemAttribute with data that is in v2m_MivaItemAttribute
        /// </summary>
        /// <param name="progress"></param>
        private static void UpdateMivaItemAttributes(ProgressItem progress)
        {
            if (Post2xMigrationUtility.IsStepComplete(Post2xMigrationStep.MivaItemAttriteData))
            {
                return;
            }

            int batchSize = 1000;

            // progress reporting
            int completedCount = 0;
            int expectedCount = 0;

            progress.Detail = "Updating Miva Item Attributes...";

            // Determine how many we think there will be, for progress purposes
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con, "SELECT COUNT(*) FROM dbo.MivaOrderItemAttribute"))
                {
                    expectedCount = (int) cmd.ExecuteScalar();
                }
            }

            // Not sure where else to put it, so ill put this here.  In v2 the UPS "AccessKey" was not encrypted, but in V3 it is.
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                while (true)
                {
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {

                        using (SqlCommand cmd = SqlCommandProvider.Create(con))
                        {
                            cmd.Transaction = transaction;

                            cmd.CommandText = String.Format(@"UPDATE top ({0}) m
                                            	SET m.MivaOptionCode = a.MivaOptionCode, m.MivaAttributeID = a.MivaAttributeID, m.MivaAttributeCode = a.MivaAttributeCode
                                            	FROM MivaOrderItemAttribute m inner join v2m_MivaItemAttribute a
                                            	ON m.OrderItemAttributeID = a.OrderItemAttributeID", batchSize);

                            int impacted = cmd.ExecuteNonQuery();
                            completedCount += impacted;

                            if (impacted == 0)
                            {
                                // no updates, we're out of rows - Done
                                break;
                            }

                            // Delete the rows we updated
                            cmd.CommandText = String.Format(@"DELETE TOP ({0}) a
                                            	FROM MivaOrderItemAttribute m inner join v2m_MivaItemAttribute a
                                            	ON m.OrderItemAttributeID = a.OrderItemAttributeID", batchSize);
                            cmd.ExecuteNonQuery();
                            
                            // commmit the updates and deletes
                            transaction.Commit();

                            // update progress
                            progress.PercentComplete = (completedCount * 100) / expectedCount;
                            progress.Detail = string.Format("Upgrading Miva Item Attributes ({0:#,##0} of {1:#,##0})...", completedCount, expectedCount);
                        }
                    }
                }
            }
            
            Post2xMigrationUtility.MarkStepComplete(Post2xMigrationStep.MivaItemAttriteData);
        }
    }
}
