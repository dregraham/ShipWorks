using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Administration;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Management;

namespace ShipWorks.UI.Dialogs
{
    /// <summary>
    /// View model for the New User Experience window
    /// </summary>
    [Component]
    public class NewUserExperienceViewModel : INewUserExperienceViewModel, INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        private readonly IAddStoreWizard addStoreWizard;
        private readonly IShipmentTypeSetupWizardFactory shipmentSetupWizardFactory;
        private IWin32Window owner;
        private NewUserExperienceSection selectedSection;
        private Action onClose;

        /// <summary>
        /// A property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public NewUserExperienceViewModel(IAddStoreWizard addStoreWizard, IShipmentTypeSetupWizardFactory shipmentSetupWizardFactory)
        {
            this.addStoreWizard = addStoreWizard;
            this.shipmentSetupWizardFactory = shipmentSetupWizardFactory;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            AddStore = new RelayCommand(AddStoreAction);
            SelectShippingSetup = new RelayCommand(SelectShippingSetupAction);
            ShowShippingSetupWizard = new RelayCommand<ShipmentTypeCode>(OpenShippingSetupWizardAction);
            UseShipWorks = new RelayCommand(UseShipWorksAction);

            SelectedSection = NewUserExperienceSection.AddStore;
        }

        /// <summary>
        /// Open a shipping setup wizard
        /// </summary>
        public ICommand ShowShippingSetupWizard { get; }

        /// <summary>
        /// Add a store
        /// </summary>
        public ICommand AddStore { get; }

        /// <summary>
        /// Customer wants to setup a shipping account
        /// </summary>
        public ICommand SelectShippingSetup { get; }

        /// <summary>
        /// Start using ShipWorks
        /// </summary>
        public ICommand UseShipWorks { get; }

        /// <summary>
        /// Selection section of the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public NewUserExperienceSection SelectedSection
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
            SelectedSection = NewUserExperienceSection.AddStore;

            addStoreWizard.RunWizard(owner);
        }

        /// <summary>
        /// Select the shipping setup section
        /// </summary>
        private void SelectShippingSetupAction()
        {
            SelectedSection = NewUserExperienceSection.AddShippingAccount;
        }

        /// <summary>
        /// Start using ShipWorks
        /// </summary>
        private void UseShipWorksAction()
        {
            SelectedSection = NewUserExperienceSection.UseShipWorks;
            onClose();
        }

        /// <summary>
        /// Open a shipping setup wizard
        /// </summary>
        private void OpenShippingSetupWizardAction(ShipmentTypeCode shipmentType)
        {
            IShipmentTypeSetupWizard wizard = shipmentSetupWizardFactory.Create(shipmentType);
            wizard.ShowDialog(owner);
        }
    }
}
