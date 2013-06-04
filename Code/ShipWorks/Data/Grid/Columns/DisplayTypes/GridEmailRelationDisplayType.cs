using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Drawing;
using ShipWorks.Data.Model;
using ShipWorks.UI;
using System.Windows.Forms;
using ShipWorks.Email.Outlook;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Display type for showing which entity is related to an outgoing email
    /// </summary>
    public class GridEmailRelationDisplayType : GridEntityDisplayType
    {
        /// <summary>
        /// The value returned from this is passed to the rest of the overridden functions
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            EmailOutboundEntity email = entity as EmailOutboundEntity;
            if (email == null)
            {
                return GetEntityDisplayInfo(null);
            }

            if (email.ContextID != null)
            {
                return GetEntityDisplayInfo(email.ContextID.Value);
            }

            return email;
        }

        /// <summary>
        /// Get the text to display
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            EmailOutboundEntity email = value as EmailOutboundEntity;
            if (email == null)
            {
                return base.GetDisplayText(value);
            }

            if (email.ContextType == null)
            {
                return "";
            }

            else
            {
                if (email.ContextID != null)
                {
                    throw new InvalidOperationException("Shouldnt get here in the singular case.");
                }

                return "(Multiple)";
            }
        }

        /// <summary>
        /// Get the image to display
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            EmailOutboundEntity email = value as EmailOutboundEntity;
            if (email == null)
            {
                return base.GetDisplayImage(value);
            }

            if (!ShowIcon)
            {
                return null;
            }

            EntityType? entityType = email.ContextType != null ? EntityUtility.GetEntityType(email.ContextType.Value) : (EntityType?) null;

            if (entityType == null)
            {
                return null;
            }

            return EntityUtility.GetEntityImage(entityType.Value);
        }

        /// <summary>
        /// We override this simply to shield the base class from getting an EmailOuttboundEntity... which it's not expecting.
        /// </summary>
        protected override Color? GetDisplayForeColor(object value)
        {
            EmailOutboundEntity email = value as EmailOutboundEntity;
            if (email == null)
            {
                return base.GetDisplayForeColor(value);
            }

            return IsHyperlinked(value) ? Color.Blue : (Color?) null;
        }

        /// <summary>
        /// Returns true if hyperlinking should be enabled
        /// </summary>
        protected override bool IsHyperlinked(object value)
        {
            EmailOutboundEntity email = value as EmailOutboundEntity;
            if (email == null)
            {
                return base.IsHyperlinked(value);
            }

            // We should only get here if its Multiple
            return email.ContextType != null;
        }

        /// <summary>
        /// Handle when the link gets clicked
        /// </summary>
        protected override void OnLinkClicked(EntityGridRow row, EntityGridColumn column)
        {
            EmailOutboundEntity email = row.GetFormattedValue(column).Value as EmailOutboundEntity;
            if (email == null)
            {
                base.OnLinkClicked(row, column);
            }
            else
            {
                if (email.ContextType != null)
                {
                    Control owner = row.Grid.SandGrid.TopLevelControl;

                    using (EmailOutboundMultipleRelationsDlg dlg = new EmailOutboundMultipleRelationsDlg(email.EmailOutboundID))
                    {
                        dlg.ShowDialog(owner);
                    }
                }
            }
        }
    }
}
