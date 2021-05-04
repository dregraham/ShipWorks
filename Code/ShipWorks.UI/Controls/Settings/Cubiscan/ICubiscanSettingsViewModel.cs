using System.Collections.ObjectModel;
using System.Windows.Input;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.UI.Controls.Settings.Cubiscan
{
    /// <summary>
    /// ViewModel for a CubiscanSettings View
    /// </summary>
    public interface ICubiscanSettingsViewModel
    {
        /// <summary>
        /// List of configured devices
        /// </summary>
        ObservableCollection<DeviceEntity> Devices { get; set; }

        /// <summary>
        /// Currently selected ShippingProfile
        /// </summary>
        DeviceEntity SelectedDevice { get; set; }

        /// <summary>
        /// Command to add a new device
        /// </summary>
        ICommand AddCommand { get; }

        /// <summary>
        /// Command to delete a device
        /// </summary>
        ICommand DeleteCommand { get; }

        /// <summary>
        /// Load the viewmodel
        /// </summary>
        void Load();
    }
}