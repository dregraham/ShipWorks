using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Interface for a GetAmazonCarrierCredentialsViewModel
    /// </summary>
    public interface IGetAmazonCarrierCredentialsViewModel
    {
        /// <summary>
        /// Load the store
        /// </summary>
        void Load(StoreEntity store);

        /// <summary>
        /// Does Carrier Exist?
        /// </summary>
        bool CarrierExists { get; set; }

        /// <summary>
        /// Trigger process to update carrier credentials
        /// </summary>
        ICommand UpdateCredentialsCommand { get; }

        /// <summary>
        /// Amazon Regions
        /// </summary>
        Dictionary<string, string> Regions { get; }

        /// <summary>
        /// The selected Region
        /// </summary>
        string SelectedRegion { get; set; }

        /// <summary>
        /// Is a URL Loading?
        /// </summary>
        bool LoadingUrl { get; set; }

        /// <summary>
        /// Trigger process to create a carrier
        /// </summary>
        ICommand CreateCredentialsCommand { get; }

        /// <summary>
        /// The token retrieved from the CreateCredentials process 
        /// </summary>
        string CredentialsToken { get; }

        /// <summary>
        /// Save carrier (if required)
        /// </summary>
        ICommand SaveCommand { get; }

        /// <summary>
        /// Cancel the process (not applicabile when updating credentials)
        /// </summary>
        ICommand CancelCommand { get; }
    }
}
