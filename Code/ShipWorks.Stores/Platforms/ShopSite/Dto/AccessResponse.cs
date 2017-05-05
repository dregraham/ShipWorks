using System.Reflection;

namespace ShipWorks.Stores.Platforms.ShopSite.Dto
{
    /// <summary>
    /// AccessResponse DTO for ShopSite 
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class AccessResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string download_url { get; set; }
        public string upload1_url { get; set; }
        public string upload2_url { get; set; }
        public string publish_url { get; set; }
    }
}
