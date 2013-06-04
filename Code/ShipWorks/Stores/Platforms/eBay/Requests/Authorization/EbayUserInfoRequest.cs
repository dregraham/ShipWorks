using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Requests;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Authorization
{
    /// <summary>
    /// An implementation of IUserInfoRequest that hits the eBay API to obtain user information (user ID, address, etc.).
    /// </summary>
    public class EbayUserInfoRequest : EbayRequest, IUserInfoRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EbayUserInfoRequest"/> class.
        /// </summary>
        public EbayUserInfoRequest(TokenData tokenData)
            : base(tokenData, "Getting eBay user information")
        { }

        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>A GetUserResponseType object.</returns>
        public GetUserResponseType GetUserInfo()
        {
            GetUserResponseType response = SubmitRequest() as GetUserResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain user information from eBay.");
            }

            return response;
        }

        /// <summary>
        /// Gets the name of the call as it is known to eBay. This value gets used
        /// as a query string parameter sent to eBay.
        /// </summary>
        /// <returns></returns>
        public override string GetEbayCallName()
        {
            return "GetUser";
        }

        /// <summary>
        /// Gets the type of the eBay request.
        /// </summary>
        /// <returns></returns>
        public override AbstractRequestType GetEbayRequest()
        {
            // Create the request; note: with the 783 version of the API, the response will return a warning indicating
            // that the REST API is being deprecated when setting the DetailLevel. According to an eBay/PayPal support
            // forum post (https://www.x.com/developers/ebay/forums/ebay-apis-search/getuser-response-always-contains-warning-about-deprecated-resttoken-errcode-21916733)
            // this can be ignored. Setting the Detail level to ReturnAll provides the registration address info
            return new GetUserRequestType()
            {
                DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll }
            };
        }
    }
}
