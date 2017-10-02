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
    /// Entity interface which represents the entity 'UspsShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUspsShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The UspsAccountID property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."UspsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UspsAccountID { get; }
        /// <summary> The HidePostage property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."HidePostage"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean HidePostage { get; }
        /// <summary> The RequireFullAddressValidation property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."RequireFullAddressValidation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean RequireFullAddressValidation { get; }
        /// <summary> The IntegratorTransactionID property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."IntegratorTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Guid IntegratorTransactionID { get; }
        /// <summary> The UspsTransactionID property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."UspsTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Guid UspsTransactionID { get; }
        /// <summary> The OriginalUspsAccountID property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."OriginalUspsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> OriginalUspsAccountID { get; }
        /// <summary> The ScanFormBatchID property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ScanFormBatchID { get; }
        /// <summary> The RequestedLabelFormat property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RequestedLabelFormat { get; }
        /// <summary> The RateShop property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."RateShop"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean RateShop { get; }
        
        IPostalShipmentEntity PostalShipment { get; }
        
        IScanFormBatchEntity ScanFormBatch { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUspsShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUspsShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UspsShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class UspsShipmentEntity : IUspsShipmentEntity
    {
        IPostalShipmentEntity IUspsShipmentEntity.PostalShipment => PostalShipment;
        
        IScanFormBatchEntity IUspsShipmentEntity.ScanFormBatch => ScanFormBatch;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUspsShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUspsShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUspsShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUspsShipmentEntity(this, objectMap);
        }

        
    }
}
