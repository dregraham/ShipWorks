using System;
using System.Collections.Generic;
using System.Linq;
using Divelements.SandRibbon;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using SandMenuItem = Divelements.SandRibbon.MenuItem;
using SandMenu = Divelements.SandRibbon.Menu;
using ShipWorks.UI;
using System.Windows.Forms;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using System.Drawing;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using Interapptive.Shared.UI;
using log4net;

namespace ShipWorks.Shipping.ScanForms
{
    public static class ScanFormUtility
    {
        private static ILog log = LogManager.GetLogger(typeof(ScanFormUtility));

        /// <summary>
        /// Populate the menu used to create scan forms
        /// </summary>
        public static void PopulateCreateScanFormMenu(SandMenuItem menu, IEnumerable<IScanFormAccountRepository> accountRepositories)
        {
            // Clear any previous items.  Bug in SandGrid... have to remove each explicitly
            menu.Items.Cast<WidgetBase>().ToList().ForEach(m => menu.Items.Remove(m));
            menu.Activate -= OnCreateScanForm;

            List<IScanFormCarrierAccount> accounts = new List<IScanFormCarrierAccount>();
            foreach (IScanFormAccountRepository accountRepository in accountRepositories)
            {
                accounts.AddRange(accountRepository.GetAccounts());
            }

            // If there is only one account, just make the menu do its thing
            if (accounts.Count() == 1)
            {
                menu.Tag = accounts.First();
                menu.Activate += OnCreateScanForm;

                if (!menu.Text.EndsWith("..."))
                {
                    menu.Text += "...";
                }
            }
            else if (accounts.Count() > 1)
            {
                // Add a child menu that contains each account
                SandMenu childMenu = new SandMenu();

                foreach (IScanFormCarrierAccount account in accounts)
                {
                    SandMenuItem menuItem = new SandMenuItem(account.GetDescription());
                    menuItem.Tag = account;                    
                    menuItem.Activate += OnCreateScanForm;

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
        /// Create a SCAN form
        /// </summary>
        private static void OnCreateScanForm(object sender, EventArgs e)
        {
            SandMenuItem menuItem = (SandMenuItem)sender;
            IScanFormCarrierAccount carrierAccount = (IScanFormCarrierAccount)menuItem.Tag;
            Control owner = DisplayHelper.GetActiveForm();

            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(owner,
                "Load Shipments",
                "ShipWorks is loading shipments for SCAN.",
                "Loading {0} of {1}");

            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            // What to do when its done
            executor.ExecuteCompleted += (object s, BackgroundExecutorCompletedEventArgs<long> args) =>
            {
                if (shipments.Count > 0)
                {
                    using (ScanFormDlg dlg = new ScanFormDlg(shipments, carrierAccount))
                    {
                        dlg.ShowDialog(owner);
                    }
                }
                else
                {
                    MessageHelper.ShowInformation(owner, "No shipments are currently eligible to be put on a SCAN form.");
                }
            };

            // Execution
            executor.ExecuteAsync
            (
                // Generate's the keys to process
                (IProgressReporter reporter) =>
                {
                    return carrierAccount.GetEligibleShipmentIDs().ToList();                    
                },

                // Process each input key
                (long shipmentID, object state, BackgroundIssueAdder<long> issueAdder) =>
                {
                    ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
                    if (shipment != null)
                    {
                        try
                        {
                            ShippingManager.EnsureShipmentLoaded(shipment);

                            PostalServiceType postalServiceType = (PostalServiceType) shipment.Postal.Service;

                            // Not a consolidator and not Endicia DHL
                            if (!ShipmentTypeManager.IsConsolidator(postalServiceType) &&
                                !(shipment.ShipmentType == (int)ShipmentTypeCode.Endicia && ShipmentTypeManager.IsEndiciaDhl(postalServiceType)))
                            {
                                shipments.Add(shipment);                                
                            }
                        }
                        catch (ObjectDeletedException)
                        {

                        }
                        catch (SqlForeignKeyException)
                        {

                        }
                    }
                }
            );
        }

        /// <summary>
        /// Populate the menu to allow a user to print a scan form that's already been generated
        /// </summary>
        public static void PopulatePrintScanFormMenu(SandMenu menu, IEnumerable<IScanFormAccountRepository> accountRepositories)
        {
            menu.Items.Clear();

            List<IScanFormCarrierAccount> accounts = new List<IScanFormCarrierAccount>();
            foreach (IScanFormAccountRepository accountRepository in accountRepositories)
            {
                accounts.AddRange(accountRepository.GetAccounts());
            }

            if (accounts.Count == 1)
            {
                FillScanFormMenuForAccount(menu, accounts[0]);
            }
            else if (accounts.Count > 1)
            {
                foreach (IScanFormCarrierAccount account in accounts)
                {
                    SandMenuItem accountMenuItem = new SandMenuItem(account.GetDescription());
                    menu.Items.Add(accountMenuItem);

                    SandMenu accountMenu = new SandMenu();
                    accountMenuItem.Items.Add(accountMenu);

                    FillScanFormMenuForAccount(accountMenu, account);
                }
            }
            else
            {
                menu.Items.Add(new SandMenuItem { Text = "(none)", Enabled = false });
            }
        }

        /// <summary>
        /// Fill the given menu with the scan forms that have been created for the account.  We list up to the last 7 days
        /// </summary>
        private static void FillScanFormMenuForAccount(SandMenu menu, IScanFormCarrierAccount account)
        {
            List<ScanFormBatch> batches = account.GetExistingScanFormBatches().OrderByDescending(b => b.CreatedDate).ToList();
            foreach (ScanFormBatch batch in batches)
            {
                try
                {
                    string batchName = string.Format("{0:MM/dd/yy h:mm tt} ({1} shipments)", batch.CreatedDate.ToLocalTime(), batch.ShipmentCount);

                    SandMenuItem formMenuItem = new SandMenuItem(batchName);
                    formMenuItem.Tag = batch;

                    formMenuItem.Activate += new EventHandler(OnPrintScanForm);
                    menu.Items.Add(formMenuItem);
                }
                catch (ShippingException ex)
                {
                    // This could occur if the batch was deleted. We just want to move on and not show this item in the menu
                    string message = string.Format("An exception was encountered while populating the scan form menu for account {0}. " +
                                                   "Skipping this scan form batch and continuing to load the menu.", account.GetDescription());

                    log.Error(message, ex);
                }
            }

            if (menu.Items.Count == 0)
            {
                menu.Items.Add(new SandMenuItem { Text = "(none)", Enabled = false });
            }
        }

        /// <summary>
        /// Print the scan form attached to the activating menu
        /// </summary>
        private static void OnPrintScanForm(object sender, EventArgs e)
        {
            SandMenuItem menuItem = (SandMenuItem)sender;
            Control owner = DisplayHelper.GetActiveForm();

            ScanFormBatch scanFormBatch = (ScanFormBatch)menuItem.Tag;
            scanFormBatch.Print(owner);
        }
    }
}
