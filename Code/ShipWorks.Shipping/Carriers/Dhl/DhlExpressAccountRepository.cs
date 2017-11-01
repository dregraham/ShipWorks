using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// DhlExpress Account Repository
    /// </summary>
    [Component]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.DhlExpress)]
    [KeyedComponent(typeof(ICarrierAccountRepository<ShipEngineAccountEntity, IShipEngineAccountEntity>), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressAccountRepository : ShipEngineAccountRepository, IDhlExpressAccountRepository
    {
        /// <summary>
        /// Get carrier specific shipment type code
        /// </summary>
        protected override ShipmentTypeCode ShipmentType => ShipmentTypeCode.DhlExpress;

        /// <summary>
        /// Gets the account associated withe the default profile. A null value is returned
        /// if there is not an account associated with the default profile.
        /// </summary>
        public override ShipEngineAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = GetPrimaryProfile(ShipmentTypeCode.DhlExpress).DhlExpress.ShipEngineAccountID;
                return GetProfileAccount(ShipmentTypeCode.DhlExpress, accountID);
            }
        }

        /// <summary>
        /// Gets the account id from the shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment)
        {
            return shipment.DhlExpress.ShipEngineAccountID;
        }
    }
}
