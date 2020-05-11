using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// UpsCredentials
    /// </summary>
    [Component]
    public class UpsCredentials : IUpsCredentials
    {
        private const string userId = "shipworks-wallet";
        private const string encryptedPassword = "Z3//h9ue7+o=";
        private const string encryptedAccessKey = "AraoUD2/jFFGbmsCWXCkqw77Q7wUFUis";
        private const string encryptedDeveloperKey = "lWAMqTPkoqPCuPiU9oGZO5mYKaTxu7WA";

        private readonly IEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsCredentials(IEncryptionProviderFactory encryptionProviderFactory)
        {
            encryptionProvider = encryptionProviderFactory.CreateSecureTextEncryptionProvider("shipengine");
        }

        /// <summary>
        /// UserId
        /// </summary>
        public string UserId => userId;

        /// <summary>
        /// Password
        /// </summary>
        public string Password => encryptionProvider.Decrypt(encryptedPassword);

        /// <summary>
        /// Access Key
        /// </summary>
        public string AccessKey => encryptionProvider.Decrypt(encryptedAccessKey);

        /// <summary>
        /// Developer Key
        /// </summary>
        public string DeveloperKey => encryptionProvider.Decrypt(encryptedDeveloperKey);
    }
}
