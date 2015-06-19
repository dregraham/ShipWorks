using System.Collections.Generic;
using System.Linq;
using System.Net;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.Errors.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus.Response;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus
{
    /// <summary>
    /// An implementation of the IStatusRequest that hits the Newegg API.
    /// </summary>
    public class StatusRequest : IStatusRequest
    {
        private const string RequestUrl = "{0}/reportmgmt/report/status?sellerid={1}";

        //public const string RequestIdParameterName = "RequestId";
        private Credentials credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusRequest"/> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        public StatusRequest(Credentials credentials)
        {
            this.credentials = credentials;
        }


        /// <summary>
        /// Gets or sets the ID of the request being inquired about.
        /// </summary>
        /// <value>The request ID.</value>
        public string RequestId { get; set; }


        /// <summary>
        /// Gets the status of the report.
        /// </summary>
        /// <returns>
        /// A StatusResult object.
        /// </returns>
        public StatusResult GetStatus()
        {
            NeweggResponse response = SubmitRequest();
            if (response.ResponseErrors.Count() > 0)
            {
                string errorMessage = string.Format("An error occurred communicating with Newegg: ");
                foreach (Error error in response.ResponseErrors)
                {
                    errorMessage += error.ErrorCode + ": " + error.Description + System.Environment.NewLine;
                }

                throw new NeweggException(errorMessage, response);
            }

            // There were not any errors, so we know this is a status result
            return response.Result as StatusResult;
        }

        
        /// <summary>
        /// Submits the request with the given credentials and parameter values.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// A NeweggResponse containing the response from the Newegg API.
        /// </returns>
        private NeweggResponse SubmitRequest()
        {
            // API URL depends on which marketplace the seller selected 
            string marketplace = "";

            switch (credentials.Channel)
            {
                case NeweggChannelType.US:
                    break;
                case NeweggChannelType.Business:
                    marketplace = "/b2b";
                    break;
                case NeweggChannelType.Canada:
                    marketplace = "/can";
                    break;
                default:
                    break;
            }

            // Format our request URL with the value of the seller ID and configure the request
            string formattedUrl = string.Format(RequestUrl, marketplace, credentials.SellerId);
            RequestConfiguration requestConfig = new RequestConfiguration("Report Status", formattedUrl)
            { 
                Method = HttpVerb.Put, 
                Body = GetRequestBody() 
            };

            NeweggHttpRequest request = new NeweggHttpRequest();
            string responseData = request.SubmitRequest(credentials, requestConfig);

            // Create and return a NeweggResponse object with our raw data and the serializer that can
            // deserialize the response into the excpected StatusResult
            return new NeweggResponse(responseData, new StatusResponseSerializer());
        }

        public string GetRequestBody()
        {
            string requestBody = string.Format(@"
                <NeweggAPIRequest>
                    <OperationType>GetReportStatusRequest</OperationType>
                    <RequestBody>
                        <GetRequestStatus>
                            <RequestIDList>
                                <RequestID>{0}</RequestID>
                            </RequestIDList>
                            <MaxCount>10</MaxCount>
                        </GetRequestStatus>
                    </RequestBody>
                </NeweggAPIRequest>", this.RequestId);

            return requestBody;
        }
    }
}
