using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebex.Mail;
using Interapptive.Shared.Utility;
using System.IO;
using HtmlAgilityPack;
using ShipWorks.Templates.Processing;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.UI.Controls.Html;

namespace ShipWorks.Email
{
    /// <summary>
    /// Utility class for working with outgoing emails in ShipWorks
    /// </summary>
    public static class EmailOutbox
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(EmailOutbox));

        /// <summary>
        /// Add the given email message to the outbox to be sent during the next send.
        /// </summary>
        public static EmailOutboundEntity AddMessage(EmailMessageHeader header, string htmlContent, string plainContent)
        {
            return AddMessage(header, htmlContent, plainContent, null);
        }

        /// <summary>
        /// Add the given email message to the outbox to be sent at a later time.  If delayUntilDate is null, the message is sent
        /// the next time the outbox is processed.  If not null, the message won't be sent until the date passes.
        /// </summary>
        public static EmailOutboundEntity AddMessage(EmailMessageHeader header, string htmlContent, string plainContent, DateTime? delayUntilDate)
        {
            EmailOutboundEntity emailOutbound = header.CreateEmailOutbound();
            emailOutbound.DontSendBefore = delayUntilDate;

            // Both can't be empty
            if (htmlContent == null && plainContent == null)
            {
                throw new ArgumentException("Both htmlContent and plainContent cannot be null.");
            }

            // Save all parts of the messages in a transaction due to resources
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Go ahead and save the message now without to get the ID to use for resources
                adapter.SaveAndRefetch(emailOutbound);

                UpdateMessage(emailOutbound, htmlContent, plainContent, adapter);

                adapter.Commit();

                return emailOutbound;
            }
        }

        /// <summary>
        /// Update the message with the new body content
        /// </summary>
        public static void UpdateMessage(EmailOutboundEntity emailOutbound, string htmlContent, string plainContent, SqlAdapter adapter)
        {
            // Remove any existing plain part
            if (emailOutbound.PlainPartResourceID > 0)
            {
                DataResourceManager.ReleaseResourceReference(emailOutbound.PlainPartResourceID);
            }

            // Remove existing html part
            if (emailOutbound.HtmlPartResourceID.HasValue)
            {
                DataResourceManager.ReleaseResourceReference(emailOutbound.HtmlPartResourceID.Value);
            }

            // If there is no plain content, generate it from the html content
            if (plainContent == null)
            {
                plainContent = HtmlUtility.GetPlainText(htmlContent);
            }

            // Create the plain part resource
            emailOutbound.PlainPartResourceID = DataResourceManager.CreateFromText(plainContent, emailOutbound.EmailOutboundID).ReferenceID;

            if (htmlContent != null)
            {
                ApplyHtmlPart(emailOutbound, htmlContent);
            }

            // Resave now with the resource ID's present
            adapter.SaveAndRefetch(emailOutbound);
        }

        /// <summary>
        /// Apply the html part of the message, fixing up local images
        /// </summary>
        private static void ApplyHtmlPart(EmailOutboundEntity emailOutbound, string htmlContent)
        {
            TemplateHtmlImageProcessor imageProcessor = new TemplateHtmlImageProcessor();
            imageProcessor.LocalImages = true;
            imageProcessor.OnlineImages = false;

            // Process all the images in the document
            htmlContent = imageProcessor.Process(htmlContent, (HtmlAttribute attribute, Uri srcUri, string imageName) =>
            {
                try
                {
                    DataResourceReference resourceItem = DataResourceManager.CreateFromFile(srcUri.LocalPath, emailOutbound.EmailOutboundID);

                    // Update the attribute with the new filename
                    attribute.Value = resourceItem.Filename;

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
            });

            emailOutbound.HtmlPartResourceID = DataResourceManager.CreateFromText(htmlContent, emailOutbound.EmailOutboundID).ReferenceID;
        }
    }
}
