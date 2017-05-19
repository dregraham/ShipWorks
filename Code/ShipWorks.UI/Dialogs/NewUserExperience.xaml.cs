using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Administration;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Management;

namespace ShipWorks.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for NewUserExperience.xaml
    /// </summary>
    [Component]
    public partial class NewUserExperience : Window, INewUserExperience
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NewUserExperience()
        {
            InitializeComponent();
            Loaded += NewUserExperience_Loaded;
        }

        private void NewUserExperience_Loaded(object sender, RoutedEventArgs e)
        {
            CarrierSetup.Visibility = Visibility.Collapsed;
        }

        private void ClickUspsButton(object sender, RoutedEventArgs e)
        {
            ShowShippingWizard(ShipmentTypeCode.Usps);
        }

        private void ClickFedExButton(object sender, RoutedEventArgs e)
        {
            ShowShippingWizard(ShipmentTypeCode.FedEx);
        }

        private void ClickUpsButton(object sender, RoutedEventArgs e)
        {
            ShowShippingWizard(ShipmentTypeCode.UpsOnLineTools);
        }

        private void ShowShippingWizard(ShipmentTypeCode shipmentType)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                var win32Window = new NativeWindow();
                win32Window.AssignHandle(new WindowInteropHelper(this).Handle);

                var wizard = lifetimeScope.Resolve<IShipmentTypeSetupWizardFactory>().Create(shipmentType);
                wizard.ShowDialog(win32Window);
            }
        }

        private void HideCarrierSetup(object sender, RoutedEventArgs e)
        {
            CarrierSetup.Visibility = Visibility.Collapsed;

            var win32Window = new NativeWindow();
            win32Window.AssignHandle(new WindowInteropHelper(this).Handle);

            AddStoreWizard.RunWizard(win32Window);
        }

        private void ShowCarrierSetup(object sender, RoutedEventArgs e)
        {
            CarrierSetup.Visibility = Visibility.Visible;
        }

        private void StartUsingShipWorks(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NewUserExperience(INewUserExperienceViewModel viewModel) : this()
        {
            NativeWindow win32Window = new NativeWindow();
            win32Window.AssignHandle(new WindowInteropHelper(this).Handle);
            viewModel.SetOwnerInteraction(win32Window, Close);
            DataContext = viewModel;
        }

        /// <summary>
        /// Set the owner of this window
        /// </summary>
        public void LoadOwner(System.Windows.Forms.IWin32Window owner)
        {
            WindowInteropHelper interopHelper = new WindowInteropHelper(this);
            interopHelper.Owner = owner.Handle;
        }
    }
}
