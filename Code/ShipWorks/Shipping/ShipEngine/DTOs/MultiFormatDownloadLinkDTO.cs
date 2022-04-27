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
    /// MultiFormatDownloadLinkDTO
    /// </summary>
    [DataContract]
    public partial class MultiFormatDownloadLinkDTO : IEquatable<MultiFormatDownloadLinkDTO>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiFormatDownloadLinkDTO" /> class.
        /// </summary>
        /// <param name="pdf">pdf.</param>
        /// <param name="png">png.</param>
        /// <param name="zpl">zpl.</param>
        /// <param name="href">href.</param>
        /// <param name="type">type.</param>
        public MultiFormatDownloadLinkDTO(string pdf = default(string), string png = default(string), string zpl = default(string), string href = default(string), string type = default(string))
        {
            this.Pdf = pdf;
            this.Png = png;
            this.Zpl = zpl;
            this.Href = href;
            this.Type = type;
        }

        /// <summary>
        /// Gets or Sets Pdf
        /// </summary>
        [DataMember(Name = "pdf", EmitDefaultValue = false)]
        public string Pdf { get; set; }

        /// <summary>
        /// Gets or Sets Png
        /// </summary>
        [DataMember(Name = "png", EmitDefaultValue = false)]
        public string Png { get; set; }

        /// <summary>
        /// Gets or Sets Zpl
        /// </summary>
        [DataMember(Name = "zpl", EmitDefaultValue = false)]
        public string Zpl { get; set; }

        /// <summary>
        /// Gets or Sets Href
        /// </summary>
        [DataMember(Name = "href", EmitDefaultValue = false)]
        public string Href { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class MultiFormatDownloadLinkDTO {\n");
            sb.Append("  Pdf: ").Append(Pdf).Append("\n");
            sb.Append("  Png: ").Append(Png).Append("\n");
            sb.Append("  Zpl: ").Append(Zpl).Append("\n");
            sb.Append("  Href: ").Append(Href).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
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
            return this.Equals(input as MultiFormatDownloadLinkDTO);
        }

        /// <summary>
        /// Returns true if MultiFormatDownloadLinkDTO instances are equal
        /// </summary>
        /// <param name="input">Instance of MultiFormatDownloadLinkDTO to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(MultiFormatDownloadLinkDTO input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Pdf == input.Pdf ||
                    (this.Pdf != null &&
                    this.Pdf.Equals(input.Pdf))
                ) &&
                (
                    this.Png == input.Png ||
                    (this.Png != null &&
                    this.Png.Equals(input.Png))
                ) &&
                (
                    this.Zpl == input.Zpl ||
                    (this.Zpl != null &&
                    this.Zpl.Equals(input.Zpl))
                ) &&
                (
                    this.Href == input.Href ||
                    (this.Href != null &&
                    this.Href.Equals(input.Href))
                ) &&
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
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
                if (this.Pdf != null)
                    hashCode = hashCode * 59 + this.Pdf.GetHashCode();
                if (this.Png != null)
                    hashCode = hashCode * 59 + this.Png.GetHashCode();
                if (this.Zpl != null)
                    hashCode = hashCode * 59 + this.Zpl.GetHashCode();
                if (this.Href != null)
                    hashCode = hashCode * 59 + this.Href.GetHashCode();
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
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
