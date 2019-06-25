using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Class representing token response
    /// </summary>
    [Obfuscation(Exclude = false, ApplyToMembers = true, StripAfterObfuscation = true)]
    [XmlType(TypeName = "TokenResponse")]
    public class TokenResponse
    {
        /// <summary>
        /// The auth token
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// A refresh token
        /// </summary>
        public string refreshToken { get; set; }

        /// <summary>
        /// A redirect token
        /// </summary>
        public string redirectToken { get; set; }

        /// <summary>
        /// An error
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// The result
        /// </summary>
        public int Result { get; set; }
    }
}
