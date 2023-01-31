using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Carriers.Services;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
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
                        var request = new FedExRegistrationRequest(account);

                        var response = await shipEngineWebClient.ConnectFedExAccount(request).ConfigureAwait(false);

                        if (response.Success)
                        {
                            account.ShipEngineCarrierID = response.Value;
                            accountRepo.Save(account);
                        }
                        else
                        {
                            throw response.Exception;
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
                    accountProgress.Failed(new Exception("Some accounts failed to migrate. See the log for details."));
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
    }
}
