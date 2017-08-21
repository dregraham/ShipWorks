using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping;
using ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus;

namespace ShipWorks.Stores.Platforms.Newegg.Net
{
    /// <summary>
    /// An implementation of IRequestFactory that will create request objects that hit
    /// the production/live Newegg API
    /// </summary>
    [Component]
    public class LiveRequestFactory : IRequestFactory
    {
        /// <summary>
        /// Creates the order request.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>An IDownloadOrderRequest object.</returns>
        public IDownloadOrderRequest
            CreateDownloadOrderRequest(Credentials credentials)
        {
            return new DownloadOrdersRequest(credentials);
        }

        /// <summary>
        /// Creates the check credential request.
        /// </summary>
        /// <returns>An ICheckCredentialRequest object.</returns>
        public ICheckCredentialRequest CreateCheckCredentialRequest()
        {
            return new CheckCredentialsRequest();
        }

        /// <summary>
        /// Creates the report status request.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>An IStatusRequest object.</returns>
        public IStatusRequest CreateReportStatusRequest(Credentials credentials)
        {
            return new ReportStatus.StatusRequest(credentials);
        }

        /// <summary>
        /// Creates the cancel order request.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>An ICancelOrderRequest object.</returns>
        public ICancelOrderRequest CreateCancelOrderRequest(Credentials credentials)
        {
            return new CancelOrderRequest(credentials);
        }

        /// <summary>
        /// Creates the shipping request.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>An IShippingRequest object.</returns>
        public IShippingRequest CreateShippingRequest(Credentials credentials)
        {
            return new ShippingRequest(credentials);
        }

        /// <summary>
        /// Creates the remove item request.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>
        /// An IRemoveItemRequest object.
        /// </returns>
        public IRemoveItemRequest CreateRemoveItemRequest(Credentials credentials)
        {
            return new RemoveItemRequest(credentials);
        }
    }
}
