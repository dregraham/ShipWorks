using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// An IShipmentProcessingSynchronizer implementation to handle the PreProcessing of a
    /// Amazon shipment 
    /// </summary>
    public class AmazonShipmentProcessingSynchronizer : IShipmentProcessingSynchronizer
    {
        private readonly IAmazonAccountManager amazonAccountManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonShipmentProcessingSynchronizer"/> class.
        /// </summary>
        /// <param name="amazonAccountManager">The account manager.</param>
        public AmazonShipmentProcessingSynchronizer(IAmazonAccountManager amazonAccountManager)
        {
            this.amazonAccountManager = amazonAccountManager;
        }

        /// <summary>
        /// Gets a value indicating whether the shipment type [has accounts].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has accounts]; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccounts
        {
            get { return amazonAccountManager.Accounts.Any(); }
        }

        /// <summary>
        /// Saves the first account ID found to the shipment. The presumption here is that a new 
        /// account was just created during the processing of a shipment, so we just want 
        /// to grab the first account (the one just created) and use it to process the shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public void SaveAccountToShipment(ShipmentEntity shipment)
        {
            if (amazonAccountManager.Accounts.Any())
            {
                // Grab the first account in the repository to set the account ID
                shipment.Amazon.AmazonAccountID = amazonAccountManager.Accounts.First().AmazonAccountID;
            }
            else
            {
                throw new AmazonException("An Amazon account must be created to process this shipment.");
            }
        }

        /// <summary>
        /// If the account on the shipment is no longer valid, and there is an account available,
        /// this method will change the account to be the first account.
        /// </summary>
        public void ReplaceInvalidAccount(ShipmentEntity shipment)
        {
            if (HasAccounts && amazonAccountManager.GetAccount(shipment.Amazon.AmazonAccountID) == null)
            {
                SaveAccountToShipment(shipment);
            }
        }
    }
}
