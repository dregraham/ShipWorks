using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Usps Terms and Conditions
    /// </summary>
    [Component(SingleInstance = true)]
    public class UspsTermsAndConditions : IUspsTermsAndConditions
    {
        private readonly UspsWebClient webClient;
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository;
        private readonly IMessageHelper messageHelper;
        private readonly Dictionary<long, bool> accountAcceptanceCache;

        private readonly Object obj = new Object();

        public UspsTermsAndConditions(
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository, 
            UspsShipmentType uspsShipmentType,
            IMessageHelper messageHelper)
        {
            accountAcceptanceCache = new Dictionary<long, bool>();
            this.accountRepository = accountRepository;
            this.messageHelper = messageHelper;
            webClient = (UspsWebClient) uspsShipmentType.CreateWebClient();
        }

        /// <summary>
        /// Validates that user has accepted the terms and conditions
        /// </summary>
        /// <remarks>
        /// If the account isn't cached, use the webclient to determine if user
        /// has needs to accept the Terms and Conditions and cache the result.
        /// Then, throw an error if the user needs to accept the TermsAndConditions.
        /// </remarks>
        public void Validate(ShipmentEntity shipment)
        {
            long accountID = shipment.Postal.Usps.UspsAccountID;

            lock (obj)
            {
                UspsAccountEntity uspsAccount = accountRepository.GetAccount(accountID);

                if (!accountAcceptanceCache.TryGetValue(accountID, out bool accepted))
                {
                    if (AreTermsAccepted(uspsAccount))
                    {
                        accountAcceptanceCache.Add(accountID, true);
                        accepted = true;
                    }
                    else
                    {
                        accepted = false;
                        accountAcceptanceCache.Add(accountID, false);
                    }
                }

                if (!accepted)
                {
                    throw new UspsTermsAndConditionsException(uspsAccount, "Terms and conditions must be accepted.", this);
                }
            }
        }

        /// <summary>
        /// Create a dialog that will allow a customer to accept the terms and conditions for USPS
        /// </summary>
        public void Open()
        {
            UspsAccountEntity account = accountAcceptanceCache
                .Where(kv => !kv.Value)?
                .Take(1)
                .Select(kv=>kv.Key)
                .Select(accountRepository.GetAccount)
                .SingleOrDefault();

            if (account == null)
            {
                // Maybe throw???
                return;
            }

            ClearNotAccepted();

            string url = webClient.GetUrl(account, UrlType.SetTermsGeneral);

            try
            {
                Thread thread = new Thread(ExceptionMonitor.WrapThread(() => OpenKioskBrowser(url)));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            catch (Exception ex)
            {
                messageHelper.ShowError($"Unable to open Terms and Conditions page. {ex.Message}");
            }
        }

        /// <summary>
        /// Removes cached accounts that have not been accepted.
        /// </summary>
        private void ClearNotAccepted()
        {
            foreach (long accountID in accountAcceptanceCache.Where(kv=>!kv.Value).Select(kv=>kv.Key).ToList())
            {
                accountAcceptanceCache.Remove(accountID);
            }
        }

        /// <summary>
        /// Open the url in a browser in kiosk mode
        /// </summary>
        private static void OpenKioskBrowser(string url)
        {
            using (WebBrowser wb = new WebBrowser())
            {
                wb.Url = new Uri("about:blank");
                wb.Document?.Window?.OpenNew(url, "location=no,menubar=no,scrollbars=yes,status=yes,toolbar=no,resizable=yes");
            }
        }

        /// <summary>
        /// Returns true if Terms are accepted
        /// </summary>
        private bool AreTermsAccepted(UspsAccountEntity uspsAccount)
        {
            AccountInfoV25 accountInfo = (AccountInfoV25) webClient.GetAccountInfo(uspsAccount);
            return false && accountInfo.Terms.TermsAR && accountInfo.Terms.TermsSL && accountInfo.Terms.TermsGP;
        }
    }
}
