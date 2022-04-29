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
    /// InternationalOptions
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class InternationalOptions : IEquatable<InternationalOptions>, IValidatableObject
    {
        /// <summary>
        /// Defines Contents
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ContentsEnum
        {

            /// <summary>
            /// Enum Merchandise for value: merchandise
            /// </summary>
            [EnumMember(Value = "merchandise")]
            Merchandise = 1,

            /// <summary>
            /// Enum Documents for value: documents
            /// </summary>
            [EnumMember(Value = "documents")]
            Documents = 2,

            /// <summary>
            /// Enum Gift for value: gift
            /// </summary>
            [EnumMember(Value = "gift")]
            Gift = 3,

            /// <summary>
            /// Enum Returnedgoods for value: returned_goods
            /// </summary>
            [EnumMember(Value = "returned_goods")]
            Returnedgoods = 4,

            /// <summary>
            /// Enum Sample for value: sample
            /// </summary>
            [EnumMember(Value = "sample")]
            Sample = 5
        }

        /// <summary>
        /// Gets or Sets Contents
        /// </summary>
        [DataMember(Name = "contents", EmitDefaultValue = false)]
        public ContentsEnum? Contents { get; set; }
        /// <summary>
        /// Defines NonDelivery
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum NonDeliveryEnum
        {

            /// <summary>
            /// Enum Returntosender for value: return_to_sender
            /// </summary>
            [EnumMember(Value = "return_to_sender")]
            Returntosender = 1,

            /// <summary>
            /// Enum Treatasabandoned for value: treat_as_abandoned
            /// </summary>
            [EnumMember(Value = "treat_as_abandoned")]
            Treatasabandoned = 2
        }

        /// <summary>
        /// Gets or Sets NonDelivery
        /// </summary>
        [DataMember(Name = "non_delivery", EmitDefaultValue = false)]
        public NonDeliveryEnum? NonDelivery { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="InternationalOptions" /> class.
        /// </summary>
        /// <param name="contents">contents.</param>
        /// <param name="customsItems">customsItems.</param>
        /// <param name="nonDelivery">nonDelivery.</param>
        public InternationalOptions(ContentsEnum? contents = default(ContentsEnum?), List<CustomsItem> customsItems = default(List<CustomsItem>), NonDeliveryEnum? nonDelivery = default(NonDeliveryEnum?))
        {
            this.Contents = contents;
            this.CustomsItems = customsItems;
            this.NonDelivery = nonDelivery;
        }


        /// <summary>
        /// Gets or Sets CustomsItems
        /// </summary>
        [DataMember(Name = "customs_items", EmitDefaultValue = false)]
        public List<CustomsItem> CustomsItems { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class InternationalOptions {\n");
            sb.Append("  Contents: ").Append(Contents).Append("\n");
            sb.Append("  CustomsItems: ").Append(CustomsItems).Append("\n");
            sb.Append("  NonDelivery: ").Append(NonDelivery).Append("\n");
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
            return this.Equals(input as InternationalOptions);
        }

        /// <summary>
        /// Returns true if InternationalOptions instances are equal
        /// </summary>
        /// <param name="input">Instance of InternationalOptions to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(InternationalOptions input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Contents == input.Contents ||
                    (this.Contents != null &&
                    this.Contents.Equals(input.Contents))
                ) &&
                (
                    this.CustomsItems == input.CustomsItems ||
                    this.CustomsItems != null &&
                    this.CustomsItems.SequenceEqual(input.CustomsItems)
                ) &&
                (
                    this.NonDelivery == input.NonDelivery ||
                    (this.NonDelivery != null &&
                    this.NonDelivery.Equals(input.NonDelivery))
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
                if (this.Contents != null)
                    hashCode = hashCode * 59 + this.Contents.GetHashCode();
                if (this.CustomsItems != null)
                    hashCode = hashCode * 59 + this.CustomsItems.GetHashCode();
                if (this.NonDelivery != null)
                    hashCode = hashCode * 59 + this.NonDelivery.GetHashCode();
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
