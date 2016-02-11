using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Stores;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// Properties of the ChannelLimitViewModel class
    /// </summary>
    public partial class ChannelLimitViewModel
    {
        /// <summary>
        /// Used to indicate if we are deleting a store
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsDeleting
        {
            get { return isDeleting; }
            set { handler.Set(nameof(IsDeleting), ref isDeleting, value); }
        }

        /// <summary>
        /// The error message displayed to the user
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { handler.Set(nameof(ErrorMessage), ref errorMessage, value); }
        }

        /// <summary>
        /// The selected store
        /// </summary>
        [Obfuscation(Exclude = true)]
        public StoreTypeCode SelectedStoreType
        {
            get { return selectedStoreType; }
            set { handler.Set(nameof(SelectedStoreType), ref selectedStoreType, value); }
        }

        /// <summary>
        /// Delete Store ClickCommand
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand DeleteStoreClickCommand { get; }

        /// <summary>
        /// Upgrade Account ClickCommand
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand UpgradeClickCommand
        {
            get { return new RelayCommand(UpgradeAccount); }
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand<Window> CloseClickCommand
        {
            get { return new RelayCommand<Window>(w => w.Close()); }
        }

        /// <summary>
        /// Collection of stores
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<StoreTypeCode> ChannelCollection
        {
            get { return channelCollection; }
            set { handler.Set(nameof(ChannelCollection), ref channelCollection, value); }
        }
    }
}
