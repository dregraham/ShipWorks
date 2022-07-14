using System.ComponentModel;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Amazon shipping store settings
    /// </summary>
    [ToolboxItem(true)]
    public partial class AmazonSFPShippingSettingsControl : UserControl
    {
        private StoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPShippingSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Populate the UI with values from the store entity
        /// </summary>
        public void LoadStore(IAmazonCredentials amazonCredentials, StoreEntity storeEntity)
        {
            MethodConditions.EnsureArgumentIsNotNull(amazonCredentials, nameof(amazonCredentials));
            MethodConditions.EnsureArgumentIsNotNull(storeEntity, nameof(storeEntity));

            store = storeEntity;

            SetButtonText();
        }

        /// <summary>
        /// Save the UI settings to the store entity
        /// </summary>
        public void SaveToEntity(IAmazonCredentials amazonCredentials)
        {
            MethodConditions.EnsureArgumentIsNotNull(amazonCredentials, nameof(amazonCredentials));
        }

        /// <summary>
        /// On click of the Credentials button to launch the dialog
        /// </summary>
        private void CredentialsButton_Click(object sender, System.EventArgs e)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            using (var scope = IoC.BeginLifetimeScope())
            {
                var viewModel = scope.Resolve<IGetAmazonCarrierCredentialsViewModel>();
                var dialog = scope.Resolve<IGetAmazonCarrierCredentialsDialog>(TypedParameter.From(viewModel));
                viewModel.Load(store);
                viewModel.OnComplete = () =>
                {
                    SetButtonText();
                    dialog.Close();
                };
                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Helper method to provide the UI text values
        /// </summary>
        private void SetButtonText()
        {
            CredentialsButton.Text = store.PlatformAmazonCarrierID.IsNullOrWhiteSpace()
                ? "Create Amazon Token"
                : "Update Credentials";
        }
    }
}