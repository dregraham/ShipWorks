using System.Collections.Generic;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Simple web client for InsureShip
    /// </summary>
    public interface IInsureShipWebClient
    {
        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        GenericResult<T> Submit<T>(string endpoint, Dictionary<string, string> postData);
    }
}