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
    /// TrackEvent
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class TrackEvent : IEquatable<TrackEvent>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackEvent" /> class.
        /// </summary>
        /// <param name="occurredAt">occurredAt.</param>
        /// <param name="description">description.</param>
        /// <param name="cityLocality">cityLocality.</param>
        /// <param name="stateProvince">stateProvince.</param>
        /// <param name="postalCode">postalCode.</param>
        /// <param name="countryCode">countryCode.</param>
        /// <param name="companyName">companyName.</param>
        /// <param name="signer">signer.</param>
        /// <param name="eventCode">eventCode.</param>
        public TrackEvent(DateTime? occurredAt = default(DateTime?), string description = default(string), string cityLocality = default(string), string stateProvince = default(string), string postalCode = default(string), string countryCode = default(string), string companyName = default(string), string signer = default(string), string eventCode = default(string))
        {
            this.OccurredAt = occurredAt;
            this.Description = description;
            this.CityLocality = cityLocality;
            this.StateProvince = stateProvince;
            this.PostalCode = postalCode;
            this.CountryCode = countryCode;
            this.CompanyName = companyName;
            this.Signer = signer;
            this.EventCode = eventCode;
        }

        /// <summary>
        /// Gets or Sets OccurredAt
        /// </summary>
        [DataMember(Name = "occurred_at", EmitDefaultValue = false)]
        public DateTime? OccurredAt { get; set; }

        /// <summary>
        /// Gets or Sets CarrierOccurredAt
        /// </summary>
        [DataMember(Name = "carrier_occurred_at", EmitDefaultValue = false)]
        public DateTime? CarrierOccurredAt { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets CityLocality
        /// </summary>
        [DataMember(Name = "city_locality", EmitDefaultValue = false)]
        public string CityLocality { get; set; }

        /// <summary>
        /// Gets or Sets StateProvince
        /// </summary>
        [DataMember(Name = "state_province", EmitDefaultValue = false)]
        public string StateProvince { get; set; }

        /// <summary>
        /// Gets or Sets PostalCode
        /// </summary>
        [DataMember(Name = "postal_code", EmitDefaultValue = false)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or Sets CountryCode
        /// </summary>
        [DataMember(Name = "country_code", EmitDefaultValue = false)]
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or Sets CompanyName
        /// </summary>
        [DataMember(Name = "company_name", EmitDefaultValue = false)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or Sets Signer
        /// </summary>
        [DataMember(Name = "signer", EmitDefaultValue = false)]
        public string Signer { get; set; }

        /// <summary>
        /// Gets or Sets EventCode
        /// </summary>
        [DataMember(Name = "event_code", EmitDefaultValue = false)]
        public string EventCode { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TrackEvent {\n");
            sb.Append("  OccurredAt: ").Append(OccurredAt).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  CityLocality: ").Append(CityLocality).Append("\n");
            sb.Append("  StateProvince: ").Append(StateProvince).Append("\n");
            sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
            sb.Append("  CountryCode: ").Append(CountryCode).Append("\n");
            sb.Append("  CompanyName: ").Append(CompanyName).Append("\n");
            sb.Append("  Signer: ").Append(Signer).Append("\n");
            sb.Append("  EventCode: ").Append(EventCode).Append("\n");
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
            return this.Equals(input as TrackEvent);
        }

        /// <summary>
        /// Returns true if TrackEvent instances are equal
        /// </summary>
        /// <param name="input">Instance of TrackEvent to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TrackEvent input)
        {
            if (input == null)
                return false;

            return
                (
                    this.OccurredAt == input.OccurredAt ||
                    (this.OccurredAt != null &&
                    this.OccurredAt.Equals(input.OccurredAt))
                ) &&
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) &&
                (
                    this.CityLocality == input.CityLocality ||
                    (this.CityLocality != null &&
                    this.CityLocality.Equals(input.CityLocality))
                ) &&
                (
                    this.StateProvince == input.StateProvince ||
                    (this.StateProvince != null &&
                    this.StateProvince.Equals(input.StateProvince))
                ) &&
                (
                    this.PostalCode == input.PostalCode ||
                    (this.PostalCode != null &&
                    this.PostalCode.Equals(input.PostalCode))
                ) &&
                (
                    this.CountryCode == input.CountryCode ||
                    (this.CountryCode != null &&
                    this.CountryCode.Equals(input.CountryCode))
                ) &&
                (
                    this.CompanyName == input.CompanyName ||
                    (this.CompanyName != null &&
                    this.CompanyName.Equals(input.CompanyName))
                ) &&
                (
                    this.Signer == input.Signer ||
                    (this.Signer != null &&
                    this.Signer.Equals(input.Signer))
                ) &&
                (
                    this.EventCode == input.EventCode ||
                    (this.EventCode != null &&
                    this.EventCode.Equals(input.EventCode))
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
                if (this.OccurredAt != null)
                    hashCode = hashCode * 59 + this.OccurredAt.GetHashCode();
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
                if (this.CityLocality != null)
                    hashCode = hashCode * 59 + this.CityLocality.GetHashCode();
                if (this.StateProvince != null)
                    hashCode = hashCode * 59 + this.StateProvince.GetHashCode();
                if (this.PostalCode != null)
                    hashCode = hashCode * 59 + this.PostalCode.GetHashCode();
                if (this.CountryCode != null)
                    hashCode = hashCode * 59 + this.CountryCode.GetHashCode();
                if (this.CompanyName != null)
                    hashCode = hashCode * 59 + this.CompanyName.GetHashCode();
                if (this.Signer != null)
                    hashCode = hashCode * 59 + this.Signer.GetHashCode();
                if (this.EventCode != null)
                    hashCode = hashCode * 59 + this.EventCode.GetHashCode();
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
