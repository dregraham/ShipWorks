using System.Security.Cryptography;
using System.Text;

namespace Interapptive.Shared.Security
{
    /// <summary>
    /// An ICipherKey implementation specific to the customer licensing.
    /// </summary>
    public class StreamCipherKey : ICipherKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamCipherKey" /> class.
        /// </summary>
        public StreamCipherKey()
        {
            InitializationVector = new byte[]
            {
                66, 15, 1, 231, 78, 187, 216, 63, 81, 130, 115, 140, 156, 57, 179, 194,
                108, 32, 29, 180, 74, 56, 183, 243, 45, 112, 17, 22, 167, 60, 180, 239
            };

            Key = Encoding.UTF8.GetBytes("wzx6XgaPnjEPsQHXVK8Q");
        }

        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        public byte[] InitializationVector { get; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public byte[] Key { get; }
    }
}
