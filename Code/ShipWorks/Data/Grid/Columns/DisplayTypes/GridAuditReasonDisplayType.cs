using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Users.Audit;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Column display type for audit reason
    /// </summary>
    public class GridAuditReasonDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridAuditReasonDisplayType()
        {
            PreviewInputType = GridColumnPreviewInputType.LiteralString;
        }

        /// <summary>
        /// Get the value to use for the entity
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            return entity;
        }

        /// <summary>
        /// Get the display text for the audit reason
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            AuditEntity audit = value as AuditEntity;
            if (audit == null)
            {
                return "";
            }

            if (!string.IsNullOrEmpty(audit.ReasonDetail))
            {
                return audit.ReasonDetail;
            }

            // Can be removed when we release.   I had some old invalid data in my test db.
            if (!Enum.IsDefined(typeof(AuditReasonType), (AuditReasonType) audit.Reason))
            {
                return "";
            }

            return EnumHelper.GetDescription((AuditReasonType) audit.Reason);
        }
    }
}
