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
    /// Entity interface which represents the entity 'OtherShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOtherShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity OtherShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OtherShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The Carrier property of the Entity OtherShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OtherShipment"."Carrier"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Carrier { get; }
        /// <summary> The Service property of the Entity OtherShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OtherShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Service { get; }
        /// <summary> The InsuranceValue property of the Entity OtherShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OtherShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal InsuranceValue { get; }
        
        IShipmentEntity Shipment { get; }
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOtherShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOtherShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OtherShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class OtherShipmentEntity : IOtherShipmentEntity
    {
        IShipmentEntity IOtherShipmentEntity.Shipment => Shipment;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOtherShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IOtherShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOtherShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOtherShipmentEntity(this, objectMap);
        }
    }
}
