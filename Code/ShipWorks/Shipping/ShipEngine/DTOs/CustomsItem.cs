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
    /// CustomsItem
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class CustomsItem : IEquatable<CustomsItem>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomsItem" /> class.
        /// </summary>
        public CustomsItem(string customsItemId = default(string), string description = default(string), int? quantity = default(int?), double? value = default(double?), string harmonizedTariffCode = default(string), string countryOfOrigin = default(string), string sku = default(string))
        {
            this.CustomsItemId = customsItemId;
            this.Description = description;
            this.Quantity = quantity;
            this.Value = value;
            this.HarmonizedTariffCode = harmonizedTariffCode;
            this.CountryOfOrigin = countryOfOrigin;
            this.SKU = sku;
        }

        /// <summary>
        /// Gets or Sets CustomsItemId
        /// </summary>
        [DataMember(Name = "customs_item_id", EmitDefaultValue = false)]
        public string CustomsItemId { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets Quantity
        /// </summary>
        [DataMember(Name = "quantity", EmitDefaultValue = false)]
        public int? Quantity { get; set; }

        /// <summary>
        /// Gets or Sets Value
        /// </summary>
        [DataMember(Name = "value", EmitDefaultValue = false)]
        public double? Value { get; set; }

        /// <summary>
        /// Gets or Sets HarmonizedTariffCode
        /// </summary>
        [DataMember(Name = "harmonized_tariff_code", EmitDefaultValue = false)]
        public string HarmonizedTariffCode { get; set; }

        /// <summary>
        /// Gets or Sets CountryOfOrigin
        /// </summary>
        [DataMember(Name = "country_of_origin", EmitDefaultValue = false)]
        public string CountryOfOrigin { get; set; }

        /// <summary>
        /// Gets or Sets SKU
        /// </summary>
        [DataMember(Name = "sku", EmitDefaultValue = false)]
        public string SKU { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CustomsItem {\n");
            sb.Append("  CustomsItemId: ").Append(CustomsItemId).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Quantity: ").Append(Quantity).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  HarmonizedTariffCode: ").Append(HarmonizedTariffCode).Append("\n");
            sb.Append("  CountryOfOrigin: ").Append(CountryOfOrigin).Append("\n");
            sb.Append("  SKU: ").Append(SKU).Append("\n");
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
            return this.Equals(input as CustomsItem);
        }

        /// <summary>
        /// Returns true if CustomsItem instances are equal
        /// </summary>
        /// <param name="input">Instance of CustomsItem to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CustomsItem input)
        {
            if (input == null)
                return false;

            return
                (
                    this.CustomsItemId == input.CustomsItemId ||
                    (this.CustomsItemId != null &&
                    this.CustomsItemId.Equals(input.CustomsItemId))
                ) &&
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) &&
                (
                    this.Quantity == input.Quantity ||
                    (this.Quantity != null &&
                    this.Quantity.Equals(input.Quantity))
                ) &&
                (
                    this.Value == input.Value ||
                    (this.Value != null &&
                    this.Value.Equals(input.Value))
                ) &&
                (
                    this.HarmonizedTariffCode == input.HarmonizedTariffCode ||
                    (this.HarmonizedTariffCode != null &&
                    this.HarmonizedTariffCode.Equals(input.HarmonizedTariffCode))
                ) &&
                (
                    this.CountryOfOrigin == input.CountryOfOrigin ||
                    (this.CountryOfOrigin != null &&
                    this.CountryOfOrigin.Equals(input.CountryOfOrigin))
                ) &&
                (
                    this.SKU == input.SKU ||
                    (this.SKU != null &&
                    this.SKU.Equals(input.SKU))
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
                if (this.CustomsItemId != null)
                    hashCode = hashCode * 59 + this.CustomsItemId.GetHashCode();
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
                if (this.Quantity != null)
                    hashCode = hashCode * 59 + this.Quantity.GetHashCode();
                if (this.Value != null)
                    hashCode = hashCode * 59 + this.Value.GetHashCode();
                if (this.HarmonizedTariffCode != null)
                    hashCode = hashCode * 59 + this.HarmonizedTariffCode.GetHashCode();
                if (this.CountryOfOrigin != null)
                    hashCode = hashCode * 59 + this.CountryOfOrigin.GetHashCode();
                if (this.SKU != null)
                    hashCode = hashCode * 59 + this.SKU.GetHashCode();
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
