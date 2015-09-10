using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using ShipWorks.AddressValidation;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Stores;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using System.Reflection;

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
        public async Task ValidateAsync(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            // Validate Shipment
            ValidateShipments(shipment);
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
