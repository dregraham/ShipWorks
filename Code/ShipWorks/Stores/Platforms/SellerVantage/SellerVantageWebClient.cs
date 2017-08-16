using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.SellerVantage
{
    /// <summary>
    /// Custom overrides for SellerVantage of the base web client
    /// </summary>
    public class SellerVantageWebClient : GenericStoreWebClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SellerVantageWebClient(GenericModuleStoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Perform request transformation
        /// </summary>
        protected override void TransformRequest(IHttpVariableRequestSubmitter request, string action)
        {
            base.TransformRequest(request, action);

            // If storeCode is set, we have to rename it to client for SellerVantage.  They had already been using client when they updated
            // from using the v2 schema to v3, and since storecode wasn't documented, we just conformed.
            string storeCode = request.Variables["storecode"];
            if (storeCode != null)
            {
                request.Variables.Remove("storecode");
                request.Variables.Add("client", storeCode);
            }
        }
    }
}
