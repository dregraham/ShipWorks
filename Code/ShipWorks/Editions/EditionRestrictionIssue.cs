using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;
using Interapptive.Shared.Utility;

namespace ShipWorks.Editions
{
    /// <summary>
    /// Represents an issue that occurred with an EditionRestriction
    /// </summary>
    public class EditionRestrictionIssue : EditionRestriction
    {
        object conflictingData;

        // Singleton special-case "non-issue"
        static EditionRestrictionIssue noneIssue = new EditionRestrictionIssue();

        /// <summary>
        /// Special constructor for creating singleton "None" issue and unit tests
        /// </summary>
        public EditionRestrictionIssue()
            : base(null, EditionFeature.None, EditionRestrictionLevel.None)
        {
            conflictingData = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EditionRestrictionIssue(EditionRestriction restriction, object conflictingData)
            : base(restriction.Edition, restriction.Feature, restriction.Data, restriction.Level)
        {
            this.conflictingData = conflictingData;
        }

        /// <summary>
        /// Special case that represents no issue (everything is fine)
        /// </summary>
        public static EditionRestrictionIssue None
        {
            get { return noneIssue; }
        }

        /// <summary>
        /// Get the user-displayable description of why the restriction is in place
        /// </summary>
        public string GetDescription()
        {
            object requiredData = Data;

            if (Feature == EditionFeature.ShipmentType)
            {
                requiredData = ShipmentTypeManager.GetType((ShipmentTypeCode) Data).ShipmentTypeName;
            }

            // You can optionall separte "RequiresUpgrade" and "Forbidden" descriptions with a Pipe
            string[] descriptions = EnumHelper.GetDescription(Feature).Split('|');

            // Default to the first (and usually the only)
            string description = descriptions[0];

            // If its Forbidden, and there's a second, use the second
            if (Level == EditionRestrictionLevel.Forbidden && descriptions.Length > 1)
            {
                description = descriptions[1];
            }

            return string.Format(description, requiredData, conflictingData);
        }
    }
}
