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
    /// Entity interface which represents the entity 'AmazonShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAmazonShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The CarrierName property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."CarrierName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CarrierName { get; }
        /// <summary> The ShippingServiceName property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."ShippingServiceName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShippingServiceName { get; }
        /// <summary> The ShippingServiceID property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."ShippingServiceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShippingServiceID { get; }
        /// <summary> The ShippingServiceOfferID property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."ShippingServiceOfferID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShippingServiceOfferID { get; }
        /// <summary> The InsuranceValue property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal InsuranceValue { get; }
        /// <summary> The DimsProfileID property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DimsAddWeight { get; }
        /// <summary> The DeliveryExperience property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DeliveryExperience"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DeliveryExperience { get; }
        /// <summary> The DeclaredValue property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DeclaredValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Decimal> DeclaredValue { get; }
        /// <summary> The AmazonUniqueShipmentID property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."AmazonUniqueShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String AmazonUniqueShipmentID { get; }
        
        IShipmentEntity Shipment { get; }
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AmazonShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class AmazonShipmentEntity : IAmazonShipmentEntity
    {
        IShipmentEntity IAmazonShipmentEntity.Shipment => Shipment;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IAmazonShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAmazonShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAmazonShipmentEntity(this, objectMap);
        }

        
    }
}
