using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Interacts with web services to buy postage and retrieve balance.
    /// </summary>
    public class StampsPostageWebClient : IPostageWebClient
    {
        private readonly StampsAccountEntity account;
        private decimal? lastKnownBalance;

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsPostageWebClient" /> class.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="lastKnownBalance">Required if purchasing.</param>
        public StampsPostageWebClient(StampsAccountEntity account, decimal? lastKnownBalance)
        {
            this.account = account;
            this.lastKnownBalance = lastKnownBalance;
        }

        /// <summary>
        /// Gets the shipment type code.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        {
            get 
            {
                return account.IsExpress1 ? ShipmentTypeCode.Express1Stamps : ShipmentTypeCode.Stamps;
            }
        }

        /// <summary>
        /// Gets the account identifier.
        /// </summary>
        public string AccountIdentifier
        {
            get { return account.Username; }
        }

        /// <summary>
        /// Gets the balance.
        /// </summary>
        public decimal GetBalance()
        {
            StampsApiSession client = new StampsApiSession();
            AccountInfo accountInfo = client.GetAccountInfo(account);

            lastKnownBalance = accountInfo.PostageBalance.AvailablePostage;
            
            return lastKnownBalance.Value;
        }

        /// <summary>
        /// Purchases the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void Purchase(decimal amount)
        {
            StampsApiSession client = new StampsApiSession();

            if (!lastKnownBalance.HasValue)
            {
                lastKnownBalance = GetBalance();
            }

            // We might want to add GetBalance as a parameter. 
            client.PurchasePostage(account, amount, lastKnownBalance.Value);
        }
    }
}
