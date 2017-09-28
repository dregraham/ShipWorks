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
    /// Read-only representation of the entity 'UspsShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUspsShipmentEntity : IUspsShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUspsShipmentEntity(IUspsShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            UspsAccountID = source.UspsAccountID;
            HidePostage = source.HidePostage;
            RequireFullAddressValidation = source.RequireFullAddressValidation;
            IntegratorTransactionID = source.IntegratorTransactionID;
            UspsTransactionID = source.UspsTransactionID;
            OriginalUspsAccountID = source.OriginalUspsAccountID;
            ScanFormBatchID = source.ScanFormBatchID;
            RequestedLabelFormat = source.RequestedLabelFormat;
            RateShop = source.RateShop;
            
            PostalShipment = (IPostalShipmentEntity) source.PostalShipment?.AsReadOnly(objectMap);
            
            ScanFormBatch = (IScanFormBatchEntity) source.ScanFormBatch?.AsReadOnly(objectMap);
            

            CopyCustomUspsShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The UspsAccountID property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."UspsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UspsAccountID { get; }
        /// <summary> The HidePostage property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."HidePostage"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean HidePostage { get; }
        /// <summary> The RequireFullAddressValidation property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."RequireFullAddressValidation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean RequireFullAddressValidation { get; }
        /// <summary> The IntegratorTransactionID property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."IntegratorTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Guid IntegratorTransactionID { get; }
        /// <summary> The UspsTransactionID property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."UspsTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Guid UspsTransactionID { get; }
        /// <summary> The OriginalUspsAccountID property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."OriginalUspsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> OriginalUspsAccountID { get; }
        /// <summary> The ScanFormBatchID property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ScanFormBatchID { get; }
        /// <summary> The RequestedLabelFormat property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        /// <summary> The RateShop property of the Entity UspsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsShipment"."RateShop"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean RateShop { get; }
        
        public IPostalShipmentEntity PostalShipment { get; }
        
        
        public IScanFormBatchEntity ScanFormBatch { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUspsShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUspsShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUspsShipmentData(IUspsShipmentEntity source);
    }
}
