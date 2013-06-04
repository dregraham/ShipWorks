using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.PayPal;
using System.Data.Common;

namespace ShipWorks.Data
{
    /// <summary>
    /// Wraps an entity to expose its paypal credentials
    /// </summary>
    public class PayPalAccountAdapter
    {
        // entity being wrapped
        EntityBase2 entity;

        // optional field name prefix
        string fieldPrefix = "";

        /// <summary>
        /// Type of credentials used when communicating with PayPal
        /// </summary>
        public PayPalCredentialType CredentialType
        {
            get { return (PayPalCredentialType)((short)entity.Fields[fieldPrefix + "ApiCredentialType"].CurrentValue); }
            set { entity.SetNewFieldValue(fieldPrefix + "ApiCredentialType", (short)value); }
        }

        /// <summary>
        /// Type username used when communicating with PayPal
        /// </summary>
        public string ApiUserName
        {
            get { return (string)entity.Fields[fieldPrefix + "ApiUserName"].CurrentValue; }
            set { entity.SetNewFieldValue(fieldPrefix + "ApiUserName", value); }
        }

        /// <summary>
        /// Type username used when communicating with PayPal
        /// </summary>
        public string ApiPassword
        {
            get { return (string)entity.Fields[fieldPrefix + "ApiPassword"].CurrentValue; }
            set { entity.SetNewFieldValue(fieldPrefix + "ApiPassword", value); }
        }

        /// <summary>
        /// Type username used when communicating with PayPal
        /// </summary>
        public string ApiSignature
        {
            get { return (string)entity.Fields[fieldPrefix + "ApiSignature"].CurrentValue; }
            set { entity.SetNewFieldValue(fieldPrefix + "ApiSignature", value); }
        }

        /// <summary>
        /// Client SSL certificate to use when communicating with PayPal
        /// </summary>
        public byte[] ApiCertificate
        {
            get { return (byte[])entity.Fields[fieldPrefix + "ApiCertificate"].CurrentValue; }
            set { entity.SetNewFieldValue(fieldPrefix + "ApiCertificate", value); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PayPalAccountAdapter(EntityBase2 entity, string fieldPrefix)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (fieldPrefix == null)
            {
                throw new ArgumentNullException("fieldPrefix");
            }

            this.entity = entity;
            this.fieldPrefix = fieldPrefix;

            VerifyEntity(entity);
        }

        /// <summary>
        /// Ensures the entity has the required PayPal credential fields
        /// </summary>
        private void VerifyEntity(EntityBase2 entity)
        {
            List<string> requiredFields = new List<string>()
            {
                fieldPrefix + "ApiCredentialType", 
                fieldPrefix + "ApiUserName", 
                fieldPrefix + "ApiPassword", 
                fieldPrefix + "ApiSignature", 
                fieldPrefix + "ApiCertificate"
            };

            // look for fields that are missing from the entity
            List<string> missingFields = requiredFields.Where(fieldName => entity.Fields[fieldName] == null).ToList();
            if (missingFields.Count > 0)
            {
                // throw an exception
                string message = "The entity provided to the PayPalAccountAdapter is invalid, missing the following fields: ";
                message += string.Join(",", missingFields.ToArray());

                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Clones the adapter and underlying entity
        /// </summary>
        public PayPalAccountAdapter Clone()
        {
            EntityBase2 clonedEntity = EntityUtility.CloneEntity<EntityBase2>(entity);

            return new PayPalAccountAdapter(clonedEntity, fieldPrefix);
        }

        /// <summary>
        /// Copy the account values from the fromEntity to the same fields in the toEntity
        /// </summary>
        public static void Copy(EntityBase2 fromEntity, EntityBase2 toEntity, string fieldPrefix)
        {
            PayPalAccountAdapter fromAdapter = new PayPalAccountAdapter(fromEntity, fieldPrefix);
            PayPalAccountAdapter toAdapter = new PayPalAccountAdapter(toEntity, fieldPrefix);

            toAdapter.CredentialType = fromAdapter.CredentialType;
            toAdapter.ApiUserName = fromAdapter.ApiUserName;
            toAdapter.ApiSignature = fromAdapter.ApiSignature;
            toAdapter.ApiPassword = fromAdapter.ApiPassword;
            toAdapter.ApiCertificate = fromAdapter.ApiCertificate;
        }

        #region equality

        /// <summary>
        /// Equals
        /// </summary>
        public override bool Equals(object obj)
        {
            PayPalAccountAdapter other = obj as PayPalAccountAdapter;
            if ((object)other == null)
            {
                return false;
            }

            return
                this.CredentialType == other.CredentialType &&
                this.ApiUserName == other.ApiUserName &&
                this.ApiPassword == other.ApiPassword &&
                this.ApiSignature == other.ApiSignature &&
                this.ApiCertificate == other.ApiCertificate;
        }

        /// <summary>
        /// Operator==
        /// </summary>
        public static bool operator ==(PayPalAccountAdapter left, PayPalAccountAdapter right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Operator!=
        /// </summary>
        public static bool operator !=(PayPalAccountAdapter left, PayPalAccountAdapter right)
        {
            return !(left.Equals(right));
        }

        /// <summary>
        /// Hash code
        /// </summary>
        public override int GetHashCode()
        {
            string codeSource = (CredentialType.ToString() + ApiUserName + ApiPassword + ApiSignature);

            if (ApiCertificate != null)
            {
                codeSource = Convert.ToBase64String(ApiCertificate);
            }

            return codeSource.GetHashCode();
        }

        #endregion 
    }
}
