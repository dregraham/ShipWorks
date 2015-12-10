using System;
using System.Diagnostics;
using System.Drawing;
using Divelements.SandGrid;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
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
        SandGrid grid;

        public EntityGridAddressSelector(string fieldPrefix) :
            base(fieldPrefix)
        {
            
        }

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

            grid = sender as SandGrid;
            Debug.Assert(grid != null);

            ShowAddressOptionMenu(grid, new AddressAdapter(entity, AddressPrefix),
                new Point(e.MouseArgs.X - grid.HScrollOffset, e.MouseArgs.Y - grid.VScrollOffset),
                () =>
                {
                    using (SqlAdapter sqlAdapter = new SqlAdapter())
                    {
                        return ValidatedAddressManager.GetSuggestedAddresses(sqlAdapter, EntityUtility.GetEntityId(entity), AddressPrefix);
                    }
                });
        }

        /// <summary>
        /// Save the entity after an address has been selected
        /// </summary>
        /// <param name="entityAdapter"></param>
        /// <param name="originalAddress"></param>
        protected override void OnAddressSelected(AddressAdapter entityAdapter, AddressAdapter originalAddress)
        {
            try
            {
                using (SqlAdapter sqlAdapter = new SqlAdapter(true))
                {
                    // If the entity is an order, we need to propagate its address to its shipments
                    OrderEntity order = selectedEntity as OrderEntity;
                    if (order != null)
                    {
                        PropagateOrderAddressChange(entityAdapter, originalAddress, order, sqlAdapter);
                    }

                    sqlAdapter.SaveAndRefetch(selectedEntity);
                    sqlAdapter.Commit();
                }

                Program.MainForm.ForceHeartbeat();

                base.OnAddressSelected(entityAdapter, originalAddress);
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(grid, "The address could not be updated because the item has recently changed.  Please try again.");
            }
        }

        /// <summary>
        /// Propagate shipping to billing or vice versa, if necessary
        /// </summary>
        /// <param name="entityAdapter"></param>
        /// <param name="originalAddress"></param>
        /// <param name="order"></param>
        /// <param name="sqlAdapter"></param>
        private void PropagateOrderAddressChange(AddressAdapter entityAdapter, AddressAdapter originalAddress, OrderEntity order, SqlAdapter sqlAdapter)
        {
            bool shouldPropagateToShipments = true;

            string sourcePrefix = AddressPrefix;
            string destinationPrefix = AddressPrefix == "Ship" ? "Bill" : "Ship";

            AddressAdapter shippingAdapter = new AddressAdapter(order, destinationPrefix);
            if (shippingAdapter == originalAddress)
            {
                entityAdapter.CopyTo(shippingAdapter);
                entityAdapter.CopyValidationDataTo(shippingAdapter);
                ValidatedAddressManager.CopyValidatedAddresses(sqlAdapter, order.OrderID, sourcePrefix, order.OrderID, destinationPrefix);
            }
            else if (sourcePrefix == "Bill")
            {
                // Don't propagate billing address changes to shipments unless the original matched the shipping address
                shouldPropagateToShipments = false;
            }

            if (shouldPropagateToShipments)
            {
                ValidatedAddressManager.PropagateAddressChangesToShipments(sqlAdapter, order.OrderID, originalAddress, entityAdapter);
            }
        }
    }
}
