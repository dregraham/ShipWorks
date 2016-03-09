using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Editions;
using ShipWorks.Shipping;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public abstract class FeatureRestriction : IFeatureRestriction
    {
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        protected FeatureRestriction(IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        public abstract EditionFeature EditionFeature { get; }

        /// <summary>
        /// Checks the restriction for this feature
        /// </summary>
        public abstract EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data);

        /// <summary>
        /// Nothing to handle, return false
        /// </summary>
        public virtual bool Handle(IWin32Window owner, ILicenseCapabilities capabilities, object data)
        {
            EditionRestrictionLevel level = Check(capabilities, data);
            
            if (EditionFeature == EditionFeature.ShipmentType)
            {
                string carrier = ShipmentTypeManager.GetType((ShipmentTypeCode)data).ShipmentTypeName;

                // You can optional separate "RequiresUpgrade" and "Forbidden" descriptions with a Pipe
                string[] descriptions = EnumHelper.GetDescription(EditionFeature).Split('|');

                // Default to the first (and usually the only)
                string description = descriptions[0];

                // If its Forbidden, and there's a second, use the second
                if (level == EditionRestrictionLevel.Forbidden && descriptions.Length > 1)
                {
                    description = descriptions[1];
                }

                messageHelper.ShowError(string.Format(description, carrier));
            }
            
            return level == EditionRestrictionLevel.None;
        }
    }
}