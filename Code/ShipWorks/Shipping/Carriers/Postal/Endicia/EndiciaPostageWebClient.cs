using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Interacts with web services to buy postage and retrieve balance.
    /// </summary>
    public class EndiciaPostageWebClient : IPostageWebClient
    {
        private readonly EndiciaAccountEntity account;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaPostageWebClient"/> class.
        /// </summary>
        public EndiciaPostageWebClient(EndiciaAccountEntity account)
        {
            this.account = account;
        }

        /// <summary>
        /// Gets the shipment type code.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        {
            get { return account.EndiciaReseller == 0 ? ShipmentTypeCode.Endicia : ShipmentTypeCode.Express1Endicia; }
        }

        /// <summary>
        /// Gets the account identifier.
        /// </summary>
        public string AccountIdentifier
        {
            get
            {
                return account.AccountNumber;
            }
        }

        /// <summary>
        /// Gets the balance.
        /// </summary>
        public decimal GetBalance()
        {
            EndiciaApiClient client = new EndiciaApiClient();
            EndiciaAccountStatus accountInfo = client.GetAccountStatus(account);
            return accountInfo.PostageBalance;
        }

        /// <summary>
        /// Purchases the specified amount.
        /// </summary>
        public void Purchase(decimal amount)
        {
            EndiciaApiClient client = new EndiciaApiClient();

            client.BuyPostage(account, amount);
        }
    }
}
