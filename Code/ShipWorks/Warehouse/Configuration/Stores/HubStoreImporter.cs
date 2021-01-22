using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Warehouse.Configuration.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Class to import stores from the Hub
    /// </summary>
    [Component(RegistrationType.Self)]
    public class HubStoreImporter
    {
        private readonly IHubStoreConfigurator storeConfigurator;
        private readonly IHubConfigurationWebClient webClient;
        private readonly IConfigurationData configurationData;
        private HubConfiguration hubConfig;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubStoreImporter(IHubStoreConfigurator storeConfigurator,
            IHubConfigurationWebClient webClient,
            IConfigurationData configurationData)
        {
            this.storeConfigurator = storeConfigurator;
            this.webClient = webClient;
            this.configurationData = configurationData;
        }

        /// <summary>
        /// Import stores from the Hub
        /// </summary>
        public HubConfiguration ImportStores(IWin32Window owner, string warehouseID)
        {
            ProgressProvider progressProvider = new ProgressProvider();
            IProgressReporter storeProgress = progressProvider.AddItem("Importing stores");
            storeProgress.CanCancel = false;
            using (ProgressDlg progressDialog = new ProgressDlg(progressProvider))
            {
                progressDialog.Title = "Importing Stores from ShipWorks Hub";
                progressDialog.AllowCloseWhenRunning = false;
                progressDialog.AutoCloseWhenComplete = true;

                Task.Run(async () =>
                {
                    storeProgress.Detail = "Getting list of stores to import";
                    storeProgress.Starting();
                    storeProgress.PercentComplete = 0;

                    hubConfig = await webClient.GetConfig(warehouseID).ConfigureAwait(false);
                    storeConfigurator.Configure(hubConfig.StoreConfigurations, storeProgress);
                });

                progressDialog.ShowDialog(owner);
            }
            return hubConfig;
        }
    }
}
