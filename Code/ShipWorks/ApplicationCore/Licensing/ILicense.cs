using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System.Web.UI.WebControls;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// License describes the capabilities of the customer's license.
    /// </summary>
    public interface ILicense
    {
        /// <summary>
        /// Refresh the License capabilities from Tango
        /// </summary>
        void Refresh();

        /// <summary>
        /// Reason the license is Disabled
        /// </summary>
        string DisabledReason { get; set; }

        /// <summary>
        /// Is the license Disabled
        /// </summary>
        bool IsDisabled { get; }

        /// <summary>
        /// The license key
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Is the license legacy
        /// </summary>
        bool IsLegacy { get; }

        /// <summary>
        /// Is the user over their channel limit
        /// </summary>
        bool IsOverChannelLimit { get; }

        /// <summary>
        /// The number of licenses needed to be deleted to be in compliance
        /// </summary>
        int NumberOfChannelsOverLimit { get; }

        /// <summary>
        /// Is the user over their shipment limit
        /// </summary>
        bool IsShipmentLimitReached { get; }

        /// <summary>
        /// Activate a new store
        /// </summary>
        EnumResult<LicenseActivationState> Activate(StoreEntity store);

        /// <summary>
        /// If License is over the channel limit prompt user to delete channels
        /// </summary>
        void EnforceChannelLimit();

        /// <summary>
        /// If license is at shipment limit, prompt user to upgrade
        /// when attempting to process a shipment
        /// </summary>
        void EnforceShipmentLimit();

        /// <summary>
        /// Deletes a store
        /// </summary>
        /// <param name="store"></param>
        void DeleteStore(StoreEntity store);
    }
}
