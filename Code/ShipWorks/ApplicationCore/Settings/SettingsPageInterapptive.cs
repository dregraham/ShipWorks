using System;
using System.Data.Common;
using System.Reflection;
using System.Windows.Forms;
using Interapptive.Shared.Data;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Settings.PrintResultCleanup;
using ShipWorks.ApplicationCore.Settings.ResourceCleanup;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Filters;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.OnTrac.Net;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Stores.Platforms.BuyDotCom;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Stores.Platforms.SearchFit;
using ShipWorks.Stores.Platforms.Sears;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;
using ShipWorks.Templates.Distribution;

namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// About page of the Options window
    /// </summary>
    public partial class SettingsPageInterapptive : SettingsPageBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SettingsPageInterapptive));

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsPageInterapptive()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Do initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            postalWebTestServer.Checked = PostalWebUtility.UseTestServer;
            uspsTestServer.Checked = UspsWebClient.UseTestServer;

            upsOnLineTools.Checked = UpsWebClient.UseTestServer;
            endiciaTestServer.Checked = EndiciaApiClient.UseTestServer;
            express1EndiciaTestServer.Checked = Express1EndiciaUtility.UseTestServer;
            express1UspsTestServer.Checked = Express1UspsConnectionDetails.UseTestServer;
            onTracTestServer.Checked = OnTracRequest.UseTestServer;

            FedExSettingsRepository fedExSettingsRepo = new FedExSettingsRepository();
            fedexListRates.Checked = fedExSettingsRepo.UseListRates;
            fedexTestServer.Checked = fedExSettingsRepo.UseTestServer;

            ebay.Checked = !EbayUrlUtilities.UseLiveServer;
            marketplaceAdvisor.Checked = !MarketplaceAdvisorOmsClient.UseLiveServer;
            payPal.Checked = !PayPalWebClient.UseLiveServer;
            newegg.Checked = !Credentials.UseLiveServerKey;
            sears.Checked = !SearsWebClient.UseLiveServer;
            overstockServer.Checked = !InterapptiveOnly.Registry.GetValue("OverstockLiveServer", true);
            marketplaceAdvisorMarkProcessed.Checked = MarketplaceAdvisorUtility.MarkProcessedAfterDownload;
            yahooDeleteMessages.Checked = YahooEmailUtility.DeleteMessagesAfterDownload;
            buyDotComArchiveOrderFile.Checked = BuyDotComUtility.ArchiveFileAfterDownload;
            searchFitDeleteAfterDownload.Checked = !SearchFitStoreType.LeaveOnServer;

            multipleInstances.Checked = InterapptiveOnly.AllowMultipleInstances;

            buyDotComMapChooser.Initialize(new BuyDotComOrderImportSchema());

            EnumHelper.BindComboBox<EndiciaTestServer>(endiciaTestServers);
            endiciaTestServers.SelectedValue = EndiciaApiClient.UseTestServerUrl;
            endiciaTestServers.Enabled = endiciaTestServer.Checked;

            useInsureShipTestServer.Checked = new InsureShipSettings().UseTestServer;
        }

        /// <summary>
        /// Save state
        /// </summary>
        public override void Save()
        {
            base.Save();

            PostalWebUtility.UseTestServer = postalWebTestServer.Checked;
            UspsWebClient.UseTestServer = uspsTestServer.Checked;
            UpsWebClient.UseTestServer = upsOnLineTools.Checked;
            EndiciaApiClient.UseTestServer = endiciaTestServer.Checked;
            Express1EndiciaUtility.UseTestServer = express1EndiciaTestServer.Checked;
            Express1UspsConnectionDetails.UseTestServer = express1UspsTestServer.Checked;
            OnTracRequest.UseTestServer = onTracTestServer.Checked;

            FedExSettingsRepository fedexSettingsRepo = new FedExSettingsRepository();
            fedexSettingsRepo.UseListRates = fedexListRates.Checked;
            fedexSettingsRepo.UseTestServer = fedexTestServer.Checked;

            EbayUrlUtilities.UseLiveServer = !ebay.Checked;
            MarketplaceAdvisorOmsClient.UseLiveServer = !marketplaceAdvisor.Checked;
            PayPalWebClient.UseLiveServer = !payPal.Checked;
            Credentials.UseLiveServerKey = !newegg.Checked;
            SearsWebClient.UseLiveServer = !sears.Checked;
            InterapptiveOnly.Registry.SetValue("OverstockLiveServer", !overstockServer.Checked);
            MarketplaceAdvisorUtility.MarkProcessedAfterDownload = marketplaceAdvisorMarkProcessed.Checked;
            YahooEmailUtility.DeleteMessagesAfterDownload = yahooDeleteMessages.Checked;
            BuyDotComUtility.ArchiveFileAfterDownload = buyDotComArchiveOrderFile.Checked;
            SearchFitStoreType.LeaveOnServer = !searchFitDeleteAfterDownload.Checked;

            InterapptiveOnly.AllowMultipleInstances = multipleInstances.Checked;

            EndiciaApiClient.UseTestServerUrl = (EndiciaTestServer) endiciaTestServers.SelectedValue;

            new InsureShipSettings().UseTestServer = useInsureShipTestServer.Checked;
        }

        /// <summary>
        /// Deploy assemblies to the database
        /// </summary>
        private void OnDeployAssemblies(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (DbTransaction transaction = con.BeginTransaction())
                {
                    SqlAssemblyDeployer.DeployAssemblies(con, transaction);
                    transaction.Commit();
                }
            }

            MessageHelper.ShowMessage(this, "Deploy complete.");
        }

        /// <summary>
        /// Deploy an assembly as chosen from an assembly file
        /// </summary>
        private void OnDeployChosenAssembly(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "ShipWorks.SqlServer.dll|ShipWorks.SqlServer.dll";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    Assembly assembly = Assembly.LoadFrom(dlg.FileName);

                    Cursor.Current = Cursors.WaitCursor;

                    using (DbConnection con = SqlSession.Current.OpenConnection())
                    {
                        using (DbTransaction transaction = con.BeginTransaction())
                        {
                            SqlAssemblyDeployer.DropAssemblies(con, transaction);
                            SqlAssemblyDeployer.DeployAssembly(assembly, con, transaction);
                            transaction.Commit();
                        }
                    }

                    MessageHelper.ShowMessage(this, "Deploy complete.");
                }
            }
        }

        /// <summary>
        /// Regenerate all the filters
        /// </summary>
        private void OnRegenerateFilters(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // We need to push a new scope for the layout context
                FilterLayoutContext.PushScope();

                // Regenerate the filters
                FilterLayoutContext.Current.RegenerateAllFilters(SqlAdapter.Default);

                // We can wipe any dirties and any current checkpoint - they don't matter since we have regenerated all filters anyway
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    SqlUtility.TruncateTable("FilterNodeContentDirty", con);
                    SqlUtility.TruncateTable("FilterNodeUpdateCheckpoint", con);
                }
            }
            finally
            {
                FilterLayoutContext.PopScope();
            }

            MessageHelper.ShowMessage(this, "Regeneration complete.");
        }

        /// <summary>
        /// Execute resource cleanup
        /// </summary>
        private void OnPurgeLabels(object sender, EventArgs e)
        {
            using (ResourceCleanupDlg dlg = new ResourceCleanupDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Let the user/support delete space-consuming print job results
        /// </summary>
        private void OnPurgePrintJobs(object sender, EventArgs e)
        {
            using (PrintResultCleanupDlg dlg = new PrintResultCleanupDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Enable/disable the list of endicia test servers, based on Use Test Server checked state.
        /// </summary>
        private void OnEndiciaTestServerCheckedChanged(object sender, EventArgs e)
        {
            endiciaTestServers.Enabled = endiciaTestServer.Checked;
        }

        /// <summary>
        /// Re-install built in templates
        /// </summary>
        /// <remarks>
        /// This will overwrite all builtin templates with the version included in ShipWorks. It will not
        /// affect copied or custom templates or user-created folders.
        /// </remarks>
        private void OnReinstallTemplates(object sender, EventArgs e) =>
            BuiltinTemplates.ReinstallTemplates();
    }
}
