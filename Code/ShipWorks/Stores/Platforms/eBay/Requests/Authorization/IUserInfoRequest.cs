using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Requests;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Authorization
{
    /// <summary>
    /// An interface for issuing requests for obtaining user information.
    /// </summary>
    public interface IUserInfoRequest
    {
        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <returns>A GetUserResponseType object.</returns>
        GetUserResponseType GetUserInfo();
    }
}
