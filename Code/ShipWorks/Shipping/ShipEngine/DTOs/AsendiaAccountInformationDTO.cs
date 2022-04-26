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
    /// AsendiaAccountInformationDTO
    /// </summary>
    [DataContract]
    public partial class AsendiaAccountInformationDTO : IEquatable<AsendiaAccountInformationDTO>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsendiaAccountInformationDTO" /> class.
        /// </summary>
        /// <param name="nickname">nickname.</param>
        /// <param name="accountNumber">accountNumber.</param>
        /// <param name="ftpUsername">ftpUsername.</param>
        /// <param name="ftpPassword">ftpPassword.</param>
        public AsendiaAccountInformationDTO(string nickname = default(string), string accountNumber = default(string), string ftpUsername = default(string), string ftpPassword = default(string))
        {
            this.Nickname = nickname;
            this.AccountNumber = accountNumber;
            this.FtpUsername = ftpUsername;
            this.FtpPassword = ftpPassword;
        }

        /// <summary>
        /// Gets or Sets Nickname
        /// </summary>
        [DataMember(Name = "nickname", EmitDefaultValue = false)]
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or Sets AccountNumber
        /// </summary>
        [DataMember(Name = "account_number", EmitDefaultValue = false)]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or Sets FtpUsername
        /// </summary>
        [DataMember(Name = "ftp_username", EmitDefaultValue = false)]
        public string FtpUsername { get; set; }

        /// <summary>
        /// Gets or Sets FtpPassword
        /// </summary>
        [DataMember(Name = "ftp_password", EmitDefaultValue = false)]
        public string FtpPassword { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AsendiaAccountInformationDTO {\n");
            sb.Append("  Nickname: ").Append(Nickname).Append("\n");
            sb.Append("  AccountNumber: ").Append(AccountNumber).Append("\n");
            sb.Append("  FtpUsername: ").Append(FtpUsername).Append("\n");
            sb.Append("  FtpPassword: ").Append(FtpPassword).Append("\n");
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
            return this.Equals(input as AsendiaAccountInformationDTO);
        }

        /// <summary>
        /// Returns true if AsendiaAccountInformationDTO instances are equal
        /// </summary>
        /// <param name="input">Instance of AsendiaAccountInformationDTO to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AsendiaAccountInformationDTO input)
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
                    this.AccountNumber == input.AccountNumber ||
                    (this.AccountNumber != null &&
                    this.AccountNumber.Equals(input.AccountNumber))
                ) &&
                (
                    this.FtpUsername == input.FtpUsername ||
                    (this.FtpUsername != null &&
                    this.FtpUsername.Equals(input.FtpUsername))
                ) &&
                (
                    this.FtpPassword == input.FtpPassword ||
                    (this.FtpPassword != null &&
                    this.FtpPassword.Equals(input.FtpPassword))
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
                if (this.AccountNumber != null)
                    hashCode = hashCode * 59 + this.AccountNumber.GetHashCode();
                if (this.FtpUsername != null)
                    hashCode = hashCode * 59 + this.FtpUsername.GetHashCode();
                if (this.FtpPassword != null)
                    hashCode = hashCode * 59 + this.FtpPassword.GetHashCode();
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
