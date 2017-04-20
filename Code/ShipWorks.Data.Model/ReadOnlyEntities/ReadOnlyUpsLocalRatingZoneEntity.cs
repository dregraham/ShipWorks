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
    /// Read-only representation of the entity 'UpsLocalRatingZone'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUpsLocalRatingZoneEntity : IUpsLocalRatingZoneEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUpsLocalRatingZoneEntity(IUpsLocalRatingZoneEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ZoneID = source.ZoneID;
            ZoneFileID = source.ZoneFileID;
            OriginZipFloor = source.OriginZipFloor;
            OriginZipCeiling = source.OriginZipCeiling;
            DestinationZipFloor = source.DestinationZipFloor;
            DestinationZipCeiling = source.DestinationZipCeiling;
            Service = source.Service;
            Zone = source.Zone;
            
            
            UpsLocalRatingZoneFile = source.UpsLocalRatingZoneFile?.AsReadOnly(objectMap);
            

            CopyCustomUpsLocalRatingZoneData(source);
        }

        
        /// <summary> The ZoneID property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."ZoneID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ZoneID { get; }
        /// <summary> The ZoneFileID property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."ZoneFileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ZoneFileID { get; }
        /// <summary> The OriginZipFloor property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."OriginZipFloor"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OriginZipFloor { get; }
        /// <summary> The OriginZipCeiling property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."OriginZipCeiling"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OriginZipCeiling { get; }
        /// <summary> The DestinationZipFloor property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."DestinationZipFloor"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DestinationZipFloor { get; }
        /// <summary> The DestinationZipCeiling property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."DestinationZipCeiling"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DestinationZipCeiling { get; }
        /// <summary> The Service property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Service { get; }
        /// <summary> The Zone property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."Zone"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Zone { get; }
        
        
        public IUpsLocalRatingZoneFileEntity UpsLocalRatingZoneFile { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLocalRatingZoneEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLocalRatingZoneEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUpsLocalRatingZoneData(IUpsLocalRatingZoneEntity source);
    }
}
