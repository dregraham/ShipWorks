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
    /// Entity interface which represents the entity 'ShipEngineManifest'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShipEngineManifestEntity
    {
        
        /// <summary> The ShipEngineManifestID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."ShipEngineManifestID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ShipEngineManifestID { get; }
        /// <summary> The CarrierAccountID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."CarrierAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 CarrierAccountID { get; }
        /// <summary> The ShipmentTypeCode property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."ShipmentTypeCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipmentTypeCode { get; }
        /// <summary> The ManifestID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."ManifestID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ManifestID { get; }
        /// <summary> The FormID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."FormID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FormID { get; }
        /// <summary> The CreatedAt property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."CreatedAt"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime CreatedAt { get; }
        /// <summary> The ShipDate property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."ShipDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime ShipDate { get; }
        /// <summary> The ShipmentCount property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."ShipmentCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipmentCount { get; }
        /// <summary> The PlatformWarehouseID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."PlatformWarehouseID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PlatformWarehouseID { get; }
        /// <summary> The SubmissionID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."SubmissionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SubmissionID { get; }
        /// <summary> The CarrierID property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."CarrierID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CarrierID { get; }
        /// <summary> The ManifestUrl property of the Entity ShipEngineManifest<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipEngineManifest"."ManifestUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ManifestUrl { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShipEngineManifestEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShipEngineManifestEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShipEngineManifest'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShipEngineManifestEntity : IShipEngineManifestEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipEngineManifestEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShipEngineManifestEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShipEngineManifestEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShipEngineManifestEntity(this, objectMap);
        }

        
    }
}
