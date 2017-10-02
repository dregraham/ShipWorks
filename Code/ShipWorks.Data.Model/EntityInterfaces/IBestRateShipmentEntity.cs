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
    /// Entity interface which represents the entity 'BestRateShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IBestRateShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The DimsProfileID property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DimsAddWeight { get; }
        /// <summary> The ServiceLevel property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."ServiceLevel"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ServiceLevel { get; }
        /// <summary> The InsuranceValue property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal InsuranceValue { get; }
        /// <summary> The RequestedLabelFormat property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RequestedLabelFormat { get; }
        
        IShipmentEntity Shipment { get; }
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IBestRateShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IBestRateShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'BestRateShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class BestRateShipmentEntity : IBestRateShipmentEntity
    {
        IShipmentEntity IBestRateShipmentEntity.Shipment => Shipment;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IBestRateShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IBestRateShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IBestRateShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyBestRateShipmentEntity(this, objectMap);
        }

        
    }
}
