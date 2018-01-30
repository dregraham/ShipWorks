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
    /// Read-only representation of the entity 'AsendiaShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAsendiaShipmentEntity : IAsendiaShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAsendiaShipmentEntity(IAsendiaShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            AsendiaAccountID = source.AsendiaAccountID;
            Service = source.Service;
            RequestedLabelFormat = source.RequestedLabelFormat;
            Contents = source.Contents;
            NonDelivery = source.NonDelivery;
            ShipEngineLabelID = source.ShipEngineLabelID;
            DimsProfileID = source.DimsProfileID;
            DimsLength = source.DimsLength;
            DimsWidth = source.DimsWidth;
            DimsHeight = source.DimsHeight;
            DimsAddWeight = source.DimsAddWeight;
            DimsWeight = source.DimsWeight;
            InsuranceValue = source.InsuranceValue;
            NonMachinable = source.NonMachinable;
            Insurance = source.Insurance;
            
            Shipment = (IShipmentEntity) source.Shipment?.AsReadOnly(objectMap);
            
            

            CopyCustomAsendiaShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The AsendiaAccountID property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."AsendiaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 AsendiaAccountID { get; }
        /// <summary> The Service property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public Interapptive.Shared.Enums.AsendiaServiceType Service { get; }
        /// <summary> The RequestedLabelFormat property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        /// <summary> The Contents property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."Contents"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Contents { get; }
        /// <summary> The NonDelivery property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."NonDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 NonDelivery { get; }
        /// <summary> The ShipEngineLabelID property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."ShipEngineLabelID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipEngineLabelID { get; }
        /// <summary> The DimsProfileID property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsHeight { get; }
        /// <summary> The DimsAddWeight property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DimsAddWeight { get; }
        /// <summary> The DimsWeight property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWeight { get; }
        /// <summary> The InsuranceValue property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal InsuranceValue { get; }
        /// <summary> The NonMachinable property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean NonMachinable { get; }
        /// <summary> The Insurance property of the Entity AsendiaShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaShipment"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Insurance { get; }
        
        public IShipmentEntity Shipment { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAsendiaShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAsendiaShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAsendiaShipmentData(IAsendiaShipmentEntity source);
    }
}
