using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Adapter.Custom;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators
{
    /// <summary>
    /// Hyperlink decorator that lets each store control whether or not the hyperlink is going to be enabled
    /// </summary>
    public class GridStoreSpecificHyperlinkDecorator : GridHyperlinkDecorator
    {
        private static readonly bool skipManual = true;

        private class RollupInfo
        {
            public int ChildCount { get; set; }
            public EntityField2 ChildField { get; set; }
        }

        /// <summary>
        /// Determines if the hyperlink should be enabled for the given formatted value
        /// </summary>
        protected override bool IsHyperlinkEnabled(GridColumnFormattedValue formattedValue)
        {
            StoreType storeType = GetStoreType(formattedValue);
            EntityField2 field = formattedValue.PrimaryField;

            if (storeType == null || (object)field == null)
            {
                return false;
            }

            // Automatically translate rollups - we'll provide the selection menu for child items automatically
            RollupInfo rollupInfo = GetRollupInfo(field, formattedValue.Entity);

            // If there is a correspond child field, use it instead.  That way the store type only has to handle the child
            if (rollupInfo != null)
            {
                if (rollupInfo.ChildCount == 0)
                {
                    return false;
                }

                field = rollupInfo.ChildField;
            }

            return storeType.GridHyperlinkSupported(formattedValue.Entity, field);
        }

        /// <summary>
        /// Get the StoreType instance represented by the given value
        /// </summary>
        private StoreType GetStoreType(GridColumnFormattedValue formattedValue)
        {
            if (formattedValue.Entity == null)
            {
                return null;
            }

            OrderEntity order = formattedValue.Entity as OrderEntity;
            if (order == null)
            {
                OrderItemEntity item = formattedValue.Entity as OrderItemEntity;
                if (item != null)
                {
                    // It's a manual item - we know it won't have online representation, and doesn't need a link
                    if (skipManual && item.IsManual)
                    {
                        return null;
                    }

                    // Try to get the order from cache
                    order = (OrderEntity)DataProvider.GetEntity(item.OrderID);
                }
            }

            if (order == null)
            {
                return null;
            }

            // It's a manual order - we know it won't have online representation, and doesn't need a link
            if (skipManual && order.IsManual)
            {
                return null;
            }

            StoreEntity store = StoreManager.GetStore(order.StoreID);
            if (store == null)
            {
                return null;
            }

            return StoreTypeManager.GetType(store);
        }

        /// <summary>
        /// If the given field represents a rollup field, then the get the corresponding child field
        /// </summary>
        private RollupInfo GetRollupInfo(EntityField2 field, EntityBase2 entity)
        {
            EntityField2 childField = null;
            EntityField2 countField = null;

            if (EntityUtility.IsSameField(field, OrderFields.RollupItemCode))
            {
                childField = OrderItemFields.Code;
                countField = OrderFields.RollupItemCount;
            }

            if (EntityUtility.IsSameField(field, OrderFields.RollupItemName))
            {
                childField = OrderItemFields.Name;
                countField = OrderFields.RollupItemCount;
            }

            // TODO
            // Add more as more rollup column support is wanted

            if ((object)childField == null)
            {
                return null;
            }

            return new RollupInfo { ChildCount = (int)entity.GetCurrentFieldValue(countField.FieldIndex), ChildField = childField };
        }

        /// <summary>
        /// A hyperlink has been clicked
        /// </summary>
        protected override void OnLinkClicked(EntityGridRow row, EntityGridColumn column, MouseEventArgs mouseArgs)
        {
            GridColumnFormattedValue formattedValue = row.GetFormattedValue(column);

            StoreType storeType = GetStoreType(formattedValue);
            if (storeType != null)
            {
                // Automatically translate rollups - we'll provide the selection menu for child items automatically
                RollupInfo rollupInfo = GetRollupInfo(formattedValue.PrimaryField, formattedValue.Entity);
                if (rollupInfo != null)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    List<EntityBase2> children = DataProvider.GetRelatedEntities(
                        (long)formattedValue.Entity.GetCurrentFieldValue(0),
                        EntityTypeProvider.GetEntityType(rollupInfo.ChildField.ContainingObjectName));

                    if (children.Count == 1)
                    {
                        // We can't check children for manualness at the time we say "Yes" to supporting the hyperlink b\c that check happens constantly and would be too expensive.
                        if (skipManual && IsManuallyEntered(children[0]))
                        {
                            MessageHelper.ShowInformation(row.Grid.SandGrid, "The selected item was manually entered into ShipWorks, and has no further online information to display.");
                        }
                        else
                        {
                            storeType.GridHyperlinkClick(rollupInfo.ChildField, children[0], row.Grid.SandGrid);
                        }
                    }
                    else if (children.Count > 1)
                    {
                        ContextMenuStrip menu = new ContextMenuStrip();

                        foreach (EntityBase2 child in children)
                        {
                            // Required to make hoisting work properly - otherwise it uses the same child instead of each loop child
                            EntityBase2 hoistedChild = child;

                            ToolStripMenuItem menuItem = new ToolStripMenuItem(child.GetCurrentFieldValue(rollupInfo.ChildField.FieldIndex).ToString());
                            menuItem.Click += (object sender, EventArgs e) => { storeType.GridHyperlinkClick(rollupInfo.ChildField, hoistedChild, row.Grid.SandGrid); };

                            // Disable manual orders \ items
                            if (skipManual && IsManuallyEntered(hoistedChild))
                            {
                                menuItem.Text += " (Manually Entered)";
                                menuItem.Enabled = false;
                            }

                            menu.Items.Add(menuItem);
                        }

                        menu.Show(Cursor.Position);
                    }
                }
                else
                {
                    storeType.GridHyperlinkClick(formattedValue.PrimaryField, formattedValue.Entity, row.Grid.SandGrid);
                }
            }
        }

        /// <summary>
        /// Indicates if the entity is a manually entered order or order item
        /// </summary>
        private static bool IsManuallyEntered(EntityBase2 entity)
        {
            var manualField = entity.Fields["IsManual"];
            if (manualField != null)
            {
                return (bool)manualField.CurrentValue;
            }
            else
            {
                return false;
            }
        }
    }
}
