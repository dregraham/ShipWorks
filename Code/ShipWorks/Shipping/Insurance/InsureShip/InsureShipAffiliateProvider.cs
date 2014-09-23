using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Manages lists of InsureShip affiliates by ShipWorks StoreID
    /// </summary>
    public class InsureShipAffiliateProvider
    {
        private readonly ConcurrentDictionary<long, InsureShipAffiliate> insureShipAffiliates = new ConcurrentDictionary<long, InsureShipAffiliate>();

        /// <summary>
        /// Adds or updates the InsureshipAffiliate to the list for the specified store.
        /// </summary>
        public void Add(long shipWorksStoreID, InsureShipAffiliate insureShipAffiliate)
        {
            insureShipAffiliates.AddOrUpdate(shipWorksStoreID, insureShipAffiliate, (key, value) => insureShipAffiliate);
        }

        /// <summary>
        /// Returns an InsureShipAffiliate for the specified store ID.
        /// If one does not exist, null is returned.
        /// </summary>
        public InsureShipAffiliate GetInsureShipAffiliate(long shipWorksStoreID)
        {
            if (!insureShipAffiliates.ContainsKey(shipWorksStoreID))
            {
                return null;
            }

            return insureShipAffiliates[shipWorksStoreID];
        }
    }
}
