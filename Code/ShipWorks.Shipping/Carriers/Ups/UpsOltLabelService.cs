﻿using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// LabelService for Ups Online Tools
    /// </summary>
    public class UpsOltLabelService : UpsLabelService
    {
        private readonly IUpsOltShipmentValidator upsOltShipmentValidator;
        private readonly IUpsLabelClientFactory labelClientFactory;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOltLabelService(IUpsOltShipmentValidator upsOltShipmentValidator,
            IUpsLabelClientFactory labelClientFactory,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository)
        {
            this.upsOltShipmentValidator = upsOltShipmentValidator;
            this.labelClientFactory = labelClientFactory;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Creates a label for Ups Online Tools
        /// </summary>
        public override Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            try
            {
                // Call the base class for setting default values as needed based on the service/package type of the shipment
                base.Create(shipment);

                upsOltShipmentValidator.ValidateShipment(shipment);

                UpsServicePackageTypeSetting.Validate(shipment);

                IUpsAccountEntity account = accountRepository.GetAccountReadOnly(shipment);

                return labelClientFactory.GetClient(account).GetLabel(shipment);
            }
            catch (UpsApiException ex)
            {
                string message = ex.Message;

                // find the "XML document is well formed but not valid" error
                if (ex.ErrorCode == "10002" &&
                    shipment.ReturnShipment &&
                    !string.IsNullOrEmpty(ex.ErrorLocation) &&
                    string.Equals(ex.ErrorLocation, "ShipmentConfirmRequest/Shipment/Package/Description", StringComparison.OrdinalIgnoreCase))
                {
                    message = "The return shipment's Contents is required.";
                }

                throw new ShippingException(message, ex);
            }
            catch (CarrierException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
    }
}