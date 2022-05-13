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
    /// Read-only representation of the entity 'ShipEngineManifest'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShipEngineManifestEntity : IShipEngineManifestEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShipEngineManifestEntity(IShipEngineManifestEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipEngineManifestID = source.ShipEngineManifestID;
            CarrierAccountID = source.CarrierAccountID;
            ShipmentTypeCode = source.ShipmentTypeCode;
            ManifestID = source.ManifestID;
            FormID = source.FormID;
            CreatedAt = source.CreatedAt;
            ShipDate = source.ShipDate;
            ShipmentCount = source.ShipmentCount;
            PlatformWarehouseID = source.PlatformWarehouseID;
            SubmissionID = source.SubmissionID;
            CarrierID = source.CarrierID;
            ManifestUrl = source.ManifestUrl;
            
            
            

            CopyCustomShipEngineManifestData(source);
        }

        
        /// <summary> The ShipEngineManifestID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."ShipEngineManifestID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ShipEngineManifestID { get; }
        /// <summary> The CarrierAccountID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."CarrierAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 CarrierAccountID { get; }
        /// <summary> The ShipmentTypeCode property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."ShipmentTypeCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipmentTypeCode { get; }
        /// <summary> The ManifestID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."ManifestID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ManifestID { get; }
        /// <summary> The FormID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."FormID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FormID { get; }
        /// <summary> The CreatedAt property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."CreatedAt"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime CreatedAt { get; }
        /// <summary> The ShipDate property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."ShipDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime ShipDate { get; }
        /// <summary> The ShipmentCount property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."ShipmentCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipmentCount { get; }
        /// <summary> The PlatformWarehouseID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."PlatformWarehouseID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PlatformWarehouseID { get; }
        /// <summary> The SubmissionID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."SubmissionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SubmissionID { get; }
        /// <summary> The CarrierID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."CarrierID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CarrierID { get; }
        /// <summary> The ManifestUrl property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."ManifestUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ManifestUrl { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipEngineManifestEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipEngineManifestEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShipEngineManifestData(IShipEngineManifestEntity source);
    }
}
