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
    /// Rate
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class Rate : IEquatable<Rate>, IValidatableObject
    {
        /// <summary>
        /// Defines RateType
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum RateTypeEnum
        {

            /// <summary>
            /// Enum Check for value: check
            /// </summary>
            [EnumMember(Value = "check")]
            Check = 1,

            /// <summary>
            /// Enum Shipment for value: shipment
            /// </summary>
            [EnumMember(Value = "shipment")]
            Shipment = 2
        }

        /// <summary>
        /// Gets or Sets RateType
        /// </summary>
        [DataMember(Name = "rate_type", EmitDefaultValue = false)]
        public RateTypeEnum? RateType { get; set; }
        /// <summary>
        /// Defines ValidationStatus
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum ValidationStatusEnum
        {

            /// <summary>
            /// Enum Valid for value: valid
            /// </summary>
            [EnumMember(Value = "valid")]
            Valid = 1,

            /// <summary>
            /// Enum Invalid for value: invalid
            /// </summary>
            [EnumMember(Value = "invalid")]
            Invalid = 2,

            /// <summary>
            /// Enum Haswarnings for value: has_warnings
            /// </summary>
            [EnumMember(Value = "has_warnings")]
            Haswarnings = 3,

            /// <summary>
            /// Enum Unknown for value: unknown
            /// </summary>
            [EnumMember(Value = "unknown")]
            Unknown = 4
        }

        /// <summary>
        /// Gets or Sets ValidationStatus
        /// </summary>
        [DataMember(Name = "validation_status", EmitDefaultValue = false)]
        public ValidationStatusEnum? ValidationStatus { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Rate" /> class.
        /// </summary>
        /// <param name="rateId">rateId.</param>
        /// <param name="rateType">rateType.</param>
        /// <param name="carrierId">carrierId.</param>
        /// <param name="shippingAmount">shippingAmount.</param>
        /// <param name="insuranceAmount">insuranceAmount.</param>
        /// <param name="confirmationAmount">confirmationAmount.</param>
        /// <param name="otherAmount">otherAmount.</param>
        /// <param name="zone">zone.</param>
        /// <param name="packageType">packageType.</param>
        /// <param name="deliveryDays">deliveryDays.</param>
        /// <param name="guaranteedService">guaranteedService.</param>
        /// <param name="estimatedDeliveryDate">estimatedDeliveryDate.</param>
        /// <param name="carrierDeliveryDays">carrierDeliveryDays.</param>
        /// <param name="shipDate">shipDate.</param>
        /// <param name="negotiatedRate">negotiatedRate.</param>
        /// <param name="serviceType">serviceType.</param>
        /// <param name="serviceCode">serviceCode.</param>
        /// <param name="trackable">trackable.</param>
        /// <param name="carrierCode">carrierCode.</param>
        /// <param name="carrierNickname">carrierNickname.</param>
        /// <param name="carrierFriendlyName">carrierFriendlyName.</param>
        /// <param name="validationStatus">validationStatus.</param>
        /// <param name="warningMessages">warningMessages.</param>
        /// <param name="errorMessages">errorMessages.</param>
        public Rate(string rateId = default(string), RateTypeEnum? rateType = default(RateTypeEnum?), string carrierId = default(string), MoneyDTO shippingAmount = default(MoneyDTO), MoneyDTO insuranceAmount = default(MoneyDTO), MoneyDTO confirmationAmount = default(MoneyDTO), MoneyDTO otherAmount = default(MoneyDTO), int? zone = default(int?), string packageType = default(string), int? deliveryDays = default(int?), bool? guaranteedService = default(bool?), DateTime? estimatedDeliveryDate = default(DateTime?), string carrierDeliveryDays = default(string), DateTime? shipDate = default(DateTime?), bool? negotiatedRate = default(bool?), string serviceType = default(string), string serviceCode = default(string), bool? trackable = default(bool?), string carrierCode = default(string), string carrierNickname = default(string), string carrierFriendlyName = default(string), ValidationStatusEnum? validationStatus = default(ValidationStatusEnum?), List<string> warningMessages = default(List<string>), List<string> errorMessages = default(List<string>))
        {
            this.RateId = rateId;
            this.RateType = rateType;
            this.CarrierId = carrierId;
            this.ShippingAmount = shippingAmount;
            this.InsuranceAmount = insuranceAmount;
            this.ConfirmationAmount = confirmationAmount;
            this.OtherAmount = otherAmount;
            this.Zone = zone;
            this.PackageType = packageType;
            this.DeliveryDays = deliveryDays;
            this.GuaranteedService = guaranteedService;
            this.EstimatedDeliveryDate = estimatedDeliveryDate;
            this.CarrierDeliveryDays = carrierDeliveryDays;
            this.ShipDate = shipDate;
            this.NegotiatedRate = negotiatedRate;
            this.ServiceType = serviceType;
            this.ServiceCode = serviceCode;
            this.Trackable = trackable;
            this.CarrierCode = carrierCode;
            this.CarrierNickname = carrierNickname;
            this.CarrierFriendlyName = carrierFriendlyName;
            this.ValidationStatus = validationStatus;
            this.WarningMessages = warningMessages;
            this.ErrorMessages = errorMessages;
        }

        /// <summary>
        /// Gets or Sets RateId
        /// </summary>
        [DataMember(Name = "rate_id", EmitDefaultValue = false)]
        public string RateId { get; set; }


        /// <summary>
        /// Gets or Sets CarrierId
        /// </summary>
        [DataMember(Name = "carrier_id", EmitDefaultValue = false)]
        public string CarrierId { get; set; }

        /// <summary>
        /// Gets or Sets ShippingAmount
        /// </summary>
        [DataMember(Name = "shipping_amount", EmitDefaultValue = false)]
        public MoneyDTO ShippingAmount { get; set; }

        /// <summary>
        /// Gets or Sets InsuranceAmount
        /// </summary>
        [DataMember(Name = "insurance_amount", EmitDefaultValue = false)]
        public MoneyDTO InsuranceAmount { get; set; }

        /// <summary>
        /// Gets or Sets ConfirmationAmount
        /// </summary>
        [DataMember(Name = "confirmation_amount", EmitDefaultValue = false)]
        public MoneyDTO ConfirmationAmount { get; set; }

        /// <summary>
        /// Gets or Sets OtherAmount
        /// </summary>
        [DataMember(Name = "other_amount", EmitDefaultValue = false)]
        public MoneyDTO OtherAmount { get; set; }

        /// <summary>
        /// Gets or Sets Zone
        /// </summary>
        [DataMember(Name = "zone", EmitDefaultValue = false)]
        public int? Zone { get; set; }

        /// <summary>
        /// Gets or Sets PackageType
        /// </summary>
        [DataMember(Name = "package_type", EmitDefaultValue = false)]
        public string PackageType { get; set; }

        /// <summary>
        /// Gets or Sets DeliveryDays
        /// </summary>
        [DataMember(Name = "delivery_days", EmitDefaultValue = false)]
        public int? DeliveryDays { get; set; }

        /// <summary>
        /// Gets or Sets GuaranteedService
        /// </summary>
        [DataMember(Name = "guaranteed_service", EmitDefaultValue = false)]
        public bool? GuaranteedService { get; set; }

        /// <summary>
        /// Gets or Sets EstimatedDeliveryDate
        /// </summary>
        [DataMember(Name = "estimated_delivery_date", EmitDefaultValue = false)]
        public DateTime? EstimatedDeliveryDate { get; set; }

        /// <summary>
        /// Gets or Sets CarrierDeliveryDays
        /// </summary>
        [DataMember(Name = "carrier_delivery_days", EmitDefaultValue = false)]
        public string CarrierDeliveryDays { get; set; }

        /// <summary>
        /// Gets or Sets ShipDate
        /// </summary>
        [DataMember(Name = "ship_date", EmitDefaultValue = false)]
        public DateTime? ShipDate { get; set; }

        /// <summary>
        /// Gets or Sets NegotiatedRate
        /// </summary>
        [DataMember(Name = "negotiated_rate", EmitDefaultValue = false)]
        public bool? NegotiatedRate { get; set; }

        /// <summary>
        /// Gets or Sets ServiceType
        /// </summary>
        [DataMember(Name = "service_type", EmitDefaultValue = false)]
        public string ServiceType { get; set; }

        /// <summary>
        /// Gets or Sets ServiceCode
        /// </summary>
        [DataMember(Name = "service_code", EmitDefaultValue = false)]
        public string ServiceCode { get; set; }

        /// <summary>
        /// Gets or Sets Trackable
        /// </summary>
        [DataMember(Name = "trackable", EmitDefaultValue = false)]
        public bool? Trackable { get; set; }

        /// <summary>
        /// Gets or Sets CarrierCode
        /// </summary>
        [DataMember(Name = "carrier_code", EmitDefaultValue = false)]
        public string CarrierCode { get; set; }

        /// <summary>
        /// Gets or Sets CarrierNickname
        /// </summary>
        [DataMember(Name = "carrier_nickname", EmitDefaultValue = false)]
        public string CarrierNickname { get; set; }

        /// <summary>
        /// Gets or Sets CarrierFriendlyName
        /// </summary>
        [DataMember(Name = "carrier_friendly_name", EmitDefaultValue = false)]
        public string CarrierFriendlyName { get; set; }


        /// <summary>
        /// Gets or Sets WarningMessages
        /// </summary>
        [DataMember(Name = "warning_messages", EmitDefaultValue = false)]
        public List<string> WarningMessages { get; set; }

        /// <summary>
        /// Gets or Sets ErrorMessages
        /// </summary>
        [DataMember(Name = "error_messages", EmitDefaultValue = false)]
        public List<string> ErrorMessages { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Rate {\n");
            sb.Append("  RateId: ").Append(RateId).Append("\n");
            sb.Append("  RateType: ").Append(RateType).Append("\n");
            sb.Append("  CarrierId: ").Append(CarrierId).Append("\n");
            sb.Append("  ShippingAmount: ").Append(ShippingAmount).Append("\n");
            sb.Append("  InsuranceAmount: ").Append(InsuranceAmount).Append("\n");
            sb.Append("  ConfirmationAmount: ").Append(ConfirmationAmount).Append("\n");
            sb.Append("  OtherAmount: ").Append(OtherAmount).Append("\n");
            sb.Append("  Zone: ").Append(Zone).Append("\n");
            sb.Append("  PackageType: ").Append(PackageType).Append("\n");
            sb.Append("  DeliveryDays: ").Append(DeliveryDays).Append("\n");
            sb.Append("  GuaranteedService: ").Append(GuaranteedService).Append("\n");
            sb.Append("  EstimatedDeliveryDate: ").Append(EstimatedDeliveryDate).Append("\n");
            sb.Append("  CarrierDeliveryDays: ").Append(CarrierDeliveryDays).Append("\n");
            sb.Append("  ShipDate: ").Append(ShipDate).Append("\n");
            sb.Append("  NegotiatedRate: ").Append(NegotiatedRate).Append("\n");
            sb.Append("  ServiceType: ").Append(ServiceType).Append("\n");
            sb.Append("  ServiceCode: ").Append(ServiceCode).Append("\n");
            sb.Append("  Trackable: ").Append(Trackable).Append("\n");
            sb.Append("  CarrierCode: ").Append(CarrierCode).Append("\n");
            sb.Append("  CarrierNickname: ").Append(CarrierNickname).Append("\n");
            sb.Append("  CarrierFriendlyName: ").Append(CarrierFriendlyName).Append("\n");
            sb.Append("  ValidationStatus: ").Append(ValidationStatus).Append("\n");
            sb.Append("  WarningMessages: ").Append(WarningMessages).Append("\n");
            sb.Append("  ErrorMessages: ").Append(ErrorMessages).Append("\n");
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
            return this.Equals(input as Rate);
        }

        /// <summary>
        /// Returns true if Rate instances are equal
        /// </summary>
        /// <param name="input">Instance of Rate to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Rate input)
        {
            if (input == null)
                return false;

            return
                (
                    this.RateId == input.RateId ||
                    (this.RateId != null &&
                    this.RateId.Equals(input.RateId))
                ) &&
                (
                    this.RateType == input.RateType ||
                    (this.RateType != null &&
                    this.RateType.Equals(input.RateType))
                ) &&
                (
                    this.CarrierId == input.CarrierId ||
                    (this.CarrierId != null &&
                    this.CarrierId.Equals(input.CarrierId))
                ) &&
                (
                    this.ShippingAmount == input.ShippingAmount ||
                    (this.ShippingAmount != null &&
                    this.ShippingAmount.Equals(input.ShippingAmount))
                ) &&
                (
                    this.InsuranceAmount == input.InsuranceAmount ||
                    (this.InsuranceAmount != null &&
                    this.InsuranceAmount.Equals(input.InsuranceAmount))
                ) &&
                (
                    this.ConfirmationAmount == input.ConfirmationAmount ||
                    (this.ConfirmationAmount != null &&
                    this.ConfirmationAmount.Equals(input.ConfirmationAmount))
                ) &&
                (
                    this.OtherAmount == input.OtherAmount ||
                    (this.OtherAmount != null &&
                    this.OtherAmount.Equals(input.OtherAmount))
                ) &&
                (
                    this.Zone == input.Zone ||
                    (this.Zone != null &&
                    this.Zone.Equals(input.Zone))
                ) &&
                (
                    this.PackageType == input.PackageType ||
                    (this.PackageType != null &&
                    this.PackageType.Equals(input.PackageType))
                ) &&
                (
                    this.DeliveryDays == input.DeliveryDays ||
                    (this.DeliveryDays != null &&
                    this.DeliveryDays.Equals(input.DeliveryDays))
                ) &&
                (
                    this.GuaranteedService == input.GuaranteedService ||
                    (this.GuaranteedService != null &&
                    this.GuaranteedService.Equals(input.GuaranteedService))
                ) &&
                (
                    this.EstimatedDeliveryDate == input.EstimatedDeliveryDate ||
                    (this.EstimatedDeliveryDate != null &&
                    this.EstimatedDeliveryDate.Equals(input.EstimatedDeliveryDate))
                ) &&
                (
                    this.CarrierDeliveryDays == input.CarrierDeliveryDays ||
                    (this.CarrierDeliveryDays != null &&
                    this.CarrierDeliveryDays.Equals(input.CarrierDeliveryDays))
                ) &&
                (
                    this.ShipDate == input.ShipDate ||
                    (this.ShipDate != null &&
                    this.ShipDate.Equals(input.ShipDate))
                ) &&
                (
                    this.NegotiatedRate == input.NegotiatedRate ||
                    (this.NegotiatedRate != null &&
                    this.NegotiatedRate.Equals(input.NegotiatedRate))
                ) &&
                (
                    this.ServiceType == input.ServiceType ||
                    (this.ServiceType != null &&
                    this.ServiceType.Equals(input.ServiceType))
                ) &&
                (
                    this.ServiceCode == input.ServiceCode ||
                    (this.ServiceCode != null &&
                    this.ServiceCode.Equals(input.ServiceCode))
                ) &&
                (
                    this.Trackable == input.Trackable ||
                    (this.Trackable != null &&
                    this.Trackable.Equals(input.Trackable))
                ) &&
                (
                    this.CarrierCode == input.CarrierCode ||
                    (this.CarrierCode != null &&
                    this.CarrierCode.Equals(input.CarrierCode))
                ) &&
                (
                    this.CarrierNickname == input.CarrierNickname ||
                    (this.CarrierNickname != null &&
                    this.CarrierNickname.Equals(input.CarrierNickname))
                ) &&
                (
                    this.CarrierFriendlyName == input.CarrierFriendlyName ||
                    (this.CarrierFriendlyName != null &&
                    this.CarrierFriendlyName.Equals(input.CarrierFriendlyName))
                ) &&
                (
                    this.ValidationStatus == input.ValidationStatus ||
                    (this.ValidationStatus != null &&
                    this.ValidationStatus.Equals(input.ValidationStatus))
                ) &&
                (
                    this.WarningMessages == input.WarningMessages ||
                    this.WarningMessages != null &&
                    this.WarningMessages.SequenceEqual(input.WarningMessages)
                ) &&
                (
                    this.ErrorMessages == input.ErrorMessages ||
                    this.ErrorMessages != null &&
                    this.ErrorMessages.SequenceEqual(input.ErrorMessages)
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
                if (this.RateId != null)
                    hashCode = hashCode * 59 + this.RateId.GetHashCode();
                if (this.RateType != null)
                    hashCode = hashCode * 59 + this.RateType.GetHashCode();
                if (this.CarrierId != null)
                    hashCode = hashCode * 59 + this.CarrierId.GetHashCode();
                if (this.ShippingAmount != null)
                    hashCode = hashCode * 59 + this.ShippingAmount.GetHashCode();
                if (this.InsuranceAmount != null)
                    hashCode = hashCode * 59 + this.InsuranceAmount.GetHashCode();
                if (this.ConfirmationAmount != null)
                    hashCode = hashCode * 59 + this.ConfirmationAmount.GetHashCode();
                if (this.OtherAmount != null)
                    hashCode = hashCode * 59 + this.OtherAmount.GetHashCode();
                if (this.Zone != null)
                    hashCode = hashCode * 59 + this.Zone.GetHashCode();
                if (this.PackageType != null)
                    hashCode = hashCode * 59 + this.PackageType.GetHashCode();
                if (this.DeliveryDays != null)
                    hashCode = hashCode * 59 + this.DeliveryDays.GetHashCode();
                if (this.GuaranteedService != null)
                    hashCode = hashCode * 59 + this.GuaranteedService.GetHashCode();
                if (this.EstimatedDeliveryDate != null)
                    hashCode = hashCode * 59 + this.EstimatedDeliveryDate.GetHashCode();
                if (this.CarrierDeliveryDays != null)
                    hashCode = hashCode * 59 + this.CarrierDeliveryDays.GetHashCode();
                if (this.ShipDate != null)
                    hashCode = hashCode * 59 + this.ShipDate.GetHashCode();
                if (this.NegotiatedRate != null)
                    hashCode = hashCode * 59 + this.NegotiatedRate.GetHashCode();
                if (this.ServiceType != null)
                    hashCode = hashCode * 59 + this.ServiceType.GetHashCode();
                if (this.ServiceCode != null)
                    hashCode = hashCode * 59 + this.ServiceCode.GetHashCode();
                if (this.Trackable != null)
                    hashCode = hashCode * 59 + this.Trackable.GetHashCode();
                if (this.CarrierCode != null)
                    hashCode = hashCode * 59 + this.CarrierCode.GetHashCode();
                if (this.CarrierNickname != null)
                    hashCode = hashCode * 59 + this.CarrierNickname.GetHashCode();
                if (this.CarrierFriendlyName != null)
                    hashCode = hashCode * 59 + this.CarrierFriendlyName.GetHashCode();
                if (this.ValidationStatus != null)
                    hashCode = hashCode * 59 + this.ValidationStatus.GetHashCode();
                if (this.WarningMessages != null)
                    hashCode = hashCode * 59 + this.WarningMessages.GetHashCode();
                if (this.ErrorMessages != null)
                    hashCode = hashCode * 59 + this.ErrorMessages.GetHashCode();
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
