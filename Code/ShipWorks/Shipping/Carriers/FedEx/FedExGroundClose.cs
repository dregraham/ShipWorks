using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using Divelements.SandRibbon;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Utility class for dealing with FedEx end of day stuff
    /// </summary>
    public static class FedExGroundClose
    {
        /// <summary>
        /// Process the end of day close.  Return true if there were any shipments to be closed
        /// </summary>
        public static List<FedExEndOfDayCloseEntity> ProcessClose()
        {
            List<FedExEndOfDayCloseEntity> closings = new List<FedExEndOfDayCloseEntity>();
            
            IFedExShippingClerk shippingClerk = FedExShippingClerkFactory.CreateShippingClerk(null, new FedExSettingsRepository());

            if (FedExAccountManager.Accounts.Count > 0 && FedExAccountManager.Accounts.All(a => a.Is2xMigrationPending))
            {
                throw new FedExException(string.Format("Your FedEx {0} migrated from ShipWorks 2, but have not yet been configured for ShipWorks 3.", 
                    FedExAccountManager.Accounts.Count > 1 ? "accounts were" : "account was"));
            }

            
            foreach (FedExAccountEntity account in FedExAccountManager.Accounts.Where(a => !a.Is2xMigrationPending))
            {
                FedExEndOfDayCloseEntity closing = shippingClerk.CloseGround(account);
                if (closing != null)
                {
                    closings.Add(closing);
                }
            }

            return closings;
        }

        /// <summary>
        /// Populate the given menu with all the menu items to print fedex end of day reports
        /// </summary>
        public static void PopulatePrintReportsMenu(Menu parentMenu)
        {
            parentMenu.Items.Clear();

            if (FedExAccountManager.Accounts.Count == 1)
            {
                PopulateFedExPrintCloseReportMenu(parentMenu, FedExAccountManager.Accounts[0]);
            }
            else
            {
                foreach (FedExAccountEntity account in FedExAccountManager.Accounts.Where(a => !a.Is2xMigrationPending))
                {
                    MenuItem accountItem = new MenuItem(account.Description);
                    parentMenu.Items.Add(accountItem);

                    Menu accountMenu = new Menu();
                    accountItem.Items.Add(accountMenu);

                    PopulateFedExPrintCloseReportMenu(accountMenu, account);
                }
            }

            if (parentMenu.Items.Count == 0)
            {
                parentMenu.Items.Add(new MenuItem("(None)") { Enabled = false });
            }
        }

        /// <summary>
        /// Populate the menu to print end of day reports for the given account
        /// </summary>
        private static void PopulateFedExPrintCloseReportMenu(Menu parent, FedExAccountEntity account)
        {
            foreach (FedExEndOfDayCloseEntity closing in GetRecentlyClosed(account))
            {
                string dateTime = StringUtility.FormatFriendlyDateTime(closing.CloseDate);

                MenuItem printDate = new MenuItem(dateTime);
                printDate.Activate += new EventHandler(OnPrintFedexEndOfDay);
                printDate.Tag = closing;

                parent.Items.Add(printDate);
            }

            // If none, add a default empty item.
            if (parent.Items.Count == 0)
            {
                parent.Items.Add(new MenuItem("(None)") { Enabled = false });
            }
        }

        /// <summary>
        /// Get the list of recently performed fedex closings
        /// </summary>
        private static IEnumerable<FedExEndOfDayCloseEntity> GetRecentlyClosed(FedExAccountEntity account)
        {
            FedExEndOfDayCloseCollection closings = FedExEndOfDayCloseCollection.Fetch(SqlAdapter.Default,
                FedExEndOfDayCloseFields.FedExAccountID == account.FedExAccountID &
                FedExEndOfDayCloseFields.CloseDate >= DateTime.UtcNow.AddDays(-5).Date & 
                FedExEndOfDayCloseFields.IsSmartPost == false);

            return closings.OrderByDescending(c => c.CloseDate);
        }

        /// <summary>
        /// Print a Fedex end of day report
        /// </summary>
        private static void OnPrintFedexEndOfDay(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem) sender;
            FedExEndOfDayCloseEntity closing = (FedExEndOfDayCloseEntity) menuItem.Tag;

            PrintCloseReports(closing);
        }

        /// <summary>
        /// Print the close reports for the given closing.  The user will be prompted for print settings.
        /// </summary>
        public static void PrintCloseReports(FedExEndOfDayCloseEntity closing)
        {
            PrintCloseReports(new FedExEndOfDayCloseEntity[] { closing });
        }

         /// <summary>
        /// Print the close reports for all of the given closings.  The user will be prompted for print settings.
        /// </summary>
        public static void PrintCloseReports(IEnumerable<FedExEndOfDayCloseEntity> closings)
        {
            StringBuilder contentToPrint = new StringBuilder();

            foreach (FedExEndOfDayCloseEntity closing in closings)
            {
                foreach (DataResourceReference resource in DataResourceManager.LoadConsumerResourceReferences(closing.FedExEndOfDayCloseID))
                {
                    contentToPrint.Append(resource.ReadAllText());
                }
            }

            PrintUtility.PrintText(null, "ShipWorks - FedEx Reports", contentToPrint.ToString(), false);
        }
    }
}
