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
    /// Service
    /// </summary>
    [DataContract]
    public partial class Service : IEquatable<Service>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Service" /> class.
        /// </summary>
        /// <param name="carrierId">carrierId.</param>
        /// <param name="carrierCode">carrierCode.</param>
        /// <param name="serviceCode">serviceCode.</param>
        /// <param name="name">name.</param>
        /// <param name="domestic">domestic.</param>
        /// <param name="international">international.</param>
        /// <param name="isMultiPackageSupported">isMultiPackageSupported.</param>
        public Service(string carrierId = default(string), string carrierCode = default(string), string serviceCode = default(string), string name = default(string), bool? domestic = default(bool?), bool? international = default(bool?), bool? isMultiPackageSupported = default(bool?))
        {
            this.CarrierId = carrierId;
            this.CarrierCode = carrierCode;
            this.ServiceCode = serviceCode;
            this.Name = name;
            this.Domestic = domestic;
            this.International = international;
            this.IsMultiPackageSupported = isMultiPackageSupported;
        }

        /// <summary>
        /// Gets or Sets CarrierId
        /// </summary>
        [DataMember(Name = "carrier_id", EmitDefaultValue = false)]
        public string CarrierId { get; set; }

        /// <summary>
        /// Gets or Sets CarrierCode
        /// </summary>
        [DataMember(Name = "carrier_code", EmitDefaultValue = false)]
        public string CarrierCode { get; set; }

        /// <summary>
        /// Gets or Sets ServiceCode
        /// </summary>
        [DataMember(Name = "service_code", EmitDefaultValue = false)]
        public string ServiceCode { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Domestic
        /// </summary>
        [DataMember(Name = "domestic", EmitDefaultValue = false)]
        public bool? Domestic { get; set; }

        /// <summary>
        /// Gets or Sets International
        /// </summary>
        [DataMember(Name = "international", EmitDefaultValue = false)]
        public bool? International { get; set; }

        /// <summary>
        /// Gets or Sets IsMultiPackageSupported
        /// </summary>
        [DataMember(Name = "is_multi_package_supported", EmitDefaultValue = false)]
        public bool? IsMultiPackageSupported { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Service {\n");
            sb.Append("  CarrierId: ").Append(CarrierId).Append("\n");
            sb.Append("  CarrierCode: ").Append(CarrierCode).Append("\n");
            sb.Append("  ServiceCode: ").Append(ServiceCode).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Domestic: ").Append(Domestic).Append("\n");
            sb.Append("  International: ").Append(International).Append("\n");
            sb.Append("  IsMultiPackageSupported: ").Append(IsMultiPackageSupported).Append("\n");
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
            return this.Equals(input as Service);
        }

        /// <summary>
        /// Returns true if Service instances are equal
        /// </summary>
        /// <param name="input">Instance of Service to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Service input)
        {
            if (input == null)
                return false;

            return
                (
                    this.CarrierId == input.CarrierId ||
                    (this.CarrierId != null &&
                    this.CarrierId.Equals(input.CarrierId))
                ) &&
                (
                    this.CarrierCode == input.CarrierCode ||
                    (this.CarrierCode != null &&
                    this.CarrierCode.Equals(input.CarrierCode))
                ) &&
                (
                    this.ServiceCode == input.ServiceCode ||
                    (this.ServiceCode != null &&
                    this.ServiceCode.Equals(input.ServiceCode))
                ) &&
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) &&
                (
                    this.Domestic == input.Domestic ||
                    (this.Domestic != null &&
                    this.Domestic.Equals(input.Domestic))
                ) &&
                (
                    this.International == input.International ||
                    (this.International != null &&
                    this.International.Equals(input.International))
                ) &&
                (
                    this.IsMultiPackageSupported == input.IsMultiPackageSupported ||
                    (this.IsMultiPackageSupported != null &&
                    this.IsMultiPackageSupported.Equals(input.IsMultiPackageSupported))
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
                if (this.CarrierId != null)
                    hashCode = hashCode * 59 + this.CarrierId.GetHashCode();
                if (this.CarrierCode != null)
                    hashCode = hashCode * 59 + this.CarrierCode.GetHashCode();
                if (this.ServiceCode != null)
                    hashCode = hashCode * 59 + this.ServiceCode.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.Domestic != null)
                    hashCode = hashCode * 59 + this.Domestic.GetHashCode();
                if (this.International != null)
                    hashCode = hashCode * 59 + this.International.GetHashCode();
                if (this.IsMultiPackageSupported != null)
                    hashCode = hashCode * 59 + this.IsMultiPackageSupported.GetHashCode();
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
