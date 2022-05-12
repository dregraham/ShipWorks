using System;
using System.Linq;
using System.Threading.Tasks;
using Divelements.SandRibbon;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.UI;
using SandMenu = Divelements.SandRibbon.Menu;
using SandMenuItem = Divelements.SandRibbon.MenuItem;

namespace ShipWorks.Shipping.ShipEngine.Manifest
{
    /// <summary>
    /// Class to manage ShipEngine manifests
    /// </summary>
    [Component(SingleInstance = true)]
    public class ShipEngineManifestUtility : IShipEngineManifestUtility
    {
        private readonly IShipEngineManifestCreator manifestCreator;
        private readonly IShipEngineManifestRepo manifestRepo;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;
        private const int MaxManifestsToFetch = 14;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineManifestUtility(IShipEngineManifestCreator manifestCreator,
            IShipEngineManifestRepo manifestRepo,
            IMessageHelper messageHelper,
            Func<Type, ILog> logFactory)
        {
            this.manifestCreator = manifestCreator;
            this.manifestRepo = manifestRepo;
            this.messageHelper = messageHelper;
            this.log = logFactory(typeof(ShipEngineManifestUtility));
        }

        /// <summary>
        /// Populate the Create Manifest menu
        /// </summary>
        public void PopulateCreateManifestMenu(SandMenuItem menu, ICarrierAccountRetriever accountRetriever)
        {
            // Clear any previous items.  Bug in SandGrid... have to remove each explicitly
            menu.Items.Cast<WidgetBase>().ToList().ForEach(m => menu.Items.Remove(m));
            menu.Activate -= OnCreateManifest;

            var accounts = accountRetriever.Accounts;

            // If there is only one account, just make the menu do its thing
            if (accounts.Count() == 1)
            {
                menu.Tag = accounts.First();
                menu.Activate += OnCreateManifest;

                if (!menu.Text.EndsWith("..."))
                {
                    menu.Text += "...";
                }
            }
            else if (accounts.Count() > 1)
            {
                // Add a child menu that contains each account
                SandMenu childMenu = new SandMenu();

                foreach (var account in accounts)
                {
                    SandMenuItem menuItem = new SandMenuItem(account.AccountDescription);
                    menuItem.Tag = account;
                    menuItem.Activate += OnCreateManifest;

                    childMenu.Items.Add(menuItem);
                }

                menu.Items.Add(childMenu);

                if (menu.Text.EndsWith("..."))
                {
                    menu.Text = menu.Text.Replace("...", "");
                }
            }
            else
            {
                menu.Enabled = false;
            }
        }

        /// <summary>
        /// Populate the Print Manifest menu
        /// </summary>
        public void PopulatePrintManifestMenu(SandMenu menu, ICarrierAccountRetriever accountRetriever)
        {
            menu.Items.Clear();

            var accounts = accountRetriever.Accounts;

            if (accounts.Count() == 1)
            {
                FillManifestMenuForAccount(menu, accounts.First());
            }
            else if (accounts.Count() > 1)
            {
                foreach (var account in accounts)
                {
                    SandMenuItem accountMenuItem = new SandMenuItem(account.AccountDescription);
                    menu.Items.Add(accountMenuItem);

                    SandMenu accountMenu = new SandMenu();
                    accountMenuItem.Items.Add(accountMenu);

                    FillManifestMenuForAccount(accountMenu, account);
                }
            }
            else
            {
                menu.Items.Add(new SandMenuItem { Text = "(none)", Enabled = false });
            }
        }

        /// <summary>
        /// The Create Manifest button is pushed
        /// </summary>
        private async void OnCreateManifest(object sender, EventArgs e)
        {
            SandMenuItem menuItem = (SandMenuItem) sender;
            var carrierAccount = (ICarrierAccount) menuItem.Tag;
            var owner = DisplayHelper.GetActiveForm();

            ProgressProvider progressProvider = new ProgressProvider();
            IProgressReporter manifestProgress = progressProvider.AddItem("Create Manifest");
            manifestProgress.CanCancel = false;
            using (ProgressDlg progressDialog = new ProgressDlg(progressProvider))
            {
                progressDialog.Title = $"Creating {EnumHelper.GetDescription(carrierAccount.ShipmentType)} Manifest";
                progressDialog.AllowCloseWhenRunning = false;
                progressDialog.AutoCloseWhenComplete = true;

                Task.Run(async () =>
                {
                    var result = await manifestCreator.CreateManifest(carrierAccount.ShipmentType, manifestProgress).ConfigureAwait(true);

                    if (result.Success)
                    {
                        manifestProgress.Detail = "Saving Manifest";
                        manifestProgress.PercentComplete = 100;
                        var saveResult = await manifestRepo.SaveManifest(result.Value, carrierAccount);

                        manifestProgress.Completed();

                        if (saveResult.Success)
                        {
                            MessageHelper.ShowMessage(owner, $"{EnumHelper.GetDescription(carrierAccount.ShipmentType)} manifest created.");
                        }
                        else
                        {
                            MessageHelper.ShowMessage(owner, saveResult.Message);
                        }

                        return;
                    }

                    manifestProgress.Completed();

                    MessageHelper.ShowError(owner, $"An error occurred creating the {EnumHelper.GetDescription(carrierAccount.ShipmentType)} manifest:\n{result.Message}");
                    log.Error(result.Exception.Message);
                });

                progressDialog.ShowDialog(owner);
            }
        }

        /// <summary>
        /// Fill the given menu with the scan forms that have been created for the account.  We list up to the last 7 days
        /// </summary>
        private async void FillManifestMenuForAccount(SandMenu menu, ICarrierAccount account)
        {
            var manifests = await manifestRepo.GetManifests(account, MaxManifestsToFetch);

            foreach (var manifest in manifests)
            {
                try
                {
                    string manifestName = string.Format("{0:MM/dd/yy h:mm tt} ({1} shipments)", manifest.ShipDate.ToLocalTime(), manifest.ShipmentCount);

                    SandMenuItem formMenuItem = new SandMenuItem(manifestName);
                    formMenuItem.Tag = manifest;

                    formMenuItem.Activate += new EventHandler(OnPrintManifest);
                    menu.Items.Add(formMenuItem);
                }
                catch (ShippingException ex)
                {
                    // This could occur if the batch was deleted. We just want to move on and not show this item in the menu
                    string message = string.Format("An exception was encountered while populating the manifest menu for account {0}. " +
                                                   "Skipping this manifest and continuing to load the menu.", account.AccountDescription);

                    log.Error(message, ex);
                }
            }

            if (menu.Items.Count == 0)
            {
                menu.Items.Add(new SandMenuItem { Text = "(none)", Enabled = false });
            }
        }

        /// <summary>
        /// Print the manifest attached to the activating menu
        /// </summary>
        private void OnPrintManifest(object sender, EventArgs e)
        {
            SandMenuItem menuItem = (SandMenuItem) sender;
            var owner = DisplayHelper.GetActiveForm();

            var manifest = (ShipEngineManifestEntity) menuItem.Tag;

            try
            {
                Print(manifest.ManifestID);
            }
            catch (Exception ex)
            {
                messageHelper.ShowError(owner, ex.Message);

                log.Error("An error occurred while attempting to print a scan form.", ex);
            }
        }

        /// <summary>
        /// Print a manifest
        /// </summary>
        private void Print(string manifestId)
        {
            throw new NotImplementedException();
        }
    }
}
