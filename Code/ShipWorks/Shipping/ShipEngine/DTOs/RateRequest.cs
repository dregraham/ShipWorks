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
    /// RateRequest
    /// </summary>
    [DataContract]
    public partial class RateRequest : IEquatable<RateRequest>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RateRequest" /> class.
        /// </summary>
        /// <param name="carrierIds">carrierIds.</param>
        /// <param name="packageTypes">packageTypes.</param>
        public RateRequest(List<string> carrierIds = default(List<string>), List<string> packageTypes = default(List<string>))
        {
            this.CarrierIds = carrierIds;
            this.PackageTypes = packageTypes;
        }

        /// <summary>
        /// Gets or Sets CarrierIds
        /// </summary>
        [DataMember(Name = "carrier_ids", EmitDefaultValue = false)]
        public List<string> CarrierIds { get; set; }

        /// <summary>
        /// Gets or Sets PackageTypes
        /// </summary>
        [DataMember(Name = "package_types", EmitDefaultValue = false)]
        public List<string> PackageTypes { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RateRequest {\n");
            sb.Append("  CarrierIds: ").Append(CarrierIds).Append("\n");
            sb.Append("  PackageTypes: ").Append(PackageTypes).Append("\n");
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
            return this.Equals(input as RateRequest);
        }

        /// <summary>
        /// Returns true if RateRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of RateRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RateRequest input)
        {
            if (input == null)
                return false;

            return
                (
                    this.CarrierIds == input.CarrierIds ||
                    this.CarrierIds != null &&
                    this.CarrierIds.SequenceEqual(input.CarrierIds)
                ) &&
                (
                    this.PackageTypes == input.PackageTypes ||
                    this.PackageTypes != null &&
                    this.PackageTypes.SequenceEqual(input.PackageTypes)
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
                if (this.CarrierIds != null)
                    hashCode = hashCode * 59 + this.CarrierIds.GetHashCode();
                if (this.PackageTypes != null)
                    hashCode = hashCode * 59 + this.PackageTypes.GetHashCode();
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
