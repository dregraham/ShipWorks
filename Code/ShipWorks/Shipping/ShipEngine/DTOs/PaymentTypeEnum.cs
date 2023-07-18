using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    [JsonConverter(typeof(StringEnumConverter))]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum PaymentTypeEnum
    {
        [EnumMember(Value = "none")]
        None = 0,
        [EnumMember(Value = "any")]
        Any = 1,
        [EnumMember(Value = "cash")]
        Cash = 2,
        [EnumMember(Value = "cash_equivalent")]
        CashEquivalent = 3,
    }
}
