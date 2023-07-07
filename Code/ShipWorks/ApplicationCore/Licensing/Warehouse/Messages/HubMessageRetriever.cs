using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.Messages
{
    /// <summary>
    /// Retrieves messages from Hub for the current account and user
    /// </summary>
    [Component(RegistrationType.Self)]
    public class HubMessageRetriever
    {
        private readonly IWarehouseRequestFactory requestFactory;
        private readonly IWarehouseRequestClient warehouseClient;
        private readonly IUserSession userSession;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubMessageRetriever(IWarehouseRequestFactory requestFactory,
            IWarehouseRequestClient warehouseClient,
            IUserSession userSession,
            Func<Type, ILog> logFactory)
        {
            this.requestFactory = requestFactory;
            this.warehouseClient = warehouseClient;
            this.userSession = userSession;
            log = logFactory(typeof(HubMessageRetriever));
        }

        /// <summary>
        /// Get and display any relevant messages from Hub
        /// </summary>
        public async Task GetMessages(IWin32Window owner)
        {
            var request = requestFactory.Create(string.Format(WarehouseEndpoints.GetMessages, HttpUtility.UrlEncode(userSession.User.Username), Assembly.GetExecutingAssembly().GetName().Version), Method.GET, null);

            var response = await warehouseClient.MakeRequest<MessagesResponse>(request, "GetMessages").ConfigureAwait(true);

            log.Info($"Got {response.Messages.Count} messages");

            if (response.Messages.Any())
            {
                var messageDlg = new HubMessagesDialog();
                messageDlg.SetMessages(response.Messages);
                messageDlg.ShowDialog(owner);
            }
        }
    }
}
