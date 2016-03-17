using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    public class ThreeDCartImageGallery
    {
        [JsonProperty("ImageGalleryID")]
        public int ImageGalleryID { get; set; }

        [JsonProperty("ImageGalleryFile")]
        public string ImageGalleryFile { get; set; }

        [JsonProperty("ImageGalleryCaption")]
        public string ImageGalleryCaption { get; set; }

        [JsonProperty("ImageGallerySorting")]
        public int ImageGallerySorting { get; set; }
    }
}