using System.Reflection;

namespace ShipWorks.ApplicationCore.Security
{
    /// <summary>
    /// Context of ciphers
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum CipherContext
    {
        Sears,
        License 
    }
}