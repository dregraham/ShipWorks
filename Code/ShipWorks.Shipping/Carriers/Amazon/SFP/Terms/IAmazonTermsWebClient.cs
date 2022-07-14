using System;
using System.Threading.Tasks;
using ShipWorks.Shipping.Carriers.Amazon.SFP.DTO;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Terms
{
    /// <summary>
    /// Interface for communicating with the Hub for Amazon Terms 
    /// </summary>
    public interface IAmazonTermsWebClient
    {
        /// <summary>
        /// Make a call to get the latest Amazon terms
        /// </summary>
        Task<AmazonTermsVersion> GetTerms();

        /// <summary>
        /// Make a call to accept Amazon terms
        /// </summary>
        Task<bool> AcceptTerms(Version version);
    }
}
