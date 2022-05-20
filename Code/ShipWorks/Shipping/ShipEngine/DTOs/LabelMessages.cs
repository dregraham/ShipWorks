using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// LabelMessages
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class LabelMessages : IEquatable<LabelMessages>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelMessages" /> class.
        /// </summary>
        /// <param name="reference1">reference1.</param>
        /// <param name="reference2">reference2.</param>
        /// <param name="reference3">reference3.</param>
        public LabelMessages(string reference1 = default(string), string reference2 = default(string), string reference3 = default(string))
        {
            this.Reference1 = reference1;
            this.Reference2 = reference2;
            this.Reference3 = reference3;
        }

        /// <summary>
        /// Gets or Sets Reference1
        /// </summary>
        [DataMember(Name = "reference1", EmitDefaultValue = false)]
        public string Reference1 { get; set; }

        /// <summary>
        /// Gets or Sets Reference2
        /// </summary>
        [DataMember(Name = "reference2", EmitDefaultValue = false)]
        public string Reference2 { get; set; }

        /// <summary>
        /// Gets or Sets Reference3
        /// </summary>
        [DataMember(Name = "reference3", EmitDefaultValue = false)]
        public string Reference3 { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class LabelMessages {\n");
            sb.Append("  Reference1: ").Append(Reference1).Append("\n");
            sb.Append("  Reference2: ").Append(Reference2).Append("\n");
            sb.Append("  Reference3: ").Append(Reference3).Append("\n");
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
            return this.Equals(input as LabelMessages);
        }

        /// <summary>
        /// Returns true if LabelMessages instances are equal
        /// </summary>
        /// <param name="input">Instance of LabelMessages to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(LabelMessages input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Reference1 == input.Reference1 ||
                    (this.Reference1 != null &&
                    this.Reference1.Equals(input.Reference1))
                ) &&
                (
                    this.Reference2 == input.Reference2 ||
                    (this.Reference2 != null &&
                    this.Reference2.Equals(input.Reference2))
                ) &&
                (
                    this.Reference3 == input.Reference3 ||
                    (this.Reference3 != null &&
                    this.Reference3.Equals(input.Reference3))
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
                if (this.Reference1 != null)
                    hashCode = hashCode * 59 + this.Reference1.GetHashCode();
                if (this.Reference2 != null)
                    hashCode = hashCode * 59 + this.Reference2.GetHashCode();
                if (this.Reference3 != null)
                    hashCode = hashCode * 59 + this.Reference3.GetHashCode();
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
