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
    /// Entity interface which represents the entity 'WorldShipProcessed'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IWorldShipProcessedEntity
    {
        
        /// <summary> The WorldShipProcessedID property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."WorldShipProcessedID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 WorldShipProcessedID { get; }
        /// <summary> The ShipmentID property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ShipmentID { get; }
        /// <summary> The RowVersion property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The PublishedCharges property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."PublishedCharges"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PublishedCharges { get; }
        /// <summary> The NegotiatedCharges property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."NegotiatedCharges"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double NegotiatedCharges { get; }
        /// <summary> The TrackingNumber property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."TrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String TrackingNumber { get; }
        /// <summary> The UspsTrackingNumber property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."UspsTrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String UspsTrackingNumber { get; }
        /// <summary> The ServiceType property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."ServiceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ServiceType { get; }
        /// <summary> The PackageType property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."PackageType"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String PackageType { get; }
        /// <summary> The UpsPackageID property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."UpsPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String UpsPackageID { get; }
        /// <summary> The DeclaredValueAmount property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."DeclaredValueAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DeclaredValueAmount { get; }
        /// <summary> The DeclaredValueOption property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."DeclaredValueOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): NChar, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String DeclaredValueOption { get; }
        /// <summary> The WorldShipShipmentID property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."WorldShipShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String WorldShipShipmentID { get; }
        /// <summary> The VoidIndicator property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."VoidIndicator"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String VoidIndicator { get; }
        /// <summary> The NumberOfPackages property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."NumberOfPackages"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String NumberOfPackages { get; }
        /// <summary> The LeadTrackingNumber property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."LeadTrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String LeadTrackingNumber { get; }
        /// <summary> The ShipmentIdCalculated property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."ShipmentIdCalculated"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ShipmentIdCalculated { get; }
        
        
        IWorldShipShipmentEntity WorldShipShipment { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IWorldShipProcessedEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IWorldShipProcessedEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'WorldShipProcessed'. <br/><br/>
    /// 
    /// </summary>
    public partial class WorldShipProcessedEntity : IWorldShipProcessedEntity
    {
        
        IWorldShipShipmentEntity IWorldShipProcessedEntity.WorldShipShipment => WorldShipShipment;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWorldShipProcessedEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IWorldShipProcessedEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IWorldShipProcessedEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyWorldShipProcessedEntity(this, objectMap);
        }
    }
}
