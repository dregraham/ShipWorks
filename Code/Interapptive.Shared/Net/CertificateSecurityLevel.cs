
namespace Interapptive.Shared.Net
{
    /// <summary>
    /// An enumeration for the different security levels that are associated with the certificate
    /// of a host/domain. These is intended to be used in conjunction with the ICertificateInspector
    /// to determine whether the certificate obtains from a host can be trusted, if it's been
    /// spoofed, or if there is no certificate.
    /// </summary>
    public enum CertificateSecurityLevel
    {
        /// <summary>
        /// There isn't a certificate associated with the host.
        /// </summary>
        None,

        /// <summary>
        /// There is a certificate associated with the host, but it is suspect.
        /// </summary>
        Spoofed,

        /// <summary>
        /// The certificate is trusted.
        /// </summary>
        Trusted
    }
}
