using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Tokens;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// An implementation of eBay's GetUser API that hits the eBay API to obtain user information (user ID, address, etc.).
    /// </summary>
    public class EbayGetUserRequest : EbayRequest<UserType, GetUserRequestType, GetUserResponseType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EbayGetUserRequest"/> class.
        /// </summary>
        public EbayGetUserRequest(EbayToken token)
            : base(token, "GetUser")
        { 
        
        }

        /// <summary>
        /// Gets the user info.
        /// </summary>
        public override UserType Execute()
        {
            GetUserResponseType response = SubmitRequest();

            return response.User;
        }

        /// <summary>
        /// Gets the type of the eBay request.
        /// </summary>
        /// <returns></returns>
        protected override AbstractRequestType CreateRequest()
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
