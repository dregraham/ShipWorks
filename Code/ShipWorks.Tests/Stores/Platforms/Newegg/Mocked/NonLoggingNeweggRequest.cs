using System;
using ShipWorks.Stores.Platforms.Newegg.Net;
using Interapptive.Shared.Net;


namespace ShipWorks.Tests.Stores.Newegg.Mocked
{
    public class NonLoggingNeweggRequest :NeweggHttpRequest
    {
        protected override void LogRequest(HttpXmlVariableRequestSubmitter submitter, RequestConfiguration requestConfiguration)
        {
            // Do nothing here so we can run unit tests without logging the API call
            // (and requiring a ShipWorks session to be active)
        }

        protected override void LogResponseText(string xmlResponse, RequestConfiguration requestConfiguration)
        {
            // Do nothing here so we can run unit tests without logging the API call
            // (and requiring a ShipWorks session to be active)
        }
    }
}
