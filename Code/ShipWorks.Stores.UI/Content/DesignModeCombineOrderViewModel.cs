using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.UI.Content
{
    /// <summary>
    /// View model to aid in developing the combine orders window
    /// </summary>
    internal class DesignModeCombineOrderViewModel : ICombineOrdersViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignModeCombineOrderViewModel()
        {
            Orders = new List<IOrderEntity>
            {
                new OrderEntity
                {
                    OrderID = 123,
                    OrderNumber = 72278,
                    RollupItemCount = 3
                },
                new OrderEntity
                {
                    OrderID = 456,
                    OrderNumber = 72279,
                    RollupItemCount = 2
                },
                new OrderEntity
                {
                    OrderID = 789,
                    OrderNumber = 72280,
                    RollupItemCount = 1
                }
            };

            NewOrderNumber = "72278-C";
            SurvivingOrder = Orders.First();
            AddressName = "Jim Jefferson";
            AddressStreet = "123 Example St.";
            AddressCityStateZip = "Somewhere, AL 77777";
        }

        /// <summary>
        /// New order number
        /// </summary>
        public string NewOrderNumber { get; set; }

        /// <summary>
        /// Surviving order id
        /// </summary>
        public IOrderEntity SurvivingOrder { get; set; }

        /// <summary>
        /// Name of the recipient of the currently selected order
        /// </summary>
        public string AddressName { get; set; }

        /// <summary>
        /// Street of the recipient of the currently selected order
        /// </summary>
        public string AddressStreet { get; set; }

        /// <summary>
        /// City, state, and zip of the currently selected order
        /// </summary>
        public string AddressCityStateZip { get; set; }

        /// <summary>
        /// Orders that will be combined
        /// </summary>
        public IEnumerable<IOrderEntity> Orders { get; set; }

        /// <summary>
        /// Details for combining orders
        /// </summary>
        public Tuple<long, string> Details { get; set; }
    }
}