using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.ShipEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Account Repository for Asendia
    /// </summary>
    [Component]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.Asendia)]
    [KeyedComponent(typeof(ICarrierAccountRepository<ShipEngineAccountEntity, IShipEngineAccountEntity>), ShipmentTypeCode.Asendia)]
    public class AsendiaAccountRepository : ShipEngineAccountRepository, IAsendiaAccountRepository
    {
        /// <summary>
        /// Get carrier specific shipment type code
        /// </summary>
        protected override ShipmentTypeCode shipmentType => ShipmentTypeCode.Asendia;

        /// <summary>
        /// Gets the account associated withe the default profile. A null value is returned
        /// if there is not an account associated with the default profile.
        /// </summary>
        public override ShipEngineAccountEntity DefaultProfileAccount
        {
            get
            {
                // todo: when profiles and shipments are implemented, follow DhlRepo
                return null;
            }
        }

        /// <summary>
        /// Gets the account id from the shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }
    }
}
