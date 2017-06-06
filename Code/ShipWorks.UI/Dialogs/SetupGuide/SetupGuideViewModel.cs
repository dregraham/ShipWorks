using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using ShipWorks.Core.UI;
using ShipWorks.Data.Administration;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Management;

namespace ShipWorks.UI.Dialogs.SetupGuide
{
    /// <summary>
    /// View model for the New User Experience window
    /// </summary>
    [Component]
    public class SetupGuideViewModel : ISetupGuideViewModel, INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        private readonly IAddStoreWizard addStoreWizard;
        private readonly IShipmentTypeSetupWizardFactory shipmentSetupWizardFactory;
        private IWin32Window owner;
        private SetupGuideSection selectedSection;
        private Action onClose;

        /// <summary>
        /// A property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public SetupGuideViewModel(IAddStoreWizard addStoreWizard, IShipmentTypeSetupWizardFactory shipmentSetupWizardFactory)
        {
            this.addStoreWizard = addStoreWizard;
            this.shipmentSetupWizardFactory = shipmentSetupWizardFactory;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            AddStore = new RelayCommand(AddStoreAction);
            SelectShippingSetup = new RelayCommand(SelectShippingSetupAction);
            DeselectShippingSetup = new RelayCommand(DeselectShippingSetupAction);
            ShowShippingSetupWizard = new RelayCommand<ShipmentTypeCode>(OpenShippingSetupWizardAction);
            UseShipWorks = new RelayCommand(UseShipWorksAction);

            SelectedSection = SetupGuideSection.AddStore;
        }

        /// <summary>
        /// Open a shipping setup wizard
        /// </summary>
        [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
        public ICommand ShowShippingSetupWizard { get; }

        /// <summary>
        /// Add a store
        /// </summary>
        [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
        public ICommand AddStore { get; }

        /// <summary>
        /// Customer wants to setup a shipping account
        /// </summary>
        [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
        public ICommand SelectShippingSetup { get; }

        /// <summary>
        /// Customer wants to close shipping account setup
        /// </summary>
        [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
        public ICommand DeselectShippingSetup { get; }

        /// <summary>
        /// Start using ShipWorks
        /// </summary>
        [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
        public ICommand UseShipWorks { get; }

        /// <summary>
        /// Selection section of the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public SetupGuideSection SelectedSection
        {
            get { return selectedSection; }
            set { handler.Set(nameof(SelectedSection), ref selectedSection, value); }
        }

        /// <summary>
        /// Set owner interaction hooks
        /// </summary>
        public void SetOwnerInteraction(IWin32Window win32Window, Action onClose)
        {
            owner = win32Window;
            this.onClose = onClose;
        }

        /// <summary>
        /// Add a store
        /// </summary>
        private void AddStoreAction()
        {
            SelectedSection = SetupGuideSection.AddStore;

            addStoreWizard.RunWizard(owner, OpenedFromSource.QuickStart);
        }

        /// <summary>
        /// Select the shipping setup section
        /// </summary>
        private void SelectShippingSetupAction()
        {
            SelectedSection = SetupGuideSection.AddShippingAccount;
        }

        /// <summary>
        /// De-select the shipping setup section
        /// </summary>
        private void DeselectShippingSetupAction()
        {
            SelectedSection = SetupGuideSection.AddStore;
        }

        /// <summary>
        /// Start using ShipWorks
        /// </summary>
        private void UseShipWorksAction()
        {
            SelectedSection = SetupGuideSection.UseShipWorks;
            onClose();
        }

        /// <summary>
        /// Open a shipping setup wizard
        /// </summary>
        private void OpenShippingSetupWizardAction(ShipmentTypeCode shipmentType)
        {
            IShipmentTypeSetupWizard wizard = shipmentSetupWizardFactory.Create(shipmentType, OpenedFromSource.QuickStart);

            if (wizard.ShowDialog(owner) == DialogResult.OK)
            {
                SelectedSection = SetupGuideSection.AddStore;
            }
        }
    }
}
