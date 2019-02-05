using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

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
        GenericResult<T> Submit<T>(string endpoint, IStoreEntity store, Dictionary<string, string> postData);
    }
}