using System.ComponentModel;
using System.Windows.Input;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
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
        /// Saves the specified store.
        /// </summary>
        bool Save(ChannelAdvisorStoreEntity store);
    }
}