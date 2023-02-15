using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Carriers.Services;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs.CarrierAccount;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Service to migrate FedEx accounts to ShipEngine
    /// </summary>
    [Component]
    public class FedExShipEngineMigrator : IFedExShipEngineMigrator
    {
        private readonly ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity> accountRepo;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShipEngineMigrator(ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity> accountRepo,
            IShipEngineWebClient shipEngineWebClient,
            Func<Type, ILog> logFactory)
        {
            this.accountRepo = accountRepo;
            this.shipEngineWebClient = shipEngineWebClient;
            log = logFactory(typeof(FedExShipEngineMigrator));
        }

        /// <summary>
        /// Perform migration of any FedEx accounts that haven't already been migrated
        /// </summary>
        public void Migrate(IWin32Window owner)
        {
            log.Info("Checking for FedEx accounts to migrate to ShipEngine");

            var accountsToMigrate = accountRepo.Accounts.Where(x => string.IsNullOrWhiteSpace(x.ShipEngineCarrierID));

            if (!accountsToMigrate.Any())
            {
                return;
            }

            ProgressProvider progressProvider = new ProgressProvider();
            IProgressReporter accountProgress = progressProvider.AddItem("Migrating FedEx Accounts to ShipEngine");
            accountProgress.CanCancel = false;
            using (ProgressDlg progressDialog = new ProgressDlg(progressProvider))
            {
                progressDialog.Title = "Migrating FedEx Accounts";
                progressDialog.AllowCloseWhenRunning = false;
                progressDialog.AutoCloseWhenComplete = true;

                Task.Run(async () =>
                {
                    await MigrateAccounts(accountProgress, accountsToMigrate).ConfigureAwait(true);
                });

                progressDialog.ShowDialog(owner);
            }
        }

        /// <summary>
        /// Perform migration of the accounts
        /// </summary>
        private async Task MigrateAccounts(IProgressReporter accountProgress, IEnumerable<FedExAccountEntity> accounts)
        {
            accountProgress.Starting();
            accountProgress.PercentComplete = 0;

            var totalAccounts = accounts.Count();
            var accountsFailed = false;

            try
            {
                var index = 1;

                accountProgress.Detail = $"Migrating account {index} of {totalAccounts}";

                var tasks = accounts.AsParallel().Select(async account =>
                {
                    try
                    {
                        var smartPostHubs = XElement.Parse(account.SmartPostHubList).Descendants("HubID").Select(n => (int) n).ToArray();
                        if (smartPostHubs.Length > 0)
                        {
                            accountsFailed = await MigrateSmartPostAccounts(account, smartPostHubs);
                        }
                        else
                        {
                            await ConnectAccountToEngine(account);
                            accountRepo.Save(account);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error($"FedEx account {account.AccountNumber} failed to migrate to ShipEngine: {ex.Message}", ex);
                        accountsFailed = true;
                    }

                    index++;
                    accountProgress.Detail = $"Migrating account {index} of {totalAccounts}";
                    accountProgress.PercentComplete = Math.Min((index * 100) / totalAccounts, 100);
                });

                await Task.WhenAll(tasks);

                accountProgress.Detail = "Done";

                if (accountsFailed)
                {
                    accountProgress.Failed(new Exception("Some accounts encountered issues migrating. See the log for details."));
                }
                else
                {
                    accountProgress.Completed();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Migrating FedEx accounts to ShipEngine failed: {ex.Message}", ex);
                accountProgress.Detail = "Done";
                accountProgress.Failed(ex);
            }
        }

        /// <summary>
        /// Create a new account for each SmartPost hub and migrate them to Engine
        /// </summary>
        private async Task<bool> MigrateSmartPostAccounts(FedExAccountEntity account, int[] hubs)
        {
            bool hubsFailed = false;

            //Migrate the existing account with the first hub
            try
            {
                var firstHub = hubs.First();
                account.SmartPostHub = firstHub;
                await ConnectAccountToEngine(account);
                await MigrateSmartPostHub(account);

                accountRepo.Save(account);
            }
            catch (Exception ex)
            {
                log.Error($"Failed to create a new FedEx smart post hub account for account: {account.FedExAccountID}, Hub: {account.SmartPostHub}", ex);
                hubsFailed = true;
            }

            if (hubs.Length > 1)
            {
                //Create new accounts for each other Hub 
                foreach (var hub in hubs.Skip(1))
                {
                    var newAccount = new FedExAccountEntity(account.Fields.CloneAsDirty());

                    newAccount.InitializeNullsToDefault();
                    //Set the accountId to 0 so the PK gets incremented correctly
                    newAccount.FedExAccountID = 0;

                    try
                    {
                        newAccount.SmartPostHub = hub;
                        var hubDescription = $" Hub: {EnumHelper.GetDescription((FedExSmartPostHub) hub)}";

                        if (FedExAccountManager.GetDefaultDescription(newAccount) == newAccount.Description)
                        {
                            var descriptionParts = newAccount.Description.Split(',').Take(2).ToList();
                            descriptionParts.Add(hubDescription);
                            newAccount.Description = string.Join(",", descriptionParts);
                        }
                        else
                        {
                            newAccount.Description = newAccount.Description + $",{hubDescription}";
                        }

                        newAccount.SmartPostHubList = $"<Root><HubID>{hub}</HubID></Root>";

                        await ConnectAccountToEngine(newAccount);
                        await MigrateSmartPostHub(newAccount);

                        accountRepo.Save(newAccount);
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Failed to create a new FedEx smart post hub account for account: {newAccount.FedExAccountID}, Hub: {newAccount.SmartPostHub}", ex);
                        hubsFailed = true;
                    }
                }
            }

            return hubsFailed;
        }

        /// <summary>
        /// Migrate a SmartPost Hub setting to Engine
        /// </summary>
        private async Task MigrateSmartPostHub(FedExAccountEntity account)
        {
            var updateResponse = await shipEngineWebClient.UpdateFedExAccount(account);

            if (!updateResponse.Success)
            {
                throw updateResponse.Exception;
            }
        }

        /// <summary>
        /// Connect a FedEx account to Engine
        /// </summary>
        private async Task ConnectAccountToEngine(FedExAccountEntity account)
        {
            var request = new FedExRegistrationRequest(account);
            var response = await shipEngineWebClient.ConnectFedExAccount(request).ConfigureAwait(false);

            if (response.Success)
            {
                account.ShipEngineCarrierID = response.Value;
            }
            else
            {
                throw response.Exception;
            }
        }
    }
}
