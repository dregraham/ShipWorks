///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'PayPalStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IPayPalStoreEntity: IStoreEntity
    {
        
        /// <summary> The ApiCredentialType property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."ApiCredentialType"<br/>
        /// Table field type characteristics (type, precision, scale, length): SmallInt, 5, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int16 ApiCredentialType { get; }
        /// <summary> The ApiUserName property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."ApiUserName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiUserName { get; }
        /// <summary> The ApiPassword property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."ApiPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiPassword { get; }
        /// <summary> The ApiSignature property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."ApiSignature"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiSignature { get; }
        /// <summary> The ApiCertificate property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."ApiCertificate"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.Byte[] ApiCertificate { get; }
        /// <summary> The LastTransactionDate property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."LastTransactionDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime LastTransactionDate { get; }
        /// <summary> The LastValidTransactionDate property of the Entity PayPalStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalStore"."LastValidTransactionDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime LastValidTransactionDate { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IPayPalStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IPayPalStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'PayPalStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class PayPalStoreEntity : IPayPalStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IPayPalStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IPayPalStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IPayPalStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyPayPalStoreEntity(this, objectMap);
        }
    }
}
