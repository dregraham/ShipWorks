using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Editions.Freemium
{
    /// <summary>
    /// Edition for ShipRush/Endicia users migrating to ShipWorks in September 2011 when Raef pulled the plug on Endicia
    /// </summary>
    public class ShipRushEndiciaEdition : Edition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipRushEndiciaEdition(StoreEntity store)
            : base(store)
        {
            AddRestriction(EditionFeature.ShipmentType, ShipmentTypeCode.Usps, EditionRestrictionLevel.Hidden);
            AddRestriction(EditionFeature.SingleStore, StoreTypeCode.Invalid, EditionRestrictionLevel.Forbidden);
        }
    }
}
