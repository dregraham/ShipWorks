using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// VoidLabelResponse
    /// </summary>
    [DataContract]
    public partial class VoidLabelResponse : IEquatable<VoidLabelResponse>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoidLabelResponse" /> class.
        /// </summary>
        /// <param name="approved">approved.</param>
        /// <param name="message">message.</param>
        public VoidLabelResponse(bool? approved = default(bool?), string message = default(string))
        {
            this.Approved = approved;
            this.Message = message;
        }

        /// <summary>
        /// Gets or Sets Approved
        /// </summary>
        [DataMember(Name = "approved", EmitDefaultValue = false)]
        public bool? Approved { get; set; }

        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [DataMember(Name = "message", EmitDefaultValue = false)]
        public string Message { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VoidLabelResponse {\n");
            sb.Append("  Approved: ").Append(Approved).Append("\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
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
            return this.Equals(input as VoidLabelResponse);
        }

        /// <summary>
        /// Returns true if VoidLabelResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of VoidLabelResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VoidLabelResponse input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Approved == input.Approved ||
                    (this.Approved != null &&
                    this.Approved.Equals(input.Approved))
                ) &&
                (
                    this.Message == input.Message ||
                    (this.Message != null &&
                    this.Message.Equals(input.Message))
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
                if (this.Approved != null)
                    hashCode = hashCode * 59 + this.Approved.GetHashCode();
                if (this.Message != null)
                    hashCode = hashCode * 59 + this.Message.GetHashCode();
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
