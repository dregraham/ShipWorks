using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class TokenResponse
    {
        [JsonProperty("id_token")]
        public string Token { get; set; }
    }
}
