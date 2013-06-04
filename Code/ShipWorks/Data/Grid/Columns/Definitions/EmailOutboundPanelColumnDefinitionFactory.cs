using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email;
using ShipWorks.Email.Outlook;
using ShipWorks.Data.Model;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Stores;
using ShipWorks.Stores.Content.Panels;
using ShipWorks.Stores.Content.Panels.CoreExtensions.Grid;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class EmailOutboundPanelColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible outbound email columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = EmailOutboundColumnDefinitionFactory.CreateDefinitions();

            // We can't have duplicate Guid's, so we have to adjust the base list
            foreach (GridColumnDefinition definition in definitions)
            {
                definition.MarkAsDerived();
            }

            definitions.AddRange(
                new GridColumnDefinition[]
                {
                    new GridColumnDefinition("{5D93881E-B80A-4cca-9F0F-869D98F509BE}", true,
                        new GridEmailEditViewDisplayType(), "View", "View",
                        EmailOutboundFields.EmailOutboundID) 
                        { 
                            DefaultWidth = 31,
                            ApplicableTest = (data) => 
                                {
                                    if (data == null)
                                    {
                                        return true;
                                    }

                                    long entityID = (long) data;

                                    if (UserSession.Security.HasPermission(PermissionType.EntityTypeSendEmail, entityID))
                                    {
                                        return true;
                                    }

                                    // For customers.. even if you can't email the customer, there can be order emails in the grid
                                    // that will need this column
                                    if (EntityUtility.GetEntityType(entityID) == EntityType.CustomerEntity)
                                    {
                                        return StoreManager.GetAllStores().Any(s => UserSession.Security.HasPermission(PermissionType.OrdersSendEmail, s.StoreID));
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                        },

                     new GridColumnDefinition("{C91B6AE9-3D54-4b18-8132-F344478FFB1F}", true,
                        new GridActionDisplayType("Delete", GridLinkAction.Delete), "Delete", "Delete",
                        EmailOutboundFields.EmailOutboundID) 
                        { 
                            DefaultWidth = 45,
                            ApplicableTest = (data) => UserSession.Security.HasPermission(PermissionType.ManageEmailAccounts)
                        },
                });

            // Return the definitions
            return definitions;
        }
    }
}
