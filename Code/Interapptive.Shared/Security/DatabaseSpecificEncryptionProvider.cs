using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.Security
{
    /// <summary>
    /// Encryption provider that is salted using the DB GUID
    /// </summary>
    [Component]
    public class DatabaseSpecificEncryptionProvider : AesEncryptionProvider, IDatabaseSpecificEncryptionProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// The purpose of this interface is so that classes can take a dependency on a specific encryption
        /// provider without needing to go through the factory. This makes tests simpler and the consuming
        /// code a bit easier to read and write.
        /// </remarks>
        public DatabaseSpecificEncryptionProvider(IDatabaseSpecificCipherKey cipherKey) : base(cipherKey)
        {

        }
    }
}
