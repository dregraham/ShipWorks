using System.Diagnostics;
using System.Drawing;
using Divelements.SandGrid;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Responsible for displaying possible validated addresses
    /// </summary>
    public class EntityGridAddressSelector : AddressSelector
    {
        IEntity2 selectedEntity;

        /// <summary>
        /// Display the list of available addresses
        /// </summary>
        public void ShowAddressOptionMenu(object sender, GridHyperlinkClickEventArgs e)
        {
            IEntity2 entity = e.Row.Entity;
            if (entity == null)
            {
                return;
            }

            selectedEntity = entity;

            SandGrid grid = sender as SandGrid;
            Debug.Assert(grid != null);
            
            ShowAddressOptionMenu(grid, new AddressAdapter(entity, "Ship"),
                new Point(e.MouseArgs.X - grid.HScrollOffset, e.MouseArgs.Y - grid.VScrollOffset), 
                EntityUtility.GetEntityId(entity));
        }

        /// <summary>
        /// Save the entity after an address has been selected
        /// </summary>
        /// <param name="entityAdapter"></param>
        /// <param name="originalAddress"></param>
        protected override void OnAddressSelected(AddressAdapter entityAdapter, AddressAdapter originalAddress)
        {
            using (SqlAdapter sqlAdapter = new SqlAdapter(true))
            {
                // If the entity is an order, we need to propagate its address to its shipments
                OrderEntity order = selectedEntity as OrderEntity;
                if (order != null)
                {
                    ValidatedAddressManager.PropagateAddressChangesToShipments(sqlAdapter, order.OrderID, originalAddress, entityAdapter);
                }

                sqlAdapter.SaveAndRefetch(selectedEntity);
                sqlAdapter.Commit();
            }

            Program.MainForm.ForceHeartbeat();

            base.OnAddressSelected(entityAdapter, originalAddress);
        }
    }
}
