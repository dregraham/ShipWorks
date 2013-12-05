using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Tokens;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// Wraps the GeteBayOfficialTime request
    /// </summary>
    public class EbayOfficialTimeRequest : EbayRequest<DateTime, GeteBayOfficialTimeRequestType, GeteBayOfficialTimeResponseType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EbayOfficialTimeRequest"/> class.
        /// </summary>
        /// <param name="token">The token data.</param>
        public EbayOfficialTimeRequest(EbayToken token)
            : base(token, "GeteBayOfficialTime")
        { 
        
        }

        /// <summary>
        /// Gets the server time in UTC.
        /// </summary>
        public override DateTime Execute()
        {
            GeteBayOfficialTimeResponseType response = SubmitRequest();

            return response.Timestamp.ToUniversalTime();
        }
    }
}
