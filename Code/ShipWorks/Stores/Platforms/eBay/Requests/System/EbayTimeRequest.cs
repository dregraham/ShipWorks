using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Requests;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.System
{
    /// <summary>
    /// An implmentation of the ITimeRequest interface that fetches the 
    /// time from the eBay service.
    /// </summary>
    public class EbayTimeRequest : EbayRequest, ITimeRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EbayTimeRequest"/> class.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        public EbayTimeRequest(TokenData tokenData)
            : base(tokenData, "GeteBayOfficialTime")
        { }

        /// <summary>
        /// Gets the server time in UTC.
        /// </summary>
        /// <returns>A DateTime object in UTC.</returns>
        public DateTime GetServerTimeInUtc()
        {
            GeteBayOfficialTimeResponseType response = SubmitRequest() as GeteBayOfficialTimeResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain the time from eBay.");
            }

            return response.Timestamp.ToUniversalTime();
        }

        /// <summary>
        /// Gets the name of the call as it is known to eBay. This value gets used
        /// as a query string parameter sent to eBay.
        /// </summary>
        /// <returns>The name of the call for this request - "GeteBayOfficialTime"</returns>
        public override string GetEbayCallName()
        {
            return "GeteBayOfficialTime";
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        /// <returns>An GeteBayOfficialTimeRequestType object.</returns>
        public override WebServices.AbstractRequestType GetEbayRequest()
        {
            return new GeteBayOfficialTimeRequestType();
        }
    }
}
