using System.Reflection;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class ThreeDCartProduct
    {
        public string WarehouseBin { get; set; }

        public string MainImageFile { get; set; }

        public string ThumbnailFile { get; set; }
    }
}