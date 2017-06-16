using System.Reflection;

namespace ShipWorks.Stores.Platforms.ShopSite.Dto
{
    /// <summary>
    /// ErrorResponse DTO for ShopSite 
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class ErrorResponse
    {
        public string error { get; set; }
        public string error_description { get; set; }
    }
}
