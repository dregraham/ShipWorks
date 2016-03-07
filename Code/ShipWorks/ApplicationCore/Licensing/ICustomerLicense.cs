using ShipWorks.Stores;
using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for customer license
    /// </summary>
    public interface ICustomerLicense : ILicense
    {
        /// <summary>
        /// Gets or sets the stamps username to create when creating a new stamps account.
        /// </summary>
        string StampsUsername { get; set; }

        /// <summary>
        /// Gets or sets the user name of the SDC account associated with this license.
        /// </summary>
        string AssociatedStampsUsername { get; set; }

        /// <summary>
        /// Saves the license
        /// </summary>
        void Save();

        /// <summary>
        /// IEnumerable of ActiveStores for the license
        /// </summary>
        IEnumerable<ActiveStore> GetActiveStores();

        /// <summary>
        /// Deletes the given channel
        /// </summary>
        void DeleteChannel(StoreTypeCode storeType);

        /// <summary>
        /// Checks the restriction for a specific feature
        /// </summary>
        EditionRestrictionLevel CheckRestriction(EditionFeature feature, object data);

        /// <summary>
        /// Handles the restriction for a specific feature
        /// </summary>
        bool HandleRestriction(EditionFeature feature, object data, IWin32Window owner);
    }
}