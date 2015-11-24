using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;
using Interapptive.Shared.Utility;

namespace ShipWorks.Editions
{
    /// <summary>
    /// Represents an edition restriction, and associated data (such as the count, for the filter count restriction) that goes with it.
    /// </summary>
    public class EditionRestriction
    {
        Edition edition;
        EditionFeature feature;
        EditionRestrictionLevel level;
        object data;

        /// <summary>
        /// Constructor
        /// </summary>
        public EditionRestriction(Edition edition, EditionFeature feature, EditionRestrictionLevel level) :
            this(edition, feature, null, level)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EditionRestriction(Edition edition, EditionFeature feature, object data, EditionRestrictionLevel level)
        {
            if (edition == null && feature != EditionFeature.None)
            {
                throw new ArgumentNullException("edition");
            }

            this.edition = edition;
            this.feature = feature;
            this.level = level;
            this.data = data;
        }

        /// <summary>
        /// The Edition that generated this restriction
        /// </summary>
        public Edition Edition
        {
            get { return edition; }
        }

        /// <summary>
        /// The feature being restricted
        /// </summary>
        public EditionFeature Feature
        {
            get { return feature; }
        }

        /// <summary>
        /// The level, if any, that this is restrcited
        /// </summary>
        public virtual EditionRestrictionLevel Level
        {
            get { return level; }
        }

        /// <summary>
        /// Data associated with the restriction, such as the count for the filter count restriction.
        /// </summary>
        public object Data
        {
            get { return data; }
        }
    }
}
