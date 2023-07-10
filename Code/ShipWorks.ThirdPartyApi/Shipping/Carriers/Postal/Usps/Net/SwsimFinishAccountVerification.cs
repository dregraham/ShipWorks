using System;
using System.Net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Net
{
    [Component]
    public class SwsimFinishAccountVerification: SwsimV135, ISwimFinishAccountVerification
    {
        private readonly string smsVerificationPhoneNumber;
        private const string Salt = "StampsVCode";

        /// <summary>
        /// Constructor
        /// </summary>
        public SwsimFinishAccountVerification(IApiLogEntry verificationPhoneNumber, string smsVerificationPhoneNumber)
        {
            this.smsVerificationPhoneNumber = smsVerificationPhoneNumber;
        }
        
        /// <summary>
        /// Finish Account Verification - only use if SMS verified account (not legacy) - also make sure to check the cert before
        /// </summary>
        public void FinishAccountVerification(Credentials credentials)
        {
            FinishAccountVerification(credentials, SecureText.Decrypt("WAkHl82fviriKU0aikqfFoj7UvCFtphC", Salt));   
        }

        /// <summary>
        /// Overrides GetWebRequest to add the FinishAccountVerification phone number header
        /// </summary>
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest request = base.GetWebRequest(uri);          
            
            request.Headers.Add("X-SDC-FINISHACCOUNTVERIFICATION-PHONE", smsVerificationPhoneNumber);
            return request;
        }
    }
}