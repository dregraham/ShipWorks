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
    /// RateShipmentRequest
    /// </summary>
    [DataContract]
    public partial class RateShipmentRequest : IEquatable<RateShipmentRequest>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RateShipmentRequest" /> class.
        /// </summary>
        /// <param name="shipmentId">shipmentId.</param>
        /// <param name="shipment">shipment.</param>
        /// <param name="rateOptions">rateOptions.</param>
        public RateShipmentRequest(string shipmentId = default(string), AddressValidatingShipment shipment = default(AddressValidatingShipment), RateRequest rateOptions = default(RateRequest))
        {
            this.ShipmentId = shipmentId;
            this.Shipment = shipment;
            this.RateOptions = rateOptions;
        }

        /// <summary>
        /// Gets or Sets ShipmentId
        /// </summary>
        [DataMember(Name = "shipment_id", EmitDefaultValue = false)]
        public string ShipmentId { get; set; }

        /// <summary>
        /// Gets or Sets Shipment
        /// </summary>
        [DataMember(Name = "shipment", EmitDefaultValue = false)]
        public AddressValidatingShipment Shipment { get; set; }

        /// <summary>
        /// Gets or Sets RateOptions
        /// </summary>
        [DataMember(Name = "rate_options", EmitDefaultValue = false)]
        public RateRequest RateOptions { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RateShipmentRequest {\n");
            sb.Append("  ShipmentId: ").Append(ShipmentId).Append("\n");
            sb.Append("  Shipment: ").Append(Shipment).Append("\n");
            sb.Append("  RateOptions: ").Append(RateOptions).Append("\n");
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
            return this.Equals(input as RateShipmentRequest);
        }

        /// <summary>
        /// Returns true if RateShipmentRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of RateShipmentRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RateShipmentRequest input)
        {
            if (input == null)
                return false;

            return
                (
                    this.ShipmentId == input.ShipmentId ||
                    (this.ShipmentId != null &&
                    this.ShipmentId.Equals(input.ShipmentId))
                ) &&
                (
                    this.Shipment == input.Shipment ||
                    (this.Shipment != null &&
                    this.Shipment.Equals(input.Shipment))
                ) &&
                (
                    this.RateOptions == input.RateOptions ||
                    (this.RateOptions != null &&
                    this.RateOptions.Equals(input.RateOptions))
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
                if (this.ShipmentId != null)
                    hashCode = hashCode * 59 + this.ShipmentId.GetHashCode();
                if (this.Shipment != null)
                    hashCode = hashCode * 59 + this.Shipment.GetHashCode();
                if (this.RateOptions != null)
                    hashCode = hashCode * 59 + this.RateOptions.GetHashCode();
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
