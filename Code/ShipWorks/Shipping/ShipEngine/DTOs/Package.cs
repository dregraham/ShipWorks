using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// Package
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class Package : IEquatable<Package>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Package" /> class.
        /// </summary>
        [JsonConstructor]
        protected Package() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Package" /> class.
        /// </summary>
        /// <param name="packageId">packageId.</param>
        /// <param name="packageCode">packageCode.</param>
        /// <param name="name">name (required).</param>
        /// <param name="dimensions">dimensions.</param>
        /// <param name="description">description.</param>
        public Package(string packageId = default(string), string packageCode = default(string), string name = default(string), Dimensions dimensions = default(Dimensions), string description = default(string))
        {
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new InvalidDataException("name is a required property for Package and cannot be null");
            }
            else
            {
                this.Name = name;
            }
            this.PackageId = packageId;
            this.PackageCode = packageCode;
            this.Dimensions = dimensions;
            this.Description = description;
        }

        /// <summary>
        /// Gets or Sets PackageId
        /// </summary>
        [DataMember(Name = "package_id", EmitDefaultValue = false)]
        public string PackageId { get; set; }

        /// <summary>
        /// Gets or Sets PackageCode
        /// </summary>
        [DataMember(Name = "package_code", EmitDefaultValue = false)]
        public string PackageCode { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Dimensions
        /// </summary>
        [DataMember(Name = "dimensions", EmitDefaultValue = false)]
        public Dimensions Dimensions { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Package {\n");
            sb.Append("  PackageId: ").Append(PackageId).Append("\n");
            sb.Append("  PackageCode: ").Append(PackageCode).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Dimensions: ").Append(Dimensions).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
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
            return this.Equals(input as Package);
        }

        /// <summary>
        /// Returns true if Package instances are equal
        /// </summary>
        /// <param name="input">Instance of Package to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Package input)
        {
            if (input == null)
                return false;

            return
                (
                    this.PackageId == input.PackageId ||
                    (this.PackageId != null &&
                    this.PackageId.Equals(input.PackageId))
                ) &&
                (
                    this.PackageCode == input.PackageCode ||
                    (this.PackageCode != null &&
                    this.PackageCode.Equals(input.PackageCode))
                ) &&
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) &&
                (
                    this.Dimensions == input.Dimensions ||
                    (this.Dimensions != null &&
                    this.Dimensions.Equals(input.Dimensions))
                ) &&
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
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
                if (this.PackageId != null)
                    hashCode = hashCode * 59 + this.PackageId.GetHashCode();
                if (this.PackageCode != null)
                    hashCode = hashCode * 59 + this.PackageCode.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.Dimensions != null)
                    hashCode = hashCode * 59 + this.Dimensions.GetHashCode();
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
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
