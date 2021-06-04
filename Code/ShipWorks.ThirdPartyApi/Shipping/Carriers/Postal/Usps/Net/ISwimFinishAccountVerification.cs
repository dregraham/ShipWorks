using System;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Net
{
    /// <summary>
    /// Interface for calling the SWSIM webservice FinishAccountVerification
    /// </summary>
    public interface ISwimFinishAccountVerification : IDisposable
    {
        /// <summary>
        /// Finish Account Verification - only use if SMS verified account (not legacy) - also make sure to check the cert before
        /// </summary>
        void FinishAccountVerification(Credentials credentials);
        
        /// <summary>
        /// Url of the web service
        /// </summary>
        string Url { get; set; }
    }
}