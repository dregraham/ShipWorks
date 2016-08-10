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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'EndiciaShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEndiciaShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The EndiciaAccountID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."EndiciaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 EndiciaAccountID { get; }
        /// <summary> The OriginalEndiciaAccountID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."OriginalEndiciaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> OriginalEndiciaAccountID { get; }
        /// <summary> The StealthPostage property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."StealthPostage"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean StealthPostage { get; }
        /// <summary> The ReferenceID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."ReferenceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ReferenceID { get; }
        /// <summary> The TransactionID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."TransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> TransactionID { get; }
        /// <summary> The RefundFormID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."RefundFormID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> RefundFormID { get; }
        /// <summary> The ScanFormBatchID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ScanFormBatchID { get; }
        /// <summary> The ScanBasedReturn property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."ScanBasedReturn"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ScanBasedReturn { get; }
        /// <summary> The RequestedLabelFormat property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RequestedLabelFormat { get; }
        
        IPostalShipmentEntity PostalShipment { get; }
        
        IScanFormBatchEntity ScanFormBatch { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEndiciaShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEndiciaShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EndiciaShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class EndiciaShipmentEntity : IEndiciaShipmentEntity
    {
        IPostalShipmentEntity IEndiciaShipmentEntity.PostalShipment => PostalShipment;
        
        IScanFormBatchEntity IEndiciaShipmentEntity.ScanFormBatch => ScanFormBatch;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEndiciaShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IEndiciaShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEndiciaShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEndiciaShipmentEntity(this, objectMap);
        }
    }
}
