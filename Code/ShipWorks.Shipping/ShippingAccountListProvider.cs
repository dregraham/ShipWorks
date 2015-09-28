using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Provides an IObservable collection of accounts for the shipment type
    /// </summary>
    public class ShippingAccountListProvider
    {
        private readonly IIndex<ShipmentTypeCode, ICarrierAccountRepository<ICarrierAccount>> lookup;
        private ShipmentTypeCode shipmentTypeCode;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingAccountListProvider(IIndex<ShipmentTypeCode, ICarrierAccountRepository<ICarrierAccount>> lookup)
        {
            this.lookup = lookup;
            AvailableAccounts = new ObservableCollection<ICarrierAccount>();
        }

        /// <summary>
        /// Gets the available accounts for the ShipmentTypeCode
        /// </summary>
        public ObservableCollection<ICarrierAccount> AvailableAccounts { get; }

        /// <summary>
        /// Gets or sets the shipment type code.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        {
            get { return shipmentTypeCode; }
            set
            {
                shipmentTypeCode = value; 
                AvailableAccounts.Clear();
                AvailableAccounts.AddRange(lookup[shipmentTypeCode].Accounts);
            }
        }
    }
}
