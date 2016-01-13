using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download;
using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation;
using ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval;

namespace ShipWorks.Tests.Stores.Newegg.Mocked.Success
{
    /// <summary>
    /// This factory will create mock requests that result in success responses
    /// </summary>
    public class MockRequestFactory : IRequestFactory
    {
        public IDownloadOrderRequest CreateDownloadOrderRequest(Credentials credentials)
        {
            throw new NotImplementedException();
        }

        public ICheckCredentialRequest CreateCheckCredentialRequest()
        {
            return new MockCredentialCheckRequest();
        }

        public IStatusRequest CreateReportStatusRequest(Credentials credentials)
        {
            return new MockReportStatusRequest();
        }
        
        public ICancelOrderRequest CreateCancelOrderRequest(Credentials credentials)
        {
            throw new NotImplementedException();
        }


        public IShippingRequest CreateShippingRequest(Credentials credentials)
        {
            return new MockShippingRequest();
        }


        public IRemoveItemRequest CreateRemoveItemRequest(Credentials credentials)
        {
            throw new NotImplementedException();
        }
    }
}
