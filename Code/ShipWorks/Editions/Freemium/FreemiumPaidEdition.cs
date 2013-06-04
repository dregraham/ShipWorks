using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Editions.Freemium
{
    /// <summary>
    /// The paid upgraded freemium version
    /// </summary>
    public class FreemiumPaidEdition : Edition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FreemiumPaidEdition(StoreEntity store)
            : base(store)
        {
            AddRestriction(EditionFeature.ShipmentType, ShipmentTypeCode.Stamps, EditionRestrictionLevel.Hidden);
        }
    }
}
