﻿using System;
using System.Threading.Tasks;
using log4net;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Validates shipment addresses
    /// </summary>
    public class ShipmentAddressValidator : IValidator<ShipmentEntity>
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentAddressValidator));

        private IValidatedAddressManager validatedAddressManager;
        private IAddressValidationWebClient addressValidationWebClient;
        private IFilterHelper filterHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentAddressValidator(IValidatedAddressManager validatedAddressManager, IAddressValidationWebClient addressValidationWebClient, IFilterHelper filterHelper)
        {
            this.validatedAddressManager = validatedAddressManager;
            this.addressValidationWebClient = addressValidationWebClient;
            this.filterHelper = filterHelper;
        }
        
        /// <summary>
        /// Validate the shipment asychronously.
        /// </summary>
        public Task ValidateAsync(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            // Validate Shipment
            return TaskEx.Run(() => ValidateShipments(shipment));
        }
        
        /// <summary>
        /// Validate all the shipments on a background thread
        /// </summary>
        private void ValidateShipments(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                return;
            }

            filterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

            AddressValidator addressValidator = new AddressValidator(addressValidationWebClient);

            validatedAddressManager.ValidateShipment(shipment, addressValidator);
        }
    }
}
