using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Users;
using Divelements.SandRibbon;
using ShipWorks.ApplicationCore.Appearance;

namespace ShipWorks.Templates.Controls
{
    /// <summary>
    /// Utility class for helping to display the template tree as a contextmenu submenu
    /// </summary>
    public static class TemplateTreePopupController
    {
        /// <summary>
        /// Show the TemplaetTree as a DropDown in resonse to a ToolStripMenuItem.Opened
        /// </summary>
        public static void ShowMenuDropDown(Form owner, ToolStripMenuItem menuItem, TemplateNodeChangedEventHandler actionHandler)
        {
            PopupController controller = CreateTemplateTreePopupController(actionHandler, null);
            controller.ShowMenu(menuItem, owner);
        }

        /// <summary>
        /// Show the TemplateTree as a popup in response to a ribbon button popup.  The user's template folder expansion state will be loaded and then saved on close.
        /// </summary>
        public static void ShowRibbonPopup(Popup sandPop, BeforePopupEventArgs e, TemplateNodeChangedEventHandler actionHandler)
        {
            ShowRibbonPopup(sandPop, e, actionHandler, null);
        }

        /// <summary>
        /// Show the TemplateTree as a popup in response to a ribbon button popup.  If expansionState is null, the user's template folder expansion state
        /// will be loaded and then saved on close.  If not null, the given expansionState will be used, and the folder state will not be saved.
        /// </summary>
        public static void ShowRibbonPopup(Popup sandPop, BeforePopupEventArgs e, TemplateNodeChangedEventHandler actionHandler, FolderExpansionState expansionState)
        {
            WidgetBase host = (WidgetBase) sandPop.PopupHost;

            PopupController controller = CreateTemplateTreePopupController(actionHandler, expansionState);
            controller.ShowDropDown(host.Bounds, host.HostControl);

            e.Cancel = true;
        }

        /// <summary>
        /// Create the PopupController that can be used to show a DropDown TemplateTree. If expansionState is null, the user's template folder expansion state
        /// will be loaded and then saved on close.  If not null, the given expansionState will be used, and the folder state will not be saved.
        /// </summary>
        private static PopupController CreateTemplateTreePopupController(TemplateNodeChangedEventHandler actionHandler, FolderExpansionState expansionState)
        {
            TemplateTreeControl tree = new TemplateTreeControl();
            tree.BorderStyle = BorderStyle.None;
            tree.HotTracking = true;
            tree.LoadTemplates();

            if (expansionState == null)
            {
                tree.ApplyFolderState(new FolderExpansionState(UserSession.User.Settings.TemplateExpandedFolders));
            }
            else
            {
                tree.ApplyFolderState(expansionState, false);
            }

            // Little hack, tag the tree with whether to save the folder
            tree.Tag = (expansionState == null);

            PopupController controller = new PopupController(tree);

            // Close the popup when a template is selected
            tree.SelectedTemplateNodeChanged += delegate(object sender, TemplateNodeChangedEventArgs e)
            {
                controller.Close(DialogResult.OK);

                // And also perform whatever action is requested
                actionHandler(sender, e);
            };

            // When the popup closes, we cleanup
            controller.PopupClosing += new EventHandler(OnTemplateTreePopupClosing);

            return controller;
        }

        /// <summary>
        /// The PopupController for a TemplateTree is closing
        /// </summary>
        private static void OnTemplateTreePopupClosing(object sender, EventArgs e)
        {
            PopupController controller = (PopupController) sender;
            TemplateTreeControl tree = (TemplateTreeControl) controller.Control;

            // Unhook
            controller.PopupClosing -= new EventHandler(OnTemplateTreePopupClosing);

            // Save the folder state (could have crashed with it open - check to be sure)
            if ((bool) tree.Tag && UserSession.IsLoggedOn)
            {
                UserSession.User.Settings.TemplateExpandedFolders = tree.GetFolderState().GetState();
            }

            // Dispose the tree
            tree.Dispose();
        }
    }
}
