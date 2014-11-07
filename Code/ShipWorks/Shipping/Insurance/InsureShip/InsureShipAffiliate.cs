using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Provides ability to determine the affiliate needed for InsureShip
    /// </summary>
    public class InsureShipAffiliate
    {
        private readonly string insureShipStoreID;
        private readonly string insureShipPolicyID;

        /// <summary>
        /// Constructor that populates the properties.
        /// </summary>
        public InsureShipAffiliate(string tangoStoreID, string tangoCustomerID)
        {
            // The Tango store ID is the same as the InsureShip store id
            insureShipStoreID = tangoStoreID;
            insureShipPolicyID = string.Format("SW{0}", tangoCustomerID);
        }

        /// <summary>
        /// The storeID from Tango
        /// </summary>
        public string InsureShipStoreID
        {
            get
            {
                return insureShipStoreID;
            }
        }

        /// <summary>
        /// The 
        /// </summary>
        public string InsureShipPolicyID
        {
            get
            {
                return insureShipPolicyID;
            }
        }
    }
}
