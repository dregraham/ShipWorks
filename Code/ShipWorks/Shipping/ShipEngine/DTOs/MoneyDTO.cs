using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// MoneyDTO
    /// </summary>
    [DataContract]
    public partial class MoneyDTO : IEquatable<MoneyDTO>, IValidatableObject
    {
        /// <summary>
        /// Defines Currency
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CurrencyEnum
        {

            /// <summary>
            /// Enum USD for value: uSD
            /// </summary>
            [EnumMember(Value = "uSD")]
            USD = 1,

            /// <summary>
            /// Enum CAD for value: cAD
            /// </summary>
            [EnumMember(Value = "cAD")]
            CAD = 2,

            /// <summary>
            /// Enum AUD for value: aUD
            /// </summary>
            [EnumMember(Value = "aUD")]
            AUD = 3,

            /// <summary>
            /// Enum GBP for value: gBP
            /// </summary>
            [EnumMember(Value = "gBP")]
            GBP = 4,

            /// <summary>
            /// Enum EUR for value: eUR
            /// </summary>
            [EnumMember(Value = "eUR")]
            EUR = 5,

            /// <summary>
            /// Enum NZD for value: nZD
            /// </summary>
            [EnumMember(Value = "nZD")]
            NZD = 6
        }

        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name = "currency", EmitDefaultValue = false)]
        public CurrencyEnum? Currency { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="MoneyDTO" /> class.
        /// </summary>
        /// <param name="currency">currency.</param>
        /// <param name="amount">amount.</param>
        public MoneyDTO(CurrencyEnum? currency = default(CurrencyEnum?), double? amount = default(double?))
        {
            this.Currency = currency;
            this.Amount = amount;
        }


        /// <summary>
        /// Gets or Sets Amount
        /// </summary>
        [DataMember(Name = "amount", EmitDefaultValue = false)]
        public double? Amount { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class MoneyDTO {\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  Amount: ").Append(Amount).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as MoneyDTO);
        }

        /// <summary>
        /// Returns true if MoneyDTO instances are equal
        /// </summary>
        /// <param name="input">Instance of MoneyDTO to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(MoneyDTO input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Currency == input.Currency ||
                    (this.Currency != null &&
                    this.Currency.Equals(input.Currency))
                ) &&
                (
                    this.Amount == input.Amount ||
                    (this.Amount != null &&
                    this.Amount.Equals(input.Amount))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Currency != null)
                    hashCode = hashCode * 59 + this.Currency.GetHashCode();
                if (this.Amount != null)
                    hashCode = hashCode * 59 + this.Amount.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
