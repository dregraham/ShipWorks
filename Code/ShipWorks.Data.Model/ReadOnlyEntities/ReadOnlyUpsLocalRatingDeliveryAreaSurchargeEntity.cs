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
    /// Read-only representation of the entity 'UpsLocalRatingDeliveryAreaSurcharge'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUpsLocalRatingDeliveryAreaSurchargeEntity : IUpsLocalRatingDeliveryAreaSurchargeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUpsLocalRatingDeliveryAreaSurchargeEntity(IUpsLocalRatingDeliveryAreaSurchargeEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            DeliveryAreaSurchargeID = source.DeliveryAreaSurchargeID;
            ZoneFileID = source.ZoneFileID;
            DestinationZip = source.DestinationZip;
            DeliveryAreaType = source.DeliveryAreaType;
            
            
            UpsLocalRatingZoneFile = source.UpsLocalRatingZoneFile?.AsReadOnly(objectMap);
            

            CopyCustomUpsLocalRatingDeliveryAreaSurchargeData(source);
        }

        
        /// <summary> The DeliveryAreaSurchargeID property of the Entity UpsLocalRatingDeliveryAreaSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingDeliveryAreaSurcharge"."DeliveryAreaSurchargeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 DeliveryAreaSurchargeID { get; }
        /// <summary> The ZoneFileID property of the Entity UpsLocalRatingDeliveryAreaSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingDeliveryAreaSurcharge"."ZoneFileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ZoneFileID { get; }
        /// <summary> The DestinationZip property of the Entity UpsLocalRatingDeliveryAreaSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingDeliveryAreaSurcharge"."DestinationZip"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DestinationZip { get; }
        /// <summary> The DeliveryAreaType property of the Entity UpsLocalRatingDeliveryAreaSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingDeliveryAreaSurcharge"."DeliveryAreaType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DeliveryAreaType { get; }
        
        
        public IUpsLocalRatingZoneFileEntity UpsLocalRatingZoneFile { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLocalRatingDeliveryAreaSurchargeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLocalRatingDeliveryAreaSurchargeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUpsLocalRatingDeliveryAreaSurchargeData(IUpsLocalRatingDeliveryAreaSurchargeEntity source);
    }
}
