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
    /// Read-only representation of the entity 'AmazonShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAmazonShipmentEntity : IAmazonShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAmazonShipmentEntity(IAmazonShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            CarrierName = source.CarrierName;
            ShippingServiceName = source.ShippingServiceName;
            ShippingServiceID = source.ShippingServiceID;
            InsuranceValue = source.InsuranceValue;
            DimsProfileID = source.DimsProfileID;
            DimsLength = source.DimsLength;
            DimsWidth = source.DimsWidth;
            DimsHeight = source.DimsHeight;
            DimsWeight = source.DimsWeight;
            DimsAddWeight = source.DimsAddWeight;
            DeliveryExperience = source.DeliveryExperience;
            DeclaredValue = source.DeclaredValue;
            AmazonUniqueShipmentID = source.AmazonUniqueShipmentID;
            
            Shipment = (IShipmentEntity) source.Shipment?.AsReadOnly(objectMap);
            
            

            CopyCustomAmazonShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The CarrierName property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."CarrierName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CarrierName { get; }
        /// <summary> The ShippingServiceName property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."ShippingServiceName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShippingServiceName { get; }
        /// <summary> The ShippingServiceID property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."ShippingServiceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShippingServiceID { get; }
        /// <summary> The InsuranceValue property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal InsuranceValue { get; }
        /// <summary> The DimsProfileID property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DimsAddWeight { get; }
        /// <summary> The DeliveryExperience property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DeliveryExperience"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DeliveryExperience { get; }
        /// <summary> The DeclaredValue property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."DeclaredValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> DeclaredValue { get; }
        /// <summary> The AmazonUniqueShipmentID property of the Entity AmazonShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonShipment"."AmazonUniqueShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String AmazonUniqueShipmentID { get; }
        
        public IShipmentEntity Shipment { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAmazonShipmentData(IAmazonShipmentEntity source);
    }
}
