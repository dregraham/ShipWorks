using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using ShipWorks.Data.Grid;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using log4net;
using ShipWorks.Data;

namespace ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Grid
{
    /// <summary>
    /// Hyperlink column definition for displaying the amazon orderID and pulling up the order info on amazon
    /// </summary>
    public class GridAmazonOrderDisplayType : GridColumnDisplayType
    {
        private static ILog log = LogManager.GetLogger(typeof (GridAmazonOrderDisplayType));

        /// <summary>
        /// Constructor
        /// </summary>
        public GridAmazonOrderDisplayType()
        {
            GridHyperlinkDecorator hyperlink = new GridHyperlinkDecorator();
            hyperlink.LinkClicked += new GridHyperlinkClickEventHandler(OnLinkClicked);

            Decorate(hyperlink);
        }

        /// <summary>
        /// Launches the browser to the amazon order page
        /// </summary>
        void OnLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            AmazonOrderEntity order = e.Row.Entity as AmazonOrderEntity;
            if (order != null)
            {
                string domainName = GetDomainName(order);
                WebHelper.OpenUrl(string.Format("https://sellercentral.{0}/gp/orders/order-details.html/?orderID={1}", domainName, order.AmazonOrderID), e.Row.Grid.SandGrid.TopLevelControl);
            }
        }

        /// <summary>
        /// Gets the domain name of the store associated with the given order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The domain name for the store (e.g. amazon.com, amazon.ca, etc.)</returns>
        private static string GetDomainName(OrderEntity order)
        {
            // Default the domain to amazon.com in case there is an exception trying to 
            // get the domain from the store type
            string domainName = "amazon.com";
            
            try
            {
                AmazonStoreEntity amazonStoreEntity = DataProvider.GetEntity(order.StoreID) as AmazonStoreEntity;

                // Obtain the domain name from the store, so we navigate to the correct URL based on 
                // the marketplace (i.e. amazon.ca vs. amazon.com)
                AmazonStoreType amazonStoreType = new AmazonStoreType(amazonStoreEntity);
                domainName = amazonStoreType.GetDomainName();
            }
            catch (AmazonException)
            {
                log.WarnFormat("The domain name could not be retreived for the Amazon store (store ID {0}); defaulting to amazon.com.", order.StoreID);
            }

            return domainName;
        }
    }
}
