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
    /// AmazonShippingUsAccountSettingsDTO
    /// </summary>
    [DataContract]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public partial class AmazonShippingUsAccountSettingsDTO : IEquatable<AmazonShippingUsAccountSettingsDTO>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonShippingUsAccountSettingsDTO" /> class.
        /// </summary>
        /// <param name="nickname">nickname.</param>
        /// <param name="isPrimaryAccount">isPrimaryAccount.</param>
        /// <param name="email">email.</param>
        /// <param name="merchantSellerId">merchantSellerId.</param>
        /// <param name="mwsAuthToken">mwsAuthToken.</param>
        public AmazonShippingUsAccountSettingsDTO(string nickname = default(string), bool? isPrimaryAccount = default(bool?), string email = default(string), string merchantSellerId = default(string), string mwsAuthToken = default(string))
        {
            this.Nickname = nickname;
            this.IsPrimaryAccount = isPrimaryAccount;
            this.Email = email;
            this.MerchantSellerId = merchantSellerId;
            this.MwsAuthToken = mwsAuthToken;
        }

        /// <summary>
        /// Gets or Sets Nickname
        /// </summary>
        [DataMember(Name = "nickname", EmitDefaultValue = false)]
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or Sets IsPrimaryAccount
        /// </summary>
        [DataMember(Name = "is_primary_account", EmitDefaultValue = false)]
        public bool? IsPrimaryAccount { get; set; }

        /// <summary>
        /// Gets or Sets Email
        /// </summary>
        [DataMember(Name = "email", EmitDefaultValue = false)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets MerchantSellerId
        /// </summary>
        [DataMember(Name = "merchant_seller_id", EmitDefaultValue = false)]
        public string MerchantSellerId { get; set; }

        /// <summary>
        /// Gets or Sets MwsAuthToken
        /// </summary>
        [DataMember(Name = "mws_auth_token", EmitDefaultValue = false)]
        public string MwsAuthToken { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AmazonShippingUsAccountSettingsDTO {\n");
            sb.Append("  Nickname: ").Append(Nickname).Append("\n");
            sb.Append("  IsPrimaryAccount: ").Append(IsPrimaryAccount).Append("\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  MerchantSellerId: ").Append(MerchantSellerId).Append("\n");
            sb.Append("  MwsAuthToken: ").Append(MwsAuthToken).Append("\n");
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
            return this.Equals(input as AmazonShippingUsAccountSettingsDTO);
        }

        /// <summary>
        /// Returns true if AmazonShippingUsAccountSettingsDTO instances are equal
        /// </summary>
        /// <param name="input">Instance of AmazonShippingUsAccountSettingsDTO to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AmazonShippingUsAccountSettingsDTO input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Nickname == input.Nickname ||
                    (this.Nickname != null &&
                    this.Nickname.Equals(input.Nickname))
                ) &&
                (
                    this.IsPrimaryAccount == input.IsPrimaryAccount ||
                    (this.IsPrimaryAccount != null &&
                    this.IsPrimaryAccount.Equals(input.IsPrimaryAccount))
                ) &&
                (
                    this.Email == input.Email ||
                    (this.Email != null &&
                    this.Email.Equals(input.Email))
                ) &&
                (
                    this.MerchantSellerId == input.MerchantSellerId ||
                    (this.MerchantSellerId != null &&
                    this.MerchantSellerId.Equals(input.MerchantSellerId))
                ) &&
                (
                    this.MwsAuthToken == input.MwsAuthToken ||
                    (this.MwsAuthToken != null &&
                    this.MwsAuthToken.Equals(input.MwsAuthToken))
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
                if (this.Nickname != null)
                    hashCode = hashCode * 59 + this.Nickname.GetHashCode();
                if (this.IsPrimaryAccount != null)
                    hashCode = hashCode * 59 + this.IsPrimaryAccount.GetHashCode();
                if (this.Email != null)
                    hashCode = hashCode * 59 + this.Email.GetHashCode();
                if (this.MerchantSellerId != null)
                    hashCode = hashCode * 59 + this.MerchantSellerId.GetHashCode();
                if (this.MwsAuthToken != null)
                    hashCode = hashCode * 59 + this.MwsAuthToken.GetHashCode();
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
