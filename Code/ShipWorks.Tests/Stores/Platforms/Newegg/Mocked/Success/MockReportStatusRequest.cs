using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus;
using ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus.Response;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Tests.Stores.Newegg.Mocked.Success
{
    class MockReportStatusRequest : IStatusRequest
    {
        public string RequestId { get; set; }

        /// <summary>
        /// Submits the request with the given credentials and parameter values. This is mocked
        /// to always return a status response of finished.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// A NeweggResponse containing the response from the Newegg API.
        /// </returns>
        public NeweggResponse SubmitRequest(Credentials credentials, Dictionary<string, object> parameters)
        {
            string responseData = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                    <NeweggAPIResponse>
                      <IsSuccess>true</IsSuccess>
                      <OperationType>OrderListReportResponse</OperationType>
                      <SellerID>A09V</SellerID>
                      <ResponseBody>
                        <ResponseList>
                          <ResponseInfo>
                            <RequestId>{0}</RequestId>
                            <RequestType>ORDER_LIST_REPORT</RequestType>
                            <RequestDate>06/13/2012 09:14:01</RequestDate>
                            <RequestStatus>FINISHED</RequestStatus>
                          </ResponseInfo>
                        </ResponseList>
                      </ResponseBody>
                    </NeweggAPIResponse>", this.RequestId);

            return new NeweggResponse(responseData, new StatusResponseSerializer());
        }

        public StatusResult GetStatus()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.US);
            return SubmitRequest(credentials, new Dictionary<string, object>()).Result as StatusResult;
        }

    }
}
