using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.SFP;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    /// <summary>
    /// A dialog responsible for creating and updating Amazon Carrier information
    /// </summary>
    [Component(RegisterAs = RegistrationType.SpecificService, Service = typeof(IGetAmazonCarrierCredentialsDialog))]
    public partial class GetAmazonCarrierCredentialsDialog : IGetAmazonCarrierCredentialsDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GetAmazonCarrierCredentialsDialog(IWin32Window owner, IGetAmazonCarrierCredentialsViewModel viewModel) : 
            base(owner, viewModel, false)
        {
            InitializeComponent();
        }
    }
}