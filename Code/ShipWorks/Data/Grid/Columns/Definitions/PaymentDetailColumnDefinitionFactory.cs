using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Users.Security;
using ShipWorks.Users;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class PaymentDetailColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible note columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
                     {
                        new GridColumnDefinition("{E302F111-2BF1-4827-BFBF-B511856E2928}", true,
                            new GridTextDisplayType(), "Label", "Payment Type",
                            OrderPaymentDetailFields.Label),

                        new GridColumnDefinition("{26AAD52E-EECB-4194-9F6A-155E8E6BB9CF}", true,
                            new GridTextDisplayType(), "Value", "Credit Card",
                            new GridColumnFunctionValueProvider((EntityBase2 entity) => 
                                {
                                   return PaymentDetailSecurity.ReadValue((OrderPaymentDetailEntity) entity);
                                }),
                            new GridColumnSortProvider(OrderPaymentDetailFields.Value)),
      
                        new GridColumnDefinition("{03AC62F4-1F9C-46bb-A99A-E90C5EB63EDF}", true,
                            new GridActionDisplayType("Edit", GridLinkAction.Edit), "Edit", "Edit",
                            OrderPaymentDetailFields.OrderPaymentDetailID) 
                            { 
                                DefaultWidth = 31,
                                ApplicableTest = (data) => data != null ? UserSession.Security.HasPermission(PermissionType.OrdersModify, (long) data) : true
                            },

                        new GridColumnDefinition("{21EA217F-8325-452d-9F75-4F9918076794}", true,
                            new GridActionDisplayType("Delete", GridLinkAction.Delete), "Delete", "Delete",
                            OrderPaymentDetailFields.OrderPaymentDetailID) 
                            { 
                                DefaultWidth = 45,
                                ApplicableTest = (data) => data != null ? UserSession.Security.HasPermission(PermissionType.OrdersModify, (long) data) : true
                            },
                     };

            return definitions;
        }
    }
}
