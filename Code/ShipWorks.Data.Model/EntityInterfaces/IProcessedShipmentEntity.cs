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
    /// Entity interface which represents the entity 'ProcessedShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IProcessedShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipmentID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The ShipmentType property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipmentType"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        ShipWorks.Shipping.ShipmentTypeCode ShipmentType { get; }
        /// <summary> The ShipDate property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipDate"<br/>
        /// View field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime ShipDate { get; }
        /// <summary> The Insurance property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."Insurance"<br/>
        /// View field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Insurance { get; }
        /// <summary> The InsuranceProvider property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."InsuranceProvider"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 InsuranceProvider { get; }
        /// <summary> The ProcessedDate property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ProcessedDate"<br/>
        /// View field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> ProcessedDate { get; }
        /// <summary> The ProcessedUserID property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ProcessedUserID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ProcessedUserID { get; }
        /// <summary> The ProcessedComputerID property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ProcessedComputerID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ProcessedComputerID { get; }
        /// <summary> The Voided property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."Voided"<br/>
        /// View field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Voided { get; }
        /// <summary> The VoidedDate property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."VoidedDate"<br/>
        /// View field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> VoidedDate { get; }
        /// <summary> The VoidedUserID property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."VoidedUserID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> VoidedUserID { get; }
        /// <summary> The VoidedComputerID property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."VoidedComputerID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> VoidedComputerID { get; }
        /// <summary> The TotalWeight property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."TotalWeight"<br/>
        /// View field type characteristics (type, precision, scale, length): Decimal, 29, 9, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal TotalWeight { get; }
        /// <summary> The TrackingNumber property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."TrackingNumber"<br/>
        /// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TrackingNumber { get; }
        /// <summary> The ShipmentCost property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipmentCost"<br/>
        /// View field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal ShipmentCost { get; }
        /// <summary> The ShipSenseStatus property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipSenseStatus"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipSenseStatus { get; }
        /// <summary> The ShipAddressValidationStatus property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipAddressValidationStatus"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipAddressValidationStatus { get; }
        /// <summary> The ShipResidentialStatus property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipResidentialStatus"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipResidentialStatus { get; }
        /// <summary> The ShipPOBox property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipPOBox"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipPOBox { get; }
        /// <summary> The ShipMilitaryAddress property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipMilitaryAddress"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipMilitaryAddress { get; }
        /// <summary> The RequestedLabelFormat property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."RequestedLabelFormat"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RequestedLabelFormat { get; }
        /// <summary> The ActualLabelFormat property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ActualLabelFormat"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ActualLabelFormat { get; }
        /// <summary> The OrderID property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."OrderID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The OrderNumberComplete property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."OrderNumberComplete"<br/>
        /// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OrderNumberComplete { get; }
        /// <summary> The Service property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."Service"<br/>
        /// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Service { get; }
        /// <summary> The ShipUSTerritory property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipUSTerritory"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipUSTerritory { get; }
        /// <summary> The ProcessedWithUiMode property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ProcessedWithUiMode"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ProcessedWithUiMode { get; }
        /// <summary> The CombineSplitStatus property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."CombineSplitStatus"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 CombineSplitStatus { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProcessedShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProcessedShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ProcessedShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class ProcessedShipmentEntity : IProcessedShipmentEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProcessedShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IProcessedShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IProcessedShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyProcessedShipmentEntity(this, objectMap);
        }

        
    }
}
