using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation;

namespace ShipWorks.Tests.Stores.Newegg.Mocked.Failure
{
    public class MockCredentialCheckRequest : ICheckCredentialRequest
    {

        public NeweggResponse SubmitRequest(Credentials credentials, Dictionary<string, object> parameters)
        {
            //This is a mocked credential request that will always return a response indicating
            // the credentials are NOT valid
            string response = 
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <Errors xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                      <Error>
                        <Code>AccessDenied</Code>
                        <Message>The specified seller id is invalid or you have not yet got the authorization from this seller.</Message>
                      </Error>
                    </Errors>";

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
            // the credentials are NOT valid
            return false;
        }
    }
}
