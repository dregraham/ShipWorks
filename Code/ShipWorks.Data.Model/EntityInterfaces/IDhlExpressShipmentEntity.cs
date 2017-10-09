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
    /// Entity interface which represents the entity 'DhlExpressShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IDhlExpressShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The DhlExpressAccountID property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."DhlExpressAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DhlExpressAccountID { get; }
        /// <summary> The Service property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Service { get; }
        /// <summary> The DeliveredDutyPaid property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."DeliveredDutyPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DeliveredDutyPaid { get; }
        /// <summary> The NonMachinable property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean NonMachinable { get; }
        /// <summary> The SaturdayDelivery property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean SaturdayDelivery { get; }
        
        IShipmentEntity Shipment { get; }
        
        
        IEnumerable<IDhlExpressPackageEntity> Packages { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDhlExpressShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDhlExpressShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'DhlExpressShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class DhlExpressShipmentEntity : IDhlExpressShipmentEntity
    {
        IShipmentEntity IDhlExpressShipmentEntity.Shipment => Shipment;
        
        
        IEnumerable<IDhlExpressPackageEntity> IDhlExpressShipmentEntity.Packages => Packages;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlExpressShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IDhlExpressShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IDhlExpressShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyDhlExpressShipmentEntity(this, objectMap);
        }

        
    }
}
