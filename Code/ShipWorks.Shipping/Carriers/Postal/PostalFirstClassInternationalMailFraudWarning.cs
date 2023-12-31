﻿using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.UI.Carriers.Postal.Usps;
using System;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Warn users not to commit Mail Fraud for first class international shipments
    /// </summary>
    [Component]
    public class PostalFirstClassInternationalMailFraudWarning : IPostalFirstClassInternationalMailFraudWarning
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository;
        private readonly ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> endiciaAccountRepository;
        private readonly Func<IFirstClassInternationalWarningDialog> warningDialogFactory;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public PostalFirstClassInternationalMailFraudWarning(
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository,
            ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> endiciaAccountRepository,
            Func<IFirstClassInternationalWarningDialog> warningDialogFactory,
            IMessageHelper messageHelper)
        {
            this.uspsAccountRepository = uspsAccountRepository;
            this.endiciaAccountRepository = endiciaAccountRepository;
            this.warningDialogFactory = warningDialogFactory;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Warn the user
        /// </summary>
        public void Warn(IShipmentEntity shipment)
        {
            if (IsLetter(shipment) &&
                shipment.Postal.Service == (int) PostalServiceType.InternationalFirst && 
                shipment.Postal.CustomsContentType == (int) PostalCustomsContentType.Documents &&
                ShouldShowWarningForShipmentsAccount(shipment))
            {
                bool? result = messageHelper.ShowDialog(() => warningDialogFactory());
                
                if (!result.HasValue || !result.Value)
                {
                    throw new ShippingException("Please update the customs general Content: type, the shipment Packaging: type, and/or the Service: type prior to processing the shipment.");
                }

                MarkWarningAsAccepted(shipment);
            }
        }
        
        /// <summary>
        /// Should we show the warning for this shipments account
        /// </summary>
        private bool ShouldShowWarningForShipmentsAccount(IShipmentEntity shipment)
        {
            if (shipment.ShipmentTypeCode == ShipmentTypeCode.Usps)
            {
                // If rate shopping is enabled and there is at least one account that hasnt accepted the FCMI Letter warning
                if (shipment.Postal.Usps.RateShop && uspsAccountRepository.AccountsReadOnly.Any(a => a.AcceptedFCMILetterWarning == false))
                {
                    return true;
                }

                // Rate shopping is not enabled, check to see if the shipments account has accepted
                if (!uspsAccountRepository.GetAccountReadOnly(shipment).AcceptedFCMILetterWarning)
                {
                    return true;
                }
            }

            if (shipment.ShipmentTypeCode == ShipmentTypeCode.Endicia)
            {
                IEndiciaAccountEntity account = endiciaAccountRepository.GetAccountReadOnly(shipment);
                if (!account.AcceptedFCMILetterWarning && account.EndiciaReseller == (int) EndiciaReseller.None)
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Mark the account for the shipment as accepted
        /// </summary>
        private void MarkWarningAsAccepted(IShipmentEntity shipment)
        {
            if (shipment.ShipmentTypeCode == ShipmentTypeCode.Usps)
            {
                if (shipment.Postal.Usps.RateShop)
                {
                    uspsAccountRepository.Accounts.ForEach(MarkWarningAsAcceptedForUspsAccount);
                }
                else
                {
                    UspsAccountEntity account = uspsAccountRepository.GetAccount(shipment);
                    MarkWarningAsAcceptedForUspsAccount(account);
                }
            }

            if (shipment.ShipmentTypeCode == ShipmentTypeCode.Endicia)
            {
                EndiciaAccountEntity account = endiciaAccountRepository.GetAccount(shipment);
                MarkWarningAsAcceptedForEndiciaAccount(account);
            }
        }

        /// <summary>
        /// Mark the account as accepting the letter/content type warning
        /// </summary>
        /// <param name="account"></param>
        private void MarkWarningAsAcceptedForUspsAccount(UspsAccountEntity account)
        {
            account.AcceptedFCMILetterWarning = true;
            uspsAccountRepository.Save(account);
        }

        /// <summary>
        /// Mark the account as accepting the letter/content type warning
        /// </summary>
        /// <param name="account"></param>
        private void MarkWarningAsAcceptedForEndiciaAccount(EndiciaAccountEntity account)
        {
            account.AcceptedFCMILetterWarning = true;
            endiciaAccountRepository.Save(account);
        }

        /// <summary>
        /// Is this shipment using a Letter packaging type
        /// </summary>
        private static bool IsLetter(IShipmentEntity shipment)
        {
            PostalPackagingType package = (PostalPackagingType) shipment.Postal.PackagingType;
            return package == PostalPackagingType.Envelope || package == PostalPackagingType.LargeEnvelope;
        }
    }
}
