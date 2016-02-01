using System.ComponentModel;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;

namespace ShipWorks.UI.Controls.CustomerLicenseActivation
{
    /// <summary>
    /// View model for the CustomerLicenseActivationDlg
    /// </summary>
    public class CustomerLicenseActivartionDlgViewModel : INotifyPropertyChanged, ICustomerLicenseActivartionDlgViewModel
    {
        private readonly PropertyChangedHandler handler;
        private ICustomerLicenseActivationViewModel licenseActivationViewModel;
        private readonly IMessageHelper messageHelper;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseActivartionDlgViewModel(ICustomerLicenseActivationViewModel customerLicenseActivationViewModel, IMessageHelper messageHelper)
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
        public RelayCommand<Window> OkClickCommand
        {
            get
            {
                return new RelayCommand<Window>(ActivateShipWorks);
            }
        }

        /// <summary>
        /// Activates ShipWorks using the LicenseActivationViewModel
        /// </summary>
        public void ActivateShipWorks(Window window)
        {
            GenericResult<ICustomerLicense> result = licenseActivationViewModel.Save();

            if (!result.Success)
            {
                messageHelper.ShowError(result.Message);
                return;
            }
            window.Close();
        }
    }
}