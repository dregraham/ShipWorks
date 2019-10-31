using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore.Licensing.Warehouse;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// View model for a routing error
    /// </summary>
    [Component]
    public class RoutingErrorViewModel : IRoutingErrorViewModel
    {
        private readonly IRoutingErrorDialog dialog;
        private readonly IAsyncMessageHelper messageHelper;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public RoutingErrorViewModel(IRoutingErrorDialog dialog, IAsyncMessageHelper messageHelper, Func<Type, ILog> createLog)
        {
            this.dialog = dialog;
            this.messageHelper = messageHelper;
            log = createLog(typeof(RoutingErrorViewModel));
        }

        /// <summary>
        /// Message to display in the error
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Message { get; private set; }

        /// <summary>
        /// Url to use for the link
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string MoreInfoUrl { get; private set; }

        /// <summary>
        /// Should the More Info link be shown
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowMoreInfoUrl { get; private set; }

        /// <summary>
        /// Show a success dialog after an order has been split
        /// </summary>
        public async Task ShowError(Exception exception)
        {
            log.Error(exception);

            (Message, MoreInfoUrl) = GetErrorDetails(exception);
            ShowMoreInfoUrl = !string.IsNullOrEmpty(MoreInfoUrl);

            await messageHelper.ShowDialog(() => SetupDialog());
        }

        /// <summary>
        /// Setup the error dialog
        /// </summary>
        private IDialog SetupDialog()
        {
            dialog.DataContext = this;
            return dialog;
        }

        /// <summary>
        /// Get details about the exception
        /// </summary>
        private (string message, string moreInfoUrl) GetErrorDetails(Exception exception)
        {
            if (exception is HubApiException hubApiException)
            {
                switch (hubApiException.ErrorCode)
                {
                    case HubErrorCode.NoWarehousesAvailableForRouting:
                        return (
                            "This order can't ship from another warehouse.",
                            "https://support.shipworks.com/hc/en-us/articles/360022469872"
                            );
                    case HubErrorCode.CustomerHasSingleWarehouse:
                        return (
                            "You must add another warehouse in order to attempt to ship this item elsewhere.",
                            "https://support.shipworks.com/hc/en-us/articles/360022469872"
                            );
                }
            }

            return ("ShipWorks Hub was not able to reroute this order.", "https://support.shipworks.com/hc/en-us/articles/360022469872");
        }
    }
}
