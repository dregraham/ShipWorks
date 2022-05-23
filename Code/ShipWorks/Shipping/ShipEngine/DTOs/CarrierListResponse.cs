using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// CarrierListResponse
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class CarrierListResponse : BaseShipEngineResponse, IEquatable<CarrierListResponse>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CarrierListResponse" /> class.
        /// </summary>
        /// <param name="carriers">carriers.</param>
        public CarrierListResponse(List<Carrier> carriers = default(List<Carrier>))
        {
            this.Carriers = carriers;
        }

        /// <summary>
        /// Gets or Sets Carriers
        /// </summary>
        [DataMember(Name = "carriers", EmitDefaultValue = false)]
        public List<Carrier> Carriers { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CarrierListResponse {\n");
            sb.Append("  Carriers: ").Append(Carriers).Append("\n");
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
            return this.Equals(input as CarrierListResponse);
        }

        /// <summary>
        /// Returns true if CarrierListResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of CarrierListResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CarrierListResponse input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Carriers == input.Carriers ||
                    this.Carriers != null &&
                    this.Carriers.SequenceEqual(input.Carriers)
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
                if (this.Carriers != null)
                    hashCode = hashCode * 59 + this.Carriers.GetHashCode();
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
