using System;
using System.Threading.Tasks;
using log4net;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Shipping.Carriers.Postal
{
    public class PostageBalance
    {
        private readonly IPostageWebClient postageWebClient;
        private readonly ITangoWebClient tangoWebClient;

        private static readonly ILog log = LogManager.GetLogger(typeof(PostageBalance));

        /// <summary>
        /// Initializes a new instance of the <see cref="PostageBalance"/> class.
        /// </summary>
        public PostageBalance(IPostageWebClient postageWebClient, ITangoWebClient tangoWebClient)
        {
            this.postageWebClient = postageWebClient;
            this.tangoWebClient = tangoWebClient;
        }

        /// <summary>
        /// Purchases the specified amount.
        /// </summary>
        public Task Purchase(decimal amount)
        {
            decimal balance = postageWebClient.GetBalance();
            postageWebClient.Purchase(amount);

            return LogAsync(amount, balance);
        }

        /// <summary>
        /// Balance with carrier
        /// </summary>
        public decimal Value
        {
            get
            {
                decimal balance = postageWebClient.GetBalance();
                LogAsync(0, balance);

                return balance;
            }
        }

        /// <summary>
        /// Balance with carrier run in background
        /// </summary>
        public Task<decimal> GetValueAsync()
        {
            return new TaskFactory().StartNew(() =>
            {
                return Value;
            });
        }

        /// <summary>
        /// Uses the Tango web client to log the postage in a fire and forget manner, so any upstream processing
        /// is not waiting on Tango to respond.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="balance">The balance.</param>
        private Task LogAsync(decimal amount, decimal balance)
        {
            return new TaskFactory().StartNew(() =>
            {
                try
                {
                    tangoWebClient.LogPostageEvent(balance, amount, postageWebClient.ShipmentTypeCode, postageWebClient.AccountIdentifier);
                }
                catch (Exception ex)
                {
                    log.Error("Error logging PostageEvent to Tango.", ex);
                }
            });
        }
    }
}