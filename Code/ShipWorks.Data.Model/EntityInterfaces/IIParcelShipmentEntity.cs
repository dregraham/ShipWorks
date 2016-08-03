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
    /// Entity interface which represents the entity 'IParcelShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IIParcelShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The IParcelAccountID property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."iParcelAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 IParcelAccountID { get; }
        /// <summary> The Service property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Service { get; }
        /// <summary> The Reference property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."Reference"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Reference { get; }
        /// <summary> The TrackByEmail property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."TrackByEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean TrackByEmail { get; }
        /// <summary> The TrackBySMS property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."TrackBySMS"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean TrackBySMS { get; }
        /// <summary> The IsDeliveryDutyPaid property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."IsDeliveryDutyPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsDeliveryDutyPaid { get; }
        /// <summary> The RequestedLabelFormat property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RequestedLabelFormat { get; }
        
        IShipmentEntity Shipment { get; }
        
        
        IEnumerable<IIParcelPackageEntity> Packages { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IIParcelShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IIParcelShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'IParcelShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class IParcelShipmentEntity : IIParcelShipmentEntity
    {
        IShipmentEntity IIParcelShipmentEntity.Shipment => Shipment;
        
        
        IEnumerable<IIParcelPackageEntity> IIParcelShipmentEntity.Packages => Packages;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IIParcelShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IIParcelShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyIParcelShipmentEntity(this, objectMap);
        }
    }
}
