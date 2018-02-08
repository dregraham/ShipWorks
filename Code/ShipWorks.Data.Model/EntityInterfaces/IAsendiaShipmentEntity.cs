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
    /// Entity interface which represents the entity 'AsendiaShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAsendiaShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The AsendiaAccountID property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."AsendiaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 AsendiaAccountID { get; }
        /// <summary> The Service property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        Interapptive.Shared.Enums.AsendiaServiceType Service { get; }
        /// <summary> The RequestedLabelFormat property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RequestedLabelFormat { get; }
        /// <summary> The Contents property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."Contents"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Contents { get; }
        /// <summary> The NonDelivery property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."NonDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 NonDelivery { get; }
        /// <summary> The ShipEngineLabelID property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."ShipEngineLabelID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipEngineLabelID { get; }
        /// <summary> The DimsProfileID property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsHeight { get; }
        /// <summary> The DimsAddWeight property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DimsAddWeight { get; }
        /// <summary> The DimsWeight property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWeight { get; }
        /// <summary> The InsuranceValue property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal InsuranceValue { get; }
        /// <summary> The NonMachinable property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean NonMachinable { get; }
        /// <summary> The Insurance property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Insurance { get; }
        
        IShipmentEntity Shipment { get; }
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAsendiaShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAsendiaShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AsendiaShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class AsendiaShipmentEntity : IAsendiaShipmentEntity
    {
        IShipmentEntity IAsendiaShipmentEntity.Shipment => Shipment;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAsendiaShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IAsendiaShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAsendiaShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAsendiaShipmentEntity(this, objectMap);
        }

        
    }
}
