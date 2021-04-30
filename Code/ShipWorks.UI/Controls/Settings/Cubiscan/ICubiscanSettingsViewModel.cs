using System.Collections.ObjectModel;
using System.Windows.Input;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.UI.Controls.Settings.Cubiscan
{
    public interface ICubiscanSettingsViewModel
    {
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

        void Load();
    }
}