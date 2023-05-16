using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;
using RestSharp;
using System.Web.UI.WebControls;
using Interapptive.Shared.Net;
using SD.Tools.BCLExtensions.CollectionsRelated;

namespace ShipWorks.Stores.Platforms.ShipEngine
{
    public class PlatformHelper
    {
        /// <summary>
        /// Get the note preface based on note type
        /// </summary>
        public static string GetNotePreface(OrderSourceNoteType noteType)
        {
            switch (noteType)
            {
                case OrderSourceNoteType.GiftMessage:
                    return "Gift Message: ";
                case OrderSourceNoteType.NotesToBuyer:
                    return "To Buyer: ";
                case OrderSourceNoteType.NotesFromBuyer:
                    return "From Buyer: ";
                case OrderSourceNoteType.InternalNotes:
                    return "Internal: ";
                default:
                    return string.Empty;
            }
        }

        public static T JsonConvertToDto<T>(string jsonString)
        {
            JObject jObject = JObject.Parse(jsonString);

            var weights = jObject.SelectTokens("orders.data[*].requestedFulfillments[*].items[*].product.weight.value");
            foreach (var weightToken in weights)
            {
                if (weightToken.Type == JTokenType.Null)
                {
                    var weightObject = (JObject) weightToken.Parent.Parent;
                    weightObject["value"] = "0";
                }
            }
            var jsonStringResult = JsonConvert.SerializeObject(jObject);
            return JsonConvert.DeserializeObject<T>(jsonStringResult);
        }
    }
}
