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
    /// StampsAccountInformationDTO
    /// </summary>
    [DataContract]
    public partial class StampsAccountInformationDTO : IEquatable<StampsAccountInformationDTO>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StampsAccountInformationDTO" /> class.
        /// </summary>
        /// <param name="nickname">nickname.</param>
        /// <param name="username">username.</param>
        /// <param name="password">password.</param>
        public StampsAccountInformationDTO(string nickname = default(string), string username = default(string), string password = default(string))
        {
            this.Nickname = nickname;
            this.Username = username;
            this.Password = password;
        }

        /// <summary>
        /// Gets or Sets Nickname
        /// </summary>
        [DataMember(Name = "nickname", EmitDefaultValue = false)]
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or Sets Username
        /// </summary>
        [DataMember(Name = "username", EmitDefaultValue = false)]
        public string Username { get; set; }

        /// <summary>
        /// Gets or Sets Password
        /// </summary>
        [DataMember(Name = "password", EmitDefaultValue = false)]
        public string Password { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class StampsAccountInformationDTO {\n");
            sb.Append("  Nickname: ").Append(Nickname).Append("\n");
            sb.Append("  Username: ").Append(Username).Append("\n");
            sb.Append("  Password: ").Append(Password).Append("\n");
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
            return this.Equals(input as StampsAccountInformationDTO);
        }

        /// <summary>
        /// Returns true if StampsAccountInformationDTO instances are equal
        /// </summary>
        /// <param name="input">Instance of StampsAccountInformationDTO to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(StampsAccountInformationDTO input)
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
                    this.Username == input.Username ||
                    (this.Username != null &&
                    this.Username.Equals(input.Username))
                ) &&
                (
                    this.Password == input.Password ||
                    (this.Password != null &&
                    this.Password.Equals(input.Password))
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
                if (this.Username != null)
                    hashCode = hashCode * 59 + this.Username.GetHashCode();
                if (this.Password != null)
                    hashCode = hashCode * 59 + this.Password.GetHashCode();
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
