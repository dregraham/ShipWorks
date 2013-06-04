using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Grid.Columns.Definitions;
using ShipWorks.Email.Accounts;
using ShipWorks.UI.Utility;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Email.Outlook
{
    /// <summary>
    /// Base user control for email folders in the EmailOutlookDlg
    /// </summary>
    [ToolboxItem(false)]
    public partial class EmailFolderControlBase : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EmailFolderControlBase()
        {
            InitializeComponent();

            ThemedBorderProvider.Apply(panelGridArea);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public virtual void Initialize(Form owner)
        {
            labelManage.Visible = UserSession.Security.HasPermission(PermissionType.ManageEmailAccounts);
            manageAccounts.Visible = UserSession.Security.HasPermission(PermissionType.ManageEmailAccounts);

            StorePermissionCoverage coverage = UserSession.Security.GetRelatedObjectPermissionCoverage(PermissionType.RelatedObjectSendEmail);
            
            // If you can't email for anything at all, then totally hide all the controls
            if (coverage == StorePermissionCoverage.None)
            {
                panelSidebar.Visible = false;
            }
        }

        /// <summary>
        /// Open the grid settings editor
        /// </summary>
        private void OnGridSettings(object sender, EventArgs e)
        {
            entityGrid.ShowColumnEditorDialog();
        }

        /// <summary>
        /// Manage email accounts
        /// </summary>
        private void OnManageAccounts(object sender, EventArgs e)
        {
            using (EmailAccountManagerDlg dlg = new EmailAccountManagerDlg())
            {
                dlg.ShowDialog(this);
            }

            entityGrid.ReloadGridRows();
        }
    }
}
