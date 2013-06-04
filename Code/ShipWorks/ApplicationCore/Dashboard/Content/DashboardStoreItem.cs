using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Drawing;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Dashboard item created by StoreType instances when there is an issue they need to display
    /// </summary>
    public class DashboardStoreItem : DashboardItem
    {
        string identifier;
        string storeName;

        Image image;
        string message;
        DashboardAction[] actions;

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardStoreItem(StoreEntity store, string identifier, Image image, string message, params DashboardAction[] actions)
        {
            this.identifier = string.Format("Store{0}_{1}", store.StoreID, identifier);
            this.image = image;
            this.storeName = store.StoreName;
            this.message = message;
            this.actions = actions;
        }

        /// <summary>
        /// Sets the bar that the item will be displayed in
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);

            Update(this);
        }

        /// <summary>
        /// Update the item to use the updated values from the given item
        /// </summary>
        public void Update(DashboardStoreItem storeItem)
        {
            DashboardBar.Image = storeItem.image;
            DashboardBar.PrimaryText = storeItem.storeName;
            DashboardBar.SecondaryText = storeItem.message;
            DashboardBar.ApplyActions(storeItem.actions);

            DashboardBar.CanUserDismiss = false;
        }

        /// <summary>
        /// The identifier of the local message
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
        }

        /// <summary>
        /// The name of the store this message is for
        /// </summary>
        public string StoreName
        {
            get { return storeName; }
        }

    }
}
