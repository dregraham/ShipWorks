using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Ups.OneBalance;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Factory for creating Ups Label Clients
    /// </summary>
    [Component]
    public class UpsLabelClientFactory : IUpsLabelClientFactory
    {
        private readonly Func<UpsShipEngineLabelClient> seLabelClientFactory;
        private readonly Func<UpsOltLabelClient> oltLabelClientFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLabelClientFactory(Func<UpsShipEngineLabelClient> seLabelClientFactory, Func<UpsOltLabelClient> oltLabelClientFactory)
        {
            this.seLabelClientFactory = seLabelClientFactory;
            this.oltLabelClientFactory = oltLabelClientFactory;
        }

        /// <summary>
        /// Get a label client for the given account
        /// </summary>
        public IUpsLabelClient GetClient(IUpsAccountEntity account)
        {
            if (account.ShipEngineCarrierId != null)
            {
                return seLabelClientFactory();
            }

            return oltLabelClientFactory();
        }
    }
}
