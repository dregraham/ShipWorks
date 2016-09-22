using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email;
using ShipWorks.Email.Accounts;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Tokens;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Templates.Emailing
{
    /// <summary>
    /// An EmailMessageHeader specific to templates.
    /// </summary>
    public class EmailTemplateMessageHeader : EmailMessageHeader
    {
        TemplateEntity template;
        IList<TemplateResult> results;

        // The StoreID to use to determine which settings to use
        long storeID;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailTemplateMessageHeader(TemplateEntity template, IList<TemplateResult> results, long storeID)
        {
            this.template = template;
            this.results = results;

            this.storeID = storeID;

            PopulateHeader();
        }

        /// <summary>
        /// The template used to produce the message content and header
        /// </summary>
        public TemplateEntity Template
        {
            get { return template; }
        }

        /// <summary>
        /// The results of template processing used to generate the message content
        /// </summary>
        public IList<TemplateResult> TemplateResults
        {
            get { return results; }
        }

        /// <summary>
        /// Add our specific information to the outbound email information
        /// </summary>
        public override EmailOutboundEntity CreateEmailOutbound()
        {
            EmailOutboundEntity emailOutbound = base.CreateEmailOutbound();

            List<long> contextKeys = results.SelectMany(r => r.XPathSource.Input.ContextKeys).Distinct().ToList();
            List<long> relatedKeys = new List<long>();

            // Log the context keys exactly as they are
            foreach (long contextKey in contextKeys)
            {
                EmailOutboundRelationEntity relation = new EmailOutboundRelationEntity();
                relation.EntityID = contextKey;
                relation.RelationType = (int) EmailOutboundRelationType.ContextObject;

                emailOutbound.RelatedObjects.Add(relation);

                EntityType contextKeyType = EntityUtility.GetEntityType(contextKey);

                // If the template had Customer context, then log it to the customer.  Otherwise, everything get's logged to the order,
                // regardless of what was originally selected.
                if (contextKeyType != EntityType.CustomerEntity)
                {
                    relatedKeys.AddRange(DataProvider.GetRelatedKeys(contextKey, EntityType.OrderEntity));
                }
                else
                {
                    relatedKeys.Add(contextKey);
                }
            }

            // Save the context information
            emailOutbound.ContextID = contextKeys.Count == 1 ? contextKeys[0] : (long?) null;
            emailOutbound.ContextType = contextKeys.Count >= 1 ? EntityUtility.GetEntitySeed(contextKeys[0]) : (int?) null;

            // Go though each unique related key
            foreach (long relatedKey in relatedKeys.Distinct())
            {
                UserSession.Security.DemandPermission(PermissionType.EntityTypeSendEmail, relatedKey);

                EmailOutboundRelationEntity relation = new EmailOutboundRelationEntity();
                relation.EntityID = relatedKey;
                relation.RelationType = (int) EmailOutboundRelationType.RelatedObject;

                emailOutbound.RelatedObjects.Add(relation);
            }

            // Set the template used and its encoding
            emailOutbound.TemplateID = template.TemplateID;
            emailOutbound.Encoding = template.OutputEncoding;

            return emailOutbound;
        }

        /// <summary>
        /// Populate the contents of the preamble given the template settings and template results used
        /// to generate the message content.
        /// </summary>
        private void PopulateHeader()
        {
            // Should only be multiple for a single message in the label sheet case
            Debug.Assert(results.Count == 1 || template.Type == (int) TemplateType.Label);

            // Get the settings to use for this store and template
            TemplateStoreSettingsEntity settings = TemplateHelper.GetStoreSettings(template, storeID);

            // See if we're actually supposed to use the global default settings for the template
            if (settings.EmailUseDefault)
            {
                settings = TemplateHelper.GetStoreSettings(template, null);
            }

            // Default account to send with
            EmailAccountID = settings.EmailAccountID;

            // If its -1, that means use the default for the store
            if (EmailAccountID == -1)
            {
                EmailAccountEntity storeDefault = EmailAccountManager.GetStoreDefault(storeID);
                if (storeDefault != null)
                {
                    EmailAccountID = storeDefault.EmailAccountID;
                }
            }

            List<long> contextKeys = new List<long>();
            foreach (TemplateResult result in results)
            {
                contextKeys.AddRange(result.XPathSource.Input.ContextKeys);
            }

            // Now we can set some things.  The replacements are b\c the Add function wants multiple addresses
            // seperated by ",".
            To = TemplateTokenProcessor.ProcessTokens(settings.EmailTo, contextKeys).Replace(";", ",");
            CC = TemplateTokenProcessor.ProcessTokens(settings.EmailCc, contextKeys).Replace(";", ",");
            BCC = TemplateTokenProcessor.ProcessTokens(settings.EmailBcc, contextKeys).Replace(";", ",");
            Subject = TemplateTokenProcessor.ProcessTokens(settings.EmailSubject, contextKeys);
        }
    }
}
