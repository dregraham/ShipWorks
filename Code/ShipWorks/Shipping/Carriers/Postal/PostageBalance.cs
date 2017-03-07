using System.Threading.Tasks;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Postage balance
    /// </summary>
    public class PostageBalance
    {
        private readonly IPostageWebClient postageWebClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostageBalance"/> class.
        /// </summary>
        public PostageBalance(IPostageWebClient postageWebClient)
        {
            this.postageWebClient = postageWebClient;
        }

        /// <summary>
        /// Purchases the specified amount.
        /// </summary>
        public void Purchase(decimal amount)
        {
            decimal balance = postageWebClient.GetBalance();
            postageWebClient.Purchase(amount);
        }

        /// <summary>
        /// Balance with carrier
        /// </summary>
        public decimal Value
        {
            get
            {
                return postageWebClient.GetBalance();
            }
        }

        /// <summary>
        /// Balance with carrier run in background
        /// </summary>
        public Task<decimal> GetValueAsync()
        {
            return Task.Run(() => Value);
        }
    }
}