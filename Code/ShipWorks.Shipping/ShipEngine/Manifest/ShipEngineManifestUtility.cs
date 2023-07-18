using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Divelements.SandRibbon;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.UI;
using static System.String;
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
#pragma warning disable CS4014
                FillManifestMenuForAccount(menu, accounts.First());
#pragma warning restore CS4014
            }
            else if (accounts.Count() > 1)
            {
                foreach (var account in accounts)
                {
                    SandMenuItem accountMenuItem = new SandMenuItem(account.AccountDescription);
                    menu.Items.Add(accountMenuItem);

                    SandMenu accountMenu = new SandMenu();
                    accountMenuItem.Items.Add(accountMenu);

#pragma warning disable CS4014
                    FillManifestMenuForAccount(accountMenu, account);
#pragma warning restore CS4014
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
        private void OnCreateManifest(object sender, EventArgs e)
        {
            SandMenuItem menuItem = (SandMenuItem) sender;
            var carrierAccount = (ICarrierAccount) menuItem.Tag;
            var owner = DisplayHelper.GetActiveForm();

            ProgressProvider progressProvider = new ProgressProvider();
            IProgressReporter manifestProgress = progressProvider.AddItem("Create Manifest");
            manifestProgress.CanCancel = false;
            using (ProgressDlg progressDialog = new ProgressDlg(progressProvider))
            {
                progressDialog.Title = "Create Manifest";
                progressDialog.Description = $"Create a manifest for {EnumHelper.GetDescription(carrierAccount.ShipmentType)}";
                progressDialog.AllowCloseWhenRunning = false;
                progressDialog.AutoCloseWhenComplete = true;

                var errorMessages = new List<string>();
                var successMessages = new List<string>();

                Task.Run(() =>
                    CreateManifestTask(carrierAccount, manifestProgress, successMessages, errorMessages)
                        .ConfigureAwait(true)).ConfigureAwait(true);

                progressDialog.ShowDialog(owner);

                if (errorMessages.Any())
                {
                    MessageHelper.ShowError(owner, Join(Environment.NewLine, errorMessages));
                }

                if (successMessages.Any())
                {
                    MessageHelper.ShowMessage(owner, Join(Environment.NewLine, successMessages));
                }
            }
        }

        /// <summary>
        /// Create the manifests
        /// </summary>
        public async Task<List<long>> CreateManifestTask(ICarrierAccount carrierAccount, 
            IProgressReporter manifestProgress,
            List<string> successMessages, 
            List<string> errorMessages)
        {
            var results = await manifestCreator.CreateManifest(carrierAccount, manifestProgress)
                .ConfigureAwait(true);

            manifestProgress.Detail = "Saving Manifest";
            manifestProgress.PercentComplete = 100;

            var manifestList = new List<long>();

            foreach (var result in results)
            {
                if (result.Success)
                {
                    var saveResult = await manifestRepo.SaveManifest(result.Value, carrierAccount);
                    
                    if (saveResult.Success)
                    {
                        var msg = $"{EnumHelper.GetDescription(carrierAccount.ShipmentType)} manifest created.";
                        if (!successMessages.Contains(msg))
                        {
                            successMessages.Add(msg);
                        }

                        manifestList.AddRange(saveResult.Value);
                    }
                    else
                    {
                        var msg = saveResult.Message;
                        if (!errorMessages.Contains(msg))
                        {
                            errorMessages.Add(msg);
                        }
                    }
                }
                else
                {
                    var msg = result.Message;
                    if (!errorMessages.Contains(msg))
                    {
                        errorMessages.Add(msg);
                    }
                }
            }

            manifestProgress.Completed();

            if (errorMessages.Any())
            {
                log.Error(errorMessages);
            }

            if (successMessages.Any())
            {
                log.Info(successMessages);
            }

            return manifestList;
        }

        /// <summary>
        /// Fill the given menu with the scan forms that have been created for the account.  We list up to the last 7 days
        /// </summary>
        private async Task FillManifestMenuForAccount(SandMenu menu, ICarrierAccount account)
        {
            var manifests = await manifestRepo.GetManifests(account, MaxManifestsToFetch);

            foreach (var manifest in manifests)
            {
                try
                {
                    string manifestName = Format("{0:MM/dd/yy h:mm tt} ({1} shipments)", manifest.CreatedAt.ToLocalTime(), manifest.ShipmentCount);

                    SandMenuItem formMenuItem = new SandMenuItem(manifestName);
                    formMenuItem.Tag = manifest;

                    formMenuItem.Activate += new EventHandler(OnPrintManifest);
                    menu.Items.Add(formMenuItem);
                }
                catch (ShippingException ex)
                {
                    // This could occur if the batch was deleted. We just want to move on and not show this item in the menu
                    string message = Format("An exception was encountered while populating the manifest menu for account {0}. " +
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
                Print(manifest.ShipEngineManifestID);
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
        public void Print(long shipEngineManifestId)
        {
            List<Image> images = new List<Image>();


            foreach (DataResourceReference resource in DataResourceManager.LoadConsumerResourceReferences(shipEngineManifestId))
            {
                try
                {
                    string fileName = resource.GetCachedFilename();
                    log.Info($"About to print resource for DHL eCommerce manifest {shipEngineManifestId}.  Filename: {fileName}");

                    images.Add(Image.FromFile(fileName));
                }
                catch (Exception ex) when (ex is OutOfMemoryException || ex is FileNotFoundException || ex is ArgumentException)
                {
                    throw new ShippingException($"One of the images is invalid for manifest '{shipEngineManifestId}'.");
                }
            }

            PrintUtility.PrintImages(null, "ShipWorks - DHL eCommerce Manifest", images, true, true);
        }
    }
}
