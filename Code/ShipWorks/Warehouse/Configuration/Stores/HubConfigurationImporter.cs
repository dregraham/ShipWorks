using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Common.Threading;
using ShipWorks.Warehouse.Configuration.DTO;
using ShipWorks.Warehouse.Configuration.Filters;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Class to import stores from the Hub
    /// </summary>
    [Component(RegistrationType.Self)]
    public class HubConfigurationImporter
    {
        private readonly IHubStoreConfigurator storeConfigurator;
        private readonly IHubFilterConfigurator filtersConfigurator;
        private readonly IHubConfigurationWebClient webClient;
        private HubConfiguration hubConfig;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubConfigurationImporter(IHubConfigurationWebClient webClient,
            IHubStoreConfigurator storeConfigurator,
            IHubFilterConfigurator filtersConfigurator)
        {
            this.storeConfigurator = storeConfigurator;
            this.filtersConfigurator = filtersConfigurator;
            this.webClient = webClient;
        }

        /// <summary>
        /// Import configuration from the Hub
        /// </summary>
        public HubConfiguration ImportConfiguration(IWin32Window owner, string warehouseID)
        {
            ProgressProvider progressProvider = new ProgressProvider();
            IProgressReporter storeProgress = progressProvider.AddItem("Importing stores");
            IProgressReporter filterProgress = progressProvider.AddItem("Importing filters");
            storeProgress.CanCancel = false;
            filterProgress.CanCancel = false;

            using (ProgressDlg progressDialog = new ProgressDlg(progressProvider))
            {
                progressDialog.Title = "Importing Configuration from ShipWorks Hub";
                progressDialog.AllowCloseWhenRunning = false;
                progressDialog.AutoCloseWhenComplete = true;

                Task.Run(async () =>
                {
                    await ImportStores(storeProgress, warehouseID).ConfigureAwait(false);
                    await ImportFilters(filterProgress).ConfigureAwait(false);
                });

                progressDialog.ShowDialog(owner);
            }

            return hubConfig;
        }

        /// <summary>
        /// Import stores from the Hub
        /// </summary>
        private async Task ImportStores(IProgressReporter storeProgress, string warehouseID)
        {
            storeProgress.Detail = "Getting list of stores to import";
            storeProgress.Starting();
            storeProgress.PercentComplete = 0;

            hubConfig = await webClient.GetConfig(warehouseID).ConfigureAwait(false);
            storeConfigurator.Configure(hubConfig.StoreConfigurations, storeProgress);
        }

        /// <summary>
        /// Import filters from the Hub
        /// </summary>
        /// <param name="filterProgress"></param>
        private async Task ImportFilters(IProgressReporter filterProgress)
        {
            filterProgress.Detail = "Getting list of filters to import";
            filterProgress.Starting();
            filterProgress.PercentComplete = 0;

            var filtersResponse = await webClient.GetFilters().ConfigureAwait(false);
            filtersConfigurator.Configure(filtersResponse, filterProgress);
        }
    }
}
