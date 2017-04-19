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
    /// Entity interface which represents the entity 'UpsLocalRatingZone'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsLocalRatingZoneEntity
    {
        
        /// <summary> The ZoneID property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."ZoneID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ZoneID { get; }
        /// <summary> The ZoneFileID property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."ZoneFileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ZoneFileID { get; }
        /// <summary> The OriginZipFloor property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."OriginZipFloor"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 5<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginZipFloor { get; }
        /// <summary> The OriginZipCeiling property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."OriginZipCeiling"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 5<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginZipCeiling { get; }
        /// <summary> The DestinationZipFloor property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."DestinationZipFloor"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 5<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DestinationZipFloor { get; }
        /// <summary> The DestinationZipCeiling property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."DestinationZipCeiling"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 5<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DestinationZipCeiling { get; }
        /// <summary> The Service property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Service { get; }
        /// <summary> The Zone property of the Entity UpsLocalRatingZone<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZone"."Zone"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Zone { get; }
        
        
        IUpsLocalRatingZoneFileEntity UpsLocalRatingZoneFile { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsLocalRatingZoneEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsLocalRatingZoneEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsLocalRatingZone'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsLocalRatingZoneEntity : IUpsLocalRatingZoneEntity
    {
        
        IUpsLocalRatingZoneFileEntity IUpsLocalRatingZoneEntity.UpsLocalRatingZoneFile => UpsLocalRatingZoneFile;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLocalRatingZoneEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUpsLocalRatingZoneEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsLocalRatingZoneEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsLocalRatingZoneEntity(this, objectMap);
        }
    }
}
