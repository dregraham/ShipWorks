using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download;
using ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval;

namespace ShipWorks.Stores.Platforms.Newegg.Net
{
    /// <summary>
    /// Interface for creating the various types of requests allowed by the Newegg API.
    /// </summary>
    public interface IRequestFactory
    {
        /// <summary>
        /// Creates the order request.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>An IDownloadOrderRequest object.</returns>
        IDownloadOrderRequest CreateDownloadOrderRequest(Credentials credentials);

        /// <summary>
        /// Creates the check credential request.
        /// </summary>
        /// <returns>An ICheckCredentialRequest object.</returns>
        ICheckCredentialRequest CreateCheckCredentialRequest();

        /// <summary>
        /// Creates the report status request.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>An IStatusRequest object.</returns>
        IStatusRequest CreateReportStatusRequest(Credentials credentials);

        /// <summary>
        /// Creates the cancel order request.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>An ICancelOrderRequest object.</returns>
        ICancelOrderRequest CreateCancelOrderRequest(Credentials credentials);

        /// <summary>
        /// Creates the shipping request.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>An IShippingRequest object.</returns>
        IShippingRequest CreateShippingRequest(Credentials credentials);

        /// <summary>
        /// Creates the remove item request.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>An IRemoveItemRequest object.</returns>
        IRemoveItemRequest CreateRemoveItemRequest(Credentials credentials);
    }
}
