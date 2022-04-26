using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    [DataContract]
    public class TaxIdentifier
    {
        /// <summary>
        /// Taxable Entity Type
        /// </summary>
        [DataMember(Name = "taxable_entity_type", EmitDefaultValue = false)]
        public string TaxableEntityType { get; set; }

        /// <summary>
        /// The type of tax identifier
        /// </summary>
        [DataMember(Name = "identifier_type", EmitDefaultValue = false)]
        public IdentifierTypeEnum? IdentifierType { get; set; }


        [JsonConverter(typeof(StringEnumConverter))]
        public enum IdentifierTypeEnum
        {
            /// <summary>
            /// value added tax
            /// </summary>
            [EnumMember(Value = "vat")]
            Vat = 0,

            /// <summary>
            /// Economic Operators Registration and Identification Number
            /// </summary>
            [EnumMember(Value = "eori")]
            Eori = 1,

            /// <summary>
            /// Social Security Number
            /// </summary>
            [EnumMember(Value = "ssn")]
            Ssn = 2,

            /// <summary>
            /// Employer Identification Number
            /// </summary>
            [EnumMember(Value = "ein")]
            Ein = 3,

            /// <summary>
            /// Tax Identification Number
            /// </summary>
            [EnumMember(Value = "tin")]
            Tin = 4,

            /// <summary>
            /// Import One Stop Shop
            /// </summary>
            [EnumMember(Value = "ioss")]
            Ioss = 5,

            /// <summary>
            /// Permanent Account Number
            /// </summary>
            [EnumMember(Value = "pan")]
            Pan = 6,

            /// <summary>
            /// Norwegian VAT On E-Commerce(VOEC)
            /// </summary>
            [EnumMember(Value = "voec")]
            Voec = 7,
        }

        /// <summary>
        /// The value for the tax identifier
        /// </summary>
        [DataMember(Name = "value", EmitDefaultValue = false)]
        public string Value { get; set; }

        [DataMember(Name = "issuing_authority", EmitDefaultValue = false)]
        public string IssuingAuthority { get; set; }
    }
}
