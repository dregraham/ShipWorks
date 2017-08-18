using System.ComponentModel;
using System.Windows.Input;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Interface for ChannelAdvisorAccountSettingsViewModel
    /// </summary>
    public interface IChannelAdvisorAccountSettingsViewModel
    {
        /// <summary>
        /// Gets or sets the access code.
        /// </summary>
        string AccessCode { get; set; }

        /// <summary>
        /// Gets the get access code command.
        /// </summary>
        ICommand GetAccessCodeCommand { get; }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Loads the specified store.
        /// </summary>
        void Load(ChannelAdvisorStoreEntity store);

        /// <summary>
        /// Saves the specified store.
        /// </summary>
        bool Save(ChannelAdvisorStoreEntity store, bool ignoreEmptyAccessCode);
    }
}