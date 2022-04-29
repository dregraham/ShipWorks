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
    /// LabelPackage
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class LabelPackage : IEquatable<LabelPackage>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelPackage" /> class.
        /// </summary>
        /// <param name="packageCode">packageCode.</param>
        /// <param name="weight">weight.</param>
        /// <param name="dimensions">dimensions.</param>
        /// <param name="insuredValue">insuredValue.</param>
        /// <param name="trackingNumber">trackingNumber.</param>
        /// <param name="labelMessages">labelMessages.</param>
        public LabelPackage(string packageCode = default(string), Weight weight = default(Weight), Dimensions dimensions = default(Dimensions), MoneyDTO insuredValue = default(MoneyDTO), string trackingNumber = default(string), LabelMessages labelMessages = default(LabelMessages))
        {
            this.PackageCode = packageCode;
            this.Weight = weight;
            this.Dimensions = dimensions;
            this.InsuredValue = insuredValue;
            this.TrackingNumber = trackingNumber;
            this.LabelMessages = labelMessages;
        }

        /// <summary>
        /// Gets or Sets PackageCode
        /// </summary>
        [DataMember(Name = "package_code", EmitDefaultValue = false)]
        public string PackageCode { get; set; }

        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        [DataMember(Name = "weight", EmitDefaultValue = false)]
        public Weight Weight { get; set; }

        /// <summary>
        /// Gets or Sets Dimensions
        /// </summary>
        [DataMember(Name = "dimensions", EmitDefaultValue = false)]
        public Dimensions Dimensions { get; set; }

        /// <summary>
        /// Gets or Sets InsuredValue
        /// </summary>
        [DataMember(Name = "insured_value", EmitDefaultValue = false)]
        public MoneyDTO InsuredValue { get; set; }

        /// <summary>
        /// Gets or Sets TrackingNumber
        /// </summary>
        [DataMember(Name = "tracking_number", EmitDefaultValue = false)]
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or Sets LabelMessages
        /// </summary>
        [DataMember(Name = "label_messages", EmitDefaultValue = false)]
        public LabelMessages LabelMessages { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class LabelPackage {\n");
            sb.Append("  PackageCode: ").Append(PackageCode).Append("\n");
            sb.Append("  Weight: ").Append(Weight).Append("\n");
            sb.Append("  Dimensions: ").Append(Dimensions).Append("\n");
            sb.Append("  InsuredValue: ").Append(InsuredValue).Append("\n");
            sb.Append("  TrackingNumber: ").Append(TrackingNumber).Append("\n");
            sb.Append("  LabelMessages: ").Append(LabelMessages).Append("\n");
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
            return this.Equals(input as LabelPackage);
        }

        /// <summary>
        /// Returns true if LabelPackage instances are equal
        /// </summary>
        /// <param name="input">Instance of LabelPackage to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(LabelPackage input)
        {
            if (input == null)
                return false;

            return
                (
                    this.PackageCode == input.PackageCode ||
                    (this.PackageCode != null &&
                    this.PackageCode.Equals(input.PackageCode))
                ) &&
                (
                    this.Weight == input.Weight ||
                    (this.Weight != null &&
                    this.Weight.Equals(input.Weight))
                ) &&
                (
                    this.Dimensions == input.Dimensions ||
                    (this.Dimensions != null &&
                    this.Dimensions.Equals(input.Dimensions))
                ) &&
                (
                    this.InsuredValue == input.InsuredValue ||
                    (this.InsuredValue != null &&
                    this.InsuredValue.Equals(input.InsuredValue))
                ) &&
                (
                    this.TrackingNumber == input.TrackingNumber ||
                    (this.TrackingNumber != null &&
                    this.TrackingNumber.Equals(input.TrackingNumber))
                ) &&
                (
                    this.LabelMessages == input.LabelMessages ||
                    (this.LabelMessages != null &&
                    this.LabelMessages.Equals(input.LabelMessages))
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
                if (this.PackageCode != null)
                    hashCode = hashCode * 59 + this.PackageCode.GetHashCode();
                if (this.Weight != null)
                    hashCode = hashCode * 59 + this.Weight.GetHashCode();
                if (this.Dimensions != null)
                    hashCode = hashCode * 59 + this.Dimensions.GetHashCode();
                if (this.InsuredValue != null)
                    hashCode = hashCode * 59 + this.InsuredValue.GetHashCode();
                if (this.TrackingNumber != null)
                    hashCode = hashCode * 59 + this.TrackingNumber.GetHashCode();
                if (this.LabelMessages != null)
                    hashCode = hashCode * 59 + this.LabelMessages.GetHashCode();
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
