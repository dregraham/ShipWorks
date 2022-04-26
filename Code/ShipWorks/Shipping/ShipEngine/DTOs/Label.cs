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
    /// Label
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class Label : IEquatable<Label>, IValidatableObject
    {
        /// <summary>
        /// Defines Status
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum StatusEnum
        {

            /// <summary>
            /// Enum Processing for value: processing
            /// </summary>
            [EnumMember(Value = "processing")]
            Processing = 1,

            /// <summary>
            /// Enum Completed for value: completed
            /// </summary>
            [EnumMember(Value = "completed")]
            Completed = 2,

            /// <summary>
            /// Enum Error for value: error
            /// </summary>
            [EnumMember(Value = "error")]
            Error = 3,

            /// <summary>
            /// Enum Voided for value: voided
            /// </summary>
            [EnumMember(Value = "voided")]
            Voided = 4
        }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public StatusEnum? Status { get; set; }

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
        /// Defines TrackingStatus
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum TrackingStatusEnum
        {

            /// <summary>
            /// Enum Unknown for value: unknown
            /// </summary>
            [EnumMember(Value = "unknown")]
            Unknown = 1,

            /// <summary>
            /// Enum Intransit for value: in_transit
            /// </summary>
            [EnumMember(Value = "in_transit")]
            Intransit = 2,

            /// <summary>
            /// Enum Error for value: error
            /// </summary>
            [EnumMember(Value = "error")]
            Error = 3,

            /// <summary>
            /// Enum Delivered for value: delivered
            /// </summary>
            [EnumMember(Value = "delivered")]
            Delivered = 4
        }

        /// <summary>
        /// Gets or Sets TrackingStatus
        /// </summary>
        [DataMember(Name = "tracking_status", EmitDefaultValue = false)]
        public TrackingStatusEnum? TrackingStatus { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label" /> class.
        /// </summary>
        /// <param name="labelId">labelId.</param>
        /// <param name="status">status.</param>
        /// <param name="shipmentId">shipmentId.</param>
        /// <param name="shipDate">shipDate.</param>
        /// <param name="createdAt">createdAt.</param>
        /// <param name="shipmentCost">shipmentCost.</param>
        /// <param name="insuranceCost">insuranceCost.</param>
        /// <param name="trackingNumber">trackingNumber.</param>
        /// <param name="isReturnLabel">isReturnLabel.</param>
        /// <param name="rmaNumber">rmaNumber.</param>
        /// <param name="isInternational">isInternational.</param>
        /// <param name="batchId">batchId.</param>
        /// <param name="carrierId">carrierId.</param>
        /// <param name="serviceCode">serviceCode.</param>
        /// <param name="packageCode">packageCode.</param>
        /// <param name="voided">voided.</param>
        /// <param name="voidedAt">voidedAt.</param>
        /// <param name="labelFormat">labelFormat.</param>
        /// <param name="labelLayout">labelLayout.</param>
        /// <param name="trackable">trackable.</param>
        /// <param name="carrierCode">carrierCode.</param>
        /// <param name="trackingStatus">trackingStatus.</param>
        /// <param name="labelDownload">labelDownload.</param>
        /// <param name="formDownload">formDownload.</param>
        /// <param name="insuranceClaim">insuranceClaim.</param>
        /// <param name="packages">packages.</param>
        public Label(string labelId = default(string), StatusEnum? status = default(StatusEnum?), string shipmentId = default(string), DateTime? shipDate = default(DateTime?), DateTime? createdAt = default(DateTime?), MoneyDTO shipmentCost = default(MoneyDTO), MoneyDTO insuranceCost = default(MoneyDTO), string trackingNumber = default(string), bool? isReturnLabel = default(bool?), string rmaNumber = default(string), bool? isInternational = default(bool?), string batchId = default(string), string carrierId = default(string), string serviceCode = default(string), string packageCode = default(string), bool? voided = default(bool?), DateTime? voidedAt = default(DateTime?), LabelFormatEnum? labelFormat = default(LabelFormatEnum?), string labelLayout = default(string), bool? trackable = default(bool?), string carrierCode = default(string), TrackingStatusEnum? trackingStatus = default(TrackingStatusEnum?), MultiFormatDownloadLinkDTO labelDownload = default(MultiFormatDownloadLinkDTO), LinkDTO formDownload = default(LinkDTO), LinkDTO insuranceClaim = default(LinkDTO), List<LabelPackage> packages = default(List<LabelPackage>))
        {
            LabelId = labelId;
            Status = status;
            ShipmentId = shipmentId;
            ShipDate = shipDate;
            CreatedAt = createdAt;
            ShipmentCost = shipmentCost;
            InsuranceCost = insuranceCost;
            TrackingNumber = trackingNumber;
            IsReturnLabel = isReturnLabel;
            RmaNumber = rmaNumber;
            IsInternational = isInternational;
            BatchId = batchId;
            CarrierId = carrierId;
            ServiceCode = serviceCode;
            PackageCode = packageCode;
            Voided = voided;
            VoidedAt = voidedAt;
            LabelFormat = labelFormat;
            LabelLayout = labelLayout;
            Trackable = trackable;
            CarrierCode = carrierCode;
            TrackingStatus = trackingStatus;
            LabelDownload = labelDownload;
            FormDownload = formDownload;
            InsuranceClaim = insuranceClaim;
            Packages = packages;
        }

        /// <summary>
        /// Gets or Sets LabelId
        /// </summary>
        [DataMember(Name = "label_id", EmitDefaultValue = false)]
        public string LabelId { get; set; }


        /// <summary>
        /// Gets or Sets ShipmentId
        /// </summary>
        [DataMember(Name = "shipment_id", EmitDefaultValue = false)]
        public string ShipmentId { get; set; }

        /// <summary>
        /// Gets or Sets ShipDate
        /// </summary>
        [DataMember(Name = "ship_date", EmitDefaultValue = false)]
        public DateTime? ShipDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>
        [DataMember(Name = "created_at", EmitDefaultValue = false)]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or Sets ShipmentCost
        /// </summary>
        [DataMember(Name = "shipment_cost", EmitDefaultValue = false)]
        public MoneyDTO ShipmentCost { get; set; }

        /// <summary>
        /// Gets or Sets InsuranceCost
        /// </summary>
        [DataMember(Name = "insurance_cost", EmitDefaultValue = false)]
        public MoneyDTO InsuranceCost { get; set; }

        /// <summary>
        /// Gets or Sets TrackingNumber
        /// </summary>
        [DataMember(Name = "tracking_number", EmitDefaultValue = false)]
        public string TrackingNumber { get; set; }

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
        /// Gets or Sets IsInternational
        /// </summary>
        [DataMember(Name = "is_international", EmitDefaultValue = false)]
        public bool? IsInternational { get; set; }

        /// <summary>
        /// Gets or Sets BatchId
        /// </summary>
        [DataMember(Name = "batch_id", EmitDefaultValue = false)]
        public string BatchId { get; set; }

        /// <summary>
        /// Gets or Sets CarrierId
        /// </summary>
        [DataMember(Name = "carrier_id", EmitDefaultValue = false)]
        public string CarrierId { get; set; }

        /// <summary>
        /// Gets or Sets ServiceCode
        /// </summary>
        [DataMember(Name = "service_code", EmitDefaultValue = false)]
        public string ServiceCode { get; set; }

        /// <summary>
        /// Gets or Sets PackageCode
        /// </summary>
        [DataMember(Name = "package_code", EmitDefaultValue = false)]
        public string PackageCode { get; set; }

        /// <summary>
        /// Gets or Sets Voided
        /// </summary>
        [DataMember(Name = "voided", EmitDefaultValue = false)]
        public bool? Voided { get; set; }

        /// <summary>
        /// Gets or Sets VoidedAt
        /// </summary>
        [DataMember(Name = "voided_at", EmitDefaultValue = false)]
        public DateTime? VoidedAt { get; set; }


        /// <summary>
        /// Gets or Sets LabelLayout
        /// </summary>
        [DataMember(Name = "label_layout", EmitDefaultValue = false)]
        public string LabelLayout { get; set; }

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
        /// Gets or Sets LabelDownload
        /// </summary>
        [DataMember(Name = "label_download", EmitDefaultValue = false)]
        public MultiFormatDownloadLinkDTO LabelDownload { get; set; }

        /// <summary>
        /// Gets or Sets FormDownload
        /// </summary>
        [DataMember(Name = "form_download", EmitDefaultValue = false)]
        public LinkDTO FormDownload { get; set; }

        /// <summary>
        /// Gets or Sets InsuranceClaim
        /// </summary>
        [DataMember(Name = "insurance_claim", EmitDefaultValue = false)]
        public LinkDTO InsuranceClaim { get; set; }

        /// <summary>
        /// Gets or Sets Packages
        /// </summary>
        [DataMember(Name = "packages", EmitDefaultValue = false)]
        public List<LabelPackage> Packages { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Label {\n");
            sb.Append("  LabelId: ").Append(LabelId).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  ShipmentId: ").Append(ShipmentId).Append("\n");
            sb.Append("  ShipDate: ").Append(ShipDate).Append("\n");
            sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
            sb.Append("  ShipmentCost: ").Append(ShipmentCost).Append("\n");
            sb.Append("  InsuranceCost: ").Append(InsuranceCost).Append("\n");
            sb.Append("  TrackingNumber: ").Append(TrackingNumber).Append("\n");
            sb.Append("  IsReturnLabel: ").Append(IsReturnLabel).Append("\n");
            sb.Append("  RmaNumber: ").Append(RmaNumber).Append("\n");
            sb.Append("  IsInternational: ").Append(IsInternational).Append("\n");
            sb.Append("  BatchId: ").Append(BatchId).Append("\n");
            sb.Append("  CarrierId: ").Append(CarrierId).Append("\n");
            sb.Append("  ServiceCode: ").Append(ServiceCode).Append("\n");
            sb.Append("  PackageCode: ").Append(PackageCode).Append("\n");
            sb.Append("  Voided: ").Append(Voided).Append("\n");
            sb.Append("  VoidedAt: ").Append(VoidedAt).Append("\n");
            sb.Append("  LabelFormat: ").Append(LabelFormat).Append("\n");
            sb.Append("  LabelLayout: ").Append(LabelLayout).Append("\n");
            sb.Append("  Trackable: ").Append(Trackable).Append("\n");
            sb.Append("  CarrierCode: ").Append(CarrierCode).Append("\n");
            sb.Append("  TrackingStatus: ").Append(TrackingStatus).Append("\n");
            sb.Append("  LabelDownload: ").Append(LabelDownload).Append("\n");
            sb.Append("  FormDownload: ").Append(FormDownload).Append("\n");
            sb.Append("  InsuranceClaim: ").Append(InsuranceClaim).Append("\n");
            sb.Append("  Packages: ").Append(Packages).Append("\n");
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
            return Equals(input as Label);
        }

        /// <summary>
        /// Returns true if Label instances are equal
        /// </summary>
        /// <param name="input">Instance of Label to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Label input)
        {
            if (input == null)
                return false;

            return
                (
                    LabelId == input.LabelId ||
                    (LabelId != null &&
                    LabelId.Equals(input.LabelId))
                ) &&
                (
                    Status == input.Status ||
                    (Status != null &&
                    Status.Equals(input.Status))
                ) &&
                (
                    ShipmentId == input.ShipmentId ||
                    (ShipmentId != null &&
                    ShipmentId.Equals(input.ShipmentId))
                ) &&
                (
                    ShipDate == input.ShipDate ||
                    (ShipDate != null &&
                    ShipDate.Equals(input.ShipDate))
                ) &&
                (
                    CreatedAt == input.CreatedAt ||
                    (CreatedAt != null &&
                    CreatedAt.Equals(input.CreatedAt))
                ) &&
                (
                    ShipmentCost == input.ShipmentCost ||
                    (ShipmentCost != null &&
                    ShipmentCost.Equals(input.ShipmentCost))
                ) &&
                (
                    InsuranceCost == input.InsuranceCost ||
                    (InsuranceCost != null &&
                    InsuranceCost.Equals(input.InsuranceCost))
                ) &&
                (
                    TrackingNumber == input.TrackingNumber ||
                    (TrackingNumber != null &&
                    TrackingNumber.Equals(input.TrackingNumber))
                ) &&
                (
                    IsReturnLabel == input.IsReturnLabel ||
                    (IsReturnLabel != null &&
                    IsReturnLabel.Equals(input.IsReturnLabel))
                ) &&
                (
                    RmaNumber == input.RmaNumber ||
                    (RmaNumber != null &&
                    RmaNumber.Equals(input.RmaNumber))
                ) &&
                (
                    IsInternational == input.IsInternational ||
                    (IsInternational != null &&
                    IsInternational.Equals(input.IsInternational))
                ) &&
                (
                    BatchId == input.BatchId ||
                    (BatchId != null &&
                    BatchId.Equals(input.BatchId))
                ) &&
                (
                    CarrierId == input.CarrierId ||
                    (CarrierId != null &&
                    CarrierId.Equals(input.CarrierId))
                ) &&
                (
                    ServiceCode == input.ServiceCode ||
                    (ServiceCode != null &&
                    ServiceCode.Equals(input.ServiceCode))
                ) &&
                (
                    PackageCode == input.PackageCode ||
                    (PackageCode != null &&
                    PackageCode.Equals(input.PackageCode))
                ) &&
                (
                    Voided == input.Voided ||
                    (Voided != null &&
                    Voided.Equals(input.Voided))
                ) &&
                (
                    VoidedAt == input.VoidedAt ||
                    (VoidedAt != null &&
                    VoidedAt.Equals(input.VoidedAt))
                ) &&
                (
                    LabelFormat == input.LabelFormat ||
                    (LabelFormat != null &&
                    LabelFormat.Equals(input.LabelFormat))
                ) &&
                (
                    LabelLayout == input.LabelLayout ||
                    (LabelLayout != null &&
                    LabelLayout.Equals(input.LabelLayout))
                ) &&
                (
                    Trackable == input.Trackable ||
                    (Trackable != null &&
                    Trackable.Equals(input.Trackable))
                ) &&
                (
                    CarrierCode == input.CarrierCode ||
                    (CarrierCode != null &&
                    CarrierCode.Equals(input.CarrierCode))
                ) &&
                (
                    TrackingStatus == input.TrackingStatus ||
                    (TrackingStatus != null &&
                    TrackingStatus.Equals(input.TrackingStatus))
                ) &&
                (
                    LabelDownload == input.LabelDownload ||
                    (LabelDownload != null &&
                    LabelDownload.Equals(input.LabelDownload))
                ) &&
                (
                    FormDownload == input.FormDownload ||
                    (FormDownload != null &&
                    FormDownload.Equals(input.FormDownload))
                ) &&
                (
                    InsuranceClaim == input.InsuranceClaim ||
                    (InsuranceClaim != null &&
                    InsuranceClaim.Equals(input.InsuranceClaim))
                ) &&
                (
                    Packages == input.Packages ||
                    Packages != null &&
                    Packages.SequenceEqual(input.Packages)
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
                if (LabelId != null)
                    hashCode = hashCode * 59 + LabelId.GetHashCode();
                if (Status != null)
                    hashCode = hashCode * 59 + Status.GetHashCode();
                if (ShipmentId != null)
                    hashCode = hashCode * 59 + ShipmentId.GetHashCode();
                if (ShipDate != null)
                    hashCode = hashCode * 59 + ShipDate.GetHashCode();
                if (CreatedAt != null)
                    hashCode = hashCode * 59 + CreatedAt.GetHashCode();
                if (ShipmentCost != null)
                    hashCode = hashCode * 59 + ShipmentCost.GetHashCode();
                if (InsuranceCost != null)
                    hashCode = hashCode * 59 + InsuranceCost.GetHashCode();
                if (TrackingNumber != null)
                    hashCode = hashCode * 59 + TrackingNumber.GetHashCode();
                if (IsReturnLabel != null)
                    hashCode = hashCode * 59 + IsReturnLabel.GetHashCode();
                if (RmaNumber != null)
                    hashCode = hashCode * 59 + RmaNumber.GetHashCode();
                if (IsInternational != null)
                    hashCode = hashCode * 59 + IsInternational.GetHashCode();
                if (BatchId != null)
                    hashCode = hashCode * 59 + BatchId.GetHashCode();
                if (CarrierId != null)
                    hashCode = hashCode * 59 + CarrierId.GetHashCode();
                if (ServiceCode != null)
                    hashCode = hashCode * 59 + ServiceCode.GetHashCode();
                if (PackageCode != null)
                    hashCode = hashCode * 59 + PackageCode.GetHashCode();
                if (Voided != null)
                    hashCode = hashCode * 59 + Voided.GetHashCode();
                if (VoidedAt != null)
                    hashCode = hashCode * 59 + VoidedAt.GetHashCode();
                if (LabelFormat != null)
                    hashCode = hashCode * 59 + LabelFormat.GetHashCode();
                if (LabelLayout != null)
                    hashCode = hashCode * 59 + LabelLayout.GetHashCode();
                if (Trackable != null)
                    hashCode = hashCode * 59 + Trackable.GetHashCode();
                if (CarrierCode != null)
                    hashCode = hashCode * 59 + CarrierCode.GetHashCode();
                if (TrackingStatus != null)
                    hashCode = hashCode * 59 + TrackingStatus.GetHashCode();
                if (LabelDownload != null)
                    hashCode = hashCode * 59 + LabelDownload.GetHashCode();
                if (FormDownload != null)
                    hashCode = hashCode * 59 + FormDownload.GetHashCode();
                if (InsuranceClaim != null)
                    hashCode = hashCode * 59 + InsuranceClaim.GetHashCode();
                if (Packages != null)
                    hashCode = hashCode * 59 + Packages.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
