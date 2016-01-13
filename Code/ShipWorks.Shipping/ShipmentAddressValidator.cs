using System;
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

        private readonly IValidatedAddressManager validatedAddressManager;
        private readonly IFilterHelper filterHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentAddressValidator(IValidatedAddressManager validatedAddressManager, IFilterHelper filterHelper)
        {
            this.validatedAddressManager = validatedAddressManager;
            this.filterHelper = filterHelper;
        }

        /// <summary>
        /// Validate the shipment asynchronously.
        /// </summary>
        public Task ValidateAsync(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            // Validate Shipment
            return ValidateShipmentsAsync(shipment);
        }

        /// <summary>
        /// Validate all the shipments on a background thread
        /// </summary>
        private async Task ValidateShipmentsAsync(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                return;
            }

            filterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

            await validatedAddressManager.ValidateShipmentAsync(shipment)
                .ConfigureAwait(false);
        }
    }
}
