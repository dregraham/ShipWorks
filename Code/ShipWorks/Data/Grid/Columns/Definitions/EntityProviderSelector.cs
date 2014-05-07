using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web.ApplicationServices;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Shows a provider menu when associated with an Entity Grid.
    /// </summary>
    public class EntityProviderSelector
    {
        /// <summary>
        /// Displays the provider.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns></returns>
        public static string DisplayProvider(object arg)
        {
            return "Select";
        }

        /// <summary>
        /// Shows the provider option menu.
        /// </summary>
        public void ShowProviderOptionMenu(object sender, GridHyperlinkClickEventArgs e)
        {
            ShipmentEntity shipment = (ShipmentEntity) e.Row.Entity;
            if (shipment==null)
            {
                return;
            }

            SandGrid grid = sender as SandGrid;
            Debug.Assert(grid != null);

            ShowProviderOptionMenu(grid, shipment, new Point(e.MouseArgs.X - grid.HScrollOffset, e.MouseArgs.Y - grid.VScrollOffset));
        }

        /// <summary>
        /// Shows the provider option menu.
        /// </summary>
        private static void ShowProviderOptionMenu(SandGrid owner, ShipmentEntity shipment, Point displayPosition)
        {
            if (shipment.Processed)
            {
                MessageHelper.ShowInformation(Program.MainForm, "Cannot change provider after shipment has been processed.");
            }

            using (var menu = new ContextMenu(ShipmentTypeManager.EnabledShipmentTypes.Select(type => new MenuItem(type.ShipmentTypeName, (sender, args) => SelectProvider(shipment, type))).ToArray()))
            {
                menu.Show(owner, displayPosition);
            }
        }

        /// <summary>
        /// Selects the provider.
        /// </summary>
        private static void SelectProvider(ShipmentEntity shipment, ShipmentType type)
        {
            shipment.ShipmentType = (int) type.ShipmentTypeCode;

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                sqlAdapter.SaveAndRefetch(shipment);
            }
        }

        /// <summary>
        /// Determines whether [is link enabled] [the specified argument].
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns></returns>
        public static bool IsLinkEnabled(object arg)
        {
            ShipmentEntity entity = arg as ShipmentEntity;
            if (entity == null)
            {
                return false;
            }

            return !entity.Processed;
        }
    }
}