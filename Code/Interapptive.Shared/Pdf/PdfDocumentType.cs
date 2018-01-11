using System.Reflection;

namespace Interapptive.Shared.Pdf
{
    /// <summary>
    /// Type of pdf document
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum PdfDocumentType
    {
        BlackAndWhite = 0,

        Color = 1
    }
}
