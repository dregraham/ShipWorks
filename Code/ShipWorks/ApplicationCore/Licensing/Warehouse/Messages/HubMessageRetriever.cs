using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public HubMessageRetriever(IWarehouseRequestFactory requestFactory, IWarehouseRequestClient warehouseClient, IUserSession userSession)
        {
            this.requestFactory = requestFactory;
            this.warehouseClient = warehouseClient;
            this.userSession = userSession;
        }

        /// <summary>
        /// Get and display any relevant messages from Hub
        /// </summary>
        public async Task GetMessages(IWin32Window owner)
        {
            var request = requestFactory.Create(string.Format(WarehouseEndpoints.GetMessages, HttpUtility.UrlEncode(userSession.User.Username)), Method.GET, null);

            var response = await warehouseClient.MakeRequest<MessagesResponse>(request, "GetMessages").ConfigureAwait(true);

            if (response.Messages.Any())
            {
                var messageDlg = new HubMessagesDialog();
                messageDlg.SetMessages(response.Messages);
                messageDlg.ShowDialog(owner);
            }
        }
    }
}
