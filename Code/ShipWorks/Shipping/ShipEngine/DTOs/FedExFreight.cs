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
    /// FedExFreight
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class FedExFreight : IEquatable<FedExFreight>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExFreight" /> class.
        /// </summary>
        /// <param name="shipperLoadAndCount">shipperLoadAndCount.</param>
        /// <param name="bookingConfirmation">bookingConfirmation.</param>
        public FedExFreight(string shipperLoadAndCount = default(string), string bookingConfirmation = default(string))
        {
            this.ShipperLoadAndCount = shipperLoadAndCount;
            this.BookingConfirmation = bookingConfirmation;
        }

        /// <summary>
        /// Gets or Sets ShipperLoadAndCount
        /// </summary>
        [DataMember(Name = "shipper_load_and_count", EmitDefaultValue = false)]
        public string ShipperLoadAndCount { get; set; }

        /// <summary>
        /// Gets or Sets BookingConfirmation
        /// </summary>
        [DataMember(Name = "booking_confirmation", EmitDefaultValue = false)]
        public string BookingConfirmation { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class FedExFreight {\n");
            sb.Append("  ShipperLoadAndCount: ").Append(ShipperLoadAndCount).Append("\n");
            sb.Append("  BookingConfirmation: ").Append(BookingConfirmation).Append("\n");
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
            return this.Equals(input as FedExFreight);
        }

        /// <summary>
        /// Returns true if FedExFreight instances are equal
        /// </summary>
        /// <param name="input">Instance of FedExFreight to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(FedExFreight input)
        {
            if (input == null)
                return false;

            return
                (
                    this.ShipperLoadAndCount == input.ShipperLoadAndCount ||
                    this.ShipperLoadAndCount != null &&
                    this.ShipperLoadAndCount.SequenceEqual(input.ShipperLoadAndCount)
                ) &&
                (
                    this.BookingConfirmation == input.BookingConfirmation ||
                    this.BookingConfirmation != null &&
                    this.BookingConfirmation.SequenceEqual(input.BookingConfirmation)
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
                if (this.ShipperLoadAndCount != null)
                    hashCode = hashCode * 59 + this.ShipperLoadAndCount.GetHashCode();
                if (this.BookingConfirmation != null)
                    hashCode = hashCode * 59 + this.BookingConfirmation.GetHashCode();
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
