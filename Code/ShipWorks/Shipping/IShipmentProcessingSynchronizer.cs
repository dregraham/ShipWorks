using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// An interface intended to be used when processing a shipment, specifically during the
    /// transition phase between seeing a counter rate and completing the setup wizard to
    /// create an account.
    /// </summary>
    public interface IShipmentProcessingSynchronizer
    {
        /// <summary>
        /// Gets a value indicating whether the shipment type [has accounts].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has accounts]; otherwise, <c>false</c>.
        /// </value>
        bool HasAccounts { get; }

        /// <summary>
        /// Saves the first account ID found to the shipment. The presumption here is that a new 
        /// account was just created during the processing of a shipment, so we just want 
        /// to grab the first account (the one just created) and use it to process the shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        void SaveAccountToShipment(ShipmentEntity shipment);

        /// <summary>
        /// If the account on the shipment is no longer valid, and there is only one account available,
        /// this method will change the account to be that one account.
        /// </summary>
        void ReplaceInvalidAccount(ShipmentEntity shipment);
    }
}
