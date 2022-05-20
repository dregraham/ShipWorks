using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// RateResponse
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class RateResponse : IEquatable<RateResponse>, IValidatableObject
    {
        /// <summary>
        /// Defines Status
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum StatusEnum
        {

            /// <summary>
            /// Enum Working for value: working
            /// </summary>
            [EnumMember(Value = "working")]
            Working = 1,

            /// <summary>
            /// Enum Completed for value: completed
            /// </summary>
            [EnumMember(Value = "completed")]
            Completed = 2,

            /// <summary>
            /// Enum Partial for value: partial
            /// </summary>
            [EnumMember(Value = "partial")]
            Partial = 3,

            /// <summary>
            /// Enum Error for value: error
            /// </summary>
            [EnumMember(Value = "error")]
            Error = 4
        }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public StatusEnum? Status { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="RateResponse" /> class.
        /// </summary>
        /// <param name="rates">rates.</param>
        /// <param name="invalidRates">invalidRates.</param>
        /// <param name="rateRequestId">rateRequestId.</param>
        /// <param name="shipmentId">shipmentId.</param>
        /// <param name="createdAt">createdAt.</param>
        /// <param name="status">status.</param>
        /// <param name="errors">errors.</param>
        public RateResponse(List<Rate> rates = default(List<Rate>), List<Rate> invalidRates = default(List<Rate>), string rateRequestId = default(string), string shipmentId = default(string), DateTime? createdAt = default(DateTime?), StatusEnum? status = default(StatusEnum?), List<ProviderError> errors = default(List<ProviderError>))
        {
            this.Rates = rates;
            this.InvalidRates = invalidRates;
            this.RateRequestId = rateRequestId;
            this.ShipmentId = shipmentId;
            this.CreatedAt = createdAt;
            this.Status = status;
            this.Errors = errors;
        }

        /// <summary>
        /// Gets or Sets Rates
        /// </summary>
        [DataMember(Name = "rates", EmitDefaultValue = false)]
        public List<Rate> Rates { get; set; }

        /// <summary>
        /// Gets or Sets InvalidRates
        /// </summary>
        [DataMember(Name = "invalid_rates", EmitDefaultValue = false)]
        public List<Rate> InvalidRates { get; set; }

        /// <summary>
        /// Gets or Sets RateRequestId
        /// </summary>
        [DataMember(Name = "rate_request_id", EmitDefaultValue = false)]
        public string RateRequestId { get; set; }

        /// <summary>
        /// Gets or Sets ShipmentId
        /// </summary>
        [DataMember(Name = "shipment_id", EmitDefaultValue = false)]
        public string ShipmentId { get; set; }

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>
        [DataMember(Name = "created_at", EmitDefaultValue = false)]
        public DateTime? CreatedAt { get; set; }


        /// <summary>
        /// Gets or Sets Errors
        /// </summary>
        [DataMember(Name = "errors", EmitDefaultValue = false)]
        public List<ProviderError> Errors { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RateResponse {\n");
            sb.Append("  Rates: ").Append(Rates).Append("\n");
            sb.Append("  InvalidRates: ").Append(InvalidRates).Append("\n");
            sb.Append("  RateRequestId: ").Append(RateRequestId).Append("\n");
            sb.Append("  ShipmentId: ").Append(ShipmentId).Append("\n");
            sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Errors: ").Append(Errors).Append("\n");
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
            return this.Equals(input as RateResponse);
        }

        /// <summary>
        /// Returns true if RateResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of RateResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RateResponse input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Rates == input.Rates ||
                    this.Rates != null &&
                    this.Rates.SequenceEqual(input.Rates)
                ) &&
                (
                    this.InvalidRates == input.InvalidRates ||
                    this.InvalidRates != null &&
                    this.InvalidRates.SequenceEqual(input.InvalidRates)
                ) &&
                (
                    this.RateRequestId == input.RateRequestId ||
                    (this.RateRequestId != null &&
                    this.RateRequestId.Equals(input.RateRequestId))
                ) &&
                (
                    this.ShipmentId == input.ShipmentId ||
                    (this.ShipmentId != null &&
                    this.ShipmentId.Equals(input.ShipmentId))
                ) &&
                (
                    this.CreatedAt == input.CreatedAt ||
                    (this.CreatedAt != null &&
                    this.CreatedAt.Equals(input.CreatedAt))
                ) &&
                (
                    this.Status == input.Status ||
                    (this.Status != null &&
                    this.Status.Equals(input.Status))
                ) &&
                (
                    this.Errors == input.Errors ||
                    this.Errors != null &&
                    this.Errors.SequenceEqual(input.Errors)
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
                if (this.Rates != null)
                    hashCode = hashCode * 59 + this.Rates.GetHashCode();
                if (this.InvalidRates != null)
                    hashCode = hashCode * 59 + this.InvalidRates.GetHashCode();
                if (this.RateRequestId != null)
                    hashCode = hashCode * 59 + this.RateRequestId.GetHashCode();
                if (this.ShipmentId != null)
                    hashCode = hashCode * 59 + this.ShipmentId.GetHashCode();
                if (this.CreatedAt != null)
                    hashCode = hashCode * 59 + this.CreatedAt.GetHashCode();
                if (this.Status != null)
                    hashCode = hashCode * 59 + this.Status.GetHashCode();
                if (this.Errors != null)
                    hashCode = hashCode * 59 + this.Errors.GetHashCode();
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
