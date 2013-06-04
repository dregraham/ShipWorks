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

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Grid
{
    /// <summary>
    /// Hyperlink column definition for displaying the buyer id and pulling up 
    /// their feedback page on ebay.com
    /// </summary>
    public class GridEbayBuyerIDDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridEbayBuyerIDDisplayType()
        {
            GridHyperlinkDecorator hyperlink = new GridHyperlinkDecorator();
            hyperlink.LinkClicked += new GridHyperlinkClickEventHandler(OnLinkClicked);

            Decorate(hyperlink);
        }

        /// <summary>
        /// Launches the browser to the eBay feedback page for the buyer
        /// </summary>
        void OnLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            EbayOrderEntity ebayOrder = e.Row.Entity as EbayOrderEntity;
            if (ebayOrder != null)
            {
                WebHelper.OpenUrl(EbayUrlUtilities.GetFeedbackUrl(ebayOrder.EbayBuyerID), e.Row.Grid.SandGrid.TopLevelControl);
            }
        }
    }
}
