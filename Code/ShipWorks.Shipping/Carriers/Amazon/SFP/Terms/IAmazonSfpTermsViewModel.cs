using System.Threading.Tasks;
using ShipWorks.Shipping.Carriers.Amazon.SFP.DTO;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Terms
{
    /// <summary>
    /// Interface for showing the Amazon Terms UI
    /// </summary>
    public interface IAmazonSfpTermsViewModel
    {
        /// <summary>
        /// Show the terms UI
        /// </summary>
        Task Show(AmazonTermsVersion amazonTermsVersion);

        /// <summary>
        /// Have the terms been accepted
        /// </summary>
        bool TermsAccepted { get; set; }
    }
}
