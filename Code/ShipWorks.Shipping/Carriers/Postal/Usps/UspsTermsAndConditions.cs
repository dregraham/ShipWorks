﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Autofac.Features.Indexed;
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
        private readonly ICarrierAccountRetriever<UspsAccountEntity, IUspsAccountEntity> accountRetriever;
        private readonly IIndex<ShipmentTypeCode, IUspsShipmentType> uspsShipmentTypes;
        private readonly IMessageHelper messageHelper;
        private readonly Dictionary<long, bool> accountAcceptanceCache;
        private readonly Object obj = new Object();

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsTermsAndConditions(ICarrierAccountRetriever<UspsAccountEntity, IUspsAccountEntity> accountRetriever,
            IIndex<ShipmentTypeCode, IUspsShipmentType> uspsShipmentTypes,
            IMessageHelper messageHelper)
        {
            accountAcceptanceCache = new Dictionary<long, bool>();
            this.accountRetriever = accountRetriever;
            this.uspsShipmentTypes = uspsShipmentTypes;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Validates that user has accepted the terms and conditions
        /// </summary>
        /// <remarks>
        /// If the account isn't cached, use the webclient to determine if user
        /// has needs to accept the Terms and Conditions and cache the result.
        /// Then, throw an error if the user needs to accept the TermsAndConditions.
        /// </remarks>
        public void Validate(IShipmentEntity shipment)
        {
            long accountID = shipment.Postal.Usps.UspsAccountID;

            lock (obj)
            {
                IUspsAccountEntity uspsAccount = accountRetriever.GetAccountReadOnly(shipment);

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
                    throw new UspsTermsAndConditionsException($"Please accept the terms and conditions for your Stamps.com account {uspsAccount.Username}.", this);
                }
            }
        }

        /// <summary>
        /// Create a dialog that will allow a customer to accept the terms and conditions for USPS
        /// </summary>
        public void Show()
        {
            IUspsAccountEntity account = accountAcceptanceCache
                .Where(kv => !kv.Value)?
                .Take(1)
                .Select(kv => kv.Key)
                .Select(accountRetriever.GetAccountReadOnly)
                .SingleOrDefault();

            if (account == null)
            {
                return;
            }

            ClearNotAccepted();

            IUspsWebClient webClient = uspsShipmentTypes[ShipmentTypeCode.Usps].CreateWebClient();
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
            foreach (long accountID in accountAcceptanceCache.Where(kv => !kv.Value).Select(kv => kv.Key).ToList())
            {
                accountAcceptanceCache.Remove(accountID);
            }
        }

        /// <summary>
        /// Open the URL in a browser in kiosk mode
        /// </summary>
        private static void OpenKioskBrowser(string url)
        {
            try
            {
                using (WebBrowser webBrowser = new WebBrowser())
                {
                    webBrowser.Url = new Uri("about:blank");
                    webBrowser.Document?.Window?.OpenNew(url, "location=no,menubar=no,scrollbars=yes,status=yes,toolbar=no,resizable=yes");
                }
            }
            catch (COMException)
            {
                Process.Start(url);
            }
        }

        /// <summary>
        /// Returns true if Terms are accepted
        /// </summary>
        private bool AreTermsAccepted(IUspsAccountEntity uspsAccount)
        {
            IUspsWebClient webClient = uspsShipmentTypes[ShipmentTypeCode.Usps].CreateWebClient();
            AccountInfoV65 accountInfo = (AccountInfoV65) webClient.GetAccountInfo(uspsAccount);
            return accountInfo.Terms.TermsAR && accountInfo.Terms.TermsSL && accountInfo.Terms.TermsGP;
        }
    }
}
