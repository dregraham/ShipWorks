using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Grid
{
    /// <summary>
    /// Hyperlink column definition for displaying the amazon orderID and pulling up the order info on amazon
    /// </summary>
    public class GridAmazonOrderDisplayType : GridOrderNumberDisplayType
    {
        private static ILog log = LogManager.GetLogger(typeof(GridAmazonOrderDisplayType));

        /// <summary>
        /// Constructor
        /// </summary>
        public GridAmazonOrderDisplayType()
        {
            GridHyperlinkDecorator hyperlink = new GridHyperlinkDecorator();
            hyperlink.LinkClicked += OnLinkClicked;

            Decorate(hyperlink);
        }

        /// <summary>
        /// Launches the browser to the amazon order page
        /// </summary>
        private async void OnLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            AmazonOrderEntity order = e.Row.Entity as AmazonOrderEntity;
            if (order != null)
            {
                string domainName = await GetDomainName(order).ConfigureAwait(false);
                string orderUrl = $"https://sellercentral.{domainName}/gp/orders/order-details.html/?orderID={order.AmazonOrderID}";
                Uri orderUri = new Uri(orderUrl);

                if (orderUri.Host != "sellercentral.amazon.com")
                {
                    HttpRequestSubmitter request = new HttpVariableRequestSubmitter();
                    request.Uri = new Uri(orderUrl);

                    try
                    {
                        // There have been cases where 502 errors have been received when trying to navigate to
                        // the store domain provided by amazon, so we'll try to bounce a request off of the URL
                        // to see whether it works; if it doesn't work, we'll resort to just hitting sellercentral.amazon.com
                        IHttpResponseReader response = request.GetResponse();
                        if (response.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                        {
                            log.Warn(string.Format(
                                "Unable to view Amazon order info at {0}. A {1} error was received.", orderUrl,
                                response.HttpWebResponse.StatusCode));
                            orderUrl = SwapDomainWithGenericAmazonDomain(orderUrl);
                        }
                    }
                    catch (WebException exception)
                    {
                        log.Warn(string.Format("Unable to view Amazon order info at {0}. {1}", orderUrl,
                            exception.Message));
                        orderUrl = SwapDomainWithGenericAmazonDomain(orderUrl);
                    }
                }


                WebHelper.OpenUrl(orderUrl, e.Row.Grid.SandGrid.TopLevelControl);
            }
        }

        /// <summary>
        /// Swaps the domain of the URL provided with the generic amazon sellercentral domain (i.e. mystore.com/xyz... becomes amazon.com/xyz...)
        /// </summary>
        /// <param name="orderUrl">The order URL.</param>
        /// <returns>The same URL except with the domain of sellercentral.amazon.com</returns>
        private static string SwapDomainWithGenericAmazonDomain(string orderUrl)
        {
            Uri uri = new Uri(orderUrl);

            log.InfoFormat("Swapping {0} with the 'sellercentral.amazon.com' domain.", uri.Host);
            return orderUrl.Replace(uri.Host, "sellercentral.amazon.com");
        }

        /// <summary>
        /// Gets the domain name of the store associated with the given order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The domain name for the store (e.g. amazon.com, amazon.ca, etc.)</returns>
        private static async Task<string> GetDomainName(OrderEntity order)
        {
            // Default the domain to amazon.com in case there is an exception trying to
            // get the domain from the store type
            string domainName = "amazon.com";

            try
            {
                StoreEntity amazonStoreEntity = DataProvider.GetEntity(order.StoreID) as AmazonStoreEntity;

                // Obtain the domain name from the store, so we navigate to the correct URL based on
                // the marketplace (i.e. amazon.ca vs. amazon.com)
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    AmazonStoreType amazonStoreType = lifetimeScope.Resolve<AmazonStoreType>(TypedParameter.From(amazonStoreEntity));
                    domainName = await amazonStoreType.GetDomainName().ConfigureAwait(false);
                }
            }
            catch (AmazonException)
            {
                log.WarnFormat("The domain name could not be retrieved for the Amazon store (store ID {0}); defaulting to amazon.com.", order.StoreID);
            }

            return domainName;
        }
    }
}
