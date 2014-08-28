using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Shipping.Carriers.Postal
{
    public class PostageBalance
    {
        private readonly IPostageWebClient postageWebClient;
        private readonly ITangoWebClient tangoWebClient;

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
        public void Purchase(decimal amount)
        {
            decimal balance = postageWebClient.GetBalance();
            postageWebClient.Purchase(amount);
            tangoWebClient.LogPostageEvent(balance, amount, postageWebClient.ShipmentTypeCode, postageWebClient.AccountIdentifier);
        }

        /// <summary>
        /// Balance with carrier
        /// </summary>
        public decimal Value
        {
            get
            {
                decimal balance = postageWebClient.GetBalance();
                tangoWebClient.LogPostageEvent(balance, 0, postageWebClient.ShipmentTypeCode, postageWebClient.AccountIdentifier);
                return balance;
            }
        }
    }
}