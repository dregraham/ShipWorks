using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Content.Panels.CoreExtensions.Grid
{
    /// <summary>
    /// Custom DisplayType for displaying Edit or View for email messages
    /// </summary>
    class GridEmailEditViewDisplayType : GridActionDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridEmailEditViewDisplayType() 
            : base(string.Empty, GridLinkAction.Edit)
        {
            
        }

        /// <summary>
        /// Get the value to use for the given entity
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            return entity;
        }

        /// <summary>
        /// Get the text to display in the column
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            EmailOutboundEntity entity = value as EmailOutboundEntity;
            if (entity == null)
            {
                return string.Empty;
            }

            if (!UserSession.Security.HasPermission(PermissionType.RelatedObjectSendEmail, entity.EmailOutboundID))
            {
                return string.Empty;
            }

            return (EmailOutboundStatus) entity.SendStatus == EmailOutboundStatus.Sent ? "View" : "Edit";
        }
    }
}
