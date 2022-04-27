using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// TrackingInformation
    /// </summary>
    [DataContract]
    public partial class TrackingInformation : BaseShipEngineResponse, IEquatable<TrackingInformation>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingInformation" /> class.
        /// </summary>
        /// <param name="trackingNumber">trackingNumber.</param>
        /// <param name="statusCode">statusCode.</param>
        /// <param name="statusDescription">statusDescription.</param>
        /// <param name="carrierStatusCode">carrierStatusCode.</param>
        /// <param name="carrierStatusDescription">carrierStatusDescription.</param>
        /// <param name="shipDate">shipDate.</param>
        /// <param name="estimatedDeliveryDate">estimatedDeliveryDate.</param>
        /// <param name="actualDeliveryDate">actualDeliveryDate.</param>
        /// <param name="exceptionDescription">exceptionDescription.</param>
        /// <param name="events">events.</param>
        public TrackingInformation(string trackingNumber = default(string), string statusCode = default(string), string statusDescription = default(string), string carrierStatusCode = default(string), string carrierStatusDescription = default(string), DateTime? shipDate = default(DateTime?), DateTime? estimatedDeliveryDate = default(DateTime?), DateTime? actualDeliveryDate = default(DateTime?), string exceptionDescription = default(string), List<TrackEvent> events = default(List<TrackEvent>))
        {
            this.TrackingNumber = trackingNumber;
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
            this.CarrierStatusCode = carrierStatusCode;
            this.CarrierStatusDescription = carrierStatusDescription;
            this.ShipDate = shipDate;
            this.EstimatedDeliveryDate = estimatedDeliveryDate;
            this.ActualDeliveryDate = actualDeliveryDate;
            this.ExceptionDescription = exceptionDescription;
            this.Events = events;
        }

        /// <summary>
        /// Gets or Sets TrackingNumber
        /// </summary>
        [DataMember(Name = "tracking_number", EmitDefaultValue = false)]
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or Sets StatusCode
        /// </summary>
        [DataMember(Name = "status_code", EmitDefaultValue = false)]
        public string StatusCode { get; set; }

        /// <summary>
        /// Gets or Sets StatusDescription
        /// </summary>
        [DataMember(Name = "status_description", EmitDefaultValue = false)]
        public string StatusDescription { get; set; }

        /// <summary>
        /// Gets or Sets CarrierStatusCode
        /// </summary>
        [DataMember(Name = "carrier_status_code", EmitDefaultValue = false)]
        public string CarrierStatusCode { get; set; }

        /// <summary>
        /// Gets or Sets CarrierStatusDescription
        /// </summary>
        [DataMember(Name = "carrier_status_description", EmitDefaultValue = false)]
        public string CarrierStatusDescription { get; set; }

        /// <summary>
        /// Gets or Sets ShipDate
        /// </summary>
        [DataMember(Name = "ship_date", EmitDefaultValue = false)]
        public DateTime? ShipDate { get; set; }

        /// <summary>
        /// Gets or Sets EstimatedDeliveryDate
        /// </summary>
        [DataMember(Name = "estimated_delivery_date", EmitDefaultValue = false)]
        public DateTime? EstimatedDeliveryDate { get; set; }

        /// <summary>
        /// Gets or Sets ActualDeliveryDate
        /// </summary>
        [DataMember(Name = "actual_delivery_date", EmitDefaultValue = false)]
        public DateTime? ActualDeliveryDate { get; set; }

        /// <summary>
        /// Gets or Sets ExceptionDescription
        /// </summary>
        [DataMember(Name = "exception_description", EmitDefaultValue = false)]
        public string ExceptionDescription { get; set; }

        /// <summary>
        /// Gets or Sets Events
        /// </summary>
        [DataMember(Name = "events", EmitDefaultValue = false)]
        public List<TrackEvent> Events { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TrackingInformation {\n");
            sb.Append("  TrackingNumber: ").Append(TrackingNumber).Append("\n");
            sb.Append("  StatusCode: ").Append(StatusCode).Append("\n");
            sb.Append("  StatusDescription: ").Append(StatusDescription).Append("\n");
            sb.Append("  CarrierStatusCode: ").Append(CarrierStatusCode).Append("\n");
            sb.Append("  CarrierStatusDescription: ").Append(CarrierStatusDescription).Append("\n");
            sb.Append("  ShipDate: ").Append(ShipDate).Append("\n");
            sb.Append("  EstimatedDeliveryDate: ").Append(EstimatedDeliveryDate).Append("\n");
            sb.Append("  ActualDeliveryDate: ").Append(ActualDeliveryDate).Append("\n");
            sb.Append("  ExceptionDescription: ").Append(ExceptionDescription).Append("\n");
            sb.Append("  Events: ").Append(Events).Append("\n");
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
            return this.Equals(input as TrackingInformation);
        }

        /// <summary>
        /// Returns true if TrackingInformation instances are equal
        /// </summary>
        /// <param name="input">Instance of TrackingInformation to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TrackingInformation input)
        {
            if (input == null)
                return false;

            return
                (
                    this.TrackingNumber == input.TrackingNumber ||
                    (this.TrackingNumber != null &&
                    this.TrackingNumber.Equals(input.TrackingNumber))
                ) &&
                (
                    this.StatusCode == input.StatusCode ||
                    (this.StatusCode != null &&
                    this.StatusCode.Equals(input.StatusCode))
                ) &&
                (
                    this.StatusDescription == input.StatusDescription ||
                    (this.StatusDescription != null &&
                    this.StatusDescription.Equals(input.StatusDescription))
                ) &&
                (
                    this.CarrierStatusCode == input.CarrierStatusCode ||
                    (this.CarrierStatusCode != null &&
                    this.CarrierStatusCode.Equals(input.CarrierStatusCode))
                ) &&
                (
                    this.CarrierStatusDescription == input.CarrierStatusDescription ||
                    (this.CarrierStatusDescription != null &&
                    this.CarrierStatusDescription.Equals(input.CarrierStatusDescription))
                ) &&
                (
                    this.ShipDate == input.ShipDate ||
                    (this.ShipDate != null &&
                    this.ShipDate.Equals(input.ShipDate))
                ) &&
                (
                    this.EstimatedDeliveryDate == input.EstimatedDeliveryDate ||
                    (this.EstimatedDeliveryDate != null &&
                    this.EstimatedDeliveryDate.Equals(input.EstimatedDeliveryDate))
                ) &&
                (
                    this.ActualDeliveryDate == input.ActualDeliveryDate ||
                    (this.ActualDeliveryDate != null &&
                    this.ActualDeliveryDate.Equals(input.ActualDeliveryDate))
                ) &&
                (
                    this.ExceptionDescription == input.ExceptionDescription ||
                    (this.ExceptionDescription != null &&
                    this.ExceptionDescription.Equals(input.ExceptionDescription))
                ) &&
                (
                    this.Events == input.Events ||
                    this.Events != null &&
                    this.Events.SequenceEqual(input.Events)
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
                if (this.TrackingNumber != null)
                    hashCode = hashCode * 59 + this.TrackingNumber.GetHashCode();
                if (this.StatusCode != null)
                    hashCode = hashCode * 59 + this.StatusCode.GetHashCode();
                if (this.StatusDescription != null)
                    hashCode = hashCode * 59 + this.StatusDescription.GetHashCode();
                if (this.CarrierStatusCode != null)
                    hashCode = hashCode * 59 + this.CarrierStatusCode.GetHashCode();
                if (this.CarrierStatusDescription != null)
                    hashCode = hashCode * 59 + this.CarrierStatusDescription.GetHashCode();
                if (this.ShipDate != null)
                    hashCode = hashCode * 59 + this.ShipDate.GetHashCode();
                if (this.EstimatedDeliveryDate != null)
                    hashCode = hashCode * 59 + this.EstimatedDeliveryDate.GetHashCode();
                if (this.ActualDeliveryDate != null)
                    hashCode = hashCode * 59 + this.ActualDeliveryDate.GetHashCode();
                if (this.ExceptionDescription != null)
                    hashCode = hashCode * 59 + this.ExceptionDescription.GetHashCode();
                if (this.Events != null)
                    hashCode = hashCode * 59 + this.Events.GetHashCode();
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
