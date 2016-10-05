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
    /// Read-only representation of the entity 'WorldShipProcessed'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyWorldShipProcessedEntity : IWorldShipProcessedEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyWorldShipProcessedEntity(IWorldShipProcessedEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            WorldShipProcessedID = source.WorldShipProcessedID;
            ShipmentID = source.ShipmentID;
            RowVersion = source.RowVersion;
            PublishedCharges = source.PublishedCharges;
            NegotiatedCharges = source.NegotiatedCharges;
            TrackingNumber = source.TrackingNumber;
            UspsTrackingNumber = source.UspsTrackingNumber;
            ServiceType = source.ServiceType;
            PackageType = source.PackageType;
            UpsPackageID = source.UpsPackageID;
            DeclaredValueAmount = source.DeclaredValueAmount;
            DeclaredValueOption = source.DeclaredValueOption;
            WorldShipShipmentID = source.WorldShipShipmentID;
            VoidIndicator = source.VoidIndicator;
            NumberOfPackages = source.NumberOfPackages;
            LeadTrackingNumber = source.LeadTrackingNumber;
            ShipmentIdCalculated = source.ShipmentIdCalculated;
            
            
            WorldShipShipment = source.WorldShipShipment?.AsReadOnly(objectMap);
            

            CopyCustomWorldShipProcessedData(source);
        }

        
        /// <summary> The WorldShipProcessedID property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."WorldShipProcessedID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 WorldShipProcessedID { get; }
        /// <summary> The ShipmentID property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ShipmentID { get; }
        /// <summary> The RowVersion property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The PublishedCharges property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."PublishedCharges"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PublishedCharges { get; }
        /// <summary> The NegotiatedCharges property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."NegotiatedCharges"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double NegotiatedCharges { get; }
        /// <summary> The TrackingNumber property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."TrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String TrackingNumber { get; }
        /// <summary> The UspsTrackingNumber property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."UspsTrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String UspsTrackingNumber { get; }
        /// <summary> The ServiceType property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."ServiceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ServiceType { get; }
        /// <summary> The PackageType property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."PackageType"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String PackageType { get; }
        /// <summary> The UpsPackageID property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."UpsPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String UpsPackageID { get; }
        /// <summary> The DeclaredValueAmount property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."DeclaredValueAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DeclaredValueAmount { get; }
        /// <summary> The DeclaredValueOption property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."DeclaredValueOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): NChar, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String DeclaredValueOption { get; }
        /// <summary> The WorldShipShipmentID property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."WorldShipShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String WorldShipShipmentID { get; }
        /// <summary> The VoidIndicator property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."VoidIndicator"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String VoidIndicator { get; }
        /// <summary> The NumberOfPackages property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."NumberOfPackages"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String NumberOfPackages { get; }
        /// <summary> The LeadTrackingNumber property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."LeadTrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String LeadTrackingNumber { get; }
        /// <summary> The ShipmentIdCalculated property of the Entity WorldShipProcessed<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipProcessed"."ShipmentIdCalculated"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ShipmentIdCalculated { get; }
        
        
        public IWorldShipShipmentEntity WorldShipShipment { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWorldShipProcessedEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWorldShipProcessedEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomWorldShipProcessedData(IWorldShipProcessedEntity source);
    }
}
