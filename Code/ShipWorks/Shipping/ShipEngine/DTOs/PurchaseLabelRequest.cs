using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// PurchaseLabelRequest
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class PurchaseLabelRequest : IEquatable<PurchaseLabelRequest>, IValidatableObject
    {
        /// <summary>
        /// Defines ValidateAddress
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum ValidateAddressEnum
        {

            /// <summary>
            /// Enum NoValidation for value: noValidation
            /// </summary>
            [EnumMember(Value = "noValidation")]
            NoValidation = 1,

            /// <summary>
            /// Enum ValidateOnly for value: validateOnly
            /// </summary>
            [EnumMember(Value = "validateOnly")]
            ValidateOnly = 2,

            /// <summary>
            /// Enum ValidateAndClean for value: validateAndClean
            /// </summary>
            [EnumMember(Value = "validateAndClean")]
            ValidateAndClean = 3
        }

        /// <summary>
        /// Gets or Sets ValidateAddress
        /// </summary>
        [DataMember(Name = "validate_address", EmitDefaultValue = false)]
        public ValidateAddressEnum? ValidateAddress { get; set; }
        /// <summary>
        /// Defines LabelFormat
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum LabelFormatEnum
        {

            /// <summary>
            /// Enum Pdf for value: pdf
            /// </summary>
            [EnumMember(Value = "pdf")]
            Pdf = 1,

            /// <summary>
            /// Enum Zpl for value: zpl
            /// </summary>
            [EnumMember(Value = "zpl")]
            Zpl = 2,

            /// <summary>
            /// Enum Png for value: png
            /// </summary>
            [EnumMember(Value = "png")]
            Png = 3
        }

        /// <summary>
        /// Gets or Sets LabelFormat
        /// </summary>
        [DataMember(Name = "label_format", EmitDefaultValue = false)]
        public LabelFormatEnum? LabelFormat { get; set; }
        /// <summary>
        /// Defines LabelDownloadType
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum LabelDownloadTypeEnum
        {

            /// <summary>
            /// Enum Url for value: url
            /// </summary>
            [EnumMember(Value = "url")]
            Url = 1,

            /// <summary>
            /// Enum Inline for value: inline
            /// </summary>
            [EnumMember(Value = "inline")]
            Inline = 2
        }

        /// <summary>
        /// Gets or Sets LabelDownloadType
        /// </summary>
        [DataMember(Name = "label_download_type", EmitDefaultValue = false)]
        public LabelDownloadTypeEnum? LabelDownloadType { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseLabelRequest" /> class.
        /// </summary>
        /// <param name="shipment">shipment.</param>
        /// <param name="isReturnLabel">isReturnLabel.</param>
        /// <param name="rmaNumber">rmaNumber.</param>
        /// <param name="testLabel">testLabel.</param>
        /// <param name="validateAddress">validateAddress.</param>
        /// <param name="labelLayout">labelLayout.</param>
        /// <param name="labelFormat">labelFormat.</param>
        /// <param name="labelDownloadType">labelDownloadType.</param>
        public PurchaseLabelRequest(Shipment shipment = default(Shipment), bool? isReturnLabel = default(bool?), string rmaNumber = default(string), bool? testLabel = default(bool?), ValidateAddressEnum? validateAddress = default(ValidateAddressEnum?), string labelLayout = default(string), LabelFormatEnum? labelFormat = default(LabelFormatEnum?), LabelDownloadTypeEnum? labelDownloadType = default(LabelDownloadTypeEnum?))
        {
            this.Shipment = shipment;
            this.IsReturnLabel = isReturnLabel;
            this.RmaNumber = rmaNumber;
            this.TestLabel = testLabel;
            this.ValidateAddress = validateAddress;
            this.LabelLayout = labelLayout;
            this.LabelFormat = labelFormat;
            this.LabelDownloadType = labelDownloadType;
        }

        /// <summary>
        /// Gets or Sets Shipment
        /// </summary>
        [DataMember(Name = "shipment", EmitDefaultValue = false)]
        public Shipment Shipment { get; set; }

        /// <summary>
        /// Gets or Sets IsReturnLabel
        /// </summary>
        [DataMember(Name = "is_return_label", EmitDefaultValue = false)]
        public bool? IsReturnLabel { get; set; }

        /// <summary>
        /// Gets or Sets RmaNumber
        /// </summary>
        [DataMember(Name = "rma_number", EmitDefaultValue = false)]
        public string RmaNumber { get; set; }

        /// <summary>
        /// Gets or Sets TestLabel
        /// </summary>
        [DataMember(Name = "test_label", EmitDefaultValue = false)]
        public bool? TestLabel { get; set; }


        /// <summary>
        /// Gets or Sets LabelLayout
        /// </summary>
        [DataMember(Name = "label_layout", EmitDefaultValue = false)]
        public string LabelLayout { get; set; }

        [DataMember(Name = "ship_to_service_point_id", EmitDefaultValue = false)]
        public string ShipToServicePointId { get; set; }



        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PurchaseLabelRequest {\n");
            sb.Append("  Shipment: ").Append(Shipment).Append("\n");
            sb.Append("  IsReturnLabel: ").Append(IsReturnLabel).Append("\n");
            sb.Append("  RmaNumber: ").Append(RmaNumber).Append("\n");
            sb.Append("  TestLabel: ").Append(TestLabel).Append("\n");
            sb.Append("  ValidateAddress: ").Append(ValidateAddress).Append("\n");
            sb.Append("  LabelLayout: ").Append(LabelLayout).Append("\n");
            sb.Append("  LabelFormat: ").Append(LabelFormat).Append("\n");
            sb.Append("  LabelDownloadType: ").Append(LabelDownloadType).Append("\n");
            sb.Append("  ShipToServicePointId: ").Append(ShipToServicePointId).Append("\n");
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
            return this.Equals(input as PurchaseLabelRequest);
        }

        /// <summary>
        /// Returns true if PurchaseLabelRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of PurchaseLabelRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PurchaseLabelRequest input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Shipment == input.Shipment ||
                    (this.Shipment != null &&
                    this.Shipment.Equals(input.Shipment))
                ) &&
                (
                    this.IsReturnLabel == input.IsReturnLabel ||
                    (this.IsReturnLabel != null &&
                    this.IsReturnLabel.Equals(input.IsReturnLabel))
                ) &&
                (
                    this.RmaNumber == input.RmaNumber ||
                    (this.RmaNumber != null &&
                    this.RmaNumber.Equals(input.RmaNumber))
                ) &&
                (
                    this.TestLabel == input.TestLabel ||
                    (this.TestLabel != null &&
                    this.TestLabel.Equals(input.TestLabel))
                ) &&
                (
                    this.ValidateAddress == input.ValidateAddress ||
                    (this.ValidateAddress != null &&
                    this.ValidateAddress.Equals(input.ValidateAddress))
                ) &&
                (
                    this.LabelLayout == input.LabelLayout ||
                    (this.LabelLayout != null &&
                    this.LabelLayout.Equals(input.LabelLayout))
                ) &&
                (
                    this.LabelFormat == input.LabelFormat ||
                    (this.LabelFormat != null &&
                    this.LabelFormat.Equals(input.LabelFormat))
                ) &&
                (
                    this.LabelDownloadType == input.LabelDownloadType ||
                    (this.LabelDownloadType != null &&
                    this.LabelDownloadType.Equals(input.LabelDownloadType))
                ) &&
                (
                    this.ShipToServicePointId == input.ShipToServicePointId ||
                    (this.ShipToServicePointId != null &&
                    this.ShipToServicePointId.Equals(input.ShipToServicePointId))
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
                if (this.Shipment != null)
                    hashCode = hashCode * 59 + this.Shipment.GetHashCode();
                if (this.IsReturnLabel != null)
                    hashCode = hashCode * 59 + this.IsReturnLabel.GetHashCode();
                if (this.RmaNumber != null)
                    hashCode = hashCode * 59 + this.RmaNumber.GetHashCode();
                if (this.TestLabel != null)
                    hashCode = hashCode * 59 + this.TestLabel.GetHashCode();
                if (this.ValidateAddress != null)
                    hashCode = hashCode * 59 + this.ValidateAddress.GetHashCode();
                if (this.LabelLayout != null)
                    hashCode = hashCode * 59 + this.LabelLayout.GetHashCode();
                if (this.LabelFormat != null)
                    hashCode = hashCode * 59 + this.LabelFormat.GetHashCode();
                if (this.LabelDownloadType != null)
                    hashCode = hashCode * 59 + this.LabelDownloadType.GetHashCode();
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
