using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Shipping.Settings;
using Autofac;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// UserControl for editing the settings of the Print Shipping Labels task
    /// </summary>
    public partial class PrintShipmentsTaskEditor : ActionTaskEditor
    {
        PrintShipmentsTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintShipmentsTaskEditor(PrintShipmentsTask task)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            this.task = task;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            label2.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);
            linkShippingSettings.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);
            label4.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);
        }

        /// <summary>
        /// Open the shipping settings window
        /// </summary>
        private void OnShippingSettings(object sender, EventArgs e)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                using (ShippingSettingsDlg dlg = new ShippingSettingsDlg(lifetimeScope))
                {
                    dlg.ShowDialog(this);
                }
            }
        }
    }
}
