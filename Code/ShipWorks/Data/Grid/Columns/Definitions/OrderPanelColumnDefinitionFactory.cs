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

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class OrderPanelColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible outbound email columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = OrderColumnDefinitionFactory.CreateDefinitions();

            // Remove the Order ID one, we'll add our own
            definitions.Remove(definitions[OrderFields.OrderNumberComplete]);

            // We can't have duplicate Guid's, so we have to adjust the base list
            foreach (GridColumnDefinition definition in definitions)
            {
                definition.MarkAsDerived();
            }

            definitions.Insert(0,
                new GridColumnDefinition("{9EC2DAF8-B513-48fd-9568-7760B4BC93E5}", true,
                    new GridEntityDisplayType(), "Order", new GridEntityDisplayInfo(6, EntityType.OrderEntity, "Order 1028"),
                    OrderFields.OrderID,
                    OrderFields.OrderNumber) { DefaultWidth = 100 });

            definitions.AddRange(
                new GridColumnDefinition[]
                {
                    new GridColumnDefinition("{17E16895-C956-4b21-8C4D-9B57DD7E87F7}", true,
                        new GridActionDisplayType("Edit", GridLinkAction.Edit), "Edit", "Edit",
                        OrderFields.OrderID) 
                        { 
                            DefaultWidth = 31
                        },

                     new GridColumnDefinition("{F59DD1AF-7DBA-48c1-9BBB-EAD3293592BA}", true,
                        new GridActionDisplayType(o => UserSession.Security.HasPermission(PermissionType.OrdersModify, (long) o) ? "Delete" : "", GridLinkAction.Delete), 
                            "Delete", "Delete",
                        OrderFields.OrderID) 
                        { 
                            DefaultWidth = 45,
                            ApplicableTest = (data) => data != null ? IsDeleteColumnApplicable((long) data) : true
                        },
                });

            // Return the definitions
            return definitions;
        }

        /// <summary>
        /// Indicates if the delete column should be visible for the given customer
        /// </summary>
        private static bool IsDeleteColumnApplicable(long customerID)
        {
            // First see if any store's are not editable
            bool allEditable = StoreManager.GetAllStores().All(s => UserSession.Security.HasPermission(PermissionType.OrdersModify, s.StoreID));

            // If there all editable, no need to check individual orders
            if (allEditable)
            {
                return true;
            }

            // The column has to be there if any order in the column is editable.
            foreach (long orderID in DataProvider.GetRelatedKeys(customerID, EntityType.OrderEntity))
            {
                if (UserSession.Security.HasPermission(PermissionType.OrdersModify, orderID))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
