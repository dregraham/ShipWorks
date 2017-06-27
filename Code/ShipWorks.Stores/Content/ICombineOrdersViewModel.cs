using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// View Model for the combine orders dialog
    /// </summary>
    public interface ICombineOrdersViewModel
    {
        /// <summary>
        /// Order number to use for the new order
        /// </summary>
        string NewOrderNumber { get; set; }

        /// <summary>
        /// Order that will be used as the basis for the combined order
        /// </summary>
        IOrderEntity SurvivingOrder { get; set; }

        /// <summary>
        /// Name of the recipient of the currently selected order
        /// </summary>
        string AddressName { get; }

        /// <summary>
        /// Street of the recipient of the currently selected order
        /// </summary>
        string AddressStreet { get; }

        /// <summary>
        /// City, state, and zip of the currently selected order
        /// </summary>
        string AddressCityStateZip { get; }

        /// <summary>
        /// Orders that will be combined
        /// </summary>
        IEnumerable<IOrderEntity> Orders { get; }

        /// <summary>
        /// Details for combining orders
        /// </summary>
        Tuple<long, string> Details { get; }
    }
}
