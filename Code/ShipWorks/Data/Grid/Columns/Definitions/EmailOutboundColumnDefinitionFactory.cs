using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email;
using ShipWorks.Email.Outlook;
using ShipWorks.Data.Model;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data.Grid.Columns.SortProviders;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class EmailOutboundColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible outbound email columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
                {
                    new GridColumnDefinition("{4D5E6F1A-30D1-4c58-83F2-74066D64685D}", true,
                        new GridEnumDisplayType<EmailOutboundStatus>(EnumSortMethod.Value), "Status", EmailOutboundStatus.Sent,
                        EmailOutboundFields.SendStatus) { DefaultWidth = 80 },

                    new GridColumnDefinition("{0B0BD70B-0767-4504-8A84-E9503B9120E9}",
                        new GridEmailAddressDisplayType(), "From", "\"Rob\" <rob@example.com>",
                        EmailOutboundFields.FromAddress),

                    new GridColumnDefinition("{24605750-DD58-479f-9060-3B618EA72050}", true,
                        new GridEmailAccountDisplayType(), "Account", "Business Account",
                        EmailOutboundFields.AccountID),

                    new GridColumnDefinition("{C637CE91-4BF7-4b4d-95A3-630B415A2C75}", true,
                        new GridEmailAddressDisplayType(), "To", "\"Joe\" <joe@example.com>",
                        EmailOutboundFields.ToList),

                    new GridColumnDefinition("{BB0A7100-08B5-46ee-AD67-2796B89C5E4A}",
                        new GridEmailAddressDisplayType(), "CC", "\"Bob\" <bob@example.com>",
                        EmailOutboundFields.CcList),

                    new GridColumnDefinition("{9D4A9A3A-5050-4017-AFDC-91A53A26A370}",
                        new GridEmailAddressDisplayType(), "BCC", "\"Ann\" <ann@example.com>",
                        EmailOutboundFields.BccList),

                    new GridColumnDefinition("{85CB6616-8E9A-4c7b-9E6F-18081878DCA4}", true,
                        new GridTextDisplayType(), "Subject", "Your Order Details",
                        EmailOutboundFields.Subject),

                    new GridColumnDefinition("{8E6C53D2-2807-406c-A9FC-032DD4C397C1}",
                        new GridDateDisplayType { UseDescriptiveDates = true }, "Composed", DateTimeUtility.ParseEnUS("03/04/2001 1:30 PM").ToUniversalTime(),
                        EmailOutboundFields.ComposedDate),

                    new GridColumnDefinition("{86D6EF68-85E4-4eda-AAB1-F8CAAF8DC0C3}",
                        new GridDateDisplayType { UseDescriptiveDates = true }, "Sent", DateTimeUtility.ParseEnUS("03/04/2001 1:30 PM").ToUniversalTime(),
                        EmailOutboundFields.SentDate),

                    new GridColumnDefinition("{BAD657EB-0039-4884-959A-45A680765FE4}", 
                        new GridDateDisplayType { UseDescriptiveDates = true }, "Delay Until", DateTimeUtility.ParseEnUS("03/04/2001 1:30 PM").ToUniversalTime(),
                        EmailOutboundFields.DontSendBefore),

                    new GridColumnDefinition("{086DC622-64DA-4b31-ADF0-3BD90736F8B6}", true,
                        new GridEmailRelationDisplayType(), "Related To", new GridEntityDisplayInfo(6, EntityType.OrderEntity, "Order 1028"),
                        new GridColumnFieldValueProvider(EmailOutboundFields.ContextID),
                        new GridColumnObjectLabelSortProvider(EmailOutboundFields.ContextID)),

                    new GridColumnDefinition("{675AB8FF-DD39-4e4c-8C42-93AD47A85D78}", true,
                        new GridEntityDisplayType() { IncludeTypePrefix = false }, "Template", new GridEntityDisplayInfo(25, EntityType.TemplateEntity, "Invoices\\Order Invoice"),
                        new GridColumnFieldValueProvider(EmailOutboundFields.TemplateID),
                        new GridColumnObjectLabelSortProvider(EmailOutboundFields.TemplateID)),

                    new GridColumnDefinition("{83FC2818-0694-4758-B027-82A9EE7A3577}",
                        new GridTextDisplayType(), "Send Attempts", 1,
                        EmailOutboundFields.SendAttemptCount)
                };

            // Return the definitions
            return definitions;
        }
    }
}
