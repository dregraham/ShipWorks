using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;

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
        public CustomerLicenseActivationDlgViewModel(ICustomerLicenseActivationViewModel customerLicenseActivationViewModel, IMessageHelper messageHelper)
        {
            this.licenseActivationViewModel = customerLicenseActivationViewModel;
            this.messageHelper = messageHelper;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The Licenses Activation View Model
        /// </summary>
        public ICustomerLicenseActivationViewModel LicenseActivationViewModel
        {
            get { return licenseActivationViewModel; }
            set { handler.Set(nameof(LicenseActivationViewModel), ref licenseActivationViewModel, value); }
        }

        /// <summary>
        /// Command for clicking OK
        /// </summary>
        public RelayCommand<CustomerLicenseActivationDlg> OkClickCommand
        {
            get
            {
                return new RelayCommand<CustomerLicenseActivationDlg>(ActivateShipWorks);
            }
        }

        /// <summary>
        /// Activates ShipWorks using the LicenseActivationViewModel
        /// </summary>
        public void ActivateShipWorks(CustomerLicenseActivationDlg window)
        {
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