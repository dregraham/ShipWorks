using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac;
using GalaSoft.MvvmLight.Command;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// View model for the shipping account required for rating footnote
    /// </summary>
    public class ShippingAccountRequiredForRatingFootnoteViewModel : IShippingAccountRequiredForRatingFootnoteViewModel
    {
        private readonly IWin32Window owner;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingAccountRequiredForRatingFootnoteViewModel(IWin32Window owner) 
        {
            this.owner = owner;
            ViewShippingSettings = new RelayCommand(ViewShippingSettingsAction);
        }

        /// <summary>
        /// Command to show the shipping settings
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ViewShippingSettings { get; }

        /// <summary>
        /// View the shipping settings dialog
        /// </summary>
        private void ViewShippingSettingsAction()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                using (ShippingSettingsDlg shippingSettingsDlg = new ShippingSettingsDlg(lifetimeScope))
                {
                    shippingSettingsDlg.ShowDialog(owner);
                }
            }
        }
    }
}
