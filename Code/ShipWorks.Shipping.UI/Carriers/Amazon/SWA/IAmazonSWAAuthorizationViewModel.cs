using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.UI.Amazon.SWA
{
    /// <summary>
    /// Represents the Amazon SWA Authorization ViewModel
    /// </summary>
    public interface IAmazonSWAAuthorizationViewModel
    {
        /// <summary>
        /// Connect to Amazon Shipping
        /// </summary>
        /// <returns></returns>
        GenericResult<string> ConnectToAmazonShipping();
    }
}