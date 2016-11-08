using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Service for returning GlobalPost services based on UspsAccounts in the database
    /// </summary>
    public class GlobalPostAvailabilityService : IInitializeForCurrentSession
    {
        private readonly CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity> accountRepo;
        private readonly Func<UspsResellerType, IUspsWebClient> uspsWebClientFactory;
        private readonly ILog log;
        private readonly List<PostalServiceType> services;

        /// <summary>
        /// Constructor
        /// </summary>
        public GlobalPostAvailabilityService(
            CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity> accountRepo,
            Func<UspsResellerType, IUspsWebClient> uspsWebClientFactory, Func<Type, ILog> logfactory)
        {
            this.accountRepo = accountRepo;
            this.uspsWebClientFactory = uspsWebClientFactory;
            log = logfactory(typeof(GlobalPostAvailabilityService));
            services = new List<PostalServiceType>();
        }

        /// <summary>
        /// Initialize services for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            Refresh();
        }

        /// <summary>
        /// Refresh every UspsAccountEntitys GlobalPostAvailability status
        /// then update the list of available services
        /// </summary>
        public void Refresh()
        {
            services.Clear();
            IEnumerable<UspsAccountEntity> accounts = accountRepo.Accounts.ToArray();

            foreach (UspsAccountEntity account in accounts)
            {
                Refresh(account, true);
            }
        }

        /// <summary>
        /// Refresh the accounts GlobalPostAvailability status
        /// then update the list of available services
        /// </summary>
        public void Refresh(UspsAccountEntity account, bool refreshAccountGlobalPostAvailability)
        {
            if (refreshAccountGlobalPostAvailability)
            {
                RefreshAccountGlobalPostAvailability(account);
            }

            RefreashServicesFromAccount(account);
        }

        /// <summary>
        /// Refresh the list of services based on the UspsAccountEntity
        /// </summary>
        /// <param name="account"></param>
        private void RefreashServicesFromAccount(UspsAccountEntity account)
        {
            if (account.GlobalPostAvailability == (int) GlobalPostServiceAvailability.None)
            {
                return;
            }

            if (account.GlobalPostAvailability == (int) GlobalPostServiceAvailability.GlobalPost)
            {
                AddToServices(PostalServiceType.GlobalPostEconomy);
                AddToServices(PostalServiceType.GlobalPostPriority);
            }

            if (account.GlobalPostAvailability == (int) GlobalPostServiceAvailability.SmartSaver)
            {
                AddToServices(PostalServiceType.GlobalPostSmartSaverEconomy);
                AddToServices(PostalServiceType.GlobalPostSmartSaverPriority);
            }

            if (account.GlobalPostAvailability == (int) (GlobalPostServiceAvailability.SmartSaver | GlobalPostServiceAvailability.GlobalPost))
            {
                AddToServices(PostalServiceType.GlobalPostEconomy);
                AddToServices(PostalServiceType.GlobalPostPriority);
                AddToServices(PostalServiceType.GlobalPostSmartSaverEconomy);
                AddToServices(PostalServiceType.GlobalPostSmartSaverPriority);
            }
        }

        /// <summary>
        /// Refresh a UspsAccountEntitys GlobalPostAvailability
        /// </summary>
        private void RefreshAccountGlobalPostAvailability(UspsAccountEntity account)
        {
            if (account != null)
            {
                IUspsWebClient webClient = uspsWebClientFactory((UspsResellerType)account.UspsReseller);

                object result;
                try
                {
                    result = webClient.GetAccountInfo(account);
                }
                catch (UspsException ex)
                {
                    log.Error("Error updating GlobalPostAvailability", ex);
                    return;
                }

                AccountInfo accountInfo = result as AccountInfo;

                if (accountInfo != null)
                {
                    GlobalPostServiceAvailability gpAvailability = accountInfo.Capabilities.CanPrintGP ?
                        GlobalPostServiceAvailability.GlobalPost :
                        GlobalPostServiceAvailability.None;

                    GlobalPostServiceAvailability gpSmartSaverAvailability = accountInfo.Capabilities.CanPrintGPSmartSaver ?
                        GlobalPostServiceAvailability.SmartSaver :
                        GlobalPostServiceAvailability.None;

                    account.GlobalPostAvailability = (int) (gpAvailability | gpSmartSaverAvailability);
                }
            }
        }

        /// <summary>
        /// Check to see if a service exists, if not add it
        /// </summary>
        /// <param name="service"></param>
        private void AddToServices(PostalServiceType service)
        {
            if (!services.Contains(service))
            {
                services.Add(service);
            }
        }

        /// <summary>
        /// GlobalPost services that are available
        /// </summary>
        public IEnumerable<PostalServiceType> Services => services;
    }
}