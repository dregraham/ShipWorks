using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Authorization
{
    /// <summary>
    /// An inteface for making requests to obtain authorization via Tango
    /// </summary>
    public interface ITangoAuthorizationRequest
    {
        /// <summary>
        /// Authorizes the specified license.
        /// </summary>
        /// <returns>The response to the authorization request.</returns>
        XmlDocument Authorize();
    }
}
