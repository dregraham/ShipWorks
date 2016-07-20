///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'PayPalStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyPayPalStoreEntity : ReadOnlyStoreEntity, IPayPalStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyPayPalStoreEntity(IPayPalStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ApiCredentialType = source.ApiCredentialType;
            ApiUserName = source.ApiUserName;
            ApiPassword = source.ApiPassword;
            ApiSignature = source.ApiSignature;
            ApiCertificate = source.ApiCertificate;
            LastTransactionDate = source.LastTransactionDate;
            LastValidTransactionDate = source.LastValidTransactionDate;
            
            
            

            CopyCustomPayPalStoreData(source);
        }

        
        /// <summary> The ApiCredentialType property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."ApiCredentialType"<br/>
        /// Table field type characteristics (type, precision, scale, length): SmallInt, 5, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int16 ApiCredentialType { get; }
        /// <summary> The ApiUserName property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."ApiUserName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiUserName { get; }
        /// <summary> The ApiPassword property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."ApiPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiPassword { get; }
        /// <summary> The ApiSignature property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."ApiSignature"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiSignature { get; }
        /// <summary> The ApiCertificate property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."ApiCertificate"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.Byte[] ApiCertificate { get; }
        /// <summary> The LastTransactionDate property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."LastTransactionDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime LastTransactionDate { get; }
        /// <summary> The LastValidTransactionDate property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."LastValidTransactionDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime LastValidTransactionDate { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IPayPalStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IPayPalStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomPayPalStoreData(IPayPalStoreEntity source);
    }
}
