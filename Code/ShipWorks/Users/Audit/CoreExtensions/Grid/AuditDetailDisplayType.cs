using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid;
using ShipWorks.UI;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Users.Audit.CoreExtensions.Grid
{
    /// <summary>
    /// Specialized DisplayType for the audit column that let's you see the detail of what happened
    /// </summary>
    public class AuditDetailDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AuditDetailDisplayType()
        {
            GridHyperlinkDecorator hyperlink = new GridHyperlinkDecorator();
            hyperlink.LinkClicked += new GridHyperlinkClickEventHandler(OnLinkClicked);

            Decorate(hyperlink);
        }

        /// <summary>
        /// Get the value to use from the entity for the formatting functions
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            return entity;
        }

        /// <summary>
        /// Get the display text for the audit item
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            AuditEntity audit = value as AuditEntity;
            if (audit != null && audit.HasEvents)
            {
                return "View";
            }

            return "";
        }

        /// <summary>
        /// The view link has been clicked
        /// </summary>
        void OnLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            EntityGridRow row = e.Row;

            AuditUtility.ShowAuditDetail(row.Grid.SandGrid, row.EntityID.Value);
        }
    }
}
