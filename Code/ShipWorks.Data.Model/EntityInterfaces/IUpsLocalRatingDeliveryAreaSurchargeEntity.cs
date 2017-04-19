﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'UpsLocalRatingDeliveryAreaSurcharge'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsLocalRatingDeliveryAreaSurchargeEntity
    {
        
        /// <summary> The DeliveryAreaSurchargeID property of the Entity UpsLocalRatingDeliveryAreaSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingDeliveryAreaSurcharge"."DeliveryAreaSurchargeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 DeliveryAreaSurchargeID { get; }
        /// <summary> The ZoneFileID property of the Entity UpsLocalRatingDeliveryAreaSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingDeliveryAreaSurcharge"."ZoneFileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ZoneFileID { get; }
        /// <summary> The DestinationZip property of the Entity UpsLocalRatingDeliveryAreaSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingDeliveryAreaSurcharge"."DestinationZip"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 5<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DestinationZip { get; }
        /// <summary> The DeliveryAreaType property of the Entity UpsLocalRatingDeliveryAreaSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingDeliveryAreaSurcharge"."DeliveryAreaType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DeliveryAreaType { get; }
        
        
        IUpsLocalRatingZoneFileEntity UpsLocalRatingZoneFile { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsLocalRatingDeliveryAreaSurchargeEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsLocalRatingDeliveryAreaSurchargeEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsLocalRatingDeliveryAreaSurcharge'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsLocalRatingDeliveryAreaSurchargeEntity : IUpsLocalRatingDeliveryAreaSurchargeEntity
    {
        
        IUpsLocalRatingZoneFileEntity IUpsLocalRatingDeliveryAreaSurchargeEntity.UpsLocalRatingZoneFile => UpsLocalRatingZoneFile;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLocalRatingDeliveryAreaSurchargeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUpsLocalRatingDeliveryAreaSurchargeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsLocalRatingDeliveryAreaSurchargeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsLocalRatingDeliveryAreaSurchargeEntity(this, objectMap);
        }
    }
}
