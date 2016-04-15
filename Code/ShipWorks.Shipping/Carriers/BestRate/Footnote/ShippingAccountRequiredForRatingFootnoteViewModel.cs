using System;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac.Features.OwnedInstances;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// View model for the shipping account required for rating footnote
    /// </summary>
    public class ShippingAccountRequiredForRatingFootnoteViewModel : IShippingAccountRequiredForRatingFootnoteViewModel
    {
        private readonly Func<Owned<ShippingSettingsDlg>> createSettingsDialog;
        private readonly IWin32Window owner;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingAccountRequiredForRatingFootnoteViewModel(IWin32Window owner, Func<Owned<ShippingSettingsDlg>> createSettingsDialog)
        {
            this.createSettingsDialog = createSettingsDialog;
            this.owner = owner;
            ViewShippingSettings = new RelayCommand(ViewShippingSettingsAction);
        }

        /// <summary>
        /// Command to show the shipping settings
        /// </summary>
        public ICommand ViewShippingSettings { get; }

        /// <summary>
        /// View the shipping settings dialog
        /// </summary>
        private void ViewShippingSettingsAction()
        {
            using (ShippingSettingsDlg settingsDialog = createSettingsDialog().Value)
            {
                settingsDialog.ShowDialog(owner);
            }
        }
    }
}
