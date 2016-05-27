using System;
using ShipWorks.Stores.Platforms.Newegg.Net;
using Interapptive.Shared.Net;

namespace ShipWorks.Tests.Stores.Newegg.Mocked
{
    public class MockedNeweggRequest : INeweggRequest
    {
        private string desiredResponse;
        
        public MockedNeweggRequest(string desiredResponse)
        {
            this.desiredResponse = desiredResponse;
        }

        /// <summary>
        /// This is a mocked implementation, so just return the response that was provided to the constructor
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="requestConfiguration"></param>
        /// <returns></returns>
        public string SubmitRequest(Credentials credentials, RequestConfiguration requestConfiguration)
        {
            // Record the values provided to the method for tests to assert on
            Url = requestConfiguration.Url;
            Body = requestConfiguration.Body;
            Method = requestConfiguration.Method;

            return desiredResponse;
        }


        public string Url { get; set; }

        public string Body { get; set; }

        public HttpVerb Method { get; set; }

    }
}
