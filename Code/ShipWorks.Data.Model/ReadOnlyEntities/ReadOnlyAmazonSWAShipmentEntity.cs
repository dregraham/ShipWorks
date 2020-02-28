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
    /// Read-only representation of the entity 'AmazonSWAShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAmazonSWAShipmentEntity : IAmazonSWAShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAmazonSWAShipmentEntity(IAmazonSWAShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            AmazonSWAAccountID = source.AmazonSWAAccountID;
            Service = source.Service;
            RequestedLabelFormat = source.RequestedLabelFormat;
            ShipEngineLabelID = source.ShipEngineLabelID;
            DimsProfileID = source.DimsProfileID;
            DimsLength = source.DimsLength;
            DimsWidth = source.DimsWidth;
            DimsHeight = source.DimsHeight;
            DimsAddWeight = source.DimsAddWeight;
            DimsWeight = source.DimsWeight;
            InsuranceValue = source.InsuranceValue;
            Insurance = source.Insurance;
            
            Shipment = (IShipmentEntity) source.Shipment?.AsReadOnly(objectMap);
            
            

            CopyCustomAmazonSWAShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The AmazonSWAAccountID property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."AmazonSWAAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 AmazonSWAAccountID { get; }
        /// <summary> The Service property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Service { get; }
        /// <summary> The RequestedLabelFormat property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        /// <summary> The ShipEngineLabelID property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."ShipEngineLabelID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipEngineLabelID { get; }
        /// <summary> The DimsProfileID property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsHeight { get; }
        /// <summary> The DimsAddWeight property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DimsAddWeight { get; }
        /// <summary> The DimsWeight property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWeight { get; }
        /// <summary> The InsuranceValue property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal InsuranceValue { get; }
        /// <summary> The Insurance property of the Entity AmazonSWAShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAShipment"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Insurance { get; }
        
        public IShipmentEntity Shipment { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonSWAShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonSWAShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAmazonSWAShipmentData(IAmazonSWAShipmentEntity source);
    }
}
