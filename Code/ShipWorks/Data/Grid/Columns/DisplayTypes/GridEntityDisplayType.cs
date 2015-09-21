using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid;
using ShipWorks.UI;
using ShipWorks.Shipping;
using System.Drawing;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using System.Xml.Serialization;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Content;
using System.Windows.Forms;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Templates;
using ShipWorks.Templates.Management;
using ShipWorks.Users.Audit;
using ShipWorks.Properties;
using ShipWorks.Stores.Content.Panels;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using ShipWorks.ApplicationCore;
using Autofac;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Specialized display type for showing entities and links to edit them
    /// </summary>
    public class GridEntityDisplayType : GridColumnDisplayType
    {
        // When testing for hyperlinks, we format our value a lot.  This is so we don't overdue going to the database to do that
        LruCache<long, GridEntityDisplayInfo> valueCache = new LruCache<long, GridEntityDisplayInfo>(50, TimeSpan.FromSeconds(10));

        /// <summary>
        /// Indicates if the icon representing the entity type is displayed
        /// </summary>
        bool showIcon = true;

        // Indicates if a prefix of the object type is shown before the object.  Such as "Order" in "Order 12"
        bool includeTypePrefix = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridEntityDisplayType()
        {
            GridHyperlinkDecorator hyperlink = new GridHyperlinkDecorator();
            hyperlink.LinkClicked += new GridHyperlinkClickEventHandler(OnLinkClicked);
            hyperlink.QueryEnabled += new GridHyperlinkQueryEnabledEventHandler(OnQueryHyperlinkEnabled);
            Decorate(hyperlink);
        }

        /// <summary>
        /// Create the editor used to control the display settings
        /// </summary>
        public override GridColumnDisplayEditor CreateEditor()
        {
            return new GridEntityDisplayEditor(this);
        }

        /// <summary>
        /// Indicates if the icon representing the entity type should be displayed
        /// </summary>
        public bool ShowIcon
        {
            get { return showIcon; }
            set { showIcon = value; }
        }

        /// <summary>
        /// Indicates if the prefix for the object type should be shown, Such as "Order" in "Order 12"
        /// </summary>
        public bool IncludeTypePrefix
        {
            get { return includeTypePrefix; }
            set { includeTypePrefix = value; }
        }

        /// <summary>
        /// The value returned from this is passed to the rest of the overridden functions
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            object value = base.GetEntityValue(entity);

            return GetEntityDisplayInfo(value == null ? (long?) null : (long) value);
        }

        /// <summary>
        /// Get the display info object for the given entity
        /// </summary>
        protected GridEntityDisplayInfo GetEntityDisplayInfo(long? entityID)
        {
            if (entityID == null)
            {
                return new GridEntityDisplayInfo(0, null, "");
            }

            long objectID = (long) entityID;

            GridEntityDisplayInfo info = valueCache[objectID];

            if (info == null)
            {
                info = new GridEntityDisplayInfo(objectID, null, "");
                valueCache[objectID] = info;
            }

            // Special case for audit "Various"
            if (objectID == AuditUtility.VariousEntityID)
            {
                info.DisplayText = "Various";
            }
            else
            {
                ObjectLabel label = ObjectLabelManager.GetLabel(objectID);

                info.DisplayText = label.GetCustomText(includeTypePrefix, true, true);
                info.EntityType = EntityUtility.GetEntityType(objectID);
                info.IsDeleted = label.IsDeleted;
            }

            return info;
        }

        /// <summary>
        /// Get the text to display
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            return ((GridEntityDisplayInfo) value).DisplayText;
        }

        /// <summary>
        /// Get the image to display
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            if (!showIcon)
            {
                return null;
            }

            GridEntityDisplayInfo info = (GridEntityDisplayInfo) value;

            if (info.EntityID == AuditUtility.VariousEntityID)
            {
                return Resources.colors16;
            }

            if (info.EntityID > 0)
            {
                return EntityUtility.GetEntityImage(info.EntityID);
            }

            if (info.EntityType != null)
            {
                return EntityUtility.GetEntityImage(info.EntityType.Value);
            }

            return null;
        }

        /// <summary>
        /// Control the color
        /// </summary>
        protected override Color? GetDisplayForeColor(object value)
        {
            GridEntityDisplayInfo info = (GridEntityDisplayInfo) value;

            return !info.IsDeleted ? base.GetDisplayForeColor(value) : SystemColors.GrayText;
        }

        /// <summary>
        /// Called back from the hyperlink decorator to determine if hyperlinking is enabled
        /// </summary>
        void OnQueryHyperlinkEnabled(object sender, GridHyperlinkQueryEnabledEventArgs e)
        {
            e.Enabled = IsHyperlinked(e.Value);
        }

        /// <summary>
        /// Indicates if the given value represents a Hyperlink
        /// </summary>
        protected virtual bool IsHyperlinked(object value)
        {
            GridEntityDisplayInfo info = (GridEntityDisplayInfo) value;

            // If its new, then you can't edit it
            if (info.EntityID < 0)
            {
                return false;
            }

            // If it doesnt exist you obviously can't edit it
            if (info.EntityType == null)
            {
                return false;
            }

            return !info.IsDeleted && CanEdit(info.EntityID);
        }

        /// <summary>
        /// Indicates if the specified entity has an editor, and if the current user has permission to edit it.
        /// </summary>
        private bool CanEdit(long entityID)
        {
            EntityType entityType = EntityUtility.GetEntityType(entityID);

            switch (entityType)
            {
                case EntityType.CustomerEntity:
                    return UserSession.Security.HasPermission(PermissionType.CustomersCreateEdit);

                case EntityType.OrderEntity:
                case EntityType.OrderItemEntity:
                case EntityType.OrderItemAttributeEntity:
                case EntityType.OrderChargeEntity:
                    return UserSession.Security.HasPermission(PermissionType.OrdersModify, entityID);

                case EntityType.ShipmentEntity:
                case EntityType.FedExPackageEntity:
                    return true;

                case EntityType.TemplateEntity:
                    return UserSession.Security.HasPermission(PermissionType.ManageTemplates);

                case EntityType.NoteEntity:
                    return UserSession.Security.HasPermission(PermissionType.RelatedObjectEditNotes, entityID);
            }

            return false;
        }

        /// <summary>
        /// Handle when the link gets clicked
        /// </summary>
        void OnLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            OnLinkClicked(e.Row, e.Column);
        }

        /// <summary>
        /// Virtual to allow derived classes a chance at processing the link
        /// </summary>
        protected virtual void OnLinkClicked(EntityGridRow row, EntityGridColumn column)
        {
            GridEntityDisplayInfo info = (GridEntityDisplayInfo) row.GetFormattedValue(column).Value;

            if (info.EntityType != null)
            {
                Control owner = row.Grid.SandGrid.TopLevelControl;

                switch (info.EntityType)
                {
                    case EntityType.OrderEntity:
                    case EntityType.OrderItemEntity:
                    case EntityType.OrderItemAttributeEntity:
                    case EntityType.OrderChargeEntity:
                        {
                            // We have to get the OrderID
                            List<long> keys = DataProvider.GetRelatedKeys(info.EntityID, EntityType.OrderEntity);

                            if (keys.Count == 0)
                            {
                                MessageHelper.ShowMessage(owner, "The selection has been deleted.");
                            }
                            else
                            {
                                OrderEditorDlg.Open(keys[0], owner);
                            }
                       }
                        break;

                    case EntityType.CustomerEntity:
                        {
                            CustomerEditorDlg.Open(info.EntityID, owner);
                        }
                        break;

                    case EntityType.ShipmentEntity:
                    case EntityType.FedExPackageEntity:
                        {
                            long shipmentID;

                            if (info.EntityType == EntityType.ShipmentEntity)
                            {
                                shipmentID = info.EntityID;
                            }
                            else
                            {
                                List<long> parentKeys = DataProvider.GetRelatedKeys(info.EntityID, EntityType.ShipmentEntity);
                                if (parentKeys.Count == 1)
                                {
                                    shipmentID = parentKeys[0];
                                }
                                else
                                {
                                    shipmentID = 0;
                                }
                            }

                            // We have to get the shipment to edit via the shipping manager
                            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);

                            if (shipment == null)
                            {
                                MessageHelper.ShowMessage(owner, "The shipment has been deleted.");
                            }
                            else
                            {
                                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                                {
                                    using (ShippingDlg dlg = new ShippingDlg(new List<ShipmentEntity> { shipment }, lifetimeScope))
                                    {
                                        dlg.ShowDialog(owner);
                                    }
                                }
                            }
                        }
                        break;

                    case EntityType.NoteEntity:
                        {
                            NoteEntity note = (NoteEntity) DataProvider.GetEntity(info.EntityID);

                            if (note == null)
                            {
                                MessageHelper.ShowMessage(owner, "The note has been deleted.");
                            }
                            else
                            {
                                using (EditNoteDlg dlg = new EditNoteDlg(note, PanelDataMode.LiveDatabase))
                                {
                                    dlg.ShowDialog(owner);
                                }
                            }
                        }
                        break;

                    case EntityType.TemplateEntity:
                        {
                            TemplateEntity template = TemplateManager.Tree.CreateEditableClone().GetTemplate(info.EntityID);

                            if (template == null)
                            {
                                MessageHelper.ShowMessage(owner, "The template has been deleted.");
                            }
                            else
                            {
                                using (TemplateEditorDlg dlg = new TemplateEditorDlg(template))
                                {
                                    dlg.ShowDialog(owner);
                                }
                            }
                        }
                        break;

                    default:
                        throw new NotImplementedException("Unhandled entity type in grid link.");
                }
            }
        }
    }
}
