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
    /// Read-only representation of the entity 'BestRateShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyBestRateShipmentEntity : IBestRateShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyBestRateShipmentEntity(IBestRateShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            DimsProfileID = source.DimsProfileID;
            DimsLength = source.DimsLength;
            DimsWidth = source.DimsWidth;
            DimsHeight = source.DimsHeight;
            DimsWeight = source.DimsWeight;
            DimsAddWeight = source.DimsAddWeight;
            ServiceLevel = source.ServiceLevel;
            InsuranceValue = source.InsuranceValue;
            RequestedLabelFormat = source.RequestedLabelFormat;
            
            Shipment = source.Shipment?.AsReadOnly(objectMap);
            
            

            CopyCustomBestRateShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The DimsProfileID property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DimsAddWeight { get; }
        /// <summary> The ServiceLevel property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."ServiceLevel"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ServiceLevel { get; }
        /// <summary> The InsuranceValue property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal InsuranceValue { get; }
        /// <summary> The RequestedLabelFormat property of the Entity BestRateShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        
        public IShipmentEntity Shipment { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IBestRateShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IBestRateShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomBestRateShipmentData(IBestRateShipmentEntity source);
    }
}
