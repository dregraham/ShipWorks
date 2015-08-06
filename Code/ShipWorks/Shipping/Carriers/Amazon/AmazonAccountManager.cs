using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Manager for working with Amazon accounts
    /// </summary>
    public class AmazonAccountManager : AccountManagerBase<AmazonAccountEntity>, IAmazonAccountManager
    {
        /// <summary>
        /// Get the default description for the given account
        /// </summary>
        public override string GetDefaultDescription(AmazonAccountEntity account)
        {
            return string.Format("Account {0}", account.MerchantID);
        }

        /// <summary>
        /// Get the index of the id field
        /// </summary>
        protected override int AccountIdFieldIndex
        {
            get
            {
                return (int)AmazonAccountFieldIndex.AmazonAccountID;
            }
        }
    }
}