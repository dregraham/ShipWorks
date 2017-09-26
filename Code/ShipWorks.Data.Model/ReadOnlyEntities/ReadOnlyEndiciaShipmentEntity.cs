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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'EndiciaShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyEndiciaShipmentEntity : IEndiciaShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyEndiciaShipmentEntity(IEndiciaShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            EndiciaAccountID = source.EndiciaAccountID;
            OriginalEndiciaAccountID = source.OriginalEndiciaAccountID;
            StealthPostage = source.StealthPostage;
            ReferenceID = source.ReferenceID;
            TransactionID = source.TransactionID;
            RefundFormID = source.RefundFormID;
            ScanFormBatchID = source.ScanFormBatchID;
            ScanBasedReturn = source.ScanBasedReturn;
            RequestedLabelFormat = source.RequestedLabelFormat;
            
            PostalShipment = (IPostalShipmentEntity) source.PostalShipment?.AsReadOnly(objectMap);
            
            ScanFormBatch = (IScanFormBatchEntity) source.ScanFormBatch?.AsReadOnly(objectMap);
            

            CopyCustomEndiciaShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The EndiciaAccountID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."EndiciaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EndiciaAccountID { get; }
        /// <summary> The OriginalEndiciaAccountID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."OriginalEndiciaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> OriginalEndiciaAccountID { get; }
        /// <summary> The StealthPostage property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."StealthPostage"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean StealthPostage { get; }
        /// <summary> The ReferenceID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."ReferenceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReferenceID { get; }
        /// <summary> The TransactionID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."TransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> TransactionID { get; }
        /// <summary> The RefundFormID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."RefundFormID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> RefundFormID { get; }
        /// <summary> The ScanFormBatchID property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ScanFormBatchID { get; }
        /// <summary> The ScanBasedReturn property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."ScanBasedReturn"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ScanBasedReturn { get; }
        /// <summary> The RequestedLabelFormat property of the Entity EndiciaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        
        public IPostalShipmentEntity PostalShipment { get; }
        
        
        public IScanFormBatchEntity ScanFormBatch { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEndiciaShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEndiciaShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomEndiciaShipmentData(IEndiciaShipmentEntity source);
    }
}
