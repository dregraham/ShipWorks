using GalaSoft.MvvmLight.Threading;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.UI.Carriers.Postal.Usps;
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
    [Component]
    public class UspsFirstClassInternationalShipmentValidator : IUspsFirstClassInternationalShipmentValidator
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository;
        private readonly Func<IFirstClassInternationalWarningDialog> warningFactory;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsFirstClassInternationalShipmentValidator(
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository,
            Func<IFirstClassInternationalWarningDialog> warningFactory,
            IMessageHelper messageHelper)
        {
            this.accountRepository = accountRepository;
            this.warningFactory = warningFactory;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Validate the shipment
        /// </summary>
        public void ValidateShipment(IShipmentEntity shipment)
        {
            if (IsLetter(shipment) &&
                shipment.Postal.Service == (int) PostalServiceType.InternationalFirst && 
                shipment.Postal.CustomsContentType == (int) PostalCustomsContentType.Documents &&
                ShouldShowWarningForShipmentsAccount(shipment))
            {
                bool? result = messageHelper.ShowDialog(() => warningFactory());
                
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
        private void MarkWarningAsAccepted(IShipmentEntity shipment)
        {
            if (shipment.ShipmentTypeCode == ShipmentTypeCode.Usps)
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

            if (shipment.ShipmentTypeCode == ShipmentTypeCode.Endicia)
            {

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
        private static bool IsLetter(IShipmentEntity shipment)
        {
            PostalPackagingType package = (PostalPackagingType) shipment.Postal.PackagingType;
            return package == PostalPackagingType.Envelope || package == PostalPackagingType.LargeEnvelope;
        }
    }
}
