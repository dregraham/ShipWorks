using GalaSoft.MvvmLight.Threading;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Validate First Class International shipments, ensure the user has accepted to not commit mail fraud
    /// </summary>
    public class UspsFirstClassInternationalShipmentValidator : IUspsFirstClassInternationalShipmentValidator
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository;
        private readonly Func<string, IDialog> warningFactory;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsFirstClassInternationalShipmentValidator(
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository,
            Func<string, IDialog> warningFactory,
            IMessageHelper messageHelper)
        {
            this.accountRepository = accountRepository;
            this.warningFactory = warningFactory;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Validate the shipment
        /// </summary>
        public void ValidateShipment(ShipmentEntity shipment)
        {
            if (IsLetter(shipment) &&
                shipment.Postal.Service == (int) PostalServiceType.InternationalFirst && 
                shipment.Postal.CustomsContentType == (int) PostalCustomsContentType.Documents &&
                ShouldShowWarningForShipmentsAccount(shipment))
            {
                bool? result = messageHelper.ShowDialog(() => warningFactory("FirstClassInternationalWarningDlg"));
                
                if (!result.Value)
                {
                    throw new ShippingException("Please change the customs content type to something other than Documents or the packaging type to something other than letter.");
                }
            }
        }
        
        /// <summary>
        /// Should we show the warning for this shipments account
        /// </summary>
        private bool ShouldShowWarningForShipmentsAccount(ShipmentEntity shipment)
        {
            // If rate shopping is enabled and there is at least one account that hasnt accepted the FCMI Letter warning
            if (shipment.Postal.Usps.RateShop && accountRepository.AccountsReadOnly.Any(a => a.AcceptedFCMILetterWarning == false))
            {
                return true;
            }

            // Rate shopping is not enabled, check to see if the shipments account has accepted
            if (!accountRepository.GetAccountReadOnly(shipment).AcceptedFCMILetterWarning)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Mark the account for the shipment as accepted
        /// </summary>
        private void MarkWarningAsAccepted(ShipmentEntity shipment)
        {
            if (shipment.Postal.Usps.RateShop)
            {
                accountRepository.Accounts.ForEach(MarkWarningAsAccepted);
            }
            else
            {
                UspsAccountEntity account = accountRepository.GetAccount(shipment);
                MarkWarningAsAccepted(account);
            }
        }

        /// <summary>
        /// Mark the account as accepting the letter/content type warning
        /// </summary>
        /// <param name="account"></param>
        private void MarkWarningAsAccepted(UspsAccountEntity account)
        {
            account.AcceptedFCMILetterWarning = true;
            accountRepository.Save(account);
        }

        /// <summary>
        /// Is this shipment using a Letter packaging type
        /// </summary>
        private static bool IsLetter(ShipmentEntity shipment)
        {
            PostalPackagingType package = (PostalPackagingType) shipment.Postal.PackagingType;
            return package == PostalPackagingType.Envelope || package == PostalPackagingType.LargeEnvelope;
        }
    }
}
