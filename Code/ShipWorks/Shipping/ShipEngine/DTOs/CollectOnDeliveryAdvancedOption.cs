using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// Advanced Option to collect on delivery
    /// </summary>
    /// 
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class CollectOnDeliveryAdvancedOption : IEquatable<CollectOnDeliveryAdvancedOption>, IValidatableObject
    {
        /// <summary>
        /// Gets or Sets payment_type
        /// </summary>
        [DataMember(Name = "payment_type", EmitDefaultValue = false)]
        public PaymentTypeEnum PaymentType { get; set; }

        /// <summary>
        /// Gets or Sets payment_amount
        /// </summary>
        [DataMember(Name = "payment_amount", EmitDefaultValue = false)]
        public MoneyDTO PaymentAmount { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CollectOnDeliveryAdvancedOption {\n");
            sb.Append("  PaymentType: ").Append(PaymentType).Append("\n");
            sb.Append("  PaymentAmount: ").Append(PaymentAmount).Append("\n");
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
            return this.Equals(input as CollectOnDeliveryAdvancedOption);
        }

        /// <summary>
        /// Returns true if AdvancedOptions instances are equal
        /// </summary>
        /// <param name="input">Instance of AdvancedOptions to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CollectOnDeliveryAdvancedOption input)
        {
            if (input == null)
                return false;

            return this.PaymentType == input.PaymentType && (input.PaymentAmount == this.PaymentAmount || (this.PaymentAmount != null && this.PaymentAmount.Equals(input.PaymentAmount)));
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
                if(this.PaymentType != null)
                {
                    hashCode = hashCode * 59 + this.PaymentType.GetHashCode();
                }
                if(this.PaymentAmount != null)
                {
                    hashCode = hashCode * 59 + this.PaymentAmount.GetHashCode();
                }
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
