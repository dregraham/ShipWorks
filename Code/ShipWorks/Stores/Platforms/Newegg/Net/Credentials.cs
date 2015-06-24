using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Stores.Platforms.Newegg.Net
{
    /// <summary>
    /// A class containing the credential information required for submitting requests 
    /// to the Newegg API.
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Credentials"/> class.
        /// </summary>
        /// <param name="sellerId">The seller ID.</param>
        /// <param name="secretKey">The secret key.</param>
        public Credentials(string sellerId, string secretKey, NeweggChannelType channel)
        {
            this.SellerId = sellerId;
            this.SecretKey = secretKey;
            this.Channel = channel;
        }


        /// <summary>
        /// Gets the integration-partner specific authorization key required by the Newegg API.
        /// </summary>
        public string AuthorizationKey
        {
            get
            {
                if (UseLiveServerKey)
                {
                    // Production key
                    return "ca58758f9d0e7ddf903b940de061e179"; 
                }
                else
                {
                    // Testing key
                    return "9b46d5694ac41d5312573e2b5fcdb615";
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use live server key].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use live server key]; otherwise, <c>false</c>.
        /// </value>
        public static bool UseLiveServerKey
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("NeweggLiveServer", true);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("NeweggLiveServer", value);
            }            
        }


        /// <summary>
        /// Gets or sets the Channel.
        /// </summary>
        /// <value>
        /// The Channel.
        /// </value>
        public NeweggChannelType Channel { get; set; }

        /// <summary>
        /// Gets or sets the seller ID.
        /// </summary>
        /// <value>
        /// The seller ID.
        /// </value>
        public string SellerId { get; set; }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        /// <value>
        /// The secret key.
        /// </value>
        public string SecretKey { get; set; }
    }
}
