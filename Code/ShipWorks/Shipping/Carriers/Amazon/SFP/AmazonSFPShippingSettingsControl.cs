using System.ComponentModel;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Amazon shipping store settings
    /// </summary>
    [ToolboxItem(true)]
    public partial class AmazonSFPShippingSettingsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPShippingSettingsControl()
        {
            InitializeComponent();

            countries.Items.Add("Germany");
            countries.Items.Add("United Kingdom");
            countries.Items.Add("United States");

            countries.SelectedItem = "United States";
        }

        /// <summary>
        /// Populate the UI with values from the store entity
        /// </summary>
        public void LoadStore(IAmazonCredentials amazonCredentials)
        {
            MethodConditions.EnsureArgumentIsNotNull(amazonCredentials, nameof(amazonCredentials));

            countries.SelectedItem = Geography.GetCountryName(amazonCredentials.Region);
            merchantID.Text = amazonCredentials.MerchantID;
            authToken.Text = amazonCredentials.AuthToken;
        }

        /// <summary>
        /// Save the UI settings to the store entity
        /// </summary>
        public void SaveToEntity(IAmazonCredentials amazonCredentials)
        {
            MethodConditions.EnsureArgumentIsNotNull(amazonCredentials, nameof(amazonCredentials));

            amazonCredentials.Region = Geography.GetCountryCode((string) countries.SelectedItem);
            amazonCredentials.MerchantID = merchantID.Text;
            amazonCredentials.AuthToken = authToken.Text;

            // If the credentials are not blank we test them to ensure they are correct
            if (!string.IsNullOrWhiteSpace(merchantID.Text) && !string.IsNullOrWhiteSpace(authToken.Text))
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    if (lifetimeScope.IsRegistered<IAmazonAccountValidator>())
                    {
                        // Account validator is available so we validate
                        IAmazonAccountValidator validator = lifetimeScope.Resolve<IAmazonAccountValidator>();
                        if (!validator.ValidateAccount(amazonCredentials))
                        {
                            throw new AmazonSFPShippingException("Invalid Amazon credentials.");
                        }
                    }
                }
            }
        }
    }
}