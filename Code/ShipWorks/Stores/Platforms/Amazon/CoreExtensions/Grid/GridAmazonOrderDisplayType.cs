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

namespace ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Grid
{
    /// <summary>
    /// Hyperlink column definition for displaying the amazon orderID and pulling up the order info on amazon
    /// </summary>
    public class GridAmazonOrderDisplayType : GridColumnDisplayType
    {
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
                WebHelper.OpenUrl(string.Format("https://sellercentral.amazon.com/gp/orders/order-details.html/?orderID={0}", order.AmazonOrderID), e.Row.Grid.SandGrid.TopLevelControl);
            }
        }
    }
}
