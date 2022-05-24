using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// Carrier
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class Carrier : IEquatable<Carrier>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Carrier" /> class.
        /// </summary>
        /// <param name="carrierId">carrierId.</param>
        /// <param name="carrierCode">carrierCode.</param>
        /// <param name="accountNumber">accountNumber.</param>
        /// <param name="requiresFundedAmount">requiresFundedAmount.</param>
        /// <param name="balance">balance.</param>
        /// <param name="nickname">nickname.</param>
        /// <param name="friendlyName">friendlyName.</param>
        /// <param name="primary">primary.</param>
        /// <param name="hasMultiPackageSupportingServices">hasMultiPackageSupportingServices.</param>
        /// <param name="supportsLabelMessages">supportsLabelMessages.</param>
        /// <param name="services">services.</param>
        /// <param name="packages">packages.</param>
        /// <param name="options">options.</param>
        public Carrier(string carrierId = default(string), string carrierCode = default(string), string accountNumber = default(string), bool? requiresFundedAmount = default(bool?), double? balance = default(double?), string nickname = default(string), string friendlyName = default(string), bool? primary = default(bool?), bool? hasMultiPackageSupportingServices = default(bool?), bool? supportsLabelMessages = default(bool?), List<Service> services = default(List<Service>), List<Package> packages = default(List<Package>), List<CarrierAdvancedOption> options = default(List<CarrierAdvancedOption>))
        {
            this.CarrierId = carrierId;
            this.CarrierCode = carrierCode;
            this.AccountNumber = accountNumber;
            this.RequiresFundedAmount = requiresFundedAmount;
            this.Balance = balance;
            this.Nickname = nickname;
            this.FriendlyName = friendlyName;
            this.Primary = primary;
            this.HasMultiPackageSupportingServices = hasMultiPackageSupportingServices;
            this.SupportsLabelMessages = supportsLabelMessages;
            this.Services = services;
            this.Packages = packages;
            this.Options = options;
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
        /// Gets or Sets AccountNumber
        /// </summary>
        [DataMember(Name = "account_number", EmitDefaultValue = false)]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or Sets RequiresFundedAmount
        /// </summary>
        [DataMember(Name = "requires_funded_amount", EmitDefaultValue = false)]
        public bool? RequiresFundedAmount { get; set; }

        /// <summary>
        /// Gets or Sets Balance
        /// </summary>
        [DataMember(Name = "balance", EmitDefaultValue = false)]
        public double? Balance { get; set; }

        /// <summary>
        /// Gets or Sets Nickname
        /// </summary>
        [DataMember(Name = "nickname", EmitDefaultValue = false)]
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or Sets FriendlyName
        /// </summary>
        [DataMember(Name = "friendly_name", EmitDefaultValue = false)]
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or Sets Primary
        /// </summary>
        [DataMember(Name = "primary", EmitDefaultValue = false)]
        public bool? Primary { get; set; }

        /// <summary>
        /// Gets or Sets HasMultiPackageSupportingServices
        /// </summary>
        [DataMember(Name = "has_multi_package_supporting_services", EmitDefaultValue = false)]
        public bool? HasMultiPackageSupportingServices { get; set; }

        /// <summary>
        /// Gets or Sets SupportsLabelMessages
        /// </summary>
        [DataMember(Name = "supports_label_messages", EmitDefaultValue = false)]
        public bool? SupportsLabelMessages { get; set; }

        /// <summary>
        /// Gets or Sets Services
        /// </summary>
        [DataMember(Name = "services", EmitDefaultValue = false)]
        public List<Service> Services { get; set; }

        /// <summary>
        /// Gets or Sets Packages
        /// </summary>
        [DataMember(Name = "packages", EmitDefaultValue = false)]
        public List<Package> Packages { get; set; }

        /// <summary>
        /// Gets or Sets Options
        /// </summary>
        [DataMember(Name = "options", EmitDefaultValue = false)]
        public List<CarrierAdvancedOption> Options { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Carrier {\n");
            sb.Append("  CarrierId: ").Append(CarrierId).Append("\n");
            sb.Append("  CarrierCode: ").Append(CarrierCode).Append("\n");
            sb.Append("  AccountNumber: ").Append(AccountNumber).Append("\n");
            sb.Append("  RequiresFundedAmount: ").Append(RequiresFundedAmount).Append("\n");
            sb.Append("  Balance: ").Append(Balance).Append("\n");
            sb.Append("  Nickname: ").Append(Nickname).Append("\n");
            sb.Append("  FriendlyName: ").Append(FriendlyName).Append("\n");
            sb.Append("  Primary: ").Append(Primary).Append("\n");
            sb.Append("  HasMultiPackageSupportingServices: ").Append(HasMultiPackageSupportingServices).Append("\n");
            sb.Append("  SupportsLabelMessages: ").Append(SupportsLabelMessages).Append("\n");
            sb.Append("  Services: ").Append(Services).Append("\n");
            sb.Append("  Packages: ").Append(Packages).Append("\n");
            sb.Append("  Options: ").Append(Options).Append("\n");
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
            return this.Equals(input as Carrier);
        }

        /// <summary>
        /// Returns true if Carrier instances are equal
        /// </summary>
        /// <param name="input">Instance of Carrier to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Carrier input)
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
                    this.AccountNumber == input.AccountNumber ||
                    (this.AccountNumber != null &&
                    this.AccountNumber.Equals(input.AccountNumber))
                ) &&
                (
                    this.RequiresFundedAmount == input.RequiresFundedAmount ||
                    (this.RequiresFundedAmount != null &&
                    this.RequiresFundedAmount.Equals(input.RequiresFundedAmount))
                ) &&
                (
                    this.Balance == input.Balance ||
                    (this.Balance != null &&
                    this.Balance.Equals(input.Balance))
                ) &&
                (
                    this.Nickname == input.Nickname ||
                    (this.Nickname != null &&
                    this.Nickname.Equals(input.Nickname))
                ) &&
                (
                    this.FriendlyName == input.FriendlyName ||
                    (this.FriendlyName != null &&
                    this.FriendlyName.Equals(input.FriendlyName))
                ) &&
                (
                    this.Primary == input.Primary ||
                    (this.Primary != null &&
                    this.Primary.Equals(input.Primary))
                ) &&
                (
                    this.HasMultiPackageSupportingServices == input.HasMultiPackageSupportingServices ||
                    (this.HasMultiPackageSupportingServices != null &&
                    this.HasMultiPackageSupportingServices.Equals(input.HasMultiPackageSupportingServices))
                ) &&
                (
                    this.SupportsLabelMessages == input.SupportsLabelMessages ||
                    (this.SupportsLabelMessages != null &&
                    this.SupportsLabelMessages.Equals(input.SupportsLabelMessages))
                ) &&
                (
                    this.Services == input.Services ||
                    this.Services != null &&
                    this.Services.SequenceEqual(input.Services)
                ) &&
                (
                    this.Packages == input.Packages ||
                    this.Packages != null &&
                    this.Packages.SequenceEqual(input.Packages)
                ) &&
                (
                    this.Options == input.Options ||
                    this.Options != null &&
                    this.Options.SequenceEqual(input.Options)
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
                if (this.AccountNumber != null)
                    hashCode = hashCode * 59 + this.AccountNumber.GetHashCode();
                if (this.RequiresFundedAmount != null)
                    hashCode = hashCode * 59 + this.RequiresFundedAmount.GetHashCode();
                if (this.Balance != null)
                    hashCode = hashCode * 59 + this.Balance.GetHashCode();
                if (this.Nickname != null)
                    hashCode = hashCode * 59 + this.Nickname.GetHashCode();
                if (this.FriendlyName != null)
                    hashCode = hashCode * 59 + this.FriendlyName.GetHashCode();
                if (this.Primary != null)
                    hashCode = hashCode * 59 + this.Primary.GetHashCode();
                if (this.HasMultiPackageSupportingServices != null)
                    hashCode = hashCode * 59 + this.HasMultiPackageSupportingServices.GetHashCode();
                if (this.SupportsLabelMessages != null)
                    hashCode = hashCode * 59 + this.SupportsLabelMessages.GetHashCode();
                if (this.Services != null)
                    hashCode = hashCode * 59 + this.Services.GetHashCode();
                if (this.Packages != null)
                    hashCode = hashCode * 59 + this.Packages.GetHashCode();
                if (this.Options != null)
                    hashCode = hashCode * 59 + this.Options.GetHashCode();
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
