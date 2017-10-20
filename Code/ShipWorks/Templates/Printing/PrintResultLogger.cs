using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using HtmlAgilityPack;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Templates.Processing;
using ShipWorks.Users;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Used to log the result of printing
    /// </summary>
    public static class PrintResultLogger
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PrintResultLogger));

        /// <summary>
        /// Log the results of the givne print job that were printed with the specified settings
        /// </summary>
        public static void LogPrintJob(Guid jobIdentifier, List<TemplateResult> resultsUsed, PrintJobSettings settings, ProgressProvider progressProvider)
        {
            // Add a progress item for the logging
            ProgressItem logProgress = new ProgressItem("Logging");
            progressProvider.ProgressItems.Add(logProgress);

            logProgress.CanCancel = false;
            logProgress.Starting();
            logProgress.Detail = "Saving print history...";

            int count = 0;

            // If there is more than one, we have to break it down into each individual result to be logged
            foreach (TemplateResult result in resultsUsed)
            {
                LogPrintResult(result, settings, jobIdentifier);

                logProgress.PercentComplete = (++count * 100) / resultsUsed.Count;
            }

            logProgress.Completed();
        }

        /// <summary>
        /// Log the result of the print
        /// </summary>
        private static void LogPrintResult(TemplateResult templateResult, PrintJobSettings settings, Guid jobIdentifier)
        {
            // Determine the input that generated this result
            TemplateInput input = templateResult.XPathSource.Context.Input;

            // Log each of the original input keys.  Will only be multiple for reports
            foreach (long key in input.ContextKeys)
            {
                long contextEntityID = key;
                long relatedEntityID = key;

                EntityType type = EntityUtility.GetEntityType(key);

                // If the template had Customer context, then log it to the customer.  Otherwise, everything get's logged to the order,
                // regardless of what was originally selected.
                if (type != EntityType.CustomerEntity)
                {
                    List<long> relatedKeys = DataProvider.GetRelatedKeys(contextEntityID, EntityType.OrderEntity);

                    // Should not happen often, if ever.  If the list is empty, that means that something was deleted in the meantime.  We'll just use the original.
                    if (relatedKeys.Count != 0)
                    {
                        Debug.Assert(relatedKeys.Count == 1);

                        relatedEntityID = relatedKeys[0];
                    }
                }

                LogPrintResultContent(templateResult, settings, relatedEntityID, contextEntityID, jobIdentifier);
            }
        }

        /// <summary>
        /// Log the result of the print
        /// </summary>
        private static void LogPrintResultContent(TemplateResult templateResult, PrintJobSettings settings, long relatedEntityID, long contextEntityID, Guid jobIdentifier)
        {
            TemplateEntity template = templateResult.XPathSource.Template;
            PrintResultEntity result = null;

            using (SqlAdapter adapter = new SqlAdapter(false))
            {
                result = CreatePrintResultEntry(settings, relatedEntityID, contextEntityID, jobIdentifier, template);

                // Save it now, b\c we need the ID to use for the resource manager
                adapter.SaveAndRefetch(result);

                try
                {
                    string printContent = templateResult.ReadResult();

                    // If its html content, then process all the images
                    if (template.OutputFormat == (int) TemplateOutputFormat.Html)
                    {
                        TemplateHtmlImageProcessor imageProcessor = new TemplateHtmlImageProcessor
                        {
                            LocalImages = true,
                            OnlineImages = false
                        };

                        // Process all the images in the document
                        printContent = imageProcessor.Process(printContent, (attribute, srcUri, imageName) =>
                            {
                                ImageProcessorImageHandler(srcUri, result, attribute, imageName);
                            });
                    }

                    // Save the print content as a resource as well
                    DataResourceReference contentResource = DataResourceManager.CreateFromText(printContent, result.PrintResultID);
                    result.ContentResourceID = contentResource.ReferenceID;

                    // Save the result while in a transaction
                    using (new LoggedStopwatch(log, "PrintResultLogger.LogPrintResultContent - committed: "))
                    {
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
                        {
                            adapter.SaveEntity(result);

                            scope.Complete();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);

                    // Delete the result we created.  If other resources were created, they should be cleaned up
                    // by the deleted abandoned resources task.
                    SqlAdapter.Default.DeleteEntity(result);

                    throw;
                }
            }

        }

        /// <summary>
        /// Handles any image needs for the html
        /// </summary>
        static void ImageProcessorImageHandler(Uri srcUri, PrintResultEntity result, HtmlAttribute attribute, string imageName)
        {
            try
            {
                DataResourceReference resource = DataResourceManager.CreateFromFile(srcUri.LocalPath, result.PrintResultID);

                // Update the attribute with the new filename
                attribute.Value = resource.Filename;

                HtmlNode img = attribute.OwnerNode;

                // Add the shipworks special attributes for template resources
                img.Attributes.Append("importedFrom", imageName);
            }
            catch (Exception ex)
            {
                // We are here b\c a copy operation failed.  Missing one file does
                // not fail the whole process.  Lots could go wrong, so for now I am having
                // it just catch the general Exception case.
                log.Error(string.Format("Error localizing URI '{0}'.", srcUri), ex);
            }
        }

        /// <summary>
        /// Create a PrintResultEntity for the print job
        /// </summary>
        public static PrintResultEntity CreatePrintResultEntry(PrintJobSettings settings, long relatedEntityID, long contextEntityID, Guid jobIdentifier, TemplateEntity template)
        {
            PrintResultEntity result = new PrintResultEntity
            {
                // Placeholder until we get the real ID
                ContentResourceID = -1,
                JobIdentifier = jobIdentifier,
                ComputerID = UserSession.Computer.ComputerID,
                PrintDate = DateTime.UtcNow,
                RelatedObjectID = relatedEntityID,
                ContextObjectID = contextEntityID,
                TemplateID = template.TemplateID,
                TemplateType = template.Type,
                OutputFormat = template.OutputFormat
            };

            // If its a label sheet, store the label sheet id used
            if (settings.LabelSettings != null)
            {
                result.LabelSheetID = settings.LabelSettings.LabelSheetID;
            }

            result.PrinterName = settings.PrinterName;
            result.PaperSource = settings.PaperSource;
            result.PaperSourceName = settings.PaperSourceName;

            result.Copies = settings.Copies;
            result.Collated = settings.Collate;

            if (settings.PageSettings != null)
            {
                result.PageWidth = settings.PageSettings.PageWidth;
                result.PageHeight = settings.PageSettings.PageHeight;
                result.PageMarginLeft = settings.PageSettings.MarginLeft;
                result.PageMarginRight = settings.PageSettings.MarginRight;
                result.PageMarginTop = settings.PageSettings.MarginTop;
                result.PageMarginBottom = settings.PageSettings.MarginBottom;
            }
            else
            {
                result.PageWidth = 0;
                result.PageHeight = 0;
                result.PageMarginLeft = 0;
                result.PageMarginRight = 0;
                result.PageMarginTop = 0;
                result.PageMarginBottom = 0;
            }

            return result;
        }

        /// <summary>
        /// Delete all print results related to the given entity
        /// </summary>
        public static void DeleteForDeletedEntity(long entityID, ISqlAdapter adapter)
        {
            // Delete all the notes directly related to this entity
            adapter.DeleteEntitiesDirectly(typeof(PrintResultEntity), new RelationPredicateBucket(PrintResultFields.RelatedObjectID == entityID));

            // For customer's, we also have to delete all the order's print results
            EntityType entityType = EntityUtility.GetEntityType(entityID);
            if (entityType == EntityType.CustomerEntity)
            {
                RelationPredicateBucket customersOrders = new RelationPredicateBucket(
                    new FieldCompareSetPredicate(PrintResultFields.RelatedObjectID, null, OrderFields.OrderID, null, SetOperator.In, OrderFields.CustomerID == entityID, false));

                adapter.DeleteEntitiesDirectly(typeof(PrintResultEntity), customersOrders);
            }
        }

        /// <summary>
        /// Delete all print results that should be deleted when the given store is deleted
        /// </summary>
        public static void DeleteForDeletedStore(RelationPredicateBucket customerBucket, SqlAdapter adapter)
        {
            // Delete all print results for the customers
            adapter.DeleteEntitiesDirectly(typeof(PrintResultEntity),
                new RelationPredicateBucket(
                    new FieldCompareSetPredicate(PrintResultFields.RelatedObjectID, null, CustomerFields.CustomerID,
                        null, SetOperator.In, customerBucket.PredicateExpression, false)));

            // Delete all print results for the orders
            adapter.DeleteEntitiesDirectly(typeof(PrintResultEntity),
                new RelationPredicateBucket(
                    new FieldCompareSetPredicate(PrintResultFields.RelatedObjectID, null, OrderFields.OrderID,
                        null, SetOperator.In, customerBucket.PredicateExpression, new RelationCollection(CustomerEntity.Relations.OrderEntityUsingCustomerID), false)));
        }
    }
}
