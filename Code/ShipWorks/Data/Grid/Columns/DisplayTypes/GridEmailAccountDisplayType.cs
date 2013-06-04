using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email.Accounts;
using System.Drawing;
using ShipWorks.Properties;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Specialized display type for display email account names
    /// </summary>
    public class GridEmailAccountDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridEmailAccountDisplayType()
        {
            this.PreviewInputType = GridColumnPreviewInputType.LiteralString;
        }

        /// <summary>
        /// Get the email account name for the account value passed
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            long accountID = (long) value;
            EmailAccountEntity account = EmailAccountManager.GetAccount(accountID);

            if (account == null)
            {
                return "[Deleted]";
            }

            return account.AccountName;
        }

        /// <summary>
        /// Display a warning image if the account has been deleted
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            long accountID = (long) value;
            EmailAccountEntity account = EmailAccountManager.GetAccount(accountID);

            if (account == null)
            {
                return Resources.warning16;
            }

            return null;
        }
    }
}
