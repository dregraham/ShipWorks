using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class ChargeColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible note columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
                     {
                        new GridColumnDefinition("{DFC8511D-3667-4aee-99F7-BBDB145AE159}", true,
                            new GridTextDisplayType(), "Type", "Shipping",
                            OrderChargeFields.Type),

                        new GridColumnDefinition("{20EE6DF2-91FB-4b23-A66F-B8B0C496A86C}", true,
                            new GridTextDisplayType(), "Description", "First Class",
                            OrderChargeFields.Description),

                        new GridColumnDefinition("{9B950072-A457-4c4f-B213-D32886FFD590}", true, 
                            new GridMoneyDisplayType(), "Amount", 6.18m,
                            OrderChargeFields.Amount) { DefaultWidth = 65 },      
      
                        new GridColumnDefinition("{480DA55A-1AD5-4081-8740-4B656809D22D}", true,
                            new GridActionDisplayType("Edit", GridLinkAction.Edit), "Edit", "Edit",
                            OrderChargeFields.OrderChargeID) 
                            { 
                                DefaultWidth = 31,
                                ApplicableTest = (data) => data != null ? UserSession.Security.HasPermission(PermissionType.OrdersModify, (long) data) : true
                            },

                        new GridColumnDefinition("{B48E1BDC-418A-479b-A50C-C2CD125F706F}", true,
                            new GridActionDisplayType("Delete", GridLinkAction.Delete), "Delete", "Delete",
                            OrderChargeFields.OrderChargeID) 
                            { 
                                DefaultWidth = 45,
                                ApplicableTest = (data) => data != null ? UserSession.Security.HasPermission(PermissionType.OrdersModify, (long) data) : true
                          },
                     };

            return definitions;
        }
    }
}
