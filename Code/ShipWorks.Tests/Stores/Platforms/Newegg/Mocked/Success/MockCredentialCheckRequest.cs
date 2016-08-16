using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation;

namespace ShipWorks.Tests.Stores.Newegg.Mocked.Success
{
    public class MockCredentialCheckRequest : ICheckCredentialRequest
    {
        public NeweggResponse SubmitRequest(Credentials credentials, Dictionary<string, object> parameters)
        {
            //This is a mocked credential request that will always return a response indicating
            // the credentials are valid
            string response = 
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <NeweggAPIResponse>
                      <IsSuccess>true</IsSuccess>
                      <OperationType>GetServiceStatus</OperationType>
                      <SellerID>A09V</SellerID>
                      <ResponseBody>
                        <Status>1</Status>
                        <Timestamp>06/12/2012 11:42:58</Timestamp>
                      </ResponseBody>
                    </NeweggAPIResponse>";

            return new NeweggResponse(response, new CheckCredentialsResponseSerializer());
        }

        /// <summary>
        /// Makes a request to determine if the credentials are valid.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>
        /// Returns true if the credentials are valid; otherwise false.
        /// </returns>
        public bool AreCredentialsValid(Credentials credentials)
        {
            //This is a mocked credential request that will always return a response indicating
            // the credentials are valid
            return true;
        }
    }
}
