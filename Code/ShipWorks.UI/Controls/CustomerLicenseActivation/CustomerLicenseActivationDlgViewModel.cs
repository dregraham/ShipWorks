using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;
using Cursor = System.Windows.Forms.Cursor;
using Cursors = System.Windows.Forms.Cursors;

namespace ShipWorks.UI.Controls.CustomerLicenseActivation
{
    /// <summary>
    /// View model for the CustomerLicenseActivationDlg
    /// </summary>
    public class CustomerLicenseActivationDlgViewModel : INotifyPropertyChanged, ICustomerLicenseActivartionDlgViewModel
    {
        private readonly PropertyChangedHandler handler;
        private ICustomerLicenseActivationViewModel licenseActivationViewModel;
        private readonly IMessageHelper messageHelper;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseActivationDlgViewModel(
            ICustomerLicenseActivationViewModel customerLicenseActivationViewModel, IMessageHelper messageHelper)
        {
            this.licenseActivationViewModel = customerLicenseActivationViewModel;
            this.messageHelper = messageHelper;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            OkClickCommand = new RelayCommand<CustomerLicenseActivationDlg>(ActivateShipWorks);
        }

        /// <summary>
        /// The Licenses Activation View Model
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICustomerLicenseActivationViewModel LicenseActivationViewModel
        {
            get { return licenseActivationViewModel; }
            set { handler.Set(nameof(LicenseActivationViewModel), ref licenseActivationViewModel, value); }
        }

        /// <summary>
        /// Command for clicking OK
        /// </summary>        
        [Obfuscation(Exclude = true)]
        public ICommand OkClickCommand { get; }

        /// <summary>
        /// Activates ShipWorks using the LicenseActivationViewModel
        /// </summary>
        public void ActivateShipWorks(CustomerLicenseActivationDlg window)
        {
            Cursor.Current = Cursors.WaitCursor;
            GenericResult<ICustomerLicense> result = licenseActivationViewModel.Save(false);

            if (!result.Success)
            {
                messageHelper.ShowError(window, result.Message);
                return;
            }

            window.DialogResult = true;
            window.Close();
        }
    }
}