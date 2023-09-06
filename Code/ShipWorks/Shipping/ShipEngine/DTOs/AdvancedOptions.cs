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
    /// AdvancedOptions
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class AdvancedOptions : IEquatable<AdvancedOptions>, IValidatableObject
    {
        /// <summary>
        /// Defines BillToParty
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public enum BillToPartyEnum
        {

            /// <summary>
            /// Enum Recipient for value: recipient
            /// </summary>
            [EnumMember(Value = "recipient")]
            Recipient = 1,

            /// <summary>
            /// Enum Thirdparty for value: third_party
            /// </summary>
            [EnumMember(Value = "third_party")]
            Thirdparty = 2
        }

        /// <summary>
        /// Gets or Sets BillToParty
        /// </summary>
        [DataMember(Name = "bill_to_party", EmitDefaultValue = false)]
        public BillToPartyEnum? BillToParty { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedOptions" /> class.
        /// </summary>
        /// <param name="billToAccount">billToAccount.</param>
        /// <param name="billToCountryCode">billToCountryCode.</param>
        /// <param name="billToParty">billToParty.</param>
        /// <param name="billToPostalCode">billToPostalCode.</param>
        /// <param name="containsAlcohol">containsAlcohol.</param>
        /// <param name="deliveredDutyPaid">deliveredDutyPaid.</param>
        /// <param name="nonMachinable">nonMachinable.</param>
        /// <param name="saturdayDelivery">saturdayDelivery.</param>
        /// <param name="dryIce">dryIce.</param>
        /// <param name="dryIceWeight">dryIceWeight.</param>
        /// <param name="useUpsGroundFreightPricing">useUpsGroundFreightPricing.</param>
        /// <param name="freightClass">freightClass.</param>
        /// <param name="customField1">customField1.</param>
        /// <param name="customField2">customField2.</param>
        /// <param name="customField3">customField3.</param>
        /// <param name="ancillaryEndorsement">ancillaryEndorsement.</param>
        public AdvancedOptions(string billToAccount = default(string), string billToCountryCode = default(string), BillToPartyEnum? billToParty = default(BillToPartyEnum?), string billToPostalCode = default(string), bool? containsAlcohol = default(bool?), bool? deliveredDutyPaid = default(bool?), bool? nonMachinable = default(bool?), bool? saturdayDelivery = default(bool?), bool? dryIce = default(bool?), Weight dryIceWeight = default(Weight), bool? useUpsGroundFreightPricing = default(bool?), string freightClass = default(string), 
            string customField1 = default(string), string customField2 = default(string), string customField3 = default(string),
            string ancillaryEndorsement = default(string), CollectOnDeliveryAdvancedOption collectOnDelivery = default(CollectOnDeliveryAdvancedOption))
        {
            this.BillToAccount = billToAccount;
            this.BillToCountryCode = billToCountryCode;
            this.BillToParty = billToParty;
            this.BillToPostalCode = billToPostalCode;
            this.ContainsAlcohol = containsAlcohol;
            this.DeliveredDutyPaid = deliveredDutyPaid;
            this.NonMachinable = nonMachinable;
            this.SaturdayDelivery = saturdayDelivery;
            this.DryIce = dryIce;
            this.DryIceWeight = dryIceWeight;
            this.UseUpsGroundFreightPricing = useUpsGroundFreightPricing;
            this.FreightClass = freightClass;
            this.CustomField1 = customField1;
            this.CustomField2 = customField2;
            this.CustomField3 = customField3;
            this.AncillaryEndorsement = ancillaryEndorsement;
            this.CollectOnDelivery = collectOnDelivery;
        }

        /// <summary>
        /// Gets or Sets BillToAccount
        /// </summary>
        [DataMember(Name = "bill_to_account", EmitDefaultValue = false)]
        public string BillToAccount { get; set; }

        /// <summary>
        /// Gets or Sets BillToCountryCode
        /// </summary>
        [DataMember(Name = "bill_to_country_code", EmitDefaultValue = false)]
        public string BillToCountryCode { get; set; }


        /// <summary>
        /// Gets or Sets BillToPostalCode
        /// </summary>
        [DataMember(Name = "bill_to_postal_code", EmitDefaultValue = false)]
        public string BillToPostalCode { get; set; }

        /// <summary>
        /// Gets or Sets ContainsAlcohol
        /// </summary>
        [DataMember(Name = "contains_alcohol", EmitDefaultValue = false)]
        public bool? ContainsAlcohol { get; set; }

        /// <summary>
        /// Gets or Sets DeliveredDutyPaid
        /// </summary>
        [DataMember(Name = "delivered_duty_paid", EmitDefaultValue = false)]
        public bool? DeliveredDutyPaid { get; set; }

        /// <summary>
        /// Gets or Sets NonMachinable
        /// </summary>
        [DataMember(Name = "non_machinable", EmitDefaultValue = false)]
        public bool? NonMachinable { get; set; }

        /// <summary>
        /// Gets or Sets SaturdayDelivery
        /// </summary>
        [DataMember(Name = "saturday_delivery", EmitDefaultValue = false)]
        public bool? SaturdayDelivery { get; set; }

        /// <summary>
        /// Gets or Sets DryIce
        /// </summary>
        [DataMember(Name = "dry_ice", EmitDefaultValue = false)]
        public bool? DryIce { get; set; }

        /// <summary>
        /// Gets or Sets DryIceWeight
        /// </summary>
        [DataMember(Name = "dry_ice_weight", EmitDefaultValue = false)]
        public Weight DryIceWeight { get; set; }

        /// <summary>
        /// Gets or Sets UseUpsGroundFreightPricing
        /// </summary>
        [DataMember(Name = "use_ups_ground_freight_pricing", EmitDefaultValue = false)]
        public bool? UseUpsGroundFreightPricing { get; set; }

        /// <summary>
        /// Gets or Sets FreightClass
        /// </summary>
        [DataMember(Name = "freight_class", EmitDefaultValue = false)]
        public string FreightClass { get; set; }

        /// <summary>
        /// Gets or Sets CustomField1
        /// </summary>
        [DataMember(Name = "custom_field1", EmitDefaultValue = false)]
        public string CustomField1 { get; set; }

        /// <summary>
        /// Gets or Sets CustomField2
        /// </summary>
        [DataMember(Name = "custom_field2", EmitDefaultValue = false)]
        public string CustomField2 { get; set; }

        /// <summary>
        /// Gets or Sets CustomField3
        /// </summary>
        [DataMember(Name = "custom_field3", EmitDefaultValue = false)]
        public string CustomField3 { get; set; }

        /// <summary>
        /// Gets or Sets AncillaryEndorsement
        /// </summary>
        [DataMember(Name = "ancillary_endorsements_option", EmitDefaultValue = false)]
        public string AncillaryEndorsement { get; set; }

        /// <summary>
        /// Gets or Sets ThirdPartyConsignee
        /// </summary>
        [DataMember(Name = "third-party-consignee", EmitDefaultValue = false)]
        public bool ThirdPartyConsignee { get; set; }

        /// <summary>
        /// Gets or Sets CollectOnDelivery
        /// </summary>
        [DataMember(Name = "collect_on_delivery", EmitDefaultValue = false)]
        public CollectOnDeliveryAdvancedOption CollectOnDelivery { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AdvancedOptions {\n");
            sb.Append("  BillToAccount: ").Append(BillToAccount).Append("\n");
            sb.Append("  BillToCountryCode: ").Append(BillToCountryCode).Append("\n");
            sb.Append("  BillToParty: ").Append(BillToParty).Append("\n");
            sb.Append("  BillToPostalCode: ").Append(BillToPostalCode).Append("\n");
            sb.Append("  ContainsAlcohol: ").Append(ContainsAlcohol).Append("\n");
            sb.Append("  DeliveredDutyPaid: ").Append(DeliveredDutyPaid).Append("\n");
            sb.Append("  NonMachinable: ").Append(NonMachinable).Append("\n");
            sb.Append("  SaturdayDelivery: ").Append(SaturdayDelivery).Append("\n");
            sb.Append("  DryIce: ").Append(DryIce).Append("\n");
            sb.Append("  DryIceWeight: ").Append(DryIceWeight).Append("\n");
            sb.Append("  UseUpsGroundFreightPricing: ").Append(UseUpsGroundFreightPricing).Append("\n");
            sb.Append("  FreightClass: ").Append(FreightClass).Append("\n");
            sb.Append("  CustomField1: ").Append(CustomField1).Append("\n");
            sb.Append("  CustomField2: ").Append(CustomField2).Append("\n");
            sb.Append("  CustomField3: ").Append(CustomField3).Append("\n");
            sb.Append("  AncillaryEndorsement: ").Append(AncillaryEndorsement).Append("\n");
            sb.Append("  ThirdPartyConsignee:  ").Append(ThirdPartyConsignee).Append("\n");
            sb.Append("  CollectOnDelivery: {\n").Append(CollectOnDelivery).Append("\n");
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
            return this.Equals(input as AdvancedOptions);
        }

        /// <summary>
        /// Returns true if AdvancedOptions instances are equal
        /// </summary>
        /// <param name="input">Instance of AdvancedOptions to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AdvancedOptions input)
        {
            if (input == null)
                return false;

            return
                (
                    this.BillToAccount == input.BillToAccount ||
                    (this.BillToAccount != null &&
                    this.BillToAccount.Equals(input.BillToAccount))
                ) &&
                (
                    this.BillToCountryCode == input.BillToCountryCode ||
                    (this.BillToCountryCode != null &&
                    this.BillToCountryCode.Equals(input.BillToCountryCode))
                ) &&
                (
                    this.BillToParty == input.BillToParty ||
                    (this.BillToParty != null &&
                    this.BillToParty.Equals(input.BillToParty))
                ) &&
                (
                    this.BillToPostalCode == input.BillToPostalCode ||
                    (this.BillToPostalCode != null &&
                    this.BillToPostalCode.Equals(input.BillToPostalCode))
                ) &&
                (
                    this.ContainsAlcohol == input.ContainsAlcohol ||
                    (this.ContainsAlcohol != null &&
                    this.ContainsAlcohol.Equals(input.ContainsAlcohol))
                ) &&
                (
                    this.DeliveredDutyPaid == input.DeliveredDutyPaid ||
                    (this.DeliveredDutyPaid != null &&
                    this.DeliveredDutyPaid.Equals(input.DeliveredDutyPaid))
                ) &&
                (
                    this.NonMachinable == input.NonMachinable ||
                    (this.NonMachinable != null &&
                    this.NonMachinable.Equals(input.NonMachinable))
                ) &&
                (
                    this.SaturdayDelivery == input.SaturdayDelivery ||
                    (this.SaturdayDelivery != null &&
                    this.SaturdayDelivery.Equals(input.SaturdayDelivery))
                ) &&
                (
                    this.DryIce == input.DryIce ||
                    (this.DryIce != null &&
                    this.DryIce.Equals(input.DryIce))
                ) &&
                (
                    this.DryIceWeight == input.DryIceWeight ||
                    (this.DryIceWeight != null &&
                    this.DryIceWeight.Equals(input.DryIceWeight))
                ) &&
                (
                    this.UseUpsGroundFreightPricing == input.UseUpsGroundFreightPricing ||
                    (this.UseUpsGroundFreightPricing != null &&
                    this.UseUpsGroundFreightPricing.Equals(input.UseUpsGroundFreightPricing))
                ) &&
                (
                    this.FreightClass == input.FreightClass ||
                    (this.FreightClass != null &&
                    this.FreightClass.Equals(input.FreightClass))
                ) &&
                (
                    this.CustomField1 == input.CustomField1 ||
                    (this.CustomField1 != null &&
                    this.CustomField1.Equals(input.CustomField1))
                ) &&
                (
                    this.CustomField2 == input.CustomField2 ||
                    (this.CustomField2 != null &&
                    this.CustomField2.Equals(input.CustomField2))
                ) &&
                (
                    this.CustomField3 == input.CustomField3 ||
                    (this.CustomField3 != null &&
                     this.CustomField3.Equals(input.CustomField3))
                ) &&
                (
                    this.AncillaryEndorsement == input.AncillaryEndorsement ||
                    (this.AncillaryEndorsement != null &&
                     this.AncillaryEndorsement.Equals(input.AncillaryEndorsement))
                ) &&
                (
                    this.ThirdPartyConsignee == input.ThirdPartyConsignee ||
                    (this.ThirdPartyConsignee != null &&
                     this.ThirdPartyConsignee.Equals(input.ThirdPartyConsignee))
                ) &&
                (
                    this.CollectOnDelivery == input.CollectOnDelivery ||
                    (this.CollectOnDelivery != null &&
                     this.CollectOnDelivery.Equals(input.CollectOnDelivery))
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
                if (this.BillToAccount != null)
                    hashCode = hashCode * 59 + this.BillToAccount.GetHashCode();
                if (this.BillToCountryCode != null)
                    hashCode = hashCode * 59 + this.BillToCountryCode.GetHashCode();
                if (this.BillToParty != null)
                    hashCode = hashCode * 59 + this.BillToParty.GetHashCode();
                if (this.BillToPostalCode != null)
                    hashCode = hashCode * 59 + this.BillToPostalCode.GetHashCode();
                if (this.ContainsAlcohol != null)
                    hashCode = hashCode * 59 + this.ContainsAlcohol.GetHashCode();
                if (this.DeliveredDutyPaid != null)
                    hashCode = hashCode * 59 + this.DeliveredDutyPaid.GetHashCode();
                if (this.NonMachinable != null)
                    hashCode = hashCode * 59 + this.NonMachinable.GetHashCode();
                if (this.SaturdayDelivery != null)
                    hashCode = hashCode * 59 + this.SaturdayDelivery.GetHashCode();
                if (this.DryIce != null)
                    hashCode = hashCode * 59 + this.DryIce.GetHashCode();
                if (this.DryIceWeight != null)
                    hashCode = hashCode * 59 + this.DryIceWeight.GetHashCode();
                if (this.UseUpsGroundFreightPricing != null)
                    hashCode = hashCode * 59 + this.UseUpsGroundFreightPricing.GetHashCode();
                if (this.FreightClass != null)
                    hashCode = hashCode * 59 + this.FreightClass.GetHashCode();
                if (this.CustomField1 != null)
                    hashCode = hashCode * 59 + this.CustomField1.GetHashCode();
                if (this.CustomField2 != null)
                    hashCode = hashCode * 59 + this.CustomField2.GetHashCode();
                if (this.CustomField3 != null)
                    hashCode = hashCode * 59 + this.CustomField3.GetHashCode();
                if (this.AncillaryEndorsement != null)
                    hashCode = hashCode * 59 + this.AncillaryEndorsement.GetHashCode();
                if(this.CollectOnDelivery != null)
                    hashCode = hashCode * 59 + this.CollectOnDelivery.GetHashCode();
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
