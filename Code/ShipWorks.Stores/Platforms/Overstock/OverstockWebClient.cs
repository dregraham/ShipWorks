using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication.Throttling;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Interface for connecting to Overstock
    /// </summary>
    [Component]
    public class OverstockWebClient : IOverstockWebClient
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OverstockWebClient));
        private readonly List<HttpStatusCode> successHttpStatusCodes;

        private const string storeSettingMissingErrorMessage = "The Overstock {0} is missing or invalid.  Please enter your {0} by going to Manage > Stores > Your Store > Edit > Store Connection.  You will find instructions on how to find the {0} there.";

        /// <summary>
        /// Create an instance of the web client for connecting to the specified store
        /// </summary>
        /// <exception cref="OverstockException" />
        public OverstockWebClient()
        {
            successHttpStatusCodes = new List<HttpStatusCode>
            {
                HttpStatusCode.OK,
                HttpStatusCode.Created,
                HttpStatusCode.Accepted,
                HttpStatusCode.NoContent
            };
        }

        /// <summary>
        /// Progress reporter associated with the client
        /// </summary>
        /// <remarks>
        /// If this is null, the client cannot be canceled and progress will not be reported
        /// </remarks>
        public IProgressReporter ProgressReporter { get; set; }

        /// <summary>
        /// Validate API access data
        /// </summary>
        private static void ValidateApiAccessData(IOverstockStoreEntity store)
        {
            if (string.IsNullOrWhiteSpace(store?.Username))
            {
                throw new OverstockException(string.Format(storeSettingMissingErrorMessage, "Store API Username"));
            }

            if (string.IsNullOrWhiteSpace(store?.Password))
            {
                throw new OverstockException(string.Format(storeSettingMissingErrorMessage, "Store API Password"));
            }
        }

        /// <summary>
        /// Update the online status and details of the given shipment
        /// </summary>
        /// <param name="orderNumber">The order number of this shipment</param>
        /// <param name="orderAddressID">The Overstock order addressID for this shipment</param>
        /// <param name="trackingNumber">Tracking number for this shipment</param>
        /// <param name="shippingMethod">Carrier and service for this shipment</param>
        /// <param name="orderItems">The list of OverstockItem's in this shipment</param>
        /// <exception cref="OverstockException" />
        public Task UploadShipmentDetails(long overstockOrderId, ShipmentEntity shipment, IOverstockStoreEntity store)
        {
            ValidateApiAccessData(store);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Make the call to Overstock to get a list of orders matching criteria
        /// </summary>
        /// <param name="orderSearchCriteria">Filter by OverstockWebClientOrderSearchCriteria.</param>
        /// <returns>List of orders matching criteria, sorted by LastUpdate ascending </returns>
        public async Task<GenericResult<List<object>>> GetOrders(IOverstockStoreEntity store)
        {
            List<object> ordersToReturn = new List<object>();

            return ordersToReturn.OrderBy(o => o.ToString()).ToList();
        }

        /// <summary>
        /// Attempt to get an order count to test connecting to Overstock.  If any error, assume connection failed.
        /// </summary>
        /// <exception cref="OverstockException" />
        public async Task<bool> TestConnection(IOverstockStoreEntity store)
        {
            // See if we can successfully call GetOrderCount, if we throw, we can't connect or login
            try
            {
                await GetOrders(store).ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
