using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using RestSharp;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Overstock.DTO;

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
        private readonly IOverstockRestClientFactory restClientFactory;
        private const string storeSettingMissingErrorMessage = "The Overstock {0} is missing or invalid.  Please enter your {0} by going to Manage > Stores > Your Store > Edit > Store Connection.  You will find instructions on how to find the {0} there.";

        /// <summary>
        /// Create an instance of the web client for connecting to the specified store
        /// </summary>
        public OverstockWebClient(IOverstockRestClientFactory restClientFactory)
        {
            this.restClientFactory = restClientFactory;

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
        public Task UploadShipmentDetails(long overstockOrderId, ShipmentEntity shipment, IOverstockStoreEntity store)
        {
            ValidateApiAccessData(store);

            //TODO: Really do the update when we get to that story.

            return Task.CompletedTask;
        }

        /// <summary>
        /// Make the call to Overstock to get a list of orders matching criteria
        /// </summary>
        public async Task<GenericResult<List<OverstockOrderDto>>> GetOrders(IOverstockStoreEntity store, DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                // Create a request for getting orders
                RestRequest request = new RestRequest(OverstockWebClientEndpoints.GetOrdersResource(startDateTime, endDateTime));

                List<OverstockOrderDto> result = await MakeRequest<RestRequest, List<OverstockOrderDto>>(request, store, "GetOrders").ConfigureAwait(false);

                return result;
            }
            catch (OverstockException overstockException)
            {
                throw new OverstockException("ShipWorks was unable to download orders for your Overstock store.", overstockException);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(OverstockException));
            }
        }

        /// <summary>
        /// Attempt to get an order count to test connecting to Overstock.  If any error, assume connection failed.
        /// </summary>
        public async Task<bool> TestConnection(IOverstockStoreEntity store)
        {
            // See if we can successfully call GetOrderCount, if we throw, we can't connect or login
            try
            {
                await GetOrders(store, DateTime.Now, DateTime.Now).ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Generic method to make a request to the Overstock rest API
        /// </summary>
        /// <typeparam name="TRestRequest">Type of the request being made</typeparam>
        /// <typeparam name="TRestResponse">Type of the response from the API call</typeparam>
        /// <returns>The response from the API call</returns>
        private async Task<TRestResponse> MakeRequest<TRestRequest, TRestResponse>(TRestRequest request, IOverstockStoreEntity store, string logAction)
            where TRestRequest : IRestRequest
            where TRestResponse : new()
        {
            TRestResponse requestResult;
            try
            {
                // Log the request
                ApiLogEntry logger = new ApiLogEntry(ApiLogSource.Overstock, logAction);
                logger.LogRequest(request.Resource);

                IRestResponse<TRestResponse> restResponse = await restClientFactory.Create(store).ExecuteTaskAsync<TRestResponse>(request).ConfigureAwait(false);
                requestResult = restResponse.Data;

                // Log the response
                logger.LogResponse(restResponse.Content, "xml");

                // See if there were any errors
                CheckRestResponseForError(restResponse);

                // TODO: Do de-serialization here.

                if (request.Method == Method.PUT || request.Method == Method.POST)
                {
                    requestResult = (TRestResponse) restResponse;
                }
            }
            catch (NotSupportedException ex)
            {
                log.Error("A NotSupportedException occurred during MakeRequest.", ex);
                throw new OverstockException(ex.Message, ex);
            }
            catch (XmlException ex)
            {
                log.Error("A XmlException occurred during MakeRequest.", ex);
                throw new OverstockException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                log.Error("A Exception occurred during MakeRequest.", ex);
                throw new OverstockException(ex.Message, ex);
            }

            return requestResult;
        }

        /// <summary>
        /// Checks an IRestResponse StatusCode and ResponseStatus for errors
        /// </summary>
        /// <param name="restResponse"></param>
        /// <exception cref="OverstockException" />
        private void CheckRestResponseForError(IRestResponse restResponse)
        {
            if (restResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Check to see if we denied access due to authentication.
                log.Error("Overstock API credentials are invalid.");
                throw new OverstockException("The Overstock API credentials are invalid", (int) restResponse.StatusCode);
            }

            if (!successHttpStatusCodes.Contains(restResponse.StatusCode) ||
                !(restResponse.ResponseStatus == ResponseStatus.Completed || restResponse.ResponseStatus == ResponseStatus.None))
            {
                // There was an error, find the error message and throw
                log.Error($"An error occurred while communicating with Overstock.  Response content: {Environment.NewLine}{restResponse.Content}", restResponse.ErrorException);

                throw new OverstockException("An error occurred while attempting to contact Overstock.", (int) restResponse.StatusCode);
            }

            if (restResponse.Content == "null")
            {
                // Overstock sometimes returns "null" in the response for some reason which was causing
                // a NullReferenceException to bubble up and crash ShipWorks
                throw new OverstockException("ShipWorks received an invalid response from Overstock. Please try again later.");
            }
        }
    }
}
